using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S10274174G_PRG2Assignment
{
    internal class Restaurant
    {
        private string restaurantId;
        private string restaurantName;
        private string restaurantEmail;
        private List<Menu> menus;
        private List<SpecialOffer>? specialOffers;
        private Queue<Order> orders;

        public string RestaurantId
        {
            get { return restaurantId; }
            set { restaurantId = value; }
        }
        public string RestaurantName
        {
            get { return restaurantName; }
            set { restaurantName = value; }
        }
        public string RestaurantEmail
        {
            get { return restaurantEmail; }
            set { restaurantEmail = value; }
        }
        public List<Menu> Menus
        {
            get { return menus; }
            set { menus = value; }
        }
        public List<SpecialOffer> SpecialOffers
        {
            get { return specialOffers; }
            set { specialOffers = value; }
        }
        public Queue<Order> Orders
        {
            get { return orders; }
            set { orders = value; }
        }
        public Restaurant(string id, string name, string email)
        {
            RestaurantId = id;
            RestaurantName = name;
            RestaurantEmail = email;
            Menus = new List<Menu>();
            SpecialOffers = new List<SpecialOffer>();
            Orders = new Queue<Order>();
        }

        public void DisplayOrders()
        {
            Console.WriteLine($"Restaurant: {RestaurantName} (ID: {RestaurantId})");
            foreach (Order order in Orders)
            {
                Console.WriteLine($"Order ID: {order.OrderId}, Total: {order.OrderTotal}, Status: {order.OrderStatus}");
            }
        }

        public void DisplaySpecialOffers()
        {
            Console.WriteLine($"Restaurant: {RestaurantName} (ID: {RestaurantId}) - Special Offers:");
            foreach (SpecialOffer offer in SpecialOffers)
            {
                Console.WriteLine(offer.ToString());
            }
        }
        public void DisplayMenus()
        {
            Console.WriteLine($"Restaurant: {RestaurantName} (ID: {RestaurantId}) - Menus:");
            foreach (Menu menu in Menus)
            {
                Console.WriteLine(menu.ToString());
            }
        }

        public void AddMenu(Menu menu)
        {
            Menus.Add(menu);
        }
        public bool RemoveMenu(Menu menu)
        {
            return Menus.Remove(menu);
        }

        public override string ToString()
        {
            return $"{RestaurantName} (ID: {RestaurantId}) - {Menus.Count} menus, {SpecialOffers.Count} special offers, {Orders.Count} orders";
        }
    }
}
