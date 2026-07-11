using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Text.Json;
using PB.BZH.Help.Library.UI.Theming;
using PB.BZH.ReleaseDashboard.Core.Models;
using PB.BZH.ReleaseDashboard.Core.Services;
using PB.BZH.ReleaseDashboard.UI.Console;
using PB.BZH.ReleaseDashboard.UI.Details;
using PB.BZH.ReleaseDashboard.UI.Grids;
using PB.BZH.ReleaseDashboard.UI.Summary;

namespace PB.BZH.ReleaseDashboard.UI;

public partial class MainForm: Form {
  private readonly ProductCatalogService _catalogService = new();
  private readonly ReleaseReportService _reportService = new();
  private readonly RemoteSha256Verifier _sha256Verifier = new();

  private string _factoryRoot = string.Empty;
  private ProductCatalog? _catalog;
  private BindingList<ProductGridRow> _rows = [];

  public MainForm() {
    InitializeComponent();
    ConsoleContextMenuConfigurator.Configure(txtConsole);
    ThemeManager.ApplyDarkTitleBar(this);
    ThemeManager.ApplyDarkDialogBorder(this,tlpMain);
    ThemeManager.StyleDarkButtons(this);
    ThemeManager.ApplyDarkTheme(this);
    ProductGridViewConfigurator.Configure(dgvProducts);
    ProductGridViewConfigurator.ApplyLightGridTheme(dgvProducts);
    ReleaseSummaryPresenter.Clear(
      lblLastCheck,
      lblSummaryOk,
      lblSummaryInfo,
      lblSummaryWarnings,
      lblSummaryErrors);
    ProductGridViewConfigurator.Configure(dgvProducts);
    lblRoot.ForeColor = Color.WhiteSmoke;
    lblLastCheck.ForeColor = Color.WhiteSmoke;
    lblSummaryOk.ForeColor = Color.LightGreen;
    lblSummaryInfo.ForeColor = Color.DeepSkyBlue;
    lblSummaryWarnings.ForeColor = Color.Orange;
    lblSummaryErrors.ForeColor = Color.OrangeRed;
    ProductDetailsPresenter.Clear(
    lblDetailProduct,
    lblDetailType,
    lblDetailVersion,
    lblDetailStatus,
    txtDetailDownloadUrl,
    txtDetailArtifactUrl,
    txtDetailSha256Url,
    txtDetailUpdateJsonUrl);

    LoadCatalog();
  }

  private void LoadCatalog() {
    try {
      _factoryRoot = FindFactoryRoot();

      string productsFile =
          Path.Combine(_factoryRoot,"products","products.json");

      _catalog =
          _catalogService.Load(productsFile);

      _rows =
          new BindingList<ProductGridRow>(
              _catalog.Products
                  .Select(product => ProductGridRow.FromProduct(
                      product,
                      _catalog.Site.SoftwaresUrl))
                  .ToList()
          );

      dgvProducts.DataSource = _rows;
      UpdateProductDetails();

      lblRoot.Text = "Root : " + _factoryRoot;

      AppendConsole("[OK] products.json loaded.");
      AppendConsole(productsFile);
      LoadLatestReportStatus();
    }
    catch (Exception ex) {
      AppendConsole("[ERROR] " + ex.Message);

      MessageBox.Show(
          ex.Message,
          "PB BZH Release Dashboard",
          MessageBoxButtons.OK,
          MessageBoxIcon.Error);
    }
  }

  private static string FindFactoryRoot() {
    DirectoryInfo? directory =
        new(AppContext.BaseDirectory);

    while (directory != null) {
      string productsFile =
          Path.Combine(directory.FullName,"products","products.json");

      string releaseScript =
          Path.Combine(directory.FullName,"release","check-release.ps1");

      if (File.Exists(productsFile) && File.Exists(releaseScript)) {
        return directory.FullName;
      }

      directory = directory.Parent;
    }

    throw new DirectoryNotFoundException(
        "Unable to find PB-BZH-Factory root from: " +
        AppContext.BaseDirectory);
  }

