using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SQLite;

namespace FunctionalityLibrary
{
    public class Helper
    {
        public static SQLiteConnection ConnectToDb()
        {
            string connectionstring = ConfigurationManager.ConnectionStrings["SqliteConnectionString"].ConnectionString;
            return new SQLiteConnection(connectionstring);
            
        }

        public static Product GetProduct(int productId)
        {
            using (SQLiteConnection connection = Helper.ConnectToDb())
            {
                try
                {
                    string query = "SELECT * FROM products where id = @id";
                    SQLiteCommand command = new SQLiteCommand(query, connection);
                    command.Parameters.AddWithValue("@id", productId);
                    SQLiteDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        return new Product(
                                            productId,
                                            reader.GetString(1),
                                            reader.GetString(2),
                                            Convert.ToDouble(reader.GetFloat(3)),
                                            reader.GetInt32(4),
                                            Convert.ToDateTime(reader.GetString(5))
                                            );
                    }
                    else
                    {
                        return null;
                    }

                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return null;
                }
            }
        }

    }
}
