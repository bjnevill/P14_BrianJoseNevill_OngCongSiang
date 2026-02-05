//==========================================================
// Student Number : S10274174G
// Student Name : Brian Jose Nevill
// Partner Name : Ong Cong Siang
//==========================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S10274174G_PRG2Assignment
{
    internal class Menu
    {
        private string menuID;
        private string menuName;
        private List<FoodItem> foodItems;

        public string MenuID
        {
            get { return menuID; }
            set { menuID = value; }
        }
        public string MenuName
        {
            get { return menuName; }
            set { menuName = value; }
        }
        public List<FoodItem> FoodItems
        {
            get { return foodItems; }
            set { foodItems = value; }
        }

        public Menu(string id, string name)
        {
            MenuID = id;
            MenuName = name;
            FoodItems = new List<FoodItem>();
        }

        public void AddFoodItem(FoodItem item)
        {
            FoodItems.Add(item);
        }

        public void RemoveFoodItem(FoodItem item)
        {
            FoodItems.Remove(item);
        }

        public void DisplayFoodItems()
        {
            Console.WriteLine($"Menu: {MenuName} (ID: {MenuID})");
            foreach (FoodItem item in FoodItems)
            {
                Console.WriteLine(item.ToString());
            }
        }

        public override string ToString()
        {
            return $"{MenuName} (ID: {MenuID}) - {FoodItems.Count} items";
        }
    }
}
