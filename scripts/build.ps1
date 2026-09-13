$ErrorActionPreference = 'Stop'
$repositoryRoot = Split-Path $PSScriptRoot -Parent
$vswherePath = Join-Path ${env:ProgramFiles(x86)} 'Microsoft Visual Studio/Installer/vswhere.exe'
if (-not (Test-Path -LiteralPath $vswherePath)) { throw 'Install Visual Studio with .NET desktop development and the .NET Framework 4.8 targeting pack.' }
$msbuildPath = & $vswherePath -latest -requires Microsoft.Component.MSBuild -find 'MSBuild/**/Bin/MSBuild.exe' | Select-Object -First 1
if (-not $msbuildPath) { throw 'Visual Studio MSBuild was not found.' }
& $msbuildPath (Join-Path $repositoryRoot 'IV_Calculator/IV_Calculator.csproj') /restore /p:Configuration=Release /p:DebugType=None /p:DebugSymbols=false /verbosity:minimal
if ($LASTEXITCODE -ne 0) { throw 'Calculator build failed.' }
