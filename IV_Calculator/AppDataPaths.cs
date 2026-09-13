using System;
using System.IO;

namespace IV_Calculator
{
    public static class AppDataPaths
    {
        public static string DirectoryPath
        {
            get
            {
                var custom = Environment.GetEnvironmentVariable("IV_CALCULATOR_DATA_DIR");
                var directory = string.IsNullOrWhiteSpace(custom)
                    ? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "PokemonIVCalculator")
                    : Path.GetFullPath(custom);
                Directory.CreateDirectory(directory);
                return directory;
            }
        }
        public static string HistoryPath => Path.Combine(DirectoryPath, "CalculationHistory.xml");
    }
}
