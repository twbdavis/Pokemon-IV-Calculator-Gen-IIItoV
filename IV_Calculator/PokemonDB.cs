using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IV_Calculator
{
    public static class PokemonDB
    {
        public static SqlConnection GetConnection()
        {
            string connectionString =
                "Data Source=(LocalDB)\\MSSQLLocalDB;" +
                "AttachDbFilename=|DataDirectory|\\PokemonDB.mdf;" +
                "Integrated Security=True";
            SqlConnection connection = new SqlConnection(connectionString);
            return connection;
        }

        private static string GetMdfPath()
        {
            string dataDirectory = AppDomain.CurrentDomain.GetData("DataDirectory") as string;
            if (string.IsNullOrEmpty(dataDirectory))
            {
                dataDirectory = AppDomain.CurrentDomain.BaseDirectory;
            }
            return Path.Combine(dataDirectory, "PokemonDB.mdf");
        }

        public static void EnsureDatabaseFileExists()
        {
            string mdfPath = GetMdfPath();
            if (File.Exists(mdfPath))
            {
                return;
            }

            string ldfPath = Path.Combine(Path.GetDirectoryName(mdfPath), "PokemonDB_log.ldf");

            string masterConnectionString =
                "Data Source=(LocalDB)\\MSSQLLocalDB;" +
                "Integrated Security=True";

            using (SqlConnection connection = new SqlConnection(masterConnectionString))
            {
                connection.Open();

                // Drop any leftover registration from a previous copy of the file,
                // otherwise CREATE DATABASE fails with a name/file conflict.
                string dropStatement = @"
                    DECLARE @staleName sysname;
                    SELECT TOP 1 @staleName = d.name
                    FROM sys.databases d
                    JOIN sys.master_files f ON d.database_id = f.database_id
                    WHERE f.physical_name = @mdfPath OR d.name = 'PokemonDB';
                    IF @staleName IS NOT NULL
                    BEGIN
                        DECLARE @dropSql nvarchar(max) = N'DROP DATABASE ' + QUOTENAME(@staleName) + N';';
                        EXEC(@dropSql);
                    END;";
                using (SqlCommand dropCommand = new SqlCommand(dropStatement, connection))
                {
                    dropCommand.Parameters.AddWithValue("@mdfPath", mdfPath);
                    dropCommand.ExecuteNonQuery();
                }

                // Create a fresh database at the expected location, then detach it so
                // the AttachDbFilename connection string can attach it normally.
                string createStatement =
                    "CREATE DATABASE [PokemonDB] " +
                    "ON PRIMARY (NAME = 'PokemonDB', FILENAME = '" + mdfPath.Replace("'", "''") + "') " +
                    "LOG ON (NAME = 'PokemonDB_log', FILENAME = '" + ldfPath.Replace("'", "''") + "'); " +
                    "EXEC sp_detach_db 'PokemonDB';";
                using (SqlCommand createCommand = new SqlCommand(createStatement, connection))
                {
                    createCommand.ExecuteNonQuery();
                }
            }
        }
    }
}
