using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace FunctionalityLibrary
{
    public class Admin : User
    {
        public Admin(int id, string name, string email, DateTime dateRegistered)
            : base(id, name, email, dateRegistered) { }

        public bool AddProduct(string name, string description, double price, int units)
        {
            string query = "INSERT INTO products VALUES(@Id, @name, @description, @price, @units, @date)";
            using (SQLiteConnection connection = Helper.ConnectToDb())
            using (SQLiteCommand command = new SQLiteCommand(query, connection))
            using (SQLiteDataAdapter adapter = new SQLiteDataAdapter())
            {
                command.Parameters.AddWithValue("@name", name);
                command.Parameters.AddWithValue("@Id", null);
                command.Parameters.AddWithValue("@description", description);
                command.Parameters.AddWithValue("@price", (float)price);
                command.Parameters.AddWithValue("@units", units);
                command.Parameters.AddWithValue("@date", DateTime.Now.ToString());

                try
                {
                    connection.Open();
                    adapter.InsertCommand = command;
                    int row = adapter.InsertCommand.ExecuteNonQuery();
                    return row > 0;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return false;
                }
            }
        }

        public void UpdateProduct(int productId, string name = "", string description = "", double price = 0.0, int units = 0)
        {
            string query;
            if (name != "" && description != "")
            {
                query = "UPDATE products SET productname = @name, productdescription" +
                        " = @description WHERE productid = @productId";

                using (SQLiteConnection connection = Helper.ConnectToDb())
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                using (SQLiteDataAdapter adapter = new SQLiteDataAdapter())
                {
                    command.Parameters.AddWithValue("@name", name);
                    command.Parameters.AddWithValue("@description", description);
                    command.Parameters.AddWithValue("@productId", productId);
                    try
                    {
                        connection.Open();
                        adapter.UpdateCommand = command;
                        int row = adapter.UpdateCommand.ExecuteNonQuery();
                        Console.WriteLine(row > 0 ?
                            "Product has been updated"
                            : "Something went wrong, please try again");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        Console.WriteLine("Something went wrong, please try again");
                    }
                }

            }
            else if (price != 0.0)
            {
                query = "UPDATE products SET price = @price WHERE productid = @productId";

                using (SQLiteConnection connection = Helper.ConnectToDb())
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                using (SQLiteDataAdapter adapter = new SQLiteDataAdapter())
                {
                    command.Parameters.AddWithValue("@price", price);
                    command.Parameters.AddWithValue("@productId", productId);
                    try
                    {
                        connection.Open();
                        adapter.UpdateCommand = command;
                        int row = adapter.UpdateCommand.ExecuteNonQuery();
                        Console.WriteLine(row > 0 ?
                            "Product has been updated"
                            : "Something went wrong, please try again");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        Console.WriteLine("Something went wrong, please try again");
                    }
                }

            }
            else
            {
                query = "UPDATE products SET units = units + @units " +
                        "WHERE productid = @productId";

                using (SQLiteConnection connection = Helper.ConnectToDb())
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                using (SQLiteDataAdapter adapter = new SQLiteDataAdapter())
                {
                    command.Parameters.AddWithValue("@units", units);
                    command.Parameters.AddWithValue("@productId", productId);
                    try
                    {
                        connection.Open();
                        adapter.UpdateCommand = command;
                        int row = adapter.UpdateCommand.ExecuteNonQuery();
                        Console.WriteLine(row > 0 ?
                            "Product has been updated"
                            : "Something went wrong, please try again");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        Console.WriteLine("Something went wrong, please try again");
                    }
                }

            }
        }

        public List<Order> ViewAllOrders()
        {
            string query = "SELECT * FROM orders";
            using (SQLiteConnection connection = Helper.ConnectToDb())
            using (SQLiteCommand command = new SQLiteCommand(query, connection))
            using (SQLiteDataAdapter adapter = new SQLiteDataAdapter())
            {
                try
                {
                    connection.Open();
                    adapter.SelectCommand = command;
                    SQLiteDataReader reader = adapter.SelectCommand.ExecuteReader();

                    if (reader.Read())
                    {
                        List<Order> allOrders = new List<Order>();
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
                        while (reader.Read())
                        {
                            productString = reader.GetString(3);
                            productIds = productString.Split(',');
                            products = new List<Product>();
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

        public List<Customer> ViewAllCustomers()
        {
            string query = "SELECT * FROM users WHERE type = @type";
            using (SQLiteConnection connection = Helper.ConnectToDb())
            using (SQLiteCommand command = new SQLiteCommand(query, connection))
            using (SQLiteDataAdapter adapter = new SQLiteDataAdapter())
            {
                try
                {
                    command.Parameters.AddWithValue("@type", "customer");
                    connection.Open();
                    adapter.SelectCommand = command;
                    SQLiteDataReader reader = adapter.SelectCommand.ExecuteReader();

                    if (reader.Read())
                    {
                        List<Customer> allCustomers = new List<Customer>
                        {
                            new Customer(
                                reader.GetInt32(0),
                                reader.GetString(1),
                                reader.GetString(2),
                                reader.GetInt32(6),
                                Convert.ToDateTime(reader.GetString(5)),
                                reader.GetString(7)
                            )
                    };
                        while (reader.Read())
                        {
                            allCustomers.Add(
                                    new Customer(
                                        reader.GetInt32(0),
                                        reader.GetString(1),
                                        reader.GetString(2),
                                        reader.GetInt32(6),
                                        Convert.ToDateTime(reader.GetString(5)),
                                        reader.GetString(7)
                                    )
                            );
                        }

                        return allCustomers;
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