  private async void btnRunReleaseCheck_Click(object sender,EventArgs e) {
    await RunScriptAsync(
        "release\\check-release.ps1",
        "-Report");

    LoadLatestReportStatus();
  }

  private async void btnOpenLastReport_Click(object sender,EventArgs e) {
    await RunScriptAsync(
        "release\\show-last-report.ps1",
        string.Empty);
  }

  private void btnOpenReportsFolder_Click(object sender,EventArgs e) {
    string reportsFolder =
        Path.Combine(_factoryRoot,"reports");

    Directory.CreateDirectory(reportsFolder);

    Process.Start(new ProcessStartInfo {
      FileName = reportsFolder,
      UseShellExecute = true
    });
  }

  private void btnOpenDownloadUrl_Click(object sender,EventArgs e) {
    ProductGridRow? row = GetSelectedRow();

    if (row == null)
      return;

    OpenUrl(row.DownloadUrl);
  }

  private void btnOpenArtifactUrl_Click(object sender,EventArgs e) {
    ProductGridRow? row = GetSelectedRow();

    if (row == null)
      return;

    OpenUrl(row.ArtifactUrl);
  }

  private async Task RunScriptAsync(
      string relativeScriptPath,
      string arguments) {
    try {
      string scriptPath =
          Path.Combine(_factoryRoot,relativeScriptPath);

      if (!File.Exists(scriptPath)) {
        throw new FileNotFoundException(
            "Script not found.",
            scriptPath);
      }

      AppendConsole("");
      AppendConsole("==================================================");
      AppendConsole("Running : " + scriptPath);
      AppendConsole("==================================================");

      SetButtonsEnabled(false);

      string psArguments =
          "-NoLogo -NoProfile -NonInteractive " +
          "-ExecutionPolicy Bypass " +
          "-File \"" +
          scriptPath +
          "\"";

      if (!string.IsNullOrWhiteSpace(arguments)) {
        psArguments += " " + arguments;
      }

      ProcessStartInfo psi = new() {
        FileName = "pwsh",
        Arguments = psArguments,
        WorkingDirectory = _factoryRoot,
        UseShellExecute = false,
        RedirectStandardOutput = true,
        RedirectStandardError = true,
        CreateNoWindow = true,
        StandardOutputEncoding = Encoding.UTF8,
        StandardErrorEncoding = Encoding.UTF8
      };

      using Process process = new() {
        StartInfo = psi,
        EnableRaisingEvents = true
      };

      process.OutputDataReceived += (_,e) => {
        if (!string.IsNullOrWhiteSpace(e.Data)) {
          BeginInvoke(() => AppendConsole(e.Data));
        }
      };

      process.ErrorDataReceived += (_,e) => {
        if (!string.IsNullOrWhiteSpace(e.Data)) {
          BeginInvoke(() => AppendConsole("[ERROR] " + e.Data));
        }
      };

      process.Start();
      process.BeginOutputReadLine();
      process.BeginErrorReadLine();

      await process.WaitForExitAsync();

      AppendConsole("");
      AppendConsole("Exit code : " + process.ExitCode);
    }
    catch (Exception ex) {
      AppendConsole("[ERROR] " + ex.Message);

      MessageBox.Show(
          ex.Message,
          "PB BZH Release Dashboard",
          MessageBoxButtons.OK,
          MessageBoxIcon.Error);
    }
    finally {
      SetButtonsEnabled(true);
    }
  }

