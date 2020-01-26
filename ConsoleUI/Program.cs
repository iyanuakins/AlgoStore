using System;
using System.Data.SQLite;
using System.Configuration;
using FunctionalityLibrary;

namespace ConsoleUI
{
    class Program
    {
        static void Main(string[] args)
        {
            
            using (SQLiteConnection connection = Helper.ConnectToDb())
            {
                connection.Open();
                Console.WriteLine(connection.State);
            }


            //string query = "SELECT * FROM users WHERE userid =  1";
            //SQLiteCommand command = new SQLiteCommand(query, connection);
            //SQLiteDataReader reader = command.ExecuteReader();
            //if (reader.Read())
            //{
            //    Console.WriteLine(reader.GetString(1));
            //}



            Console.ReadKey();

        }
    }
}
