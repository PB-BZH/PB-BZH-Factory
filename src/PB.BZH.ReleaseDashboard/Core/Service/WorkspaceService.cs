using PB.BZH.ReleaseDashboard.Core.Models;

namespace PB.BZH.ReleaseDashboard.Core.Services;

public sealed class WorkspaceService {
  public WorkspaceSettings Resolve(WorkspaceSettings? configuredWorkspace) {
    if (IsUsable(configuredWorkspace)) {
      return Ensure(configuredWorkspace!);
    }

    string? developmentRoot =
        TryFindDevelopmentRoot();

    if (!string.IsNullOrWhiteSpace(developmentRoot)) {
      return Ensure(new WorkspaceSettings {
        ProductsFolder = Path.Combine(developmentRoot,"products"),
        ReportsFolder = Path.Combine(developmentRoot,"reports")
      });
    }

    string documents =
        Environment.GetFolderPath(
            Environment.SpecialFolder.MyDocuments);

    string defaultRoot =
        Path.Combine(
            documents,
            "PB-BZH-Factory");

    return Ensure(new WorkspaceSettings {
      ProductsFolder = Path.Combine(defaultRoot,"products"),
      ReportsFolder = Path.Combine(defaultRoot,"reports")
    });
  }

  public WorkspaceSettings Ensure(WorkspaceSettings workspace) {
    if (workspace == null)
      throw new ArgumentNullException(nameof(workspace));

    if (string.IsNullOrWhiteSpace(workspace.ProductsFolder))
      throw new InvalidOperationException("Products folder is empty.");

    if (string.IsNullOrWhiteSpace(workspace.ReportsFolder))
      throw new InvalidOperationException("Reports folder is empty.");

    workspace.ProductsFolder =
        Path.GetFullPath(workspace.ProductsFolder);

    workspace.ReportsFolder =
        Path.GetFullPath(workspace.ReportsFolder);

    Directory.CreateDirectory(workspace.ProductsFolder);
    Directory.CreateDirectory(workspace.ReportsFolder);

    return workspace;
  }

  private static bool IsUsable(WorkspaceSettings? workspace) {
    if (workspace == null)
      return false;

    if (string.IsNullOrWhiteSpace(workspace.ProductsFolder))
      return false;

    if (string.IsNullOrWhiteSpace(workspace.ReportsFolder))
      return false;

    return true;
  }

  private static string? TryFindDevelopmentRoot() {
    DirectoryInfo? directory =
        new(AppContext.BaseDirectory);

    while (directory != null) {
      string productsJson =
          Path.Combine(
              directory.FullName,
              "products",
              "products.json");

      if (File.Exists(productsJson))
        return directory.FullName;

      directory = directory.Parent;
    }

    return null;
  }
}