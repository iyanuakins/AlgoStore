using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SQLite;
using System.Threading;

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

        public static void Register(string name, string email, string password)
        {
            using (SQLiteConnection connection = Helper.ConnectToDb())
            {
                string query =
                    "INSERT INTO order VALUES(@name, @email, @type, @password, @date, @totalOrder)";
                SQLiteCommand command = new SQLiteCommand(query, connection);
                command.Parameters.AddWithValue("@name", name);
                command.Parameters.AddWithValue("@email", email);
                command.Parameters.AddWithValue("@type", "customer");
                command.Parameters.AddWithValue("@password", password);
                command.Parameters.AddWithValue("@date", DateTime.Now.ToString());
                command.Parameters.AddWithValue("@totalOrder", 0);

                try
                {
                    connection.Open();
                    int row = command.ExecuteNonQuery();
                    Console.WriteLine(row > 0
                        ? "Registration successful, proceed to login"
                        : "Registration failed, please retry");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
        }
    }
}
