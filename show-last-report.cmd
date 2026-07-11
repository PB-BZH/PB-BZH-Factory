@echo off
cd /d "%~dp0"
pwsh -ExecutionPolicy Bypass -File ".\release\show-last-report.ps1"
pause