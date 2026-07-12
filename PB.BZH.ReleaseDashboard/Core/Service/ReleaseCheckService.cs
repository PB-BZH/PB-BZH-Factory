using System.Diagnostics;
using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.RegularExpressions;
using PB.BZH.ReleaseDashboard.Core.Models;

namespace PB.BZH.ReleaseDashboard.Core.Services;

public sealed class ReleaseCheckService {
  private static readonly HttpClient HttpClient = new() {
    Timeout = TimeSpan.FromSeconds(30)
  };

  public async Task<ReleaseCheckReport> RunAsync(
      ProductCatalog catalog,
      Action<string>? log,
      CancellationToken cancellationToken = default) {
    if (catalog == null)
      throw new ArgumentNullException(nameof(catalog));

    Stopwatch stopwatch =
        Stopwatch.StartNew();

    ReleaseCheckReport report =
        new() {
          GeneratedAtLocal = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
          GeneratedAtUtc = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ")
        };

    log?.Invoke("==================================================");
    log?.Invoke("PB BZH Release Check - Native C#");
    log?.Invoke("==================================================");

    await CheckSoftwaresRootAsync(
        report,
        catalog,
        log,
        cancellationToken);

    foreach (ProductInfo product in catalog.Products) {
      cancellationToken.ThrowIfCancellationRequested();

      log?.Invoke("");
      log?.Invoke("--------------------------------------------------");
      log?.Invoke(product.DisplayName + " (" + product.Id + ")");
      log?.Invoke("--------------------------------------------------");

      AddLocalCheckInfo(
          report,
          product,
          log);

      await CheckUpdateJsonAsync(
          report,
          catalog,
          product,
          log,
          cancellationToken);

      await CheckArtifactUrlAsync(
          report,
          catalog,
          product,
          log,
          cancellationToken);

      await CheckSha256UrlAsync(
          report,
          catalog,
          product,
          log,
          cancellationToken);

      await CheckDownloadUrlAsync(
          report,
          catalog,
          product,
          log,
          cancellationToken);
    }

    stopwatch.Stop();

    report.DurationSeconds =
        Math.Round(stopwatch.Elapsed.TotalSeconds,2);

    log?.Invoke("");
    log?.Invoke("==================================================");
    log?.Invoke("Summary");
    log?.Invoke("==================================================");
    log?.Invoke("OK       : " + report.Summary.Ok);
    log?.Invoke("Infos    : " + report.Summary.Info);
    log?.Invoke("Warnings : " + report.Summary.Warnings);
    log?.Invoke("Errors   : " + report.Summary.Errors);

    return report;
  }

  private static async Task CheckSoftwaresRootAsync(
      ReleaseCheckReport report,
      ProductCatalog catalog,
      Action<string>? log,
      CancellationToken cancellationToken) {
    string url =
        catalog.Site.SoftwaresUrl;

    WebCheckResult result =
        await CheckUrlAsync(
            url,
            cancellationToken);

    AddCheck(
        report,
        "Site",
        "site",
        result.Success ? "OK" : "FAIL",
        "Softwares root",
        result.Details,
        log);
  }

  private static void AddLocalCheckInfo(
      ReleaseCheckReport report,
      ProductInfo product,
      Action<string>? log) {
    if (string.Equals(
        product.LocalCheck,
        "disabled",
        StringComparison.OrdinalIgnoreCase)) {
      AddCheck(
          report,
          product,
          "INFO",
          "Contrôle local",
          "Désactivé pour ce produit.",
          log);

      return;
    }

    AddCheck(
        report,
        product,
        "INFO",
        "Contrôle local",
        "Contrôle local non implémenté dans le moteur natif.",
        log);
  }

