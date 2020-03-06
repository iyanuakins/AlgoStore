using System;
using System.Collections.Generic;

namespace FunctionalityLibrary
{
    public class Order
    {
        public int Id { get; private set; }
        public int TotalProductInOrder { get; private set; }
        public int User { get; private set; }
        public List<Product> ProductsInOrder { get; set; }
        public double TotalPrice { get; private set; }
        public DateTime OrderDate { get; private set; }

        public Order(int id, int totalProductInOrder, int user, List<Product> productsInOrder, double totalPrice, DateTime orderDate)
        {
            Id = id;
            TotalProductInOrder = totalProductInOrder;
            User = user;
            ProductsInOrder = productsInOrder;
            TotalPrice = totalPrice;
            OrderDate = orderDate;
        }
    }
}
