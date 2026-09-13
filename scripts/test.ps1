$ErrorActionPreference = 'Stop'
$repositoryRoot = Split-Path $PSScriptRoot -Parent
& (Join-Path $PSScriptRoot 'build.ps1')
$vswherePath = Join-Path ${env:ProgramFiles(x86)} 'Microsoft Visual Studio/Installer/vswhere.exe'
$msbuildPath = & $vswherePath -latest -requires Microsoft.Component.MSBuild -find 'MSBuild/**/Bin/MSBuild.exe' | Select-Object -First 1
& $msbuildPath (Join-Path $repositoryRoot 'tests/RegressionTests.csproj') /restore /p:Configuration=Release /verbosity:minimal
if ($LASTEXITCODE -ne 0) { throw 'Test build failed.' }
$env:IV_CALCULATOR_TEST_ROOT = Join-Path $repositoryRoot 'artifacts/test-data'
try {
    & (Join-Path $repositoryRoot 'tests/bin/Release/RegressionTests.exe')
    if ($LASTEXITCODE -ne 0) { throw 'Calculator regression checks failed.' }
} finally { Remove-Item Env:IV_CALCULATOR_TEST_ROOT -ErrorAction SilentlyContinue }
