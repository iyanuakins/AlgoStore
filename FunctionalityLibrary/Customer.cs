using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace FunctionalityLibrary
{
    public class Customer : User
    {
        public Customer(int id, string name, string email, int totalOrders, DateTime dateRegistered, string address)
            : base(id, name, email, dateRegistered)
        {
            TotalOrders = totalOrders;
            Address = address;
        }

        public string Address { get; private set; }
        public Cart Cart { get; private set; } = new Cart();
        public int TotalOrders { get; private set; }
        public int AddToCart(Product product)
        {
            bool isInCart = false;

            if (Cart.ProductsInCart.Count > 0)
            {
                foreach (var prod in Cart.ProductsInCart)
                {
                    if (product.Id == prod.Id)
                    {
                        isInCart = true;
                        break;
                    }
                }
            }

            if (!isInCart)
            {
                List<Product> newProduct = new List<Product>
                {
                    product
                };

                Cart.TotalPrice = product.Price;
                Cart.ProductsInCart = newProduct;
                Console.WriteLine("\nProduct added to cart\n");
                Console.WriteLine($"You have {Cart.ProductsInCart.Count} items in your cart");
            }
            else
            {
                Console.WriteLine("\nProduct already added to cart\n");
            }

            return Cart.ProductsInCart.Count;
        }

        public int RemoveFromCart(int index)
        {
            Cart.TotalPrice = Cart.ProductsInCart[index].Price * -1;
            Cart.ProductsInCart.RemoveAt(index);
            return Cart.ProductsInCart.Count;
        }

        public void ClearCart()
        {
            Cart = new Cart();
        }

        public int EditProfile(string name = "", string address = "", string password = "")
        {
            string query;
            if (!string.IsNullOrEmpty(name))
            {
                query = "UPDATE users SET name = @name WHERE userid = @userId";
                using (SQLiteConnection connection = Helper.ConnectToDb())
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                using (SQLiteDataAdapter adapter = new SQLiteDataAdapter())
                {
                    command.Parameters.AddWithValue("@name", name);
                    command.Parameters.AddWithValue("@userId", Id);
                    try
                    {
                        connection.Open();
                        adapter.UpdateCommand = command;
                        int row = adapter.UpdateCommand.ExecuteNonQuery();
                        if (row > 0)
                        {
                            Name = name;
                            Console.WriteLine("Your name has been updated");
                        }
                        else
                        {
                            Console.WriteLine("Something went wrong, please try again");
                        }

                        return row;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        Console.WriteLine("Something went wrong, please try again");
                        return 0;
                    }
                }
            }
            else if(!string.IsNullOrEmpty(address))
            {
                query = "UPDATE users SET address = @address WHERE userid = @userId";
                using (SQLiteConnection connection = Helper.ConnectToDb())
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                using (SQLiteDataAdapter adapter = new SQLiteDataAdapter())
                {
                    command.Parameters.AddWithValue("@address", address);
                    command.Parameters.AddWithValue("@userId", Id);
                    try
                    {
                        connection.Open();
                        adapter.UpdateCommand = command;
                        int row = adapter.UpdateCommand.ExecuteNonQuery();
                        if (row > 0)
                        {
                            Address = address;
                            Console.WriteLine("Your address has been updated");
                        }
                        else
                        {
                            Console.WriteLine("Something went wrong, please try again");
                        }

                        return row;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        Console.WriteLine("Something went wrong, please try again");
                        return 0;
                    }
                }
            }
            else
            {
                query = "UPDATE users SET password = @password WHERE userid = @userId";
                using (SQLiteConnection connection = Helper.ConnectToDb())
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                using (SQLiteDataAdapter adapter = new SQLiteDataAdapter())
                {
                    command.Parameters.AddWithValue("@password", password);
                    command.Parameters.AddWithValue("@userId", Id);
                    try
                    {
                        connection.Open();
                        adapter.UpdateCommand = command;
                        int row = adapter.UpdateCommand.ExecuteNonQuery();
                        if (row > 0)
                        {
                            Name = name;
                            Console.WriteLine("Your password has been changed");
                        }
                        else
                        {
                            Console.WriteLine("Something went wrong, please try again");
                        }

                        return row;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        Console.WriteLine("Something went wrong, please try again");
                        return 0;
                    }
                }
            }

        }

        public void Checkout()
        {
            int productCount = Cart.ProductsInCart.Count;
            if (productCount > 0)
            {
                StringBuilder productStringBuilder = new StringBuilder();
                productStringBuilder.Append(Cart.ProductsInCart[0].Id);
                for (var i = 1; i < productCount; i++)
                {
                    productStringBuilder.Append($",{Cart.ProductsInCart[1].Id}");
                }

                string query =
                    "INSERT INTO orders VALUES(@Id, @totalProduct, @customer, @productsInOrder, @totalPrice, @orderDate)";
                using (SQLiteConnection connection = Helper.ConnectToDb())
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                using (SQLiteDataAdapter adapter = new SQLiteDataAdapter())
                {
                    command.Parameters.AddWithValue("@totalProduct", productCount);
                    command.Parameters.AddWithValue("@Id", null);
                    command.Parameters.AddWithValue("@customer", Id);
                    command.Parameters.AddWithValue("@productsInOrder", productStringBuilder.ToString());
                    command.Parameters.AddWithValue("@totalPrice", Cart.TotalPrice);
                    command.Parameters.AddWithValue("@orderDate", DateTime.Now.ToString());

                    try
                    {
                        connection.Open();
                        adapter.InsertCommand = command;
                        int row = adapter.InsertCommand.ExecuteNonQuery();
                        if (row > 0)
                        {
                            //Update customer total order count.
                            query = "UPDATE users SET totalorders = totalorders + 1";

                            using (SQLiteCommand newCommand = new SQLiteCommand(query, connection))
                            {
                                try
                                {
                                    adapter.UpdateCommand = newCommand;
                                    row = adapter.UpdateCommand.ExecuteNonQuery();
                                    if (row > 0)
                                    {
                                        Cart = new Cart();
                                        Console.WriteLine("Your order has been processed");
                                    }
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine(e.Message);
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Gosh! Something went wrong, please try again");
                    }
                }
            }

        }
        public List<Order> ViewOrdersHistory()
        {
            string query = "SELECT * FROM orders WHERE user = @id";
            using (SQLiteConnection connection = Helper.ConnectToDb())
            using (SQLiteCommand command = new SQLiteCommand(query, connection))
            using (SQLiteDataAdapter adapter = new SQLiteDataAdapter())
            {
                try
                { 
                    command.Parameters.AddWithValue("@id", Id);
                    connection.Open();
                    adapter.SelectCommand = command;
                    SQLiteDataReader reader = adapter.SelectCommand.ExecuteReader();

                    if (reader.Read())
                    {
                        List<Order> orders = new List<Order>();
                        string productString = reader.GetString(3);
                        string[] productIds = productString.Split(',');
                        List<Product> products = new List<Product>();
                        foreach (var id in productIds)
                        {
                            products.Add(Helper.GetProduct(int.Parse(id)));
                        }

                        orders.Add(new Order(
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

                            orders.Add(new Order(
                                                    reader.GetInt32(0),
                                                    reader.GetInt32(1),
                                                    reader.GetInt32(2),
                                                    products,
                                                    Convert.ToDouble(reader.GetFloat(4)),
                                                    Convert.ToDateTime(reader.GetString(5))
                                                ));
                        }

                        return orders;
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
