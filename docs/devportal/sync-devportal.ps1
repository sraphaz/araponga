param(
  [switch]$WhatIf
)

$ErrorActionPreference = "Stop"

function Copy-FileUtf8([string]$Source, [string]$Destination) {
  if (!(Test-Path -LiteralPath $Source)) {
    throw "Arquivo não encontrado: $Source"
  }

  $content = Get-Content -LiteralPath $Source -Raw -Encoding UTF8

  if ($WhatIf) {
    Write-Host "[WhatIf] Escrever: $Destination"
    return
  }

  $destDir = Split-Path -Parent $Destination
  if (!(Test-Path -LiteralPath $destDir)) {
    New-Item -ItemType Directory -Path $destDir | Out-Null
  }

  Set-Content -LiteralPath $Destination -Value $content -Encoding UTF8
  Write-Host "Atualizado: $Destination"
}

function Sync-DevPortal {
  $repoRoot = Split-Path -Parent (Split-Path -Parent $PSScriptRoot)

  $docsIndex = Join-Path $repoRoot "docs\devportal\index.html"
  $docsCss   = Join-Path $repoRoot "docs\assets\css\devportal.css"
  $docsJs    = Join-Path $repoRoot "docs\assets\js\devportal.js"

  $wwwIndex = Join-Path $repoRoot "backend\Araponga.Api\wwwroot\devportal\index.html"
  $wwwCss   = Join-Path $repoRoot "backend\Araponga.Api\wwwroot\devportal\assets\css\devportal.css"
  $wwwJs    = Join-Path $repoRoot "backend\Araponga.Api\wwwroot\devportal\assets\js\devportal.js"

  # Fonte única: docs/
  Copy-FileUtf8 $docsCss $wwwCss
  Copy-FileUtf8 $docsJs $wwwJs

  # HTML precisa de ajustes de paths (docs/ e wwwroot/ usam estruturas diferentes)
  $html = Get-Content -LiteralPath $docsIndex -Raw -Encoding UTF8
  $html = $html.Replace('href="../assets/', 'href="./assets/')
  $html = $html.Replace('src="../assets/', 'src="./assets/')
  $html = $html.Replace('href="../openapi.json"', 'href="./openapi.json"')
  $html = $html.Replace('<script src="../assets/js/devportal.js"></script>', '<script src="./assets/js/devportal.js"></script>')

  if ($WhatIf) {
    Write-Host "[WhatIf] Escrever: $wwwIndex"
    return
  }

  Set-Content -LiteralPath $wwwIndex -Value $html -Encoding UTF8
  Write-Host "Atualizado: $wwwIndex"
}

Sync-DevPortal
