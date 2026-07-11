param(
  [switch]$SkipWeb,
  [switch]$Report
)

$ErrorActionPreference = "Stop"
[Console]::OutputEncoding = [System.Text.UTF8Encoding]::new($false)
$OutputEncoding = [System.Text.UTF8Encoding]::new($false)

$ScriptDirectory = $PSScriptRoot
$RootDirectory = Split-Path -Parent $ScriptDirectory
$ProductsFile = Join-Path $RootDirectory "products\products.json"

$script:ErrorCount = 0
$script:WarningCount = 0
$script:InfoCount = 0
$script:OkCount = 0

$script:RunStartedAt = Get-Date
$script:CurrentProduct = "Global"
$script:CurrentProductId = ""
$script:ReportRows = New-Object System.Collections.Generic.List[object]

function ConvertTo-MarkdownCell {
  param([string]$Value)

  if ($null -eq $Value) {
    return ""
  }

  return ([string]$Value).
      Replace("|", "\|").
      Replace("`r", " ").
      Replace("`n", " ")
}

function Save-ReleaseReport {
  param(
    [string]$RootDirectory,
    [string]$MainUrl,
    [string]$SoftwaresUrl,
    [string]$LocalSoftwaresRoot,
    [bool]$SkipWeb
  )

  $reportsDirectory =
      Join-Path $RootDirectory "reports"

[System.IO.Directory]::CreateDirectory($reportsDirectory) | Out-Null

  $timestamp =
      Get-Date -Format "yyyyMMdd_HHmmss"

  $markdownPath =
      Join-Path $reportsDirectory ("release-check_" + $timestamp + ".md")

  $jsonPath =
      Join-Path $reportsDirectory ("release-check_" + $timestamp + ".json")

  $runEndedAt =
      Get-Date

  $jsonReport =
      [pscustomobject]@{
        GeneratedAtLocal = $runEndedAt.ToString("yyyy-MM-dd HH:mm:ss")
        GeneratedAtUtc = $runEndedAt.ToUniversalTime().ToString("O")
        DurationSeconds = [math]::Round(($runEndedAt - $script:RunStartedAt).TotalSeconds,2)
        Site = [pscustomobject]@{
          MainUrl = $MainUrl
          SoftwaresUrl = $SoftwaresUrl
          LocalSoftwaresRoot = $LocalSoftwaresRoot
          WebChecksSkipped = $SkipWeb
        }
        Summary = [pscustomobject]@{
          Ok = $script:OkCount
          Info = $script:InfoCount
          Warnings = $script:WarningCount
          Errors = $script:ErrorCount
        }
        Checks = $script:ReportRows
      }

  $jsonReport |
      ConvertTo-Json -Depth 8 |
      Set-Content -Path $jsonPath -Encoding UTF8

  $builder =
      New-Object System.Text.StringBuilder

  [void]$builder.AppendLine("# PB BZH Release Check")
  [void]$builder.AppendLine("")
  [void]$builder.AppendLine("Date locale : " + $runEndedAt.ToString("yyyy-MM-dd HH:mm:ss"))
  [void]$builder.AppendLine("Durée : " + [math]::Round(($runEndedAt - $script:RunStartedAt).TotalSeconds,2) + " s")
  [void]$builder.AppendLine("")
  [void]$builder.AppendLine("## Site")
  [void]$builder.AppendLine("")
  [void]$builder.AppendLine("- Site principal : " + $MainUrl)
  [void]$builder.AppendLine("- Catalogue logiciels : " + $SoftwaresUrl)
  [void]$builder.AppendLine("- Racine locale : " + $LocalSoftwaresRoot)
  [void]$builder.AppendLine("- Tests web ignorés : " + $SkipWeb)
  [void]$builder.AppendLine("")
  [void]$builder.AppendLine("## Summary")
  [void]$builder.AppendLine("")
  [void]$builder.AppendLine("| Status | Count |")
  [void]$builder.AppendLine("|---|---:|")
  [void]$builder.AppendLine("| OK | " + $script:OkCount + " |")
  [void]$builder.AppendLine("| Info | " + $script:InfoCount + " |")
  [void]$builder.AppendLine("| Warnings | " + $script:WarningCount + " |")
  [void]$builder.AppendLine("| Errors | " + $script:ErrorCount + " |")
  [void]$builder.AppendLine("")
  [void]$builder.AppendLine("## Checks")
  [void]$builder.AppendLine("")

  $groups =
      $script:ReportRows |
      Group-Object Product

  foreach ($group in $groups) {
    [void]$builder.AppendLine("### " + $group.Name)
    [void]$builder.AppendLine("")
    [void]$builder.AppendLine("| Status | Check | Details |")
    [void]$builder.AppendLine("|---|---|---|")

    foreach ($row in $group.Group) {
      [void]$builder.AppendLine(
          "| " +
          (ConvertTo-MarkdownCell $row.Status) +
          " | " +
          (ConvertTo-MarkdownCell $row.Label) +
          " | " +
          (ConvertTo-MarkdownCell $row.Details) +
          " |"
      )
    }

    [void]$builder.AppendLine("")
  }

[System.IO.File]::WriteAllText(
    $markdownPath,
    $builder.ToString(),
    [System.Text.UTF8Encoding]::new($false)
)

  Write-Host ""
  Write-Host "[OK] Markdown report generated : $markdownPath" -ForegroundColor Green
  Write-Host "[OK] JSON report generated     : $jsonPath" -ForegroundColor Green
}

function Write-Title {
  param([string]$Text)

  Write-Host ""
  Write-Host "==================================================" -ForegroundColor DarkCyan
  Write-Host $Text -ForegroundColor Cyan
  Write-Host "==================================================" -ForegroundColor DarkCyan
}

function Add-ReportRow {
  param(
    [string]$Status,
    [string]$Label,
    [string]$Details = ""
  )

  $script:ReportRows.Add(
    [pscustomobject]@{
      Time = (Get-Date).ToString("HH:mm:ss")
      Product = $script:CurrentProduct
      ProductId = $script:CurrentProductId
      Status = $Status
      Label = $Label
      Details = $Details
    }
  ) | Out-Null
}

function Write-Check {
  param(
    [string]$Label,
    [bool]$Ok,
    [string]$Details = "",
    [bool]$Warning = $false,
    [bool]$Info = $false
  )

  if ($Ok) {
    $script:OkCount++
    Add-ReportRow -Status "OK" -Label $Label -Details $Details

    Write-Host ("  [OK]   " + $Label) -ForegroundColor Green
    if (-not [string]::IsNullOrWhiteSpace($Details)) {
      Write-Host ("         " + $Details) -ForegroundColor DarkGray
    }
    return
  }

  if ($Info) {
    $script:InfoCount++
    Add-ReportRow -Status "INFO" -Label $Label -Details $Details

    Write-Host ("  [INFO] " + $Label) -ForegroundColor Cyan
    if (-not [string]::IsNullOrWhiteSpace($Details)) {
      Write-Host ("         " + $Details) -ForegroundColor DarkGray
    }
    return
  }

  if ($Warning) {
    $script:WarningCount++
    Add-ReportRow -Status "WARN" -Label $Label -Details $Details

    Write-Host ("  [WARN] " + $Label) -ForegroundColor Yellow
    if (-not [string]::IsNullOrWhiteSpace($Details)) {
      Write-Host ("         " + $Details) -ForegroundColor Yellow
    }
    return
  }

  $script:ErrorCount++
  Add-ReportRow -Status "FAIL" -Label $Label -Details $Details

  Write-Host ("  [FAIL] " + $Label) -ForegroundColor Red
  if (-not [string]::IsNullOrWhiteSpace($Details)) {
    Write-Host ("         " + $Details) -ForegroundColor Red
  }
}

function Join-Url {
  param(
    [Parameter(ValueFromRemainingArguments = $true)]
    [string[]]$Parts
  )

  $cleanParts =
    $Parts |
    Where-Object { -not [string]::IsNullOrWhiteSpace($_) } |
    ForEach-Object { $_.Trim().Trim("/") }

  return ($cleanParts -join "/")
}

function New-DownloadUrl {
  param(
    [string]$BaseUrl,
    [string]$Category,
    [string]$ProductId
  )

  $encodedCategory = [Uri]::EscapeDataString($Category)
  $encodedProduct = [Uri]::EscapeDataString($ProductId)

  return $BaseUrl.TrimEnd("/") +
    "/download.php?category=" +
    $encodedCategory +
    "&product=" +
    $encodedProduct
}

function Test-WebUrl {
  param([string]$Url)

  try {
    $response = Invoke-WebRequest `
      -Uri $Url `
      -Method Head `
      -MaximumRedirection 5 `
      -TimeoutSec 15 `
      -UseBasicParsing

    return @{
      Ok = ($response.StatusCode -ge 200 -and $response.StatusCode -lt 400)
      StatusCode = $response.StatusCode
      Error = ""
    }
  }
  catch {
    try {
      $response = Invoke-WebRequest `
        -Uri $Url `
        -Method Get `
        -Headers @{ Range = "bytes=0-0" } `
        -MaximumRedirection 5 `
        -TimeoutSec 15 `
        -UseBasicParsing

      return @{
        Ok = ($response.StatusCode -ge 200 -and $response.StatusCode -lt 400)
        StatusCode = $response.StatusCode
        Error = ""
      }
    }
    catch {
      return @{
        Ok = $false
        StatusCode = 0
        Error = $_.Exception.Message
      }
    }
  }
}

function Get-JsonPropertyValue {
  param(
    [object]$Object,
    [string[]]$Names
  )

  foreach ($name in $Names) {
    if ($Object.PSObject.Properties.Name -contains $name) {
      return $Object.$name
    }
  }

  return $null
}

function Test-Sha256File {
  param(
    [string]$ArtifactPath,
    [string]$ShaPath
  )

  if (-not (Test-Path $ArtifactPath)) {
    return @{
      Ok = $false
      Details = "Artefact absent, SHA256 impossible à vérifier."
    }
  }

  if (-not (Test-Path $ShaPath)) {
    return @{
      Ok = $false
      Details = "Fichier SHA256 absent : $ShaPath"
    }
  }

  $computedHash =
    (Get-FileHash -Algorithm SHA256 -Path $ArtifactPath).Hash.ToLowerInvariant()

  $shaContent =
    (Get-Content $ShaPath -Raw).ToLowerInvariant()

  if ($shaContent.Contains($computedHash)) {
    return @{
      Ok = $true
      Details = $computedHash
    }
  }

  return @{
    Ok = $false
    Details = "SHA256 incohérent. Calculé : $computedHash"
  }
}

function Test-RemoteUpdateJson {
  param(
    [string]$Url,
    [string]$ExpectedVersion,
    [string]$ExpectedArtifactUrl
  )

  try {
    $response =
      Invoke-WebRequest `
        -Uri $Url `
        -Method Get `
        -MaximumRedirection 5 `
        -TimeoutSec 15 `
        -UseBasicParsing

    if ($response.StatusCode -lt 200 -or $response.StatusCode -ge 400) {
      return @{
        Ok = $false
        Details = "HTTP " + $response.StatusCode + " - " + $Url
      }
    }

    try {
      $json =
        $response.Content |
        ConvertFrom-Json
    }
    catch {
      return @{
        Ok = $false
        Details = "update.json distant invalide : " + $_.Exception.Message
      }
    }

    $details =
      "HTTP " + $response.StatusCode + " ; JSON valide"

    $version =
      Get-JsonPropertyValue `
        -Object $json `
        -Names @("version", "Version", "latestVersion", "LatestVersion")

    if ($null -ne $version) {
      if ([string]$version -ne $ExpectedVersion) {
        return @{
          Ok = $false
          Details = "Version distante = $version ; attendu = $ExpectedVersion"
        }
      }

      $details += " ; version OK"
    }

    $url =
      Get-JsonPropertyValue `
        -Object $json `
        -Names @("url", "Url", "downloadUrl", "DownloadUrl", "apkUrl", "ApkUrl", "artifactUrl", "ArtifactUrl")

    if ($null -ne $url) {
      if ([string]$url -ne $ExpectedArtifactUrl) {
        return @{
          Ok = $false
          Details = "URL distante différente : $url"
        }
      }

      $details += " ; URL OK"
    }

    return @{
      Ok = $true
      Details = $details
    }
  }
  catch {
    return @{
      Ok = $false
      Details = $_.Exception.Message
    }
  }
}

function Test-UpdateJson {
  param(
    [string]$UpdateJsonPath,
    [string]$ExpectedVersion,
    [string]$ExpectedArtifactUrl
  )

  if (-not (Test-Path $UpdateJsonPath)) {
    return @{
      Ok = $false
      Details = "update.json absent : $UpdateJsonPath"
    }
  }

  try {
    $json = Get-Content $UpdateJsonPath -Raw | ConvertFrom-Json
  }
  catch {
    return @{
      Ok = $false
      Details = "update.json invalide : " + $_.Exception.Message
    }
  }

  $details = "JSON valide"

  $version =
    Get-JsonPropertyValue `
      -Object $json `
      -Names @("version", "Version", "latestVersion", "LatestVersion")

  if ($null -ne $version) {
    if ([string]$version -ne $ExpectedVersion) {
      return @{
        Ok = $false
        Details = "Version update.json = $version ; attendu = $ExpectedVersion"
      }
    }

    $details += " ; version OK"
  }

  $url =
    Get-JsonPropertyValue `
      -Object $json `
      -Names @("url", "Url", "downloadUrl", "DownloadUrl", "apkUrl", "ApkUrl", "artifactUrl", "ArtifactUrl")

  if ($null -ne $url) {
    if ([string]$url -ne $ExpectedArtifactUrl) {
      return @{
        Ok = $false
        Details = "URL update.json différente : $url"
      }
    }

    $details += " ; URL OK"
  }

  return @{
    Ok = $true
    Details = $details
  }
}

Write-Title "PB BZH Release Check"

if (-not (Test-Path $ProductsFile)) {
  Write-Host "Fichier introuvable : $ProductsFile" -ForegroundColor Red
  exit 1
}

$config =
  Get-Content $ProductsFile -Raw |
  ConvertFrom-Json

$site = $config.site
$products = $config.products

if ($null -eq $site) {
  Write-Host "Section site absente dans products.json." -ForegroundColor Red
  exit 1
}

if ($null -eq $products -or $products.Count -eq 0) {
  Write-Host "Aucun produit défini dans products.json." -ForegroundColor Red
  exit 1
}

$mainUrl = [string]$site.mainUrl
$softwaresUrl = [string]$site.softwaresUrl
$localSoftwaresRoot = [string]$site.localSoftwaresRoot

Write-Host "Site principal    : $mainUrl"
Write-Host "Catalogue logiciel: $softwaresUrl"
Write-Host "Racine locale     : $localSoftwaresRoot"
Write-Host "Tests web         : $(-not $SkipWeb)"
Write-Host ""

Write-Check `
  -Label "Racine locale softwares" `
  -Ok (Test-Path $localSoftwaresRoot) `
  -Details $localSoftwaresRoot

foreach ($product in $products) {
  $id = [string]$product.id
  $displayName = [string]$product.displayName
  $script:CurrentProduct = $displayName
  $script:CurrentProductId = $id
  $type = [string]$product.type
  $category = [string]$product.category
  $version = [string]$product.version
  $artifactFile = [string]$product.artifactFile
  $hasSha256 = [bool]$product.hasSha256
  $hasUpdateJson = [bool]$product.hasUpdateJson
    $localCheck = [string]$product.localCheck

    if ([string]::IsNullOrWhiteSpace($localCheck)) {
      $localCheck = "optional"
    }
  Write-Title $displayName

  Write-Host "  Id       : $id"
  Write-Host "  Type     : $type"
  Write-Host "  Version  : $version"
  Write-Host "  Category : $category"
  Write-Host ""

  $productFolder =
    Join-Path `
      (Join-Path $localSoftwaresRoot $category) `
      $id

  $artifactPath =
    Join-Path $productFolder $artifactFile

  $shaPath =
    $artifactPath + ".sha256.txt"

  $updateJsonPath =
    Join-Path $productFolder "update.json"

  $artifactUrl =
    Join-Url `
      $softwaresUrl `
      $category `
      $id `
      $artifactFile

    $shaUrl = $artifactUrl + ".sha256.txt"

    $updateJsonUrl =
      Join-Url `
        $softwaresUrl `
        $category `
        $id `
        "update.json"

  $downloadUrl =
    New-DownloadUrl `
      -BaseUrl $softwaresUrl `
      -Category $category `
      -ProductId $id

if ($localCheck -eq "disabled") {
  Write-Check `
    -Label "Contrôle local" `
    -Ok $false `
    -Info $true `
    -Details "Désactivé pour ce produit."
}
else {
  $localFolderExists = Test-Path $productFolder
  $localArtifactExists = Test-Path $artifactPath

  Write-Check `
    -Label "Dossier web local" `
    -Ok $localFolderExists `
    -Warning ($localCheck -eq "optional") `
    -Details $productFolder

  Write-Check `
    -Label "Artefact local" `
    -Ok $localArtifactExists `
    -Warning ($localCheck -eq "optional") `
    -Details $artifactPath

  if ($hasSha256) {
    if ($localArtifactExists) {
      $shaResult =
        Test-Sha256File `
          -ArtifactPath $artifactPath `
          -ShaPath $shaPath

      Write-Check `
        -Label "SHA256 local" `
        -Ok $shaResult.Ok `
        -Warning ($localCheck -eq "optional") `
        -Details $shaResult.Details
    }
    else {
      Write-Check `
        -Label "SHA256 local" `
        -Ok $false `
        -Warning ($localCheck -eq "optional") `
        -Details "Artefact local absent, contrôle local ignoré."
    }
  }
}

if ($hasUpdateJson) {
  if ($localCheck -ne "disabled" -and (Test-Path $updateJsonPath)) {
    $updateResult =
      Test-UpdateJson `
        -UpdateJsonPath $updateJsonPath `
        -ExpectedVersion $version `
        -ExpectedArtifactUrl $artifactUrl

    Write-Check `
      -Label "update.json local" `
      -Ok $updateResult.Ok `
      -Details $updateResult.Details
  }
  elseif ($SkipWeb) {
    Write-Check `
      -Label "update.json" `
      -Ok $false `
      -Warning $true `
      -Details "Contrôle distant ignoré car -SkipWeb est actif."
  }
  else {
    $remoteUpdateResult =
      Test-RemoteUpdateJson `
        -Url $updateJsonUrl `
        -ExpectedVersion $version `
        -ExpectedArtifactUrl $artifactUrl

    Write-Check `
      -Label "update.json distant" `
      -Ok $remoteUpdateResult.Ok `
      -Details $remoteUpdateResult.Details
  }
}
else {
  Write-Check `
    -Label "update.json" `
    -Ok $false `
    -Warning $true `
    -Details "Non demandé pour ce produit."
}
  Write-Host ""
  Write-Host "  Download URL : $downloadUrl"
  Write-Host "  Artifact URL : $artifactUrl"

  if (-not $SkipWeb) {
    $artifactWebResult = Test-WebUrl -Url $artifactUrl

    Write-Check `
      -Label "URL artefact" `
      -Ok $artifactWebResult.Ok `
      -Details ("HTTP " + $artifactWebResult.StatusCode + " - " + $artifactUrl)

    $downloadWebResult = Test-WebUrl -Url $downloadUrl
if ($hasSha256) {
  $shaWebResult = Test-WebUrl -Url $shaUrl

  Write-Check `
    -Label "URL SHA256" `
    -Ok $shaWebResult.Ok `
    -Details ("HTTP " + $shaWebResult.StatusCode + " - " + $shaUrl)
}

    Write-Check `
      -Label "URL download.php" `
      -Ok $downloadWebResult.Ok `
      -Details ("HTTP " + $downloadWebResult.StatusCode + " - " + $downloadUrl)
  }
}

Write-Title "Summary"

Write-Host "OK       : $script:OkCount" -ForegroundColor Green
Write-Host "Infos    : $script:InfoCount" -ForegroundColor Cyan
Write-Host "Warnings : $script:WarningCount" -ForegroundColor Yellow
Write-Host "Errors   : $script:ErrorCount" -ForegroundColor Red

if ($Report) {
  Save-ReleaseReport `
    -RootDirectory $RootDirectory `
    -MainUrl $mainUrl `
    -SoftwaresUrl $softwaresUrl `
    -LocalSoftwaresRoot $localSoftwaresRoot `
    -SkipWeb $SkipWeb
}

if ($script:ErrorCount -gt 0) {
  exit 1
}

exit 0