using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Text.Encodings.Web;
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
  private readonly RemoteProductCatalogBuilder _remoteCatalogBuilder = new();
  private string _lastRebuiltProductsJsonPath = string.Empty;
  private readonly DashboardSettingsService _settingsService = new();
  private readonly WorkspaceService _workspaceService = new();
  private readonly ReleaseCheckService _releaseCheckService = new();
  private readonly ReleaseReportWriter _releaseReportWriter = new();

  private DashboardSettings _settings = new();
  private WorkspaceSettings _workspace = new();
  private ReleaseCheckReport? _latestReport;

  private ProductCatalog? _catalog;
  private BindingList<ProductGridRow> _rows = [];

  public MainForm() {
    InitializeComponent();

    InitializeWorkspace();

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

  private SiteInfo GetSiteInfo() {
    if (_catalog != null &&
        !string.IsNullOrWhiteSpace(_catalog.Site.SoftwaresUrl)) {
      return _catalog.Site;
    }

    if (_settings.Site == null) {
      _settings.Site =
          new SiteInfo {
            MainUrl = "https://www.pb-bzh-concept.fr",
            SoftwaresUrl = "https://www.pb-bzh-concept.fr/softwares",
            LocalSoftwaresRoot = string.Empty
          };
    }

    return _settings.Site;
  }

  private void RefreshDashboardView() {
    LoadLatestReportStatus();
    UpdateProductDetails();
  }

  private void InitializeWorkspace() {
    _settings =
        _settingsService.Load();

    _workspace =
        _workspaceService.Resolve(_settings.Workspace);

    _settings.Workspace =
        _workspace;

    _settingsService.Save(_settings);

    UpdateWorkspaceLabel();
  }

  private string GetProductsFolderPath() {
    return _workspace.ProductsFolder;
  }

  private string GetReportsFolderPath() {
    return _workspace.ReportsFolder;
  }

  private string GetProductsJsonPath() {
    return Path.Combine(
        GetProductsFolderPath(),
        "products.json");
  }

  private void UpdateWorkspaceLabel() {
    if (lblRoot == null)
      return;

    lblRoot.Text =
        "Products : " + _workspace.ProductsFolder +
        "    Reports : " + _workspace.ReportsFolder;
  }

  private void LoadCatalog() {
    try {
      string productsFile =
          GetProductsJsonPath();

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

      UpdateWorkspaceLabel();

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

  private async void btnRunReleaseCheck_Click(object sender,EventArgs e) {
    await RunReleaseCheckActionAsync();
  }

  private async void mnuRunReleaseCheck_Click(object sender,EventArgs e) {
    await RunReleaseCheckActionAsync();
  }

  private async Task RunReleaseCheckActionAsync() {
    if (_catalog == null) {
      AppendConsole("[ERROR] products catalog is not loaded.");
      return;
    }

    try {
      SetButtonsEnabled(false);

      AppendConsole("");
      AppendConsole("==================================================");
      AppendConsole("Run native release check");
      AppendConsole("==================================================");

      ReleaseCheckReport report =
          await _releaseCheckService.RunAsync(
              _catalog,
              AppendConsole);

      (string markdownPath,string jsonPath) =
          _releaseReportWriter.Save(
              GetReportsFolderPath(),
              report);

      AppendConsole("");
      AppendConsole("[OK] Markdown report generated : " + markdownPath);
      AppendConsole("[OK] JSON report generated     : " + jsonPath);

      RefreshDashboardView();
    }
    catch (Exception ex) {
      AppendConsole("[ERROR] Native release check failed : " + ex.Message);
    }
    finally {
      SetButtonsEnabled(true);
    }
  }

  private void LoadLatestReportStatus() {
    _latestReport =
        _reportService.LoadLatestReport(
            GetReportsFolderPath());

    if (_latestReport == null) {
      foreach (ProductGridRow row in _rows) {
        row.Status = "NOT CHECKED";
        row.LastCheck = string.Empty;
      }

      ClearSummaryLabels();

      _rows.ResetBindings();
      dgvProducts.Refresh();

      return;
    }

    ApplySummaryLabels(_latestReport);

    foreach (ProductGridRow row in _rows) {
      bool hasChecks =
          _reportService.HasProductChecks(
              _latestReport,
              row.Id,
              row.DisplayName);

      if (!hasChecks) {
        row.Status = "NOT CHECKED";
        row.LastCheck = string.Empty;
        continue;
      }

      row.Status =
          _reportService.GetProductStatus(
              _latestReport,
              row.Id,
              row.DisplayName);

      row.LastCheck =
          _latestReport.GeneratedAtLocal;
    }

    _rows.ResetBindings();
    dgvProducts.Refresh();
  }

  private void ApplyGridRowColors() {
    ProductGridViewConfigurator.ApplyStatusColors(dgvProducts);
  }

  private void ClearSummaryLabels() {
    lblLastCheck.Text = "Last check : -";
    lblSummaryOk.Text = "OK : 0";
    lblSummaryInfo.Text = "Infos : 0";
    lblSummaryWarnings.Text = "Warnings : 0";
    lblSummaryErrors.Text = "Errors : 0";

    lblSummaryOk.ForeColor = Color.LightGray;
    lblSummaryInfo.ForeColor = Color.LightGray;
    lblSummaryWarnings.ForeColor = Color.LightGray;
    lblSummaryErrors.ForeColor = Color.LightGray;
  }

  private void ApplySummaryLabels(ReleaseCheckReport report) {
    lblLastCheck.Text =
        "Last check : " + report.GeneratedAtLocal;

    lblSummaryOk.Text =
        "OK : " + report.Summary.Ok;

    lblSummaryInfo.Text =
        "Infos : " + report.Summary.Info;

    lblSummaryWarnings.Text =
        "Warnings : " + report.Summary.Warnings;

    lblSummaryErrors.Text =
        "Errors : " + report.Summary.Errors;

    lblSummaryOk.ForeColor =
        Color.LightGreen;

    lblSummaryInfo.ForeColor =
        Color.LightSkyBlue;

    lblSummaryWarnings.ForeColor =
        report.Summary.Warnings == 0
            ? Color.LightGreen
            : Color.Orange;

    lblSummaryErrors.ForeColor =
        report.Summary.Errors == 0
            ? Color.LightGreen
            : Color.OrangeRed;
  }

  private void SetButtonsEnabled(bool enabled) {
    btnRunReleaseCheck.Enabled = enabled;
    btnOpenLastReport.Enabled = enabled;
    btnReloadCatalog.Enabled = enabled;
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
          _reportService.LoadLatestReport(GetReportsFolderPath());

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

  private async Task RebuildProductsJsonFromSiteAsync() {
    try {
      SetButtonsEnabled(false);

      AppendConsole("");
      AppendConsole("==================================================");
      AppendConsole("Rebuild products.json from website");
      AppendConsole("==================================================");

      ProductCatalog sourceCatalog =
          new() {
            Site = GetSiteInfo(),
            Products = []
          };

      RemoteProductCatalogBuildResult result =
          await _remoteCatalogBuilder.BuildAsync(
              sourceCatalog,
              "msi-software-packager");

      foreach (string message in result.Messages) {
        AppendConsole(message);
      }

      foreach (string error in result.Errors) {
        AppendConsole(error);
      }

      string productsDirectory = GetProductsFolderPath();

      Directory.CreateDirectory(productsDirectory);

      string rebuiltFile =
          Path.Combine(
              productsDirectory,
              "products.rebuilt.json");

      JsonSerializerOptions jsonOptions = new() {
        WriteIndented = true,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
      };

      string rebuiltJson =
          JsonSerializer.Serialize(
              result.Catalog,
              jsonOptions);

      File.WriteAllText(
          rebuiltFile,
          rebuiltJson,
          new UTF8Encoding(false));

      AppendConsole("");
      AppendConsole("[OK] Rebuilt products file generated : " + rebuiltFile);
      AppendConsole("");
      AppendConsole(FormatJson(rebuiltJson));

      string productsFile =
          GetProductsJsonPath();

      string backupFile =
          Path.Combine(
              productsDirectory,
              "products.backup_" +
              DateTime.Now.ToString("yyyyMMdd_HHmmss") +
              ".json");

      if (File.Exists(productsFile)) {
        File.Copy(
            productsFile,
            backupFile,
            overwrite: true);

        AppendConsole("[OK] Backup created : " + backupFile);
      }

      File.Copy(
          rebuiltFile,
          productsFile,
          overwrite: true);

      _lastRebuiltProductsJsonPath = rebuiltFile;

      AppendConsole("");
      AppendConsole("[OK] Rebuilt products file generated : " + rebuiltFile);
      AppendConsole("[INFO] products.json was not replaced.");
      AppendConsole("[INFO] Review the generated JSON, then use Apply rebuilt products.json.");
      AppendConsole("");
      AppendConsole(FormatJson(rebuiltJson));
    }
    catch (Exception ex) {
      AppendConsole("[ERROR] Unable to rebuild products.json : " + ex.Message);
    }
    finally {
      SetButtonsEnabled(true);
    }
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

  private void ApplyRebuiltProductsJson() {
    try {
      string productsDirectory =
          GetProductsFolderPath();

      string rebuiltFile =
          _lastRebuiltProductsJsonPath;

      if (string.IsNullOrWhiteSpace(rebuiltFile)) {
        rebuiltFile =
            Path.Combine(
                productsDirectory,
                "products.rebuilt.json");
      }

      if (!File.Exists(rebuiltFile)) {
        AppendConsole("[ERROR] Rebuilt products file not found : " + rebuiltFile);
        return;
      }

      string productsFile =
          GetProductsJsonPath();

      DialogResult dialogResult =
          MessageBox.Show(
              "Replace products.json with products.rebuilt.json ?" +
              Environment.NewLine +
              Environment.NewLine +
              "A backup will be created before replacement.",
              "Apply rebuilt products.json",
              MessageBoxButtons.YesNo,
              MessageBoxIcon.Question);

      if (dialogResult != DialogResult.Yes) {
        AppendConsole("[INFO] products.json replacement cancelled.");
        return;
      }

      string backupFile =
          Path.Combine(
              productsDirectory,
              "products.backup_" +
              DateTime.Now.ToString("yyyyMMdd_HHmmss") +
              ".json");

      if (File.Exists(productsFile)) {
        File.Copy(
            productsFile,
            backupFile,
            overwrite: true);

        AppendConsole("[OK] Backup created : " + backupFile);
      }

      File.Copy(
          rebuiltFile,
          productsFile,
          overwrite: true);

      AppendConsole("[OK] products.json replaced by : " + rebuiltFile);

      LoadCatalog();
      RefreshDashboardView();

      AppendConsole("[OK] Catalog reloaded.");
    }
    catch (Exception ex) {
      AppendConsole("[ERROR] Unable to apply rebuilt products.json : " + ex.Message);
    }
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

  //---------------------------------------------------------------------
  private void OpenDownloadUrlAction() {
    ProductGridRow? row =
        GetSelectedRow();

    if (row == null)
      return;

    OpenUrl(row.DownloadUrl);
  }

  private void OpenArtifactUrlAction() {
    ProductGridRow? row =
        GetSelectedRow();

    if (row == null)
      return;

    OpenUrl(row.ArtifactUrl);
  }

  private async Task ViewUpdateJsonActionAsync() {
    ProductGridRow? row =
        GetSelectedRow();

    if (row == null)
      return;

    await ShowRemoteJsonInConsoleAsync(
        row.UpdateJsonUrl,
        "update.json - " + row.DisplayName);
  }

  private async Task ViewSha256ActionAsync() {
    ProductGridRow? row =
        GetSelectedRow();

    if (row == null)
      return;

    await ShowRemoteTextInConsoleAsync(
        row.Sha256Url,
        "SHA256 - " + row.DisplayName);
  }

  private async Task VerifySha256ActionAsync() {
    ProductGridRow? row =
        GetSelectedRow();

    if (row == null)
      return;

    await VerifySelectedProductSha256Async(row);
  }

  private void OpenLastReportAction() {
    string reportsFolder =
        GetReportsFolderPath();

    string? latestReport =
        _reportService.GetLatestMarkdownReportPath(reportsFolder);

    if (string.IsNullOrWhiteSpace(latestReport)) {
      AppendConsole("[INFO] No Markdown report found in : " + reportsFolder);
      return;
    }

    Process.Start(new ProcessStartInfo {
      FileName = latestReport,
      UseShellExecute = true
    });
  }

  private void OpenReportsFolderAction() {
    string reportsFolder =
        GetReportsFolderPath();

    Directory.CreateDirectory(reportsFolder);

    Process.Start(new ProcessStartInfo {
      FileName = reportsFolder,
      UseShellExecute = true
    });
  }

  private void ViewProductsJsonAction() {
    try {
      string productsFile = GetProductsJsonPath();

      if (!File.Exists(productsFile)) {
        AppendConsole("[ERROR] products.json not found : " + productsFile);
        return;
      }

      string json =
          File.ReadAllText(
              productsFile,
              Encoding.UTF8);

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

  private void EditProductsJsonAction() {
    try {
      string productsFile = GetProductsJsonPath();

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

  private void ReloadCatalogAction() {
    try {
      LoadCatalog();
      RefreshDashboardView();

      AppendConsole("[OK] products.json reloaded.");
    }
    catch (Exception ex) {
      AppendConsole("[ERROR] Unable to reload products.json : " + ex.Message);
    }
  }

  private void ConfigureWorkspaceAction() {
    try {
      using FolderBrowserDialog productsDialog = new() {
        Description = "Select the folder containing products.json",
        UseDescriptionForTitle = true,
        SelectedPath = Directory.Exists(_workspace.ProductsFolder)
            ? _workspace.ProductsFolder
            : Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
      };

      if (productsDialog.ShowDialog(this) != DialogResult.OK)
        return;

      using FolderBrowserDialog reportsDialog = new() {
        Description = "Select the reports output folder",
        UseDescriptionForTitle = true,
        SelectedPath = Directory.Exists(_workspace.ReportsFolder)
            ? _workspace.ReportsFolder
            : Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
      };

      if (reportsDialog.ShowDialog(this) != DialogResult.OK)
        return;

      WorkspaceSettings newWorkspace =
          new() {
            ProductsFolder = productsDialog.SelectedPath,
            ReportsFolder = reportsDialog.SelectedPath
          };

      _workspace =
          _workspaceService.Ensure(newWorkspace);

      _settings.Workspace =
          _workspace;

      _settingsService.Save(_settings);

      UpdateWorkspaceLabel();

      LoadCatalog();
      RefreshDashboardView();

      AppendConsole("[OK] Workspace updated.");
      AppendConsole("[INFO] Products folder : " + _workspace.ProductsFolder);
      AppendConsole("[INFO] Reports folder  : " + _workspace.ReportsFolder);
    }
    catch (Exception ex) {
      AppendConsole("[ERROR] Unable to configure workspace : " + ex.Message);
    }
  }

  private async Task RebuildProductsJsonActionAsync() {
    await RebuildProductsJsonFromSiteAsync();
  }

  private void ApplyRebuiltProductsJsonAction() {
    ApplyRebuiltProductsJson();
  }

  private void ViewProductChecksAction() {
    ShowSelectedProductChecks();
  }

  private void btnOpenDownloadUrl_Click(object sender,EventArgs e) {
    OpenDownloadUrlAction();
  }

  private void mnuOpenDownloadUrl_Click(object sender,EventArgs e) {
    OpenDownloadUrlAction();
  }

  private void btnOpenArtifactUrl_Click(object sender,EventArgs e) {
    OpenArtifactUrlAction();
  }

  private void mnuOpenArtifactUrl_Click(object sender,EventArgs e) {
    OpenArtifactUrlAction();
  }

  private async void btnOpenUpdateJsonUrl_Click(object sender,EventArgs e) {
    await ViewUpdateJsonActionAsync();
  }

  private async void mnuViewUpdateJson_Click(object sender,EventArgs e) {
    await ViewUpdateJsonActionAsync();
  }

  private async void btnViewSha256Url_Click(object sender,EventArgs e) {
    await ViewSha256ActionAsync();
  }

  private async void mnuViewSha256_Click(object sender,EventArgs e) {
    await ViewSha256ActionAsync();
  }

  private async void btnVerifySha256_Click(object sender,EventArgs e) {
    await VerifySha256ActionAsync();
  }

  private async void mnuVerifySha256_Click(object sender,EventArgs e) {
    await VerifySha256ActionAsync();
  }

  private void btnViewProductChecks_Click(object sender,EventArgs e) {
    ViewProductChecksAction();
  }

  private void mnuViewProductChecks_Click(object sender,EventArgs e) {
    ViewProductChecksAction();
  }

  private void btnOpenLastReport_Click(object sender,EventArgs e) {
    OpenLastReportAction();
  }

  private void mnuOpenLastReport_Click(object sender,EventArgs e) {
    OpenLastReportAction();
  }

  private void btnOpenReportsFolder_Click(object sender,EventArgs e) {
    OpenReportsFolderAction();
  }

  private void mnuOpenReportsFolder_Click(object sender,EventArgs e) {
    OpenReportsFolderAction();
  }
  private void btnEditProductsJson_Click(object sender,EventArgs e) {
    EditProductsJsonAction();
  }

  private void mnuEditProductsJson_Click(object sender,EventArgs e) {
    EditProductsJsonAction();
  }

  private void btnReloadCatalog_Click(object sender,EventArgs e) {
    ReloadCatalogAction();
  }

  private void mnuReloadCatalog_Click(object sender,EventArgs e) {
    ReloadCatalogAction();
  }

  private async void btnRebuildProductsJson_Click(object sender,EventArgs e) {
    await RebuildProductsJsonActionAsync();
  }

  private async void mnuRebuildProductsJson_Click(object sender,EventArgs e) {
    await RebuildProductsJsonActionAsync();
  }

  private void btnApplyRebuiltProductsJson_Click(object sender,EventArgs e) {
    ApplyRebuiltProductsJsonAction();
  }
  private void mnuApplyRebuiltProductsJson_Click(object sender,EventArgs e) {
    ApplyRebuiltProductsJsonAction();
  }

  private void mnuConfigureWorkspace_Click(object sender,EventArgs e) {
    ConfigureWorkspaceAction();
  }
}
