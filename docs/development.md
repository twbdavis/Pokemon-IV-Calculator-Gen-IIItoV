# Build and troubleshooting

## Prerequisites

Install Visual Studio with the **.NET desktop development** workload and .NET
Framework 4.8 targeting pack, plus SQL Server Express LocalDB. Windows is required.

From the repository root:

```powershell
./scripts/build.ps1
./scripts/test.ps1
./IV_Calculator/bin/Release/IV_Calculator.exe
```

Alternatively, open IV_Calculator/IV_Calculator.csproj in Visual Studio and build.
The build script locates MSBuild using Visual Studio Installer's vswhere utility.

## Troubleshooting

- **LocalDB unavailable:** verify installation with `SqlLocalDB info MSSQLLocalDB`.
- **Missing data file or existing log:** restore your own backup or select a fresh
  IV_CALCULATOR_DATA_DIR. The application deliberately preserves conflicting data.
- **History missing after upgrade:** copy the older CalculationHistory.xml into
  the new user-data folder with the app closed. Back up both copies first.
- **No examples:** current startup seeds an empty example table automatically.
- **Impossible IVs:** check the species, game-specific base stats, nature, EVs,
  observed stat, and level. Different forms and generations may have different data.

## Tests and packaging

The regression executable tests history round trips, working-directory independence,
calculator results, and isolated first-run database seeding. It creates its own test
data directory. The database integration portion requires LocalDB.

Run `scripts/package.ps1` to produce a ZIP and SHA-256 checksum in artifacts.
The package requires .NET Framework 4.8 and LocalDB on the destination machine.
Packaging does not publish a release or change Git history.
