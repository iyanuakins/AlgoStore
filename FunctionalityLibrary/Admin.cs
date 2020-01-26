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
                    query = "UPDATE products SET productname = @name, productdescription = @description, price = @price, units = units + @units";
                    command = new SQLiteCommand(query, connection);
                    command.Parameters.AddWithValue("@name", name);
                    command.Parameters.AddWithValue("@description ", description);
                    command.Parameters.AddWithValue("@price", price);
                    command.Parameters.AddWithValue("@units", units);
                }
                else if(name != "" && description != "")
                {
                    query = "UPDATE products SET productname = @name, productdescription = @description";
                    command = new SQLiteCommand(query, connection);
                    command.Parameters.AddWithValue("@name", name);
                    command.Parameters.AddWithValue("@description ", description);
                }
                else if (price != 0.0 && units != 0)
                {
                    query = "UPDATE products SET price = @price, units = units + @units";
                    command = new SQLiteCommand(query, connection);
                    command.Parameters.AddWithValue("@price", price);
                    command.Parameters.AddWithValue("@units", units);
                }
                else if (price != 0.0)
                {
                    query = "UPDATE products SET price = @price";
                    command = new SQLiteCommand(query, connection);
                    command.Parameters.AddWithValue("@price", price);
                }
                else
                {
                    query = "UPDATE products SET units = units + @units";
                    command = new SQLiteCommand(query, connection);
                    command.Parameters.AddWithValue("@units", units);
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
    }
}
