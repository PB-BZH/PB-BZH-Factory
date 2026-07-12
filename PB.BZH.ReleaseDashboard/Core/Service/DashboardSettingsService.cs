using System.Text;
using System.Text.Json;
using PB.BZH.ReleaseDashboard.Core.Models;

namespace PB.BZH.ReleaseDashboard.Core.Services;

public sealed class DashboardSettingsService {
  private static readonly JsonSerializerOptions JsonOptions = new() {
    WriteIndented = true,
    PropertyNameCaseInsensitive = true,
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    ReadCommentHandling = JsonCommentHandling.Skip,
    AllowTrailingCommas = true
  };

  public string SettingsFolder {
    get {
      string appData =
          Environment.GetFolderPath(
              Environment.SpecialFolder.ApplicationData);

      return Path.Combine(
          appData,
          "PB-BZH-Factory",
          "ReleaseDashboard");
    }
  }

  public string SettingsFilePath =>
      Path.Combine(SettingsFolder,"settings.json");

  public DashboardSettings Load() {
    try {
      if (!File.Exists(SettingsFilePath))
        return new DashboardSettings();

      string json =
          File.ReadAllText(SettingsFilePath,Encoding.UTF8);

      DashboardSettings? settings =
          JsonSerializer.Deserialize<DashboardSettings>(
              json,
              JsonOptions);

      return settings ?? new DashboardSettings();
    }
    catch {
      return new DashboardSettings();
    }
  }

  public void Save(DashboardSettings settings) {
    Directory.CreateDirectory(SettingsFolder);

    string json =
        JsonSerializer.Serialize(
            settings,
            JsonOptions);

    File.WriteAllText(
        SettingsFilePath,
        json,
        new UTF8Encoding(false));
  }
}