namespace PB.BZH.ReleaseDashboard.Core.Models;

public sealed class DashboardSettings {
  public WorkspaceSettings Workspace { get; set; } = new();
}
