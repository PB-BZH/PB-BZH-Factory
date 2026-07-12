namespace PB.BZH.ReleaseDashboard.UI.Details;

public static class ProductDetailsPresenter {
  public static void Clear(
      Label lblProduct,
      Label lblType,
      Label lblVersion,
      Label lblStatus,
      TextBoxBase txtDownloadUrl,
      TextBoxBase txtArtifactUrl,
      TextBoxBase txtSha256Url,
      TextBoxBase txtUpdateJsonUrl) {
    lblProduct.Text = "Product : -";
    lblType.Text = "Type : -";
    lblVersion.Text = "Version : -";
    lblStatus.Text = "Status : -";

    txtDownloadUrl.Text = string.Empty;
    txtArtifactUrl.Text = string.Empty;
    txtSha256Url.Text = string.Empty;
    txtUpdateJsonUrl.Text = string.Empty;

    lblStatus.ForeColor = Color.Gainsboro;
  }

  public static void Apply(
      string product,
      string type,
      string version,
      string status,
      string downloadUrl,
      string artifactUrl,
      string sha256Url,
      string updateJsonUrl,
      Label lblProduct,
      Label lblType,
      Label lblVersion,
      Label lblStatus,
      TextBoxBase txtDownloadUrl,
      TextBoxBase txtArtifactUrl,
      TextBoxBase txtSha256Url,
      TextBoxBase txtUpdateJsonUrl) {
    lblProduct.Text = "Product : " + product;
    lblType.Text = "Type : " + type;
    lblVersion.Text = "Version : " + version;
    lblStatus.Text = "Status : " + status;

    txtDownloadUrl.Text = downloadUrl;
    txtArtifactUrl.Text = artifactUrl;
    txtSha256Url.Text = sha256Url;
    txtUpdateJsonUrl.Text = updateJsonUrl;

    lblStatus.ForeColor =
        status.ToUpperInvariant() switch {
          "OK" => Color.LimeGreen,
          "WARN" => Color.Orange,
          "FAIL" => Color.OrangeRed,
          "NOT CHECKED" => Color.LightGray,
          _ => Color.Silver
        };
  }
}