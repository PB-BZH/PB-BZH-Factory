using System.Net;
using System.Text.Json;
using System.Text.RegularExpressions;
using PB.BZH.ReleaseDashboard.Core.Models;

namespace PB.BZH.ReleaseDashboard.Core.Services;

public sealed class RemoteProductCatalogBuilder {
  private static readonly HttpClient HttpClient = new();

  public async Task<RemoteProductCatalogBuildResult> BuildAsync(
      ProductCatalog currentCatalog,
      string category,
      CancellationToken cancellationToken = default) {
    if (currentCatalog == null)
      throw new ArgumentNullException(nameof(currentCatalog));

    if (string.IsNullOrWhiteSpace(category))
      throw new ArgumentException("Category is empty.",nameof(category));

    RemoteProductCatalogBuildResult result = new();

    string softwaresUrl =
        currentCatalog.Site.SoftwaresUrl.TrimEnd('/');

    string categoryUrl =
        softwaresUrl + "/" + category.Trim('/') + "/";

    result.Messages.Add("Catalogue URL : " + categoryUrl);

    string html =
        await HttpClient.GetStringAsync(
            categoryUrl,
            cancellationToken);

    List<string> productIds =
        ExtractProductIdsFromHtml(
            html,
            category,
            categoryUrl);

    result.Messages.Add("Products found : " + productIds.Count);

    ProductCatalog rebuiltCatalog = new() {
      Site = currentCatalog.Site,
      Products = []
    };

    foreach (string productId in productIds) {
      string updateJsonUrl =
          softwaresUrl +
          "/" +
          category.Trim('/') +
          "/" +
          productId +
          "/update.json";

      try {
        result.Messages.Add("Reading : " + updateJsonUrl);

        string json =
            await HttpClient.GetStringAsync(
                updateJsonUrl,
                cancellationToken);

        ProductInfo? product =
            BuildProductFromUpdateJson(
                productId,
                category,
                softwaresUrl,
                json);

        if (product == null) {
          result.Errors.Add("Unable to build product from : " + updateJsonUrl);
          continue;
        }

        rebuiltCatalog.Products.Add(product);

        result.Messages.Add(
            "[OK] " +
            product.DisplayName +
            " | " +
            product.Type +
            " | " +
            product.Version +
            " | " +
            product.ArtifactFile);
      }
      catch (Exception ex) {
        result.Errors.Add(
            "[ERROR] " +
            productId +
            " : " +
            ex.Message);
      }
    }

    result.Catalog = rebuiltCatalog;

    return result;
  }

  private static List<string> ExtractProductIdsFromHtml(
      string html,
      string category,
      string categoryUrl) {
    HashSet<string> ids =
        new(StringComparer.OrdinalIgnoreCase);

    string decodedHtml =
        WebUtility.HtmlDecode(html ?? string.Empty);

    string normalizedCategory =
        category.Trim('/');

    Uri baseUri =
        new(categoryUrl);

    // ==================================================
    // 1. Récupération de tous les href=""
    // ==================================================

    foreach (Match match in Regex.Matches(
        decodedHtml,
        @"href\s*=\s*[""'](?<href>[^""']+)[""']",
        RegexOptions.IgnoreCase)) {
      string href =
          match.Groups["href"].Value.Trim();

      if (string.IsNullOrWhiteSpace(href))
        continue;

      TryAddProductIdFromHref(
          ids,
          href,
          baseUri,
          normalizedCategory);
    }

    // ==================================================
    // 2. Fallback ancien format download.php?...product=
    // ==================================================

    foreach (Match match in Regex.Matches(
        decodedHtml,
        @"download\.php\?[^""'<>]*product=(?<product>[^&""'<>]+)",
        RegexOptions.IgnoreCase)) {
      string id =
          WebUtility.UrlDecode(
              match.Groups["product"].Value.Trim());

      if (IsValidProductId(id))
        ids.Add(id);
    }

    // ==================================================
    // 3. Fallback chemins visibles dans le HTML
    // ==================================================

    string escapedCategory =
        Regex.Escape(normalizedCategory);

    foreach (Match match in Regex.Matches(
        decodedHtml,
        escapedCategory + @"/(?<product>[^/""'<>]+)/",
        RegexOptions.IgnoreCase)) {
      string id =
          match.Groups["product"].Value.Trim();

      if (IsValidProductId(id))
        ids.Add(id);
    }

    return ids
        .OrderBy(id => id,StringComparer.OrdinalIgnoreCase)
        .ToList();
  }

