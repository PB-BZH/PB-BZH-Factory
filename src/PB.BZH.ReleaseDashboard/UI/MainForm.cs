using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using PB.BZH.Help.Library.UI.Theming;
using PB.BZH.ReleaseDashboard.Core.Models;
using PB.BZH.ReleaseDashboard.Core.Services;
using PB.BZH.ReleaseDashboard.UI.Console;
using PB.BZH.ReleaseDashboard.UI.Grids;
using PB.BZH.ReleaseDashboard.UI.Summary;

namespace PB.BZH.ReleaseDashboard.UI;

public partial class MainForm: Form {
  private readonly ProductCatalogService _catalogService = new();
  private readonly ReleaseReportService _reportService = new();

  private string _factoryRoot = string.Empty;
  private ProductCatalog? _catalog;
  private BindingList<ProductGridRow> _rows = [];

  public MainForm() {
    InitializeComponent();
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

    public static ProductGridRow FromProduct(
        ProductInfo product,
        string softwaresUrl) {
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
        UpdateJsonUrl = product.GetUpdateJsonUrl(softwaresUrl)
      };
    }
  }

  private void btnOpenUpdateJsonUrl_Click(object sender,EventArgs e) {
    ProductGridRow? row = GetSelectedRow();

    if (row == null)
      return;

    OpenUrl(row.UpdateJsonUrl);
  }

  private void txtConsole_LinkClicked(object sender,LinkClickedEventArgs e) {
    if (string.IsNullOrWhiteSpace(e.LinkText))
      return;

    Process.Start(new ProcessStartInfo {
      FileName = e.LinkText,
      UseShellExecute = true
    });
  }
}