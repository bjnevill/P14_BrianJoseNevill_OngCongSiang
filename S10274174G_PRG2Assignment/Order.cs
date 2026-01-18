using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S10274174G_PRG2Assignment
{
    internal class Order
    {
        private int orderId;
        private DateTime orderDateTime;
        private double orderTotal;
        private string orderStatus;
        private DateTime deliveryDateTime;
        private string deliveryAddress;
        private string orderPaymentMethod;
        private bool orderPaid;
        private List<OrderedFoodItem> orderedFoodItems;
        private SpecialOffer? specialOffer;
        private Customer customer;
        private Restaurant restaurant;

        public int OrderId
        {
            get { return orderId; }
            set { orderId = value; }
        }
        public DateTime OrderDateTime
        {
            get { return orderDateTime; }
            set { orderDateTime = value; }
        }
        public double OrderTotal
        {
            get { return orderTotal; }
            set { orderTotal = value; }
        }
        public string OrderStatus
        {
            get { return orderStatus; }
            set { orderStatus = value; }
        }
        public DateTime DeliveryDateTime
        {
            get { return deliveryDateTime; }
            set { deliveryDateTime = value; }
        }
        public string DeliveryAddress
        {
            get { return deliveryAddress; }
            set { deliveryAddress = value; }
        }
        public string OrderPaymentMethod
        {
            get { return orderPaymentMethod; }
            set { orderPaymentMethod = value; }
        }
        public bool OrderPaid
        {
            get { return orderPaid; }
            set { orderPaid = value; }
        }
        public List<OrderedFoodItem> OrderedFoodItems
        {
            get { return orderedFoodItems; }
            set { orderedFoodItems = value; }
        }
        public SpecialOffer? SpecialOffer
        {
            get { return specialOffer; }
            set { specialOffer = value; }
        }
        public Customer Customer
        {
            get { return customer; }
            set { customer = value; }
        }
        public Restaurant Restaurant
        {
            get { return restaurant; }
            set { restaurant = value; }
        }
        public Order(int id, DateTime dateTime, double total, string status, DateTime deliveryDateTime, string deliveryAddress, string paymentMethod, bool paid, Customer customer, Restaurant restaurant)
        {
            OrderId = id;
            OrderDateTime = dateTime;
            OrderTotal = total;
            OrderStatus = status;
            DeliveryDateTime = deliveryDateTime;
            DeliveryAddress = deliveryAddress;
            OrderPaymentMethod = paymentMethod;
            OrderPaid = paid;
            OrderedFoodItems = new List<OrderedFoodItem>();
            Customer = customer;
            Restaurant = restaurant;
        }
        public Order(int id, DateTime dateTime, double total, string status, DateTime deliveryDateTime, string deliveryAddress, string paymentMethod, bool paid, SpecialOffer? specialOffer, Customer customer, Restaurant restaurant)
        {
            OrderId = id;
            OrderDateTime = dateTime;
            OrderTotal = total;
            OrderStatus = status;
            DeliveryDateTime = deliveryDateTime;
            DeliveryAddress = deliveryAddress;
            OrderPaymentMethod = paymentMethod;
            OrderPaid = paid;
            OrderedFoodItems = new List<OrderedFoodItem>();
            SpecialOffer = specialOffer;
            Customer = customer;
            Restaurant = restaurant;
        }
        public double CalculateOrderTotal()
        {
            const double DeliveryFee = 5.00;
            double total = 0;
            foreach (OrderedFoodItem item in OrderedFoodItems)
            {
                total += item.CalculateSubTotal();
            }
            
            return OrderTotal = total + DeliveryFee;
        }

        public void AddOrderedFoodItem(OrderedFoodItem item)
        {
            OrderedFoodItems.Add(item);
        }

        public bool RemoveOrderedFoodItem(OrderedFoodItem item)
        {
            return OrderedFoodItems.Remove(item);
        }

        public void DisplayOrderedFoodItems()
        {
            Console.WriteLine($"Order ID: {OrderId}, DateTime: {OrderDateTime}, Total: ${OrderTotal}, Status: {OrderStatus}");
            Console.WriteLine($"Delivery DateTime: {DeliveryDateTime}, Address: {DeliveryAddress}, Payment Method: {OrderPaymentMethod}, Paid: {OrderPaid}");
            Console.WriteLine("Ordered Food Items:");
            foreach (OrderedFoodItem item in OrderedFoodItems)
            {
                Console.WriteLine($"{item.ItemName} - Qty: {item.QtyOrdered} - SubTotal: ${item.CalculateSubTotal()}");
            }
        }

        public override string ToString()
        {
            return $"Order ID: {OrderId}, DateTime: {OrderDateTime}, Total: ${OrderTotal}, Status: {OrderStatus}, Delivery Address: {DeliveryAddress}";
        }
    }   
}