  private static void TryAddProductIdFromHref(
    HashSet<string> ids,
    string href,
    Uri baseUri,
    string category) {
    if (string.IsNullOrWhiteSpace(href))
      return;

    if (href.StartsWith("#"))
      return;

    if (href.StartsWith("mailto:",StringComparison.OrdinalIgnoreCase))
      return;

    if (href.StartsWith("javascript:",StringComparison.OrdinalIgnoreCase))
      return;

    Uri uri;

    try {
      uri =
          new Uri(baseUri,href);
    }
    catch {
      return;
    }

    string query =
        uri.Query;

    // Cas :
    // download.php?category=msi-software-packager&product=LicenseGenerator
    if (!string.IsNullOrWhiteSpace(query)) {
      Dictionary<string,string> queryValues =
          ParseQueryString(query);

      if (queryValues.TryGetValue("product",out string? productFromQuery)) {
        if (IsValidProductId(productFromQuery))
          ids.Add(productFromQuery);

        return;
      }
    }

    string absolutePath =
        uri.AbsolutePath.Trim('/');

    if (string.IsNullOrWhiteSpace(absolutePath))
      return;

    string[] segments =
        absolutePath
            .Split('/',StringSplitOptions.RemoveEmptyEntries);

    if (segments.Length == 0)
      return;

    int categoryIndex =
        Array.FindIndex(
            segments,
            segment => segment.Equals(
                category,
                StringComparison.OrdinalIgnoreCase));

    // Cas :
    // /softwares/msi-software-packager/ProductId/
    // /softwares/msi-software-packager/ProductId/File.msi
    if (categoryIndex >= 0 &&
        categoryIndex + 1 < segments.Length) {
      string productId =
          segments[categoryIndex + 1];

      if (IsValidProductId(productId))
        ids.Add(productId);

      return;
    }

    // Cas relatif :
    // ProductId/
    // ProductId/update.json
    // ProductId/ProductId.msi
    if (segments.Length >= 1) {
      string firstSegment =
          segments[0];

      if (IsValidProductId(firstSegment))
        ids.Add(firstSegment);
    }
  }

  private static Dictionary<string,string> ParseQueryString(string query) {
    Dictionary<string,string> result =
        new(StringComparer.OrdinalIgnoreCase);

    string cleanQuery =
        query.TrimStart('?');

    foreach (string part in cleanQuery.Split('&',StringSplitOptions.RemoveEmptyEntries)) {
      string[] pieces =
          part.Split('=',2);

      if (pieces.Length != 2)
        continue;

      string key =
          WebUtility.UrlDecode(pieces[0]);

      string value =
          WebUtility.UrlDecode(pieces[1]);

      if (!string.IsNullOrWhiteSpace(key)) {
        result[key] = value;
      }
    }

    return result;
  }

  private static bool IsValidProductId(string id) {
    if (string.IsNullOrWhiteSpace(id))
      return false;

    string value =
        id.Trim();

    if (value.Equals("index.php",StringComparison.OrdinalIgnoreCase))
      return false;

    if (value.Equals("download.php",StringComparison.OrdinalIgnoreCase))
      return false;

    if (value.Equals("assets",StringComparison.OrdinalIgnoreCase))
      return false;

    if (value.Equals("softwares",StringComparison.OrdinalIgnoreCase))
      return false;

    if (value.Equals("msi-software-packager",StringComparison.OrdinalIgnoreCase))
      return false;

    if (value.Contains('.'))
      return false;

    if (value.Contains('?'))
      return false;

    if (value.Contains('#'))
      return false;

    return Regex.IsMatch(
        value,
        @"^[A-Za-z0-9][A-Za-z0-9._-]*$");
  }

