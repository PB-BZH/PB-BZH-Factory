/*
╔════════════════════════════════════════════════════════════════════════════════╗
║                                                                                ║
║                    ───────────────────────────────────                         ║
║                      © Copyright PB-BZH Concept 2026                           ║
║                    ───────────────────────────────────                         ║
║                                                                                ║
║                 contact : mailto:admin@pb-bzh-concept.fr                       ║
╚════════════════════════════════════════════════════════════════════════════════╝

╔════════════════════════════════════════════════════════════════════════════════╗
║  Auteur : Patrick Bourges - PB-BZH Concept                                     ║
║  Le 21/6/2026 - 17:49
╟────────────────────────────────────────────────────────────────────────────────║
║     Projet Visual Studio Professional 2026 : GestionServicesGEII
╟────────────────────────────────────────────────────────────────────────────────║
║     Version : 1.5.3
╟────────────────────────────────────────────────────────────────────────────────║
║                Visual Studio Professional 2026 - Insiders                      ║
║                ──────────────────────────────────────────                      ║
║  Langage     : C# 14                                                           ║
║  Technologie : .NET 10 / WinForms                                              ║
║  Plateforme  : Windows 10 / Windows 11                                         ║
║  Encodage    : UTF-8                                                           ║
╟────────────────────────────────────────────────────────────────────────────────║
║  Nom de fichier : ServiceManagerProfile.cs
╚════════════════════════════════════════════════════════════════════════════════╝
*/
namespace PB.BZH.ReleaseDashboard.Core.Profiles {
  public sealed class DashboardProfileManager {
    public ProductOptions Product { get; set; } = new();
    public UpdateManifest UpdateManifest { get; set; } = new();
  }

  public sealed class UpdateManifest {
    public string ProductName { get; set; } = "";
    public string Version { get; set; } = "";
    public string Publisher { get; set; } = "";
    public string DownloadPage { get; set; } = "https://www.pb-bzh-concept.fr";
    public string PrivacyPage { get; set; } = "https://www.pb-bzh-concept.fr/privacy.php";
    public string MsiUrl { get; set; } = "";
    public string WebSetupUrl { get; set; } = "";
    public string UpdateManifestUrl { get; set; } = "";
    public string ReleaseDate { get; set; } = "";
    public string ApplicationId { get; set; } = "";
  }

  public sealed class ProductOptions {
    public string ProductName { get; set; } = "PB BZH Release Dashboard";
    public string ProductId { get; set; } = "PB.BZH.ReleaseDashboard";
    public string ProductFolder { get; set; } = "PB.BZH.ReleaseDashboard";
    public string Manufacturer { get; set; } = "PB BZH Concept";
    public string Version { get; set; } = "1.0.0";
    public string Description { get; set; } = "Gestion du service du bar Le Bellevue";
    public string UpgradeCode { get; set; } = "";
    public string IconPath { get; set; } = "";
    public Image? LogoImage { get; set; } = Properties.Resources.Application;
    public string DownloadPageUrl { get; set; } = "https://www.pb-bzh-concept.fr";
    public string PrivacyPageUrl { get; set; } = "https://www.pb-bzh-concept.fr/privacy.php";
    public string Copyright { get; set; } = "© Copyright PB BZH Concept 2026";
    public string EmailContact { get; set; } = "admin@pb-bzh-concept.fr";
  }
}