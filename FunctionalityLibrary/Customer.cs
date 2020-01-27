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
        public Customer(int id, string name, string email, int totalOrders, DateTime dateRegistered): base(id, name, email,dateRegistered )
        {
            TotalOrders = totalOrders;
        }

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
            }
            else
            {
                Console.WriteLine("Product already added to cart");
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

        public void EditProfile()
        {
            throw new NotImplementedException();
        }

        public void Checkout()
        {
            int productCount = Cart.ProductsInCart.Count;
            if (productCount > 0)
            {
                using (SQLiteConnection connection = Helper.ConnectToDb())
                {
                    StringBuilder productStringBuilder = new StringBuilder();
                    productStringBuilder.Append(Cart.ProductsInCart[0].Id);
                    for (var i = 1; i <  productCount; i++)
                    {
                        productStringBuilder.Append($",{Cart.ProductsInCart[1].Id}");
                    }


                    string query =
                        "INSERT INTO orders VALUES(@totalProduct, @customer, @productsInOrder, @totalPrice, @orderDate)";
                    SQLiteCommand command = new SQLiteCommand(query, connection);
                    command.Parameters.AddWithValue("@totalProduct", productCount);
                    command.Parameters.AddWithValue("@customer", Id);
                    command.Parameters.AddWithValue("@productsInOrder", productStringBuilder.ToString());
                    command.Parameters.AddWithValue("@totalPrice", Cart.TotalPrice);
                    command.Parameters.AddWithValue("@orderDate", DateTime.Now.ToString());

                    try
                    {
                        connection.Open();
                        int row = command.ExecuteNonQuery();
                        if (row > 0)
                        {
                            try
                            {
                                query = "UPDATE users SET totalorders = totalorders + 1";
                                command = new SQLiteCommand(query, connection);
                                row = command.ExecuteNonQuery();
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
                    catch (Exception e)
                    {
                        Console.WriteLine("Gosh! Something went wrong, please try again");
                    }
                    finally
                    {
                        connection.Close();
                    }
                    
                }
            }
            
        }

        public List<Order> ViewOrdersHistory()
        {
            using (SQLiteConnection connection = Helper.ConnectToDb())
            {
                try
                {
                    string query = "SELECT * FROM orders WHERE user = @id";
                    SQLiteCommand command = new SQLiteCommand(query, connection);
                    command.Parameters.AddWithValue("@id", Id);
                    connection.Open();
                    SQLiteDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        List<Order> orders = new List<Order>();
                        while (reader.Read())
                        {
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
                finally
                {
                    connection.Close();
                }
            }
        }
    }
}