  private static ProductInfo? BuildProductFromUpdateJson(
      string productId,
      string category,
      string softwaresUrl,
      string json) {
    using JsonDocument document =
        JsonDocument.Parse(json);

    JsonElement root =
        document.RootElement;

    string version =
        GetStringProperty(
            root,
            "version",
            "Version",
            "latestVersion",
            "LatestVersion");

    string displayName =
        GetStringProperty(
            root,
            "productName",
            "ProductName",
            "name",
            "Name",
            "applicationName",
            "ApplicationName");

    if (string.IsNullOrWhiteSpace(displayName)) {
      displayName = productId;
    }

    string artifactUrl =
        GetBestArtifactUrl(root);

    string artifactFile =
        GetArtifactFileName(artifactUrl);

    if (string.IsNullOrWhiteSpace(artifactFile)) {
      artifactFile =
          GetStringProperty(
              root,
              "fileName",
              "FileName",
              "artifactFile",
              "ArtifactFile");
    }

    string type =
        GetStringProperty(
            root,
            "type",
            "Type");

    if (string.IsNullOrWhiteSpace(type)) {
      type =
          GuessTypeFromArtifact(
              artifactFile,
              artifactUrl,
              root);
    }

    if (string.IsNullOrWhiteSpace(type)) {
      type = "unknown";
    }

    int? androidVersionCode =
        GetIntProperty(
            root,
            "androidVersionCode",
            "AndroidVersionCode",
            "versionCode",
            "VersionCode",
            "applicationVersionCode",
            "ApplicationVersionCode");

    return new ProductInfo {
      Id = productId,
      DisplayName = displayName,
      Type = type.ToLowerInvariant(),
      Category = category,
      Version = version,
      AndroidVersionCode = androidVersionCode,
      ArtifactFile = artifactFile,
      HasSha256 = true,
      HasUpdateJson = true,
      LocalCheck = "disabled"
    };
  }

  private static string GetBestArtifactUrl(JsonElement root) {
    string[] names = [
        "artifactUrl",
        "ArtifactUrl",
        "apkUrl",
        "ApkUrl",
        "msiUrl",
        "MsiUrl",
        "url",
        "Url",
        "downloadUrl",
        "DownloadUrl"
    ];

    List<string> candidates = [];

    foreach (string name in names) {
      string value =
          GetStringProperty(root,name);

      if (!string.IsNullOrWhiteSpace(value)) {
        candidates.Add(value);
      }
    }

    string? artifactCandidate =
        candidates.FirstOrDefault(LooksLikeArtifactUrl);

    if (!string.IsNullOrWhiteSpace(artifactCandidate))
      return artifactCandidate;

    return candidates.FirstOrDefault() ?? string.Empty;
  }

  private static bool LooksLikeArtifactUrl(string value) {
    if (string.IsNullOrWhiteSpace(value))
      return false;

    string path =
        value.Split('?')[0];

    return path.EndsWith(".apk",StringComparison.OrdinalIgnoreCase) ||
           path.EndsWith(".msi",StringComparison.OrdinalIgnoreCase) ||
           path.EndsWith(".exe",StringComparison.OrdinalIgnoreCase);
  }

  private static string GetArtifactFileName(string artifactUrl) {
    if (string.IsNullOrWhiteSpace(artifactUrl))
      return string.Empty;

    try {
      Uri uri =
          new(artifactUrl);

      return Path.GetFileName(uri.AbsolutePath);
    }
    catch {
      string clean =
          artifactUrl.Split('?')[0];

      return Path.GetFileName(clean);
    }
  }

  private static string GuessTypeFromArtifact(
      string artifactFile,
      string artifactUrl,
      JsonElement root) {
    string combined =
        (artifactFile + " " + artifactUrl).ToLowerInvariant();

    if (combined.Contains(".apk"))
      return "apk";

    if (combined.Contains(".msi"))
      return "msi";

    string targetFramework =
        GetStringProperty(
            root,
            "targetFramework",
            "TargetFramework");

    if (targetFramework.Contains(
        "android",
        StringComparison.OrdinalIgnoreCase)) {
      return "apk";
    }

    return string.Empty;
  }

  private static string GetStringProperty(
      JsonElement root,
      params string[] names) {
    foreach (JsonProperty property in root.EnumerateObject()) {
      foreach (string name in names) {
        if (!property.Name.Equals(
            name,
            StringComparison.OrdinalIgnoreCase)) {
          continue;
        }

        return property.Value.ValueKind switch {
          JsonValueKind.String => property.Value.GetString() ?? string.Empty,
          JsonValueKind.Number => property.Value.GetRawText(),
          JsonValueKind.True => "true",
          JsonValueKind.False => "false",
          _ => string.Empty
        };
      }
    }

    return string.Empty;
  }

  private static int? GetIntProperty(
      JsonElement root,
      params string[] names) {
    string value =
        GetStringProperty(root,names);

    if (int.TryParse(value,out int result))
      return result;

    return null;
  }
}

public sealed class RemoteProductCatalogBuildResult {
  public ProductCatalog Catalog { get; set; } = new();
  public List<string> Messages { get; set; } = [];
  public List<string> Errors { get; set; } = [];
}