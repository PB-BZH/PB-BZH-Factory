namespace PB.BZH.ReleaseDashboard.Core.Models;

public sealed class ReleaseCheckReport {
  public string GeneratedAtLocal { get; set; } = string.Empty;
  public string GeneratedAtUtc { get; set; } = string.Empty;
  public double DurationSeconds { get; set; }
  public ReleaseCheckSummary Summary { get; set; } = new();
  public List<ReleaseCheckRow> Checks { get; set; } = [];
}

public sealed class ReleaseCheckSummary {
  public int Ok { get; set; }
  public int Info { get; set; }
  public int Warnings { get; set; }
  public int Errors { get; set; }
}

public sealed class ReleaseCheckRow {
  public string Time { get; set; } = string.Empty;
  public string Product { get; set; } = string.Empty;
  public string ProductId { get; set; } = string.Empty;
  public string Status { get; set; } = string.Empty;
  public string Label { get; set; } = string.Empty;
  public string Details { get; set; } = string.Empty;
}