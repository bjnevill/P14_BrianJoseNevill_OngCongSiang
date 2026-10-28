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
    internal class Customer
    {
        private string emailAdress;
        private string customerName;
        private List<Order> orders;
        private List<Favourite> favourites; 

        public string EmailAdress
        {
            get { return emailAdress; }
            set { emailAdress = value; }
        }
        public string CustomerName
        {
            get { return customerName; }
            set { customerName = value; }
        }
        public List<Order> Orders
        {
            get { return orders; }
            set { orders = value; }
        }
        public List<Favourite> Favourites
        {
            get { return favourites; }
            set { favourites = value; }
        }
        public Customer(string email, string name)
        {
            EmailAdress = email;
            CustomerName = name;
            Orders = new List<Order>();
            Favourites = new List<Favourite>();
        }
        public void AddOrder(Order order)
        {
            Orders.Add(order);
        }
        public void DisplayOrders()
        {
            Console.WriteLine($"Customer: {CustomerName} (Email: {EmailAdress})");
            foreach (Order order in Orders)
            {
                Console.WriteLine($"Order ID: {order.OrderId}, Total: {order.OrderTotal}, Status: {order.OrderStatus}");
            }
        }
        public bool RemoveOrder(Order order)
        {
            return Orders.Remove(order);
        }

        public override string ToString()
        {
            return $"{CustomerName} (Email: {EmailAdress}) - {Orders.Count} orders";
        }
    }
}