  private static async Task CheckUpdateJsonAsync(
      ReleaseCheckReport report,
      ProductCatalog catalog,
      ProductInfo product,
      Action<string>? log,
      CancellationToken cancellationToken) {
    if (!product.HasUpdateJson) {
      AddCheck(
          report,
          product,
          "INFO",
          "update.json distant",
          "Non attendu pour ce produit.",
          log);

      return;
    }

    string url =
        product.GetUpdateJsonUrl(catalog.Site.SoftwaresUrl);

    try {
      string json =
          await GetRemoteTextAsync(
              url,
              cancellationToken);

      using JsonDocument document =
          JsonDocument.Parse(json);

      string? remoteVersion =
          TryGetStringProperty(
              document.RootElement,
              "version",
              "latestVersion",
              "displayVersion",
              "ApplicationDisplayVersion");

      string? artifactUrl =
          TryGetStringProperty(
              document.RootElement,
              "artifactUrl",
              "url",
              "downloadUrl",
              "msiUrl",
              "apkUrl",
              "setupUrl",
              "webInstallerUrl");

      List<string> details =
          [];

      details.Add("HTTP 200 ; JSON valide");

      bool hasWarning =
          false;

      if (string.IsNullOrWhiteSpace(remoteVersion)) {
        hasWarning = true;
        details.Add("version absente");
      }
      else if (!string.Equals(
          remoteVersion,
          product.Version,
          StringComparison.OrdinalIgnoreCase)) {
        hasWarning = true;
        details.Add("version attendue " + product.Version + " ; version distante " + remoteVersion);
      }
      else {
        details.Add("version OK");
      }

      if (string.IsNullOrWhiteSpace(artifactUrl)) {
        if (!json.Contains(product.ArtifactFile,StringComparison.OrdinalIgnoreCase)) {
          hasWarning = true;
          details.Add("URL artefact absente");
        }
        else {
          details.Add("artefact référencé");
        }
      }
      else if (!artifactUrl.Contains(product.ArtifactFile,StringComparison.OrdinalIgnoreCase)) {
        hasWarning = true;
        details.Add("URL artefact différente : " + artifactUrl);
      }
      else {
        details.Add("URL artefact OK");
      }

      AddCheck(
          report,
          product,
          hasWarning ? "WARN" : "OK",
          "update.json distant",
          string.Join(" ; ",details),
          log);
    }
    catch (Exception ex) {
      AddCheck(
          report,
          product,
          "FAIL",
          "update.json distant",
          ex.Message,
          log);
    }
  }

  private static async Task CheckArtifactUrlAsync(
      ReleaseCheckReport report,
      ProductCatalog catalog,
      ProductInfo product,
      Action<string>? log,
      CancellationToken cancellationToken) {
    string url =
        product.GetArtifactUrl(catalog.Site.SoftwaresUrl);

    WebCheckResult result =
        await CheckUrlAsync(
            url,
            cancellationToken);

    AddCheck(
        report,
        product,
        result.Success ? "OK" : "FAIL",
        "URL artefact",
        result.Details,
        log);
  }

  private static async Task CheckSha256UrlAsync(
      ReleaseCheckReport report,
      ProductCatalog catalog,
      ProductInfo product,
      Action<string>? log,
      CancellationToken cancellationToken) {
    if (!product.HasSha256) {
      AddCheck(
          report,
          product,
          "INFO",
          "SHA256 distant",
          "Non attendu pour ce produit.",
          log);

      return;
    }

    string url =
        product.GetSha256Url(catalog.Site.SoftwaresUrl);

    try {
      string text =
          await GetRemoteTextAsync(
              url,
              cancellationToken);

      Match match =
          Regex.Match(
              text,
              @"\b[a-fA-F0-9]{64}\b");

      if (!match.Success) {
        AddCheck(
            report,
            product,
            "FAIL",
            "SHA256 distant",
            "HTTP 200 ; aucun hash SHA256 valide trouvé",
            log);

        return;
      }

      AddCheck(
          report,
          product,
          "OK",
          "SHA256 distant",
          "HTTP 200 ; hash valide : " + match.Value,
          log);
    }
    catch (Exception ex) {
      AddCheck(
          report,
          product,
          "FAIL",
          "SHA256 distant",
          ex.Message,
          log);
    }
  }

  private static async Task CheckDownloadUrlAsync(
      ReleaseCheckReport report,
      ProductCatalog catalog,
      ProductInfo product,
      Action<string>? log,
      CancellationToken cancellationToken) {
    string url =
        product.GetDownloadUrl(catalog.Site.SoftwaresUrl);

    WebCheckResult result =
        await CheckUrlAsync(
            url,
            cancellationToken);

    AddCheck(
        report,
        product,
        result.Success ? "OK" : "FAIL",
        "download.php",
        result.Details,
        log);
  }

