using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionalityLibrary
{
    public class Product
    {
        public Product(int id, string name, string description, double price, int unit, DateTime dateAdded)
        {
            Id = id;
            Name = name;
            Description = description;
            Price = price;
            Unit = unit;
            DateAdded = dateAdded;
        }

        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public double Price { get; private set; }
        public int Unit { get; private set; }
        public DateTime DateAdded { get; private set; }


    }
}
