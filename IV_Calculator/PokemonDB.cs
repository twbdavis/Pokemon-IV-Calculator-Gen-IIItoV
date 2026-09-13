using System;
using System.Data.SqlClient;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace IV_Calculator
{
    public static class PokemonDB
    {
        private static string MdfPath => Path.Combine(AppDataPaths.DirectoryPath, "PokemonDB.mdf");
        public static string DatabaseName
        {
            get
            {
                using (var hash = SHA256.Create())
                {
                    var digest = hash.ComputeHash(Encoding.UTF8.GetBytes(MdfPath.ToUpperInvariant()));
                    return "PokemonIVCalculator_" + BitConverter.ToString(digest, 0, 12).Replace("-", "");
                }
            }
        }
        public static SqlConnection GetConnection()
        {
            return new SqlConnection(new SqlConnectionStringBuilder
            {
                DataSource = @"(LocalDB)\MSSQLLocalDB",
                AttachDBFilename = MdfPath,
                InitialCatalog = DatabaseName,
                IntegratedSecurity = true
            }.ConnectionString);
        }
        public static void EnsureDatabaseFileExists()
        {
            if (File.Exists(MdfPath)) return;
            using (var connection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=master;Integrated Security=True"))
            {
                connection.Open();
                using (var check = new SqlCommand("SELECT COUNT(*) FROM sys.databases d JOIN sys.master_files f ON d.database_id = f.database_id WHERE d.name = @name OR f.physical_name = @path", connection))
                {
                    check.Parameters.AddWithValue("@name", DatabaseName);
                    check.Parameters.AddWithValue("@path", MdfPath);
                    if ((int)check.ExecuteScalar() != 0)
                        throw new InvalidOperationException("The calculator database is registered but its data file is missing. Restore your backup or choose a new IV_CALCULATOR_DATA_DIR. No existing database has been changed.");
                }
                var logPath = Path.Combine(AppDataPaths.DirectoryPath, "PokemonDB_log.ldf");
                if (File.Exists(logPath))
                    throw new InvalidOperationException("A database log exists without its data file. Restore your backup or choose a new IV_CALCULATOR_DATA_DIR. The existing log has been preserved.");
                // The catalog is unique to this application's data directory.
                // Never drop another database to resolve a name or file collision.
                var sql = "CREATE DATABASE [" + DatabaseName + "] ON PRIMARY (NAME = 'PokemonData', FILENAME = '"
                    + MdfPath.Replace("'", "''") + "') LOG ON (NAME = 'PokemonLog', FILENAME = '"
                    + logPath.Replace("'", "''") + "')";
                using (var create = new SqlCommand(sql, connection)) create.ExecuteNonQuery();
            }
        }
    }
}
