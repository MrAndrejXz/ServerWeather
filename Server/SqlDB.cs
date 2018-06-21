using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Data;

namespace Server
{
    public static class SqlDB
    {
        public static DataTable QueryDB(string query)
        {
            DataTable Table = new DataTable();
            MySqlConnectionStringBuilder SqlConnection = new MySqlConnectionStringBuilder();
            SqlConnection.Server = "127.0.0.1";
            SqlConnection.Database = "new_schema"; SqlConnection.UserID = "root";
            SqlConnection.Password = "123";
            SqlConnection.CharacterSet = @"utf8";

            using (MySqlConnection connect = new MySqlConnection())
            {
                connect.ConnectionString = SqlConnection.ConnectionString;
                MySqlCommand command = new MySqlCommand(query, connect);
                try
                {
                    connect.Open();
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            Table.Load(reader);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
            return Table;
        }
    }
}