  private void LoadLatestReportStatus() {
    try {
      if (_catalog == null || _rows.Count == 0)
        return;

      ReleaseCheckReport? report =
          _reportService.LoadLatestReport(_factoryRoot);

      if (report == null) {
        foreach (ProductGridRow row in _rows) {
          row.Status = "UNKNOWN";
          row.LastCheck = string.Empty;
        }

        ReleaseSummaryPresenter.Clear(
          lblLastCheck,
          lblSummaryOk,
          lblSummaryInfo,
          lblSummaryWarnings,
          lblSummaryErrors
        );

        dgvProducts.Refresh();
        ApplyGridRowColors();
        AppendConsole("[INFO] No release report found yet.");
        return;
      }

      foreach (ProductGridRow row in _rows) {
        bool hasChecks =
            _reportService.HasProductChecks(
                report,
                row.Id,
                row.DisplayName);

        if (!hasChecks) {
          row.Status = "NOT CHECKED";
          row.LastCheck = string.Empty;
          continue;
        }

        row.Status =
            _reportService.GetProductStatus(
                report,
                row.Id,
                row.DisplayName);

        row.LastCheck =
            report.GeneratedAtLocal;
      }

      ReleaseSummaryPresenter.Apply(
        report,
        lblLastCheck,
        lblSummaryOk,
        lblSummaryInfo,
        lblSummaryWarnings,
        lblSummaryErrors
      );

      dgvProducts.Refresh();
      ApplyGridRowColors();
      UpdateProductDetails();

      AppendConsole("[OK] Latest release report loaded : " + report.GeneratedAtLocal);
    }
    catch (Exception ex) {
      AppendConsole("[ERROR] Unable to load latest report : " + ex.Message);
    }
  }

  private void ApplyGridRowColors() {
    ProductGridViewConfigurator.ApplyStatusColors(dgvProducts);
  }

  private void SetButtonsEnabled(bool enabled) {
    btnRunReleaseCheck.Enabled = enabled;
    btnOpenLastReport.Enabled = enabled;
    btnOpenReportsFolder.Enabled = enabled;
    btnOpenDownloadUrl.Enabled = enabled;
    btnOpenArtifactUrl.Enabled = enabled;
    btnOpenUpdateJsonUrl.Enabled = enabled;
    btnViewSha256Url.Enabled = enabled;
    btnVerifySha256.Enabled = enabled;
    btnViewProductsJson.Enabled = enabled;
    btnEditProductsJson.Enabled = enabled;
    btnReloadCatalog.Enabled = enabled;
    btnViewProductChecks.Enabled = enabled;
  }

  private ProductGridRow? GetSelectedRow() {
    if (dgvProducts.CurrentRow?.DataBoundItem is ProductGridRow row) {
      return row;
    }

    return null;
  }

  private static void OpenUrl(string url) {
    if (string.IsNullOrWhiteSpace(url))
      return;

    Process.Start(new ProcessStartInfo {
      FileName = url,
      UseShellExecute = true
    });
  }

  private void AppendConsole(string message) {
    ConsoleLogHighlighter.AppendLine(
        txtConsole,
        message);
  }

  private void dgvProducts_SelectionChanged(object? sender,EventArgs e) {
    UpdateProductDetails();
  }

  private void UpdateProductDetails() {
    ProductGridRow? row = GetSelectedRow();

    if (row == null) {
      ProductDetailsPresenter.Clear(
          lblDetailProduct,
          lblDetailType,
          lblDetailVersion,
          lblDetailStatus,
          txtDetailDownloadUrl,
          txtDetailArtifactUrl,
          txtDetailSha256Url,
          txtDetailUpdateJsonUrl);
      return;
    }

    ProductDetailsPresenter.Apply(
        row.DisplayName,
        row.Type,
        row.Version,
        row.Status,
        row.DownloadUrl,
        row.ArtifactUrl,
        row.Sha256Url,
        row.UpdateJsonUrl,
        lblDetailProduct,
        lblDetailType,
        lblDetailVersion,
        lblDetailStatus,
        txtDetailDownloadUrl,
        txtDetailArtifactUrl,
        txtDetailSha256Url,
        txtDetailUpdateJsonUrl);
  }

