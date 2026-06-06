using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IV_Calculator
{
    public static class ExamplePokemonDB
    {
        public static List<ExamplePokemon> GetAllExamples()
        {
            List<ExamplePokemon> exampleList = new List<ExamplePokemon>();
            SqlConnection connection = PokemonDB.GetConnection();

            string selectStatement =
                "SELECT ExampleID, PokemonName, Level, NatureName, " +
                "HP, Attack, Defense, SpAttack, SpDefense, Speed, " +
                "EVHP, EVAttack, EVDefense, EVSpAttack, EVSpDefense, EVSpeed " +
                "FROM ExamplePokemon";

            SqlCommand selectCommand = new SqlCommand(selectStatement, connection);

            try
            {
                connection.Open();
                SqlDataReader reader = selectCommand.ExecuteReader();

                while (reader.Read())
                {
                    ExamplePokemon ex = new ExamplePokemon
                    {
                        ExampleID = (int)reader["ExampleID"],
                        Name = reader["PokemonName"].ToString(),
                        Level = (int)reader["Level"],
                        Nature = reader["NatureName"].ToString(),
                        StatHP = (int)reader["HP"],
                        StatAttack = (int)reader["Attack"],
                        StatDefense = (int)reader["Defense"],
                        StatSpAttack = (int)reader["SpAttack"],
                        StatSpDefense = (int)reader["SpDefense"],
                        StatSpeed = (int)reader["Speed"],
                        EVHP = (int)reader["EVHP"],
                        EVAttack = (int)reader["EVAttack"],
                        EVDefense = (int)reader["EVDefense"],
                        EVSpAttack = (int)reader["EVSpAttack"],
                        EVSpDefense = (int)reader["EVSpDefense"],
                        EVSpeed = (int)reader["EVSpeed"]
                    };
                    exampleList.Add(ex);
                }
                reader.Close();
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                connection.Close();
            }

            return exampleList;
        }

        public static ExamplePokemon GetRandomExample()
        {
            List<ExamplePokemon> allExamples = GetAllExamples();
            if (allExamples.Count == 0)
                return null;

            Random random = new Random();
            int randomIndex = random.Next(allExamples.Count);
            return allExamples[randomIndex];
        }
  

        public static bool AddExample(ExamplePokemon example)
        {
            SqlConnection connection = PokemonDB.GetConnection();

            // Simpler duplicate check - just check name, level, nature, and HP
            string checkSql = @"
        SELECT COUNT(*) FROM ExamplePokemon 
        WHERE PokemonName = @Name 
        AND Level = @Level 
        AND NatureName = @Nature
        AND HP = @HP";

            string insertSql = @"
        INSERT INTO ExamplePokemon 
        (PokemonName, Level, NatureName, HP, Attack, Defense, SpAttack, SpDefense, Speed,
         EVHP, EVAttack, EVDefense, EVSpAttack, EVSpDefense, EVSpeed)
        VALUES 
        (@Name, @Level, @Nature, @HP, @Attack, @Defense, @SpAttack, @SpDefense, @Speed,
         @EVHP, @EVAttack, @EVDefense, @EVSpAttack, @EVSpDefense, @EVSpeed)";

            try
            {
                connection.Open();

                // Check for duplicate first
                using (SqlCommand checkCmd = new SqlCommand(checkSql, connection))
                {
                    checkCmd.Parameters.AddWithValue("@Name", example.Name);
                    checkCmd.Parameters.AddWithValue("@Level", example.Level);
                    checkCmd.Parameters.AddWithValue("@Nature", example.Nature);
                    checkCmd.Parameters.AddWithValue("@HP", example.StatHP);

                    int count = (int)checkCmd.ExecuteScalar();
                    if (count > 0)
                    {
                        MessageBox.Show(
                            "This Pokémon already exists in the examples!",
                            "Duplicate Entry",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);
                        return false;
                    }
                }

                // Insert if not a duplicate - NEW SqlCommand with its own parameters
                using (SqlCommand insertCmd = new SqlCommand(insertSql, connection))
                {
                    insertCmd.Parameters.AddWithValue("@Name", example.Name);
                    insertCmd.Parameters.AddWithValue("@Level", example.Level);
                    insertCmd.Parameters.AddWithValue("@Nature", example.Nature);
                    insertCmd.Parameters.AddWithValue("@HP", example.StatHP);
                    insertCmd.Parameters.AddWithValue("@Attack", example.StatAttack);
                    insertCmd.Parameters.AddWithValue("@Defense", example.StatDefense);
                    insertCmd.Parameters.AddWithValue("@SpAttack", example.StatSpAttack);
                    insertCmd.Parameters.AddWithValue("@SpDefense", example.StatSpDefense);
                    insertCmd.Parameters.AddWithValue("@Speed", example.StatSpeed);
                    insertCmd.Parameters.AddWithValue("@EVHP", example.EVHP);
                    insertCmd.Parameters.AddWithValue("@EVAttack", example.EVAttack);
                    insertCmd.Parameters.AddWithValue("@EVDefense", example.EVDefense);
                    insertCmd.Parameters.AddWithValue("@EVSpAttack", example.EVSpAttack);
                    insertCmd.Parameters.AddWithValue("@EVSpDefense", example.EVSpDefense);
                    insertCmd.Parameters.AddWithValue("@EVSpeed", example.EVSpeed);

                    insertCmd.ExecuteNonQuery();
                }
                return true;
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Error adding example: " + ex.Message, "Database Error");
                return false;
            }
            finally
            {
                connection.Close();
            }

        }

        public static void ResetToDefaults()
        {
            SqlConnection connection = PokemonDB.GetConnection();

            try
            {
                connection.Open();

                // Clear all existing examples
                string deleteSql = "DELETE FROM ExamplePokemon";
                using (SqlCommand cmd = new SqlCommand(deleteSql, connection))
                {
                    cmd.ExecuteNonQuery();
                }
            }
            finally
            {
                connection.Close();
            }

            // Re-run the seeding (call SeedDatabase which will now find 0 records)
            DatabaseInitializer.SeedDatabase();
        }

        public static bool ExampleExists(ExamplePokemon example)
        {
            SqlConnection connection = PokemonDB.GetConnection();

            string checkSql = @"
        SELECT COUNT(*) FROM ExamplePokemon 
        WHERE PokemonName = @Name 
        AND Level = @Level 
        AND NatureName = @Nature
        AND HP = @HP";

            try
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand(checkSql, connection))
                {
                    cmd.Parameters.AddWithValue("@Name", example.Name);
                    cmd.Parameters.AddWithValue("@Level", example.Level);
                    cmd.Parameters.AddWithValue("@Nature", example.Nature);
                    cmd.Parameters.AddWithValue("@HP", example.StatHP);

                    int count = (int)cmd.ExecuteScalar();
                    return count > 0;
                }
            }
            finally
            {
                connection.Close();
            }
        }
    }
    
}

