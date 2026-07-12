using System.Text.Json;
using PB.BZH.ReleaseDashboard.Core.Models;

namespace PB.BZH.ReleaseDashboard.Core.Services;

public sealed class ReleaseReportService {
  private static readonly JsonSerializerOptions JsonOptions = new() {
    PropertyNameCaseInsensitive = true,
    ReadCommentHandling = JsonCommentHandling.Skip,
    AllowTrailingCommas = true
  };

  public string? GetLatestJsonReportPath(string reportsFolder) {
    return GetLatestReportPath(
        reportsFolder,
        "release-check_*.json");
  }

  public string? GetLatestMarkdownReportPath(string reportsFolder) {
    return GetLatestReportPath(
        reportsFolder,
        "release-check_*.md");
  }

  public ReleaseCheckReport? LoadLatestReport(string reportsFolder) {
    string? reportPath =
        GetLatestJsonReportPath(reportsFolder);

    if (string.IsNullOrWhiteSpace(reportPath))
      return null;

    if (!File.Exists(reportPath))
      return null;

    string json =
        File.ReadAllText(reportPath);

    return JsonSerializer.Deserialize<ReleaseCheckReport>(
        json,
        JsonOptions);
  }

  public string GetProductStatus(
      ReleaseCheckReport report,
      string productId,
      string displayName) {
    List<ReleaseCheckRow> rows =
        report.Checks
            .Where(row =>
                string.Equals(row.ProductId,productId,StringComparison.OrdinalIgnoreCase) ||
                string.Equals(row.Product,displayName,StringComparison.OrdinalIgnoreCase))
            .ToList();

    if (rows.Count == 0)
      return "UNKNOWN";

    if (rows.Any(row => string.Equals(row.Status,"FAIL",StringComparison.OrdinalIgnoreCase)))
      return "FAIL";

    if (rows.Any(row => string.Equals(row.Status,"WARN",StringComparison.OrdinalIgnoreCase)))
      return "WARN";

    return "OK";
  }

  public bool HasProductChecks(
      ReleaseCheckReport report,
      string productId,
      string displayName) {
    return report.Checks.Any(row =>
        string.Equals(row.ProductId,productId,StringComparison.OrdinalIgnoreCase) ||
        string.Equals(row.Product,displayName,StringComparison.OrdinalIgnoreCase));
  }

  public List<ReleaseCheckRow> GetProductChecks(
      ReleaseCheckReport report,
      string productId,
      string displayName) {
    if (report == null)
      throw new ArgumentNullException(nameof(report));

    return report.Checks
        .Where(row =>
            string.Equals(row.ProductId,productId,StringComparison.OrdinalIgnoreCase) ||
            string.Equals(row.Product,displayName,StringComparison.OrdinalIgnoreCase))
        .ToList();
  }

  private static string? GetLatestReportPath(
      string reportsFolder,
      string searchPattern) {
    if (string.IsNullOrWhiteSpace(reportsFolder))
      return null;

    if (!Directory.Exists(reportsFolder))
      return null;

    return Directory
        .EnumerateFiles(
            reportsFolder,
            searchPattern,
            SearchOption.TopDirectoryOnly)
        .OrderByDescending(File.GetLastWriteTime)
        .FirstOrDefault();
  }
}