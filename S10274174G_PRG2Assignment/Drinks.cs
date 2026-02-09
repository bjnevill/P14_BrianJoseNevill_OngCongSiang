using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S10274174G_PRG2Assignment
{
    class Drink
    {
        public string Name { get; set; }
        public double Price { get; set; }

        public Drink(string name, double price)
        {
            Name = name;
            Price = price;
        }
    }
}
