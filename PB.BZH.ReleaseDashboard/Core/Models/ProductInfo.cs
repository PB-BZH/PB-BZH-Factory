namespace PB.BZH.ReleaseDashboard.Core.Models;

public sealed class ProductInfo {
  public string Id { get; set; } = string.Empty;
  public string DisplayName { get; set; } = string.Empty;
  public string Type { get; set; } = string.Empty;
  public string Category { get; set; } = string.Empty;
  public string Version { get; set; } = string.Empty;
  public int? AndroidVersionCode { get; set; }
  public string ArtifactFile { get; set; } = string.Empty;
  public bool HasSha256 { get; set; }
  public bool HasUpdateJson { get; set; }
  public string LocalCheck { get; set; } = "disabled";

  public string GetArtifactUrl(string softwaresUrl) {
    return
        softwaresUrl.TrimEnd('/') +
        "/" +
        Category.Trim('/') +
        "/" +
        Id.Trim('/') +
        "/" +
        ArtifactFile;
  }

  public string GetDownloadUrl(string softwaresUrl) {
    return
        softwaresUrl.TrimEnd('/') +
        "/download.php?category=" +
        Uri.EscapeDataString(Category) +
        "&product=" +
        Uri.EscapeDataString(Id);
  }

  public string GetUpdateJsonUrl(string softwaresUrl) {
    return
        softwaresUrl.TrimEnd('/') +
        "/" +
        Category.Trim('/') +
        "/" +
        Id.Trim('/') +
        "/update.json";
  }

  public string GetSha256Url(string softwaresUrl) {
    return GetArtifactUrl(softwaresUrl) + ".sha256.txt";
  }
}