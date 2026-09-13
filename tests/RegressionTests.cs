using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using IV_Calculator;

internal static class RegressionTests
{
    private static int count;
    private static void Check(bool condition, string name)
    {
        if (!condition) throw new Exception(name);
        Console.WriteLine("PASS " + name);
        count++;
    }

    [STAThread]
    private static int Main()
    {
        var testRoot = Environment.GetEnvironmentVariable("IV_CALCULATOR_TEST_ROOT");
        if (string.IsNullOrWhiteSpace(testRoot)) throw new Exception("Set IV_CALCULATOR_TEST_ROOT to an isolated test folder.");
        var testDirectory = Path.Combine(Path.GetFullPath(testRoot), Guid.NewGuid().ToString("N"));
        var originalData = Environment.GetEnvironmentVariable("IV_CALCULATOR_DATA_DIR");
        var originalDirectory = Environment.CurrentDirectory;
        Environment.SetEnvironmentVariable("IV_CALCULATOR_DATA_DIR", testDirectory);
        try
        {
            var originalName = PokemonDB.DatabaseName;
            Environment.SetEnvironmentVariable("IV_CALCULATOR_DATA_DIR", testDirectory + "-other");
            Check(originalName != PokemonDB.DatabaseName, "Different data directories use different database names");
            Environment.SetEnvironmentVariable("IV_CALCULATOR_DATA_DIR", testDirectory);

            var entries = new List<CalculationHistory>
            {
                new CalculationHistory { Pokemon = "Bulbasaur", Level = 5, Nature = "Modest", HPIVResult = "10-29", CalculationTime = new DateTime(2026, 9, 12) },
                new CalculationHistory { Pokemon = "Pikachu", Level = 50, Nature = "Hardy", HPIVResult = "31", CalculationTime = new DateTime(2026, 9, 12) }
            };
            CalculationHistoryDB.SaveHistory(entries);
            Check(XDocument.Load(AppDataPaths.HistoryPath).Root.Elements("Entry").Count() == 2, "Saved records are siblings, not nested XML");
            Environment.CurrentDirectory = Path.GetDirectoryName(testDirectory);
            var restored = CalculationHistoryDB.GetHistory();
            Check(restored.Count == 2 && restored[1].Pokemon == "Pikachu", "History survives a different working directory");

            DatabaseInitializer.SeedDatabase();
            DatabaseInitializer.SeedDatabase();
            using (var connection = PokemonDB.GetConnection())
            {
                connection.Open();
                using (var command = new SqlCommand("SELECT COUNT(*) FROM Natures", connection))
                    Check((int)command.ExecuteScalar() == 25, "Natures seed exactly once");
                using (var command = new SqlCommand("SELECT COUNT(*) FROM ExamplePokemon", connection))
                    Check((int)command.ExecuteScalar() == 8, "Fresh installations have examples without duplication");
            }
            using (var form = new FRMCalculator())
            {
                Check(form.CalculateHPIVRange(20, 5, 0, 45) == "10-29", "Bulbasaur worked example");
                Check(form.CalculateHPIVRange(231, 100, 0, 45) == "31", "Level 100 exact HP IV");
                Check(form.CalculateHPIVRange(999, 5, 0, 45) == "Impossible", "Impossible observed HP is rejected");
            }
            Console.WriteLine(count + " regression checks passed.");
            return 0;
        }
        catch (Exception ex) { Console.Error.WriteLine(ex); return 1; }
        finally
        {
            Environment.CurrentDirectory = originalDirectory;
            Environment.SetEnvironmentVariable("IV_CALCULATOR_DATA_DIR", originalData);
            // Keep the isolated data folder for diagnosing a failed LocalDB check.
            // No existing user database or history is modified by these checks.
        }
    }
}
