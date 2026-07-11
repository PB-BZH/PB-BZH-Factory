using System.Text.Json;
using PB.BZH.ReleaseDashboard.Core.Models;

namespace PB.BZH.ReleaseDashboard.Core.Services;

public sealed class ReleaseReportService {
  private static readonly JsonSerializerOptions JsonOptions = new() {
    PropertyNameCaseInsensitive = true,
    ReadCommentHandling = JsonCommentHandling.Skip,
    AllowTrailingCommas = true
  };

  public string? GetLatestJsonReportPath(string factoryRoot) {
    if (string.IsNullOrWhiteSpace(factoryRoot))
      return null;

    string reportsDirectory =
        Path.Combine(factoryRoot,"reports");

    if (!Directory.Exists(reportsDirectory))
      return null;

    FileInfo? latest =
        new DirectoryInfo(reportsDirectory)
            .GetFiles("release-check_*.json")
            .OrderByDescending(file => file.LastWriteTime)
            .FirstOrDefault();

    return latest?.FullName;
  }

  public ReleaseCheckReport? LoadLatestReport(string factoryRoot) {
    string? reportPath =
        GetLatestJsonReportPath(factoryRoot);

    if (string.IsNullOrWhiteSpace(reportPath))
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
}