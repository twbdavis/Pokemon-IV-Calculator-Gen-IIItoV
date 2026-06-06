using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IV_Calculator
{
    public static class PokemonDataDB
    {
        public static List<Pokemon> GetAllPokemon()
        {
            List<Pokemon> pokemonList = new List<Pokemon>();
            SqlConnection connection = PokemonDB.GetConnection();
            string selectStatement =
                "SELECT PokemonID, Name, BaseHP, BaseAttack, BaseDefense, " +
                "BaseSpAttack, BaseSpDefense, BaseSpeed " +
                "FROM Pokemon ORDER BY Name";
            SqlCommand selectCommand = new SqlCommand(selectStatement, connection);

            try
            {
                connection.Open();
                SqlDataReader reader = selectCommand.ExecuteReader();
                while (reader.Read())
                {
                    Pokemon p = new Pokemon();
                    p.PokemonID = (int)reader["PokemonID"];
                    p.Name = reader["Name"].ToString();
                    p.BaseHP = (int)reader["BaseHP"];
                    p.BaseAttack = (int)reader["BaseAttack"];
                    p.BaseDefense = (int)reader["BaseDefense"];
                    p.BaseSpAttack = (int)reader["BaseSpAttack"];
                    p.BaseSpDefense = (int)reader["BaseSpDefense"];
                    p.BaseSpeed = (int)reader["BaseSpeed"];
                    pokemonList.Add(p);
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
            return pokemonList;
        }

        public static Pokemon GetPokemonByName(string name)
        {
            SqlConnection connection = PokemonDB.GetConnection();
            string selectStatement =
                "SELECT PokemonID, Name, BaseHP, BaseAttack, BaseDefense, " +
                "BaseSpAttack, BaseSpDefense, BaseSpeed " +
                "FROM Pokemon WHERE Name = @Name";
            SqlCommand selectCommand = new SqlCommand(selectStatement, connection);
            selectCommand.Parameters.AddWithValue("@Name", name);

            try
            {
                connection.Open();
                SqlDataReader reader = selectCommand.ExecuteReader();
                if (reader.Read())
                {
                    Pokemon p = new Pokemon();
                    p.PokemonID = (int)reader["PokemonID"];
                    p.Name = reader["Name"].ToString();
                    p.BaseHP = (int)reader["BaseHP"];
                    p.BaseAttack = (int)reader["BaseAttack"];
                    p.BaseDefense = (int)reader["BaseDefense"];
                    p.BaseSpAttack = (int)reader["BaseSpAttack"];
                    p.BaseSpDefense = (int)reader["BaseSpDefense"];
                    p.BaseSpeed = (int)reader["BaseSpeed"];
                    return p;
                }
                return null;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                connection.Close();
            }
        }
    }
}
