using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IV_Calculator
{
    public static class NatureDB
    {
        public static List<Nature> GetAllNatures()
        {
            List<Nature> natures = new List<Nature>();
            SqlConnection connection = PokemonDB.GetConnection();
            string selectStatement =
                "SELECT NatureID, NatureName, BoostedStat, LoweredStat " +
                "FROM Natures ORDER BY NatureName";
            SqlCommand selectCommand = new SqlCommand(selectStatement, connection);

            try
            {
                connection.Open();
                SqlDataReader reader = selectCommand.ExecuteReader();
                while (reader.Read())
                {
                    Nature n = new Nature();
                    n.NatureID = (int)reader["NatureID"];
                    n.NatureName = reader["NatureName"].ToString();
                    n.BoostedStat = reader["BoostedStat"].ToString();
                    n.LoweredStat = reader["LoweredStat"].ToString();
                    natures.Add(n);
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
            return natures;
        }
    }
}