  private static async Task<WebCheckResult> CheckUrlAsync(
      string url,
      CancellationToken cancellationToken) {
    if (string.IsNullOrWhiteSpace(url)) {
      return new WebCheckResult {
        Success = false,
        Details = "URL vide"
      };
    }

    try {
      using HttpRequestMessage headRequest =
          CreateRequest(
              HttpMethod.Head,
              url);

      using HttpResponseMessage headResponse =
          await HttpClient.SendAsync(
              headRequest,
              HttpCompletionOption.ResponseHeadersRead,
              cancellationToken);

      if (headResponse.IsSuccessStatusCode) {
        return new WebCheckResult {
          Success = true,
          Details = "HTTP " + (int)headResponse.StatusCode + " - " + url
        };
      }

      if (headResponse.StatusCode != HttpStatusCode.MethodNotAllowed &&
          headResponse.StatusCode != HttpStatusCode.Forbidden &&
          headResponse.StatusCode != HttpStatusCode.NotImplemented) {
        return new WebCheckResult {
          Success = false,
          Details = "HTTP " + (int)headResponse.StatusCode + " - " + url
        };
      }
    }
    catch {
      // Certains serveurs ou scripts PHP refusent HEAD.
      // On tente donc un GET en lecture d'en-têtes uniquement.
    }

    try {
      using HttpRequestMessage getRequest =
          CreateRequest(
              HttpMethod.Get,
              url);

      getRequest.Headers.Range =
          new RangeHeaderValue(0,0);

      using HttpResponseMessage getResponse =
          await HttpClient.SendAsync(
              getRequest,
              HttpCompletionOption.ResponseHeadersRead,
              cancellationToken);

      return new WebCheckResult {
        Success = getResponse.IsSuccessStatusCode,
        Details = "HTTP " + (int)getResponse.StatusCode + " - " + url
      };
    }
    catch (Exception ex) {
      return new WebCheckResult {
        Success = false,
        Details = ex.Message + " - " + url
      };
    }
  }

  private static async Task<string> GetRemoteTextAsync(
      string url,
      CancellationToken cancellationToken) {
    using HttpRequestMessage request =
        CreateRequest(
            HttpMethod.Get,
            url);

    using HttpResponseMessage response =
        await HttpClient.SendAsync(
            request,
            HttpCompletionOption.ResponseHeadersRead,
            cancellationToken);

    string text =
        await response.Content.ReadAsStringAsync(
            cancellationToken);

    if (!response.IsSuccessStatusCode) {
      throw new InvalidOperationException(
          "HTTP " + (int)response.StatusCode + " - " + url);
    }

    return text;
  }

  private static HttpRequestMessage CreateRequest(
      HttpMethod method,
      string url) {
    HttpRequestMessage request =
        new(method,url);

    request.Headers.UserAgent.ParseAdd(
        "PB-BZH-ReleaseDashboard/1.0");

    return request;
  }

  private static string? TryGetStringProperty(
      JsonElement element,
      params string[] propertyNames) {
    if (element.ValueKind != JsonValueKind.Object)
      return null;

    foreach (JsonProperty property in element.EnumerateObject()) {
      foreach (string propertyName in propertyNames) {
        if (!string.Equals(
            property.Name,
            propertyName,
            StringComparison.OrdinalIgnoreCase)) {
          continue;
        }

        if (property.Value.ValueKind == JsonValueKind.String)
          return property.Value.GetString();

        if (property.Value.ValueKind == JsonValueKind.Number)
          return property.Value.ToString();

        return null;
      }
    }

    return null;
  }

  private static void AddCheck(
      ReleaseCheckReport report,
      ProductInfo product,
      string status,
      string label,
      string details,
      Action<string>? log) {
    AddCheck(
        report,
        product.DisplayName,
        product.Id,
        status,
        label,
        details,
        log);
  }

  private static void AddCheck(
      ReleaseCheckReport report,
      string productName,
      string productId,
      string status,
      string label,
      string details,
      Action<string>? log) {
    string normalizedStatus =
        status.ToUpperInvariant();

    ReleaseCheckRow row =
        new() {
          Time = DateTime.Now.ToString("HH:mm:ss"),
          Product = productName,
          ProductId = productId,
          Status = normalizedStatus,
          Label = label,
          Details = details
        };

    report.Checks.Add(row);

    switch (normalizedStatus) {
      case "OK":
        report.Summary.Ok++;
        break;

      case "INFO":
        report.Summary.Info++;
        break;

      case "WARN":
        report.Summary.Warnings++;
        break;

      case "FAIL":
      case "ERROR":
        report.Summary.Errors++;
        break;
    }

    log?.Invoke("[" + normalizedStatus + "] " + label);
    log?.Invoke("       " + details);
  }

  private sealed class WebCheckResult {
    public bool Success { get; set; }
    public string Details { get; set; } = string.Empty;
  }
}