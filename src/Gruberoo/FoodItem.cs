//==========================================================
// Student Name : Brian Jose Nevill
// Partner Name : Ong Cong Siang
//==========================================================


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gruberoo
{
    internal class FoodItem
    {
        private string itemName;
        private string itemDesc;
        private double itemPrice;
        private string customise;

        public string ItemName
        {
            get { return itemName; }
            set { itemName = value; }
        }
        public string ItemDesc
        {
            get { return itemDesc; }
            set { itemDesc = value; }
        }
        public double ItemPrice
        {
            get { return itemPrice; }
            set { itemPrice = value; }
        }
        public string Customise
        {
            get { return customise; }
            set { customise = value; }
        }

        public FoodItem(string name, string desc, double price)
        {
            ItemName = name;
            ItemDesc = desc;
            ItemPrice = price;
        }

        public FoodItem(string name, string desc, double price, string customise)
        {
            ItemName = name;
            ItemDesc = desc;
            ItemPrice = price;
            Customise = customise;
        }

        public override string ToString()
        {
            return $"  - {ItemName}: {ItemDesc} - ${ItemPrice.ToString("0.00")}";
        }
    }
}
