using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S10274174G_PRG2Assignment
{
    internal class OrderedFoodItem : FoodItem
    {
        private int qtyOrdered;
        private double subTotal;

        public int QtyOrdered
        {
            get { return qtyOrdered; }
            set { qtyOrdered = value; }
        }

        public double SubTotal
        {
            get { return subTotal; }
            set { subTotal = value; }
        }

        public OrderedFoodItem(string name, string desc, double price, int qtyOrdered)
            : base(name, desc, price)
        {
            QtyOrdered = qtyOrdered;
        }
        public OrderedFoodItem(string name, string desc, double price, string customise, int qtyOrdered)
            : base(name, desc, price, customise)
        {
            QtyOrdered = qtyOrdered;
        }

        public double CalculateSubTotal()
        {
            return SubTotal = base.ItemPrice * QtyOrdered;
        }
    }
}
