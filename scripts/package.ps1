$ErrorActionPreference = 'Stop'
$repositoryRoot = Split-Path $PSScriptRoot -Parent
Push-Location $repositoryRoot
try {
    $artifactRoot = Join-Path $repositoryRoot 'artifacts'
    New-Item -ItemType Directory -Path $artifactRoot -Force | Out-Null
    $packageDirectory = Join-Path $artifactRoot ('package-' + [guid]::NewGuid().ToString('N'))
    New-Item -ItemType Directory -Path $packageDirectory | Out-Null
    & (Join-Path $PSScriptRoot 'build.ps1')
    Copy-Item -Path 'IV_Calculator/bin/Release/*' -Destination $packageDirectory -Recurse -Exclude '*.pdb'
    Copy-Item -LiteralPath 'README.md','LICENSE','CONTRIBUTING.md','SECURITY.md' -Destination $packageDirectory
    Copy-Item -LiteralPath 'docs' -Destination $packageDirectory -Recurse
    $zipPath = Join-Path $artifactRoot 'PokemonIVCalculator-windows.zip'
    Compress-Archive -Path (Join-Path $packageDirectory '*') -DestinationPath $zipPath -Force
    $hash = (Get-FileHash -LiteralPath $zipPath -Algorithm SHA256).Hash.ToLowerInvariant()
    Set-Content -LiteralPath ($zipPath + '.sha256') -Value ($hash + '  ' + [IO.Path]::GetFileName($zipPath)) -Encoding ascii
    Write-Output $zipPath
} finally { Pop-Location }