  private sealed class ProductGridRow {
    public string Id { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string Version { get; set; } = string.Empty;
    public string Status { get; set; } = "UNKNOWN";
    public string LastCheck { get; set; } = string.Empty;
    public string ArtifactFile { get; set; } = string.Empty;
    public string LocalCheck { get; set; } = string.Empty;
    public string DownloadUrl { get; set; } = string.Empty;
    public string ArtifactUrl { get; set; } = string.Empty;
    public string UpdateJsonUrl { get; set; } = string.Empty;
    public string Sha256Url { get; set; } = string.Empty;

    public static ProductGridRow FromProduct(ProductInfo product,string softwaresUrl) {
      return new ProductGridRow {
        Id = product.Id,
        DisplayName = product.DisplayName,
        Type = product.Type.ToUpperInvariant(),
        Version = product.Version,
        Status = "UNKNOWN",
        LastCheck = string.Empty,
        ArtifactFile = product.ArtifactFile,
        LocalCheck = product.LocalCheck,
        DownloadUrl = product.GetDownloadUrl(softwaresUrl),
        ArtifactUrl = product.GetArtifactUrl(softwaresUrl),
        UpdateJsonUrl = product.GetUpdateJsonUrl(softwaresUrl),
        Sha256Url = product.GetSha256Url(softwaresUrl),
      };
    }
  }

  private string GetProductsJsonPath() {
    if (string.IsNullOrWhiteSpace(_factoryRoot)) {
      throw new InvalidOperationException("Factory root is not defined.");
    }

    return Path.Combine(
        _factoryRoot,
        "products",
        "products.json");
  }

  private void btnViewProductsJson_Click(object sender,EventArgs e) {
    try {
      string productsFile =
          GetProductsJsonPath();

      if (!File.Exists(productsFile)) {
        AppendConsole("[ERROR] products.json not found : " + productsFile);
        return;
      }

      string json =
          File.ReadAllText(productsFile,Encoding.UTF8);

      string formattedJson =
          FormatJson(json);

      AppendConsole("");
      AppendConsole("==================================================");
      AppendConsole("products.json");
      AppendConsole("==================================================");
      AppendConsole(productsFile);
      AppendConsole("");
      AppendConsole(formattedJson);
    }
    catch (Exception ex) {
      AppendConsole("[ERROR] Unable to display products.json : " + ex.Message);
    }
  }

  private static void OpenFileInEditor(string filePath) {
    if (string.IsNullOrWhiteSpace(filePath))
      return;

    string editorPath =
        ResolveNotepadPlusPlusPath();

    Process.Start(new ProcessStartInfo {
      FileName = editorPath,
      Arguments = "\"" + filePath + "\"",
      UseShellExecute = false
    });
  }

  private static string ResolveNotepadPlusPlusPath() {
    string programFiles =
        Environment.GetFolderPath(
            Environment.SpecialFolder.ProgramFiles);

    string programFilesX86 =
        Environment.GetFolderPath(
            Environment.SpecialFolder.ProgramFilesX86);

    string[] candidates = [
        Path.Combine(programFiles,"Notepad++","notepad++.exe"),
      Path.Combine(programFilesX86,"Notepad++","notepad++.exe")
    ];

    foreach (string candidate in candidates) {
      if (File.Exists(candidate))
        return candidate;
    }

    string? pathEnvironment =
        Environment.GetEnvironmentVariable("PATH");

    if (!string.IsNullOrWhiteSpace(pathEnvironment)) {
      foreach (string directory in pathEnvironment.Split(Path.PathSeparator)) {
        if (string.IsNullOrWhiteSpace(directory))
          continue;

        string candidate =
            Path.Combine(directory.Trim(),"notepad++.exe");

        if (File.Exists(candidate))
          return candidate;
      }
    }

    return "notepad.exe";
  }

  private void btnEditProductsJson_Click(object sender,EventArgs e) {
    try {
      string productsFile =
          GetProductsJsonPath();

      if (!File.Exists(productsFile)) {
        AppendConsole("[ERROR] products.json not found : " + productsFile);
        return;
      }

      OpenFileInEditor(productsFile);
    }
    catch (Exception ex) {
      AppendConsole("[ERROR] Unable to edit products.json : " + ex.Message);
    }
  }

