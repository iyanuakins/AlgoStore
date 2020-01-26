using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Configuration;
using FunctionalityLibrary;

namespace ConsoleUI
{
    class Program
    {
        static void Main(string[] args)
        {
            
//            using (SQLiteConnection connection = Helper.ConnectToDb())
//            {
//                connection.Open();
//                Console.WriteLine(connection.State);
////            }

//            Product newProduct = new Product(1, "orange", "Sweet", 5.556, 8, DateTime.Now);
//            List<Product> prod = new List<Product>
//            {
//                newProduct
//            };
//            Cart currentCart = new Cart();
//            currentCart.TotalPrice = newProduct.Price;
//            currentCart.ProductsInCart = prod;
//            foreach (var pro in currentCart.ProductsInCart)
//            {
//                Console.WriteLine(pro.Id);
//                Console.WriteLine(pro.Name);
//                Console.WriteLine(pro.Price);
//            }
//            Console.WriteLine($"Price: {currentCart.TotalPrice}");
//            Console.WriteLine($"Length: {currentCart.ProductsInCart.Count}");
//            prod = new List<Product>
//            {
//                new Product(89, "Mango", "Sweet", 6.556, 2, DateTime.Now)
//            };
//            Console.WriteLine();
//            Console.WriteLine();

//            currentCart.TotalPrice = 2.0;
//            currentCart.ProductsInCart = prod;

//            Console.WriteLine("--------------------------------------------------");
//            foreach (var pro in currentCart.ProductsInCart)
//            {
//                Console.WriteLine(pro.Id);
//                Console.WriteLine(pro.Name);
//                Console.WriteLine(pro.Price);
//            }
//            Console.WriteLine($"Price: {currentCart.TotalPrice}");
//            Console.WriteLine($"Length: {currentCart.ProductsInCart.Count}");
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
