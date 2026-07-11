@echo off
cd /d "%~dp0"
pwsh -ExecutionPolicy Bypass -File ".\release\check-release.ps1" -Report
pause