namespace PB.BZH.ReleaseDashboard.Core.Models;

public sealed class ProductCatalog {
  public SiteInfo Site { get; set; } = new();
  public List<ProductInfo> Products { get; set; } = [];
}