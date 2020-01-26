using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionalityLibrary
{
    public class Cart
    {
        private double _TotalPrice = 0.0;
        private List<Product> _ProductsInCart = new List<Product>();
        public Cart()
        {
            
        }

        public List<Product> ProductsInCart
        {
            get => _ProductsInCart;
            set
            {
                _ProductsInCart.AddRange(value);
            }
        }

        public double TotalPrice
        {
            get => _TotalPrice;
            set => _TotalPrice += value;
        }
    }
}
