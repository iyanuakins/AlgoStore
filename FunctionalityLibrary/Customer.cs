using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionalityLibrary
{
    public class Customer : User
    {
        public Customer(int id, string name, string email, int totalOrders, DateTime dateRegistered): base(id, name, email,dateRegistered )
        {
            TotalOrders = totalOrders;
        }

        public Cart Cart { get; } = new Cart();
        public int TotalOrders { get; private set; }

        public void AddToCart(Product product)
        {
            List<Product> newProduct = new List<Product>
            {
                product
            };

            Cart.TotalPrice = product.Price;
            Cart.ProductsInCart = newProduct;
        }
    }
}
