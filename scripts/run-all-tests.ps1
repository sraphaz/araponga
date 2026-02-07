#!/usr/bin/env pwsh
# Roda build e todos os testes antes do commit.
# Uso: ./scripts/run-all-tests.ps1
# Testes de stress são pulados por padrão; use $env:RUN_STRESS_TESTS="true" para incluí-los.

$ErrorActionPreference = "Stop"
$root = Split-Path -Parent (Split-Path -Parent $MyInvocation.MyCommand.Path)
Set-Location $root

Write-Host "Restore..." -ForegroundColor Cyan
dotnet restore Arah.sln
if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }

Write-Host "Build Release..." -ForegroundColor Cyan
dotnet build Arah.sln --no-restore --configuration Release
if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }

Write-Host "Testes Arah.Tests..." -ForegroundColor Cyan
dotnet test backend/Tests/Arah.Tests/Arah.Tests.csproj --no-build --configuration Release --verbosity minimal
if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }

Write-Host "Testes Arah.Tests.Bff..." -ForegroundColor Cyan
dotnet test backend/Tests/Arah.Tests.Bff/Arah.Tests.Bff.csproj --no-build --configuration Release --verbosity minimal
if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }

Write-Host "`nTodos os testes passaram." -ForegroundColor Green
