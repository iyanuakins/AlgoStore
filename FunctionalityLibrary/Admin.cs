using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace FunctionalityLibrary
{
    class Admin : User
    {
        public Admin(int id, string name, string email, DateTime dateRegistered) 
            :base(id, name, email,dateRegistered ){ }

        public bool AddProduct(string name, string description, double price, int units)
        {
            using (SQLiteConnection connection = Helper.ConnectToDb())
            {
                string query = "INSERT INTO products VALUES(@name, @description, @price, @units, @date)";
                SQLiteCommand command = new SQLiteCommand(query, connection);
                command.Parameters.AddWithValue("@name", name);
                command.Parameters.AddWithValue("@description ", description);
                command.Parameters.AddWithValue("@price", price);
                command.Parameters.AddWithValue("@units", units);
                command.Parameters.AddWithValue("@date", DateTime.Now.ToString());

                try
                {
                    connection.Open();
                    int row = command.ExecuteNonQuery();
                    return row > 0;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return false;
                }
            }
        }

        public bool UpdateProduct(int productId, string name = "", string description = "", double price = 0.0, int units = 0)
        {
            using (SQLiteConnection connection = Helper.ConnectToDb())
            {
                string query;
                SQLiteCommand command;
                if (name != "" && description != "" && price != 0.0 && units != 0)
                {
                    query = "UPDATE products SET productname = @name, productdescription = @description, price = @price, units = units + @units WHERE productid = @productId";
                    command = new SQLiteCommand(query, connection);
                    command.Parameters.AddWithValue("@name", name);
                    command.Parameters.AddWithValue("@description ", description);
                    command.Parameters.AddWithValue("@price", price);
                    command.Parameters.AddWithValue("@units", units);
                    command.Parameters.AddWithValue("@productId", productId);
                }
                else if(name != "" && description != "")
                {
                    query = "UPDATE products SET productname = @name, productdescription = @description WHERE productid = @productId";
                    command = new SQLiteCommand(query, connection);
                    command.Parameters.AddWithValue("@name", name);
                    command.Parameters.AddWithValue("@description ", description);
                    command.Parameters.AddWithValue("@productId", productId);
                }
                else if (price != 0.0 && units != 0)
                {
                    query = "UPDATE products SET price = @price, units = units + @units WHERE productid = @productId";
                    command = new SQLiteCommand(query, connection);
                    command.Parameters.AddWithValue("@price", price);
                    command.Parameters.AddWithValue("@units", units);
                    command.Parameters.AddWithValue("@productId", productId);
                }
                else if (price != 0.0)
                {
                    query = "UPDATE products SET price = @price WHERE productid = @productId";
                    command = new SQLiteCommand(query, connection);
                    command.Parameters.AddWithValue("@price", price);
                    command.Parameters.AddWithValue("@productId", productId);
                }
                else
                {
                    query = "UPDATE products SET units = units + @units WHERE productid = @productId";
                    command = new SQLiteCommand(query, connection);
                    command.Parameters.AddWithValue("@units", units);
                    command.Parameters.AddWithValue("@productId", productId);
                }

                try
                {
                    connection.Open();
                    int row = command.ExecuteNonQuery();
                    return row > 0;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return false;
                }
            }
        }

        public List<Order> ViewAllOrders()
        {
            using (SQLiteConnection connection = Helper.ConnectToDb())
            {
                try
                {
                    string query = "SELECT * FROM orders";
                    SQLiteCommand command = new SQLiteCommand(query, connection);
                    connection.Open();
                    SQLiteDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        List<Order> allOrders = new List<Order>();
                        while (reader.Read())
                        {
                            string productString = reader.GetString(3);
                            string[] productIds = productString.Split(',');
                            List<Product> products = new List<Product>();
                            foreach (var id in productIds)
                            {
                                products.Add(Helper.GetProduct(int.Parse(id)));
                            }

                            allOrders.Add(new Order(
                                                    reader.GetInt32(0),
                                                    reader.GetInt32(1),
                                                    reader.GetInt32(2),
                                                    products,
                                                    Convert.ToDouble(reader.GetFloat(4)),
                                                    Convert.ToDateTime(reader.GetString(5))
                                                ));
                        }

                        return allOrders;
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