  private void btnReloadCatalog_Click(object sender,EventArgs e) {
    try {
      LoadCatalog();
      LoadLatestReportStatus();
      UpdateProductDetails();

      AppendConsole("[OK] products.json reloaded.");
    }
    catch (Exception ex) {
      AppendConsole("[ERROR] Unable to reload products.json : " + ex.Message);
    }
  }

  private async Task ShowRemoteTextInConsoleAsync(
    string url,
    string title) {
    if (string.IsNullOrWhiteSpace(url)) {
      AppendConsole("[WARN] Remote text URL is empty.");
      return;
    }

    try {
      AppendConsole("");
      AppendConsole("==================================================");
      AppendConsole(title);
      AppendConsole("==================================================");
      AppendConsole(url);
      AppendConsole("");

      using HttpClient client = new();

      string content =
          await client.GetStringAsync(url);

      AppendConsole(content.Trim());
    }
    catch (Exception ex) {
      AppendConsole("[ERROR] Unable to read remote text file : " + ex.Message);
      AppendConsole("URL : " + url);
    }
  }

  private async Task ShowRemoteJsonInConsoleAsync(
    string url,
    string title) {
    if (string.IsNullOrWhiteSpace(url)) {
      AppendConsole("[WARN] JSON URL is empty.");
      return;
    }

    try {
      AppendConsole("");
      AppendConsole("==================================================");
      AppendConsole(title);
      AppendConsole("==================================================");
      AppendConsole(url);

      using HttpClient client = new();

      string jsonText =
          await client.GetStringAsync(url);

      string formattedJson =
          FormatJson(jsonText);

      AppendConsole("");
      AppendConsole(formattedJson);
    }
    catch (Exception ex) {
      AppendConsole("[ERROR] Unable to read remote JSON : " + ex.Message);
      AppendConsole("URL : " + url);
    }
  }

  private static string FormatJson(string jsonText) {
    if (string.IsNullOrWhiteSpace(jsonText))
      return string.Empty;

    try {
      using JsonDocument document =
          JsonDocument.Parse(jsonText);

      return JsonSerializer.Serialize(
          document.RootElement,
          new JsonSerializerOptions {
            WriteIndented = true
          });
    }
    catch {
      return jsonText;
    }
  }

  private void ShowSelectedProductChecks() {
    ProductGridRow? row =
        GetSelectedRow();

    if (row == null) {
      AppendConsole("[WARN] No product selected.");
      return;
    }

    try {
      ReleaseCheckReport? report =
          _reportService.LoadLatestReport(_factoryRoot);

      if (report == null) {
        AppendConsole("[WARN] No release report found.");
        return;
      }

      List<ReleaseCheckRow> checks =
          _reportService.GetProductChecks(
              report,
              row.Id,
              row.DisplayName);

      AppendConsole("");
      AppendConsole("==================================================");
      AppendConsole("Checks - " + row.DisplayName);
      AppendConsole("==================================================");
      AppendConsole("Last report : " + report.GeneratedAtLocal);
      AppendConsole("");

      if (checks.Count == 0) {
        AppendConsole("[INFO] No checks found for this product in the latest report.");
        AppendConsole("[INFO] Run release check to update the report.");
        return;
      }

      int okCount =
          checks.Count(check => IsStatus(check.Status,"OK"));

      int infoCount =
          checks.Count(check => IsStatus(check.Status,"INFO"));

      int warnCount =
          checks.Count(check => IsStatus(check.Status,"WARN"));

      int failCount =
          checks.Count(check => IsStatus(check.Status,"FAIL"));

      AppendConsole("Product summary");
      AppendConsole("OK       : " + okCount);
      AppendConsole("Infos    : " + infoCount);
      AppendConsole("Warnings : " + warnCount);
      AppendConsole("Errors   : " + failCount);
      AppendConsole("");

      foreach (ReleaseCheckRow check in checks) {
        AppendConsole(
            "[" +
            NormalizeReportStatus(check.Status) +
            "] " +
            check.Label);

        if (!string.IsNullOrWhiteSpace(check.Details)) {
          AppendConsole("       " + check.Details);
        }
      }
    }
    catch (Exception ex) {
      AppendConsole("[ERROR] Unable to show product checks : " + ex.Message);
    }
  }

