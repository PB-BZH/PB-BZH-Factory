using System.Text.Json;
using PB.BZH.ReleaseDashboard.Core.Models;

namespace PB.BZH.ReleaseDashboard.Core.Services;

public sealed class ProductCatalogService {
  private static readonly JsonSerializerOptions JsonOptions = new() {
    PropertyNameCaseInsensitive = true,
    ReadCommentHandling = JsonCommentHandling.Skip,
    AllowTrailingCommas = true
  };

  public ProductCatalog Load(string filePath) {
    if (string.IsNullOrWhiteSpace(filePath)) {
      throw new InvalidOperationException("Products file path is empty.");
    }

    if (!File.Exists(filePath)) {
      throw new FileNotFoundException(
          "Products file was not found.",
          filePath);
    }

    string json =
        File.ReadAllText(filePath);

    ProductCatalog? catalog =
        JsonSerializer.Deserialize<ProductCatalog>(
            json,
            JsonOptions);

    if (catalog == null) {
      throw new InvalidOperationException("Unable to read products catalog.");
    }

    return catalog;
  }
}