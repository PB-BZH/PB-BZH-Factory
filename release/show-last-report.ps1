param(
  [switch]$Json,
  [switch]$List,
  [switch]$Folder
)

$ErrorActionPreference = "Stop"
[Console]::OutputEncoding = [System.Text.Encoding]::UTF8

$ScriptDirectory = $PSScriptRoot
$RootDirectory = Split-Path -Parent $ScriptDirectory
$ReportsDirectory = Join-Path $RootDirectory "reports"

function Write-Title {
  param([string]$Text)

  Write-Host ""
  Write-Host "==================================================" -ForegroundColor DarkCyan
  Write-Host $Text -ForegroundColor Cyan
  Write-Host "==================================================" -ForegroundColor DarkCyan
}

Write-Title "PB BZH Last Release Report"

if (-not (Test-Path $ReportsDirectory)) {
  Write-Host "[WARN] Reports directory not found : $ReportsDirectory" -ForegroundColor Yellow
  Write-Host ""
  Write-Host "Generate a report first with:"
  Write-Host "pwsh -ExecutionPolicy Bypass -File .\release\check-release.ps1 -Report" -ForegroundColor Cyan
  exit 1
}

if ($Folder) {
  Write-Host "[INFO] Opening reports folder : $ReportsDirectory" -ForegroundColor Cyan
  Invoke-Item $ReportsDirectory
  exit 0
}

$extension = "*.md"

if ($Json) {
  $extension = "*.json"
}

$reports =
  Get-ChildItem `
    -Path $ReportsDirectory `
    -Filter "release-check_$extension" `
    -File `
  | Sort-Object LastWriteTime -Descending

if ($reports.Count -eq 0) {
  Write-Host "[WARN] No report found in : $ReportsDirectory" -ForegroundColor Yellow
  Write-Host ""
  Write-Host "Generate a report first with:"
  Write-Host "pwsh -ExecutionPolicy Bypass -File .\release\check-release.ps1 -Report" -ForegroundColor Cyan
  exit 1
}

if ($List) {
  Write-Host "Last reports:" -ForegroundColor Cyan
  Write-Host ""

  $reports |
    Select-Object -First 10 |
    ForEach-Object {
      Write-Host ("  " + $_.LastWriteTime.ToString("yyyy-MM-dd HH:mm:ss") + "  " + $_.Name)
    }

  exit 0
}

$latestReport =
  $reports |
  Select-Object -First 1

Write-Host "[OK] Latest report found:" -ForegroundColor Green
Write-Host "     $($latestReport.FullName)" -ForegroundColor DarkGray
Write-Host ""

Invoke-Item $latestReport.FullName

exit 0