  private static bool IsStatus(
    string value,
    string expected) {
    return string.Equals(
        value,
        expected,
        StringComparison.OrdinalIgnoreCase);
  }

  private static string NormalizeReportStatus(string status) {
    if (string.IsNullOrWhiteSpace(status))
      return "INFO";

    return status.Trim().ToUpperInvariant() switch {
      "OK" => "OK",
      "INFO" => "INFO",
      "WARN" => "WARN",
      "WARNING" => "WARN",
      "FAIL" => "FAIL",
      "ERROR" => "FAIL",
      _ => status.Trim().ToUpperInvariant()
    };
  }

  private async Task VerifySelectedProductSha256Async(ProductGridRow row) {
    try {
      SetButtonsEnabled(false);

      AppendConsole("");
      AppendConsole("==================================================");
      AppendConsole("Verify SHA256 - " + row.DisplayName);
      AppendConsole("==================================================");
      AppendConsole("Artifact URL : " + row.ArtifactUrl);
      AppendConsole("SHA256 URL   : " + row.Sha256Url);
      AppendConsole("");
      AppendConsole("[INFO] Downloading SHA256 file and remote artifact...");
      AppendConsole("[INFO] This may take a while for large MSI/APK files.");

      RemoteSha256VerificationResult result =
          await _sha256Verifier.VerifyAsync(
              row.ArtifactUrl,
              row.Sha256Url);

      AppendConsole("");
      AppendConsole("Expected SHA256 : " + result.ExpectedHash);
      AppendConsole("Actual SHA256   : " + result.ActualHash);

      if (result.Success) {
        AppendConsole("[OK] SHA256 verification succeeded for " + row.DisplayName);
      }
      else {
        AppendConsole("[FAIL] SHA256 verification failed for " + row.DisplayName);
        AppendConsole("[ERROR] " + result.Message);
      }
    }
    catch (Exception ex) {
      AppendConsole("[ERROR] Unable to verify SHA256 : " + ex.Message);
    }
    finally {
      SetButtonsEnabled(true);
    }
  }

  private async void btnOpenUpdateJsonUrl_Click(object sender,EventArgs e) {
    ProductGridRow? row = GetSelectedRow();

    if (row == null)
      return;

    await ShowRemoteJsonInConsoleAsync(
        row.UpdateJsonUrl,
        "update.json - " + row.DisplayName);
  }

  private void txtConsole_LinkClicked(object sender,LinkClickedEventArgs e) {
    if (string.IsNullOrWhiteSpace(e.LinkText))
      return;

    Process.Start(new ProcessStartInfo {
      FileName = e.LinkText,
      UseShellExecute = true
    });
  }

  private void detailUrl_LinkClicked(object sender,LinkClickedEventArgs e) {
    if (string.IsNullOrWhiteSpace(e.LinkText))
      return;

    OpenUrl(e.LinkText);
  }

  private async void btnViewSha256Url_Click(object sender,EventArgs e) {
    ProductGridRow? row = GetSelectedRow();

    if (row == null)
      return;

    await ShowRemoteTextInConsoleAsync(
        row.Sha256Url,
        "SHA256 - " + row.DisplayName);
  }

  private async void btnVerifySha256_Click(object sender,EventArgs e) {
    ProductGridRow? row = GetSelectedRow();

    if (row == null)
      return;

    await VerifySelectedProductSha256Async(row);
  }

  private void btnViewProductChecks_Click(object sender,EventArgs e) {
    ShowSelectedProductChecks();
  }
}