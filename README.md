IV Calculator
A Windows Forms application for calculating Pokémon IV ranges, hidden power type, and saving calculation history. It uses a local SQL Server LocalDB database for Pokémon and nature data, and stores calculation history as XML.
Features

Calculate IV ranges for all six stats: HP, Attack, Defense, Special Attack, Special Defense, and Speed
Compute possible Hidden Power types from IV parities
Load example Pokémon entries with pre-filled level, nature, stats, and EVs
Save calculations to history, view them in a separate form, and clear or remove individual entries
Persist history across sessions using CalculationHistory.xml
Seed Pokémon base stats and nature tables automatically on first run

Project Structure

FRMCalculator.cs - Main calculator form, input validation, IV and hidden power calculations
FRMHistory.cs - History viewer form with row delete and clear functionality
DatabaseIntializer.cs - Ensures database tables exist and seeds initial Pokémon and nature data
PokemonDB.cs - Database connection helper for the local MDF file
PokemonDataDB.cs - Queries Pokémon base stats from the database
CalculationHistoryDB.cs - Reads and writes calculation history to XML
CalculationHistory.cs - Model for saved calculation records
ExamplePokemon.cs / ExamplePokemonDB.cs - Support for example Pokémon load functionality
PokemonBaseStats.cs, Nature.cs, Pokemon.cs - Domain models used by the application
PokemonDB.mdf / PokemonDB_log.ldf - LocalDB files containing seeded application data

Requirements

Windows
.NET Framework 4.8
Microsoft SQL Server Express LocalDB (installed and available as (LocalDB)\MSSQLLocalDB)
Visual Studio with Windows Forms support

Build and Run

Open IV_Calculator.csproj in Visual Studio
Restore or verify references for .NET Framework 4.8
Build the solution
Run the project

The application initializes the database schema and seeds Pokémon and nature data on first launch.
Usage

Select a Pokémon from the dropdown
Enter the level, nature, current stats, and EV values
Click Calculate to compute IV ranges and Hidden Power type
Use Load Example to populate input fields from saved example entries
Open History to review saved calculations and manage records

Notes

History is persisted in CalculationHistory.xml
The app prevents duplicate history entries for the same Pokémon, level, nature, stats, and EVs
Hidden Power calculations use IV parity values for the Generation 3 to 5 formula so no stat changes from Gen 6 and onward appear.

License
This repository is public. Fork, modify, or contribute improvements.
