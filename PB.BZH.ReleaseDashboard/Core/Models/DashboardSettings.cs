namespace PB.BZH.ReleaseDashboard.Core.Models;

public sealed class DashboardSettings {
  public WorkspaceSettings Workspace { get; set; } = new();

  public SiteInfo Site { get; set; } =
      new() {
        MainUrl = "https://www.pb-bzh-concept.fr",
        SoftwaresUrl = "https://www.pb-bzh-concept.fr/softwares",
        LocalSoftwaresRoot = string.Empty
      };
}