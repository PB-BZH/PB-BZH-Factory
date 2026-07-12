using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace PB.BZH.ReleaseDashboard.Core.Services;

public sealed class RemoteSha256Verifier {
  private static readonly HttpClient HttpClient = new();

  public async Task<RemoteSha256VerificationResult> VerifyAsync(
      string artifactUrl,
      string sha256Url,
      CancellationToken cancellationToken = default) {
    if (string.IsNullOrWhiteSpace(artifactUrl)) {
      return RemoteSha256VerificationResult.Fail(
          "Artifact URL is empty.");
    }

    if (string.IsNullOrWhiteSpace(sha256Url)) {
      return RemoteSha256VerificationResult.Fail(
          "SHA256 URL is empty.");
    }

    string sha256Text =
        await HttpClient.GetStringAsync(
            sha256Url,
            cancellationToken);

    string expectedHash =
        ExtractSha256Hash(sha256Text);

    if (string.IsNullOrWhiteSpace(expectedHash)) {
      return RemoteSha256VerificationResult.Fail(
          "Unable to extract SHA256 hash from remote file.",
          expectedHash: string.Empty,
          actualHash: string.Empty,
          sha256Text: sha256Text);
    }

    string actualHash =
        await ComputeRemoteFileSha256Async(
            artifactUrl,
            cancellationToken);

    bool isValid =
        string.Equals(
            expectedHash,
            actualHash,
            StringComparison.OrdinalIgnoreCase);

    return new RemoteSha256VerificationResult {
      Success = isValid,
      Message = isValid
          ? "SHA256 verification succeeded."
          : "SHA256 verification failed.",
      ExpectedHash = expectedHash,
      ActualHash = actualHash,
      Sha256Text = sha256Text
    };
  }

  private static string ExtractSha256Hash(string text) {
    if (string.IsNullOrWhiteSpace(text))
      return string.Empty;

    Match match =
        Regex.Match(
            text,
            @"\b[a-fA-F0-9]{64}\b");

    return match.Success
        ? match.Value.ToUpperInvariant()
        : string.Empty;
  }

  private static async Task<string> ComputeRemoteFileSha256Async(
      string artifactUrl,
      CancellationToken cancellationToken) {
    using HttpResponseMessage response =
        await HttpClient.GetAsync(
            artifactUrl,
            HttpCompletionOption.ResponseHeadersRead,
            cancellationToken);

    response.EnsureSuccessStatusCode();

    await using Stream stream =
        await response.Content.ReadAsStreamAsync(cancellationToken);

    using IncrementalHash hash =
        IncrementalHash.CreateHash(HashAlgorithmName.SHA256);

    byte[] buffer =
        new byte[1024 * 1024];

    while (true) {
      int bytesRead =
        await stream.ReadAsync(
            buffer.AsMemory(0,buffer.Length),
            cancellationToken);

      if (bytesRead == 0)
        break;

      hash.AppendData(
          buffer,
          0,
          bytesRead);
    }

    byte[] hashBytes =
        hash.GetHashAndReset();

    return Convert
        .ToHexString(hashBytes)
        .ToUpperInvariant();
  }
}

public sealed class RemoteSha256VerificationResult {
  public bool Success { get; set; }
  public string Message { get; set; } = string.Empty;
  public string ExpectedHash { get; set; } = string.Empty;
  public string ActualHash { get; set; } = string.Empty;
  public string Sha256Text { get; set; } = string.Empty;

  public static RemoteSha256VerificationResult Fail(
      string message,
      string expectedHash = "",
      string actualHash = "",
      string sha256Text = "") {
    return new RemoteSha256VerificationResult {
      Success = false,
      Message = message,
      ExpectedHash = expectedHash,
      ActualHash = actualHash,
      Sha256Text = sha256Text
    };
  }
}