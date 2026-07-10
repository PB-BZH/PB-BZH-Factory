param(
  [switch]$SkipWeb
)

$ErrorActionPreference = "Stop"
[Console]::OutputEncoding = [System.Text.Encoding]::UTF8

$ScriptDirectory = $PSScriptRoot
$RootDirectory = Split-Path -Parent $ScriptDirectory
$ProductsFile = Join-Path $RootDirectory "products\products.json"

$script:ErrorCount = 0
$script:WarningCount = 0
$script:OkCount = 0

function Write-Title {
  param([string]$Text)

  Write-Host ""
  Write-Host "==================================================" -ForegroundColor DarkCyan
  Write-Host $Text -ForegroundColor Cyan
  Write-Host "==================================================" -ForegroundColor DarkCyan
}

function Write-Check {
  param(
    [string]$Label,
    [bool]$Ok,
    [string]$Details = "",
    [bool]$Warning = $false
  )

  if ($Ok) {
    $script:OkCount++
    Write-Host ("  [OK]   " + $Label) -ForegroundColor Green
    if (-not [string]::IsNullOrWhiteSpace($Details)) {
      Write-Host ("         " + $Details) -ForegroundColor DarkGray
    }
    return
  }

  if ($Warning) {
    $script:WarningCount++
    Write-Host ("  [WARN] " + $Label) -ForegroundColor Yellow
    if (-not [string]::IsNullOrWhiteSpace($Details)) {
      Write-Host ("         " + $Details) -ForegroundColor Yellow
    }
    return
  }

  $script:ErrorCount++
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
  $type = [string]$product.type
  $category = [string]$product.category
  $version = [string]$product.version
  $artifactFile = [string]$product.artifactFile
  $hasSha256 = [bool]$product.hasSha256
  $hasUpdateJson = [bool]$product.hasUpdateJson

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

  $downloadUrl =
    New-DownloadUrl `
      -BaseUrl $softwaresUrl `
      -Category $category `
      -ProductId $id

  Write-Check `
    -Label "Dossier web local" `
    -Ok (Test-Path $productFolder) `
    -Details $productFolder

  Write-Check `
    -Label "Artefact local" `
    -Ok (Test-Path $artifactPath) `
    -Details $artifactPath

  if ($hasSha256) {
    $shaResult =
      Test-Sha256File `
        -ArtifactPath $artifactPath `
        -ShaPath $shaPath

    Write-Check `
      -Label "SHA256" `
      -Ok $shaResult.Ok `
      -Details $shaResult.Details
  }
  else {
    Write-Check `
      -Label "SHA256" `
      -Ok $false `
      -Warning $true `
      -Details "Non demandé pour ce produit."
  }

  if ($hasUpdateJson) {
    $updateResult =
      Test-UpdateJson `
        -UpdateJsonPath $updateJsonPath `
        -ExpectedVersion $version `
        -ExpectedArtifactUrl $artifactUrl

    Write-Check `
      -Label "update.json" `
      -Ok $updateResult.Ok `
      -Details $updateResult.Details
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

    Write-Check `
      -Label "URL download.php" `
      -Ok $downloadWebResult.Ok `
      -Details ("HTTP " + $downloadWebResult.StatusCode + " - " + $downloadUrl)
  }
}

Write-Title "Summary"

Write-Host "OK       : $script:OkCount" -ForegroundColor Green
Write-Host "Warnings : $script:WarningCount" -ForegroundColor Yellow
Write-Host "Errors   : $script:ErrorCount" -ForegroundColor Red

if ($script:ErrorCount -gt 0) {
  exit 1
}

exit 0