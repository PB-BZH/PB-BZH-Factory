namespace PB.BZH.ReleaseDashboard.Core.Models;

public sealed class WorkspaceSettings {
  public string ProductsFolder { get; set; } = string.Empty;
  public string ReportsFolder { get; set; } = string.Empty;
  public string ProductsJsonPath => Path.Combine(ProductsFolder,"products.json");
}