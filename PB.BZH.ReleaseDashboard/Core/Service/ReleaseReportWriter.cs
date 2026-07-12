using System.Text;
using System.Text.Json;
using PB.BZH.ReleaseDashboard.Core.Models;

namespace PB.BZH.ReleaseDashboard.Core.Services;

public sealed class ReleaseReportWriter {
  private static readonly JsonSerializerOptions JsonOptions = new() {
    WriteIndented = true
  };

  public (string MarkdownPath,string JsonPath) Save(
      string reportsFolder,
      ReleaseCheckReport report) {
    if (string.IsNullOrWhiteSpace(reportsFolder))
      throw new InvalidOperationException("Reports folder is empty.");

    if (report == null)
      throw new ArgumentNullException(nameof(report));

    Directory.CreateDirectory(reportsFolder);

    string timestamp =
        DateTime.Now.ToString("yyyyMMdd_HHmmss");

    string jsonPath =
        Path.Combine(
            reportsFolder,
            "release-check_" + timestamp + ".json");

    string markdownPath =
        Path.Combine(
            reportsFolder,
            "release-check_" + timestamp + ".md");

    string json =
        JsonSerializer.Serialize(
            report,
            JsonOptions);

    File.WriteAllText(
        jsonPath,
        json,
        new UTF8Encoding(false));

    string markdown =
        BuildMarkdown(report);

    File.WriteAllText(
        markdownPath,
        markdown,
        new UTF8Encoding(false));

    return (markdownPath,jsonPath);
  }

  private static string BuildMarkdown(
      ReleaseCheckReport report) {
    StringBuilder builder =
        new();

    builder.AppendLine("# PB BZH Release Check");
    builder.AppendLine();
    builder.AppendLine("Generated local : " + report.GeneratedAtLocal);
    builder.AppendLine("Generated UTC   : " + report.GeneratedAtUtc);
    builder.AppendLine("Duration        : " + report.DurationSeconds + " s");
    builder.AppendLine();

    builder.AppendLine("## Summary");
    builder.AppendLine();
    builder.AppendLine("- OK       : " + report.Summary.Ok);
    builder.AppendLine("- Infos    : " + report.Summary.Info);
    builder.AppendLine("- Warnings : " + report.Summary.Warnings);
    builder.AppendLine("- Errors   : " + report.Summary.Errors);
    builder.AppendLine();

    builder.AppendLine("## Checks");
    builder.AppendLine();
    builder.AppendLine("| Time | Product | Status | Label | Details |");
    builder.AppendLine("|---|---|---|---|---|");

    foreach (ReleaseCheckRow row in report.Checks) {
      builder.Append("| ");
      builder.Append(EscapeMarkdown(row.Time));
      builder.Append(" | ");
      builder.Append(EscapeMarkdown(row.Product));
      builder.Append(" | ");
      builder.Append(EscapeMarkdown(row.Status));
      builder.Append(" | ");
      builder.Append(EscapeMarkdown(row.Label));
      builder.Append(" | ");
      builder.Append(EscapeMarkdown(row.Details));
      builder.AppendLine(" |");
    }

    return builder.ToString();
  }

  private static string EscapeMarkdown(
      string value) {
    if (string.IsNullOrEmpty(value))
      return string.Empty;

    return value
        .Replace("|","\\|")
        .Replace("\r"," ")
        .Replace("\n"," ");
  }
}