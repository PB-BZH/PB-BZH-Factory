using PB.BZH.ReleaseDashboard.Core.Models;

namespace PB.BZH.ReleaseDashboard.UI.Summary;

public static class ReleaseSummaryPresenter {
  public static void Clear(
      Label lblLastCheck,
      Label lblSummaryOk,
      Label lblSummaryInfo,
      Label lblSummaryWarnings,
      Label lblSummaryErrors) {
    lblLastCheck.Text = "Last check : -";
    lblSummaryOk.Text = "OK : -";
    lblSummaryInfo.Text = "Infos : -";
    lblSummaryWarnings.Text = "Warnings : -";
    lblSummaryErrors.Text = "Errors : -";
  }

  public static void Apply(
      ReleaseCheckReport report,
      Label lblLastCheck,
      Label lblSummaryOk,
      Label lblSummaryInfo,
      Label lblSummaryWarnings,
      Label lblSummaryErrors) {
    if (report == null)
      throw new ArgumentNullException(nameof(report));

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
  }
}