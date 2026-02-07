//==========================================================
// Student Number : S10274174G
// Student Name : Brian Jose Nevill - Questions:2,3,5,7
// Partner Name : Ong Cong Siang - Questions:1,4,6,8
//==========================================================

using S10274174G_PRG2Assignment;
using System.Runtime.CompilerServices;
using System.Security.Principal;
using static System.Runtime.InteropServices.JavaScript.JSType;

Dictionary<string, Restaurant> restaurants = LoadRestaurants();
Dictionary<string, FoodItem> foodItems = LoadFoodItems(restaurants);
Dictionary<string, Customer> customers = LoadCustomer();
Dictionary<int, Order> orders = LoadOrders(customers, restaurants, foodItems);

Stack<Order> refundStack = new Stack<Order>();

Console.WriteLine("Welcome to the Gruberoo Food Delivery System");
Console.WriteLine($"{restaurants.Count} restaurants loaded!");
Console.WriteLine($"{foodItems.Count} food items loaded!");
Console.WriteLine($"{customers.Count} customers loaded!");
Console.WriteLine($"{orders.Count} orders loaded!");
Console.WriteLine("");



int choice;
do
{
    Console.WriteLine("===== Gruberoo Food Delivery System =====");
    Console.WriteLine("1. List all restaurants and menu items");
    Console.WriteLine("2. List all orders");
    Console.WriteLine("3. Create a new order");
    Console.WriteLine("4. Process an order");
    Console.WriteLine("5. Modify an existing order");
    Console.WriteLine("6. Delete an existing order");
    Console.WriteLine("7. Bulk processing of unprocessed orders for a current day");
    Console.WriteLine("8. Display total order amount");
    Console.WriteLine("0. Exit");
    Console.Write("Enter your choice: ");
    choice = Convert.ToInt32(Console.ReadLine());
    Console.WriteLine("");
    if (choice == 0)
    {
        Console.WriteLine("Exiting...");
    }
    else if (choice == 1)
    {
        while (true)
        {
            Console.WriteLine("\nRestaurants");
            Console.WriteLine("===========");
            foreach (var r in restaurants.Values)
            {
                Console.WriteLine($"{r.RestaurantId} - {r.RestaurantName}");
            }

            Console.Write("Select Restaurant ID (0 to go back): ");
            string input = Console.ReadLine().Trim();

            if (input == "0")
                break;

            if (!restaurants.ContainsKey(input))
            {
                Console.WriteLine("Invalid Restaurant ID.");
                continue;
            }

            Restaurant selected = restaurants[input];

            Console.WriteLine($"\n{selected.RestaurantName} Menu:");
            foreach (Menu menu in selected.Menus)
            {
                Console.WriteLine($"Menu: {menu.MenuName}");
                foreach (FoodItem item in menu.FoodItems)
                {
                    Console.WriteLine(item.ToString());
                }
            }

            Console.WriteLine("\nPress any key to return to the restaurant list...");
            Console.ReadKey();
            Console.Clear();
        }
    }
    else if (choice == 2)
    {
        Console.WriteLine("All Orders");
        Console.WriteLine("==========");
        Console.WriteLine($"{"Order ID",-12}{"Customer",-14}{"Restaurant",-17}{"Delivery Date/Time",-22}{"Amount",-10}{"Status",-10}");
        Console.WriteLine($"{"--------",-12}{"----------",-14}{"-------------",-17}{"------------------",-22}{"------",-10}{"---------",-10}");


        foreach (Order order in orders.Values)
        {
            Console.WriteLine(order.ToString());
        }
        Console.WriteLine("");
    }
    else if (choice == 3)
    {
        CreateNewOrder();
    }
    else if (choice == 4)
    {
        Console.WriteLine("Process Order");
        Console.WriteLine("=============");
        Console.Write("Enter Restaurant ID: ");
        string restaurantId = Console.ReadLine().Trim();

        if (!restaurants.ContainsKey(restaurantId))
        {
            Console.WriteLine("Invalid Restaurant ID.\n");
            continue;
        }

        Queue<Order> orderQueue = restaurants[restaurantId].Orders;

        if (orderQueue.Count == 0)
        {
            Console.WriteLine("No orders for this restaurant.\n");
            continue;
        }

        foreach (Order order in orderQueue)
        {
            Console.WriteLine($"Order {order.OrderId}:");
            Console.WriteLine($"Customer: {order.Customer.CustomerName}");
            Console.WriteLine("Ordered Items:");

            int itemNo = 1;
            foreach (OrderedFoodItem item in order.OrderedFoodItems)
            {
                Console.WriteLine($"{itemNo}. {item.ItemName} - {item.QtyOrdered}");
                itemNo++;
            }

            Console.WriteLine($"Delivery date/time: {order.DeliveryDateTime:dd/MM/yyyy HH:mm}");
            Console.WriteLine($"Total Amount: ${order.OrderTotal:0.00}");
            Console.WriteLine($"Order Status: {order.OrderStatus}");

            Console.Write("[C]onfirm / [R]eject / [S]kip / [D]eliver: ");
            string option = Console.ReadLine().Trim().ToUpper();

            switch (option)
            {
                case "C":
                    if (order.OrderStatus == Order.STATUS_PENDING)
                    {
                        order.OrderStatus = Order.STATUS_PREPARING;
                        Console.WriteLine($"Order {order.OrderId} confirmed. Status: Preparing");
                    }
                    else
                    {
                        Console.WriteLine("Order cannot be confirmed.");
                    }
                    break;

                case "R":
                    if (order.OrderStatus == Order.STATUS_PENDING)
                    {
                        order.OrderStatus = Order.STATUS_REJECTED;
                        refundStack.Push(order);
                        Console.WriteLine("Order rejected. Refund initiated.");
                    }
                    else
                    {
                        Console.WriteLine("Order cannot be rejected.");
                    }
                    break;

                case "S":
                    if (order.OrderStatus == Order.STATUS_CANCELLED)
                    {
                        Console.WriteLine("Order skipped.");
                    }
                    else
                    {
                        Console.WriteLine("Only cancelled orders can be skipped.");
                    }
                    break;

                case "D":
                    if (order.OrderStatus == Order.STATUS_PREPARING)
                    {
                        order.OrderStatus = Order.STATUS_DELIVERED;
                        Console.WriteLine($"Order {order.OrderId} delivered successfully.");
                    }
                    else
                    {
                        Console.WriteLine("Order cannot be delivered.");
                    }
                    break;

                default:
                    Console.WriteLine("Invalid option.");
                    break;
            }

            Console.WriteLine();
        }
    }

    List<Order> queueList = new List<Order>();
foreach (Restaurant restaurant in restaurants.Values)
{
    foreach (Order order in restaurant.Orders)
    {
        if (order.OrderStatus == "Pending" || order.OrderStatus == "Preparing")
        {
            queueList.Add(order);
        }
    }
}
List<Order> refundList = refundStack.ToList();
SaveOrdersToCsv(queueList, "queue.csv");
SaveOrdersToCsv(refundList, "stack.csv");


void SaveOrdersToCsv(List<Order> orderList, string filename)
{
    string data = "";
    foreach (Order order in orderList)
    {
        int orderid = order.OrderId;
        string custEmail = order.Customer.EmailAdress;
        string restaurantId = order.Restaurant.RestaurantId;
        string deliveryDate = order.DeliveryDateTime.ToString("dd/MM/yyyy");
        string deliveryTime = order.DeliveryDateTime.ToString("HH:mm");
        string deliveryAddress = order.DeliveryAddress;
        string createdDateTime = order.OrderDateTime.ToString("dd/MM/yyyy HH:mm");
        string totalAmount = order.OrderTotal.ToString("0.00");
        string status = order.OrderStatus;
        string items = "";
        foreach (OrderedFoodItem ofi in order.OrderedFoodItems)
        {
            items += $"{ofi.ItemName},{ofi.QtyOrdered}|";
        }
        items = items.Trim('|');
        data += $"{orderid},{custEmail},{restaurantId},{deliveryDate},{deliveryTime},{deliveryAddress},{createdDateTime},{totalAmount},{status},{items}\n";
    }

    data = data.Trim('\n');
    using (StreamWriter sw = new StreamWriter(filename, false))
    {
        sw.WriteLine("OrderId,CustomerEmail,RestaurantId,DeliveryDate,DeliveryTime,DeliveryAddress,CreatedDateTime,TotalAmount,Status,Items");
        sw.WriteLine(data);
    }
}




void DisplayTotals(Dictionary<string, Restaurant> restaurants)
{
    const double DELIVERY_FEE = 5.00;

    double grandTotalOrders = 0;
    double grandTotalRefunds = 0;

    Console.WriteLine("Order Summary by Restaurant");
    Console.WriteLine("===========================");

    foreach (Restaurant restaurant in restaurants.Values)
    {
        double restaurantOrderTotal = 0;
        double restaurantRefundTotal = 0;

        foreach (Order order in restaurant.Orders)
        {
            // Successful orders
            if (order.OrderStatus == Order.STATUS_DELIVERED)
            {
                // Remove delivery fee
                restaurantOrderTotal += (order.OrderTotal - DELIVERY_FEE);
            }
            // Refunded orders
            else if (order.OrderStatus == Order.STATUS_CANCELLED
                  || order.OrderStatus == Order.STATUS_REJECTED)
            {
                restaurantRefundTotal += order.OrderTotal;
            }
        }

        Console.WriteLine($"\nRestaurant: {restaurant.RestaurantName}");
        Console.WriteLine($"Total Successful Orders: ${restaurantOrderTotal:0.00}");
        Console.WriteLine($"Total Refunds: ${restaurantRefundTotal:0.00}");

        grandTotalOrders += restaurantOrderTotal;
        grandTotalRefunds += restaurantRefundTotal;
    }

    Console.WriteLine("\nOverall Summary");
    Console.WriteLine("===============");
    Console.WriteLine($"Total Order Amount: ${grandTotalOrders:0.00}");
    Console.WriteLine($"Total Refunds: ${grandTotalRefunds:0.00}");
    Console.WriteLine($"Final Amount Gruberoo Earns: ${(grandTotalOrders - grandTotalRefunds):0.00}");
}

void RestaurantSubMenu(Dictionary<string, Restaurant> restaurants)
{
    while (true)
    {
        Console.WriteLine("\nRestaurants");
        Console.WriteLine("===========");
        foreach (var r in restaurants.Values)
        {
            Console.WriteLine($"{r.RestaurantId} - {r.RestaurantName}");
        }

        Console.Write("Select Restaurant ID (0 to go back): ");
        string input = Console.ReadLine().Trim();

        if (input == "0")
            break; 

        if (!restaurants.ContainsKey(input))
        {
            Console.WriteLine("Invalid Restaurant ID.");
            continue;
        }

        Restaurant selected = restaurants[input];

        Console.WriteLine($"\n{selected.RestaurantName} Menu:");
        foreach (Menu menu in selected.Menus)
        {
            Console.WriteLine($"Menu: {menu.MenuName}");
            foreach (FoodItem item in menu.FoodItems)
            {
                Console.WriteLine(item.ToString());
            }
        }

        Console.WriteLine("\nPress any key to return to the restaurant list...");
        Console.ReadKey();
        Console.Clear();
    }
}


Dictionary<string, Restaurant> LoadRestaurants()
{
    Dictionary<string, Restaurant> restaurants = new Dictionary<string, Restaurant>();
    string[] data = File.ReadAllLines("restaurants.csv");

    for (int i = 1; i < data.Length; i++)
    {
        string[] restaurantData = data[i].Split(',');

        string id = restaurantData[0];
        string name = restaurantData[1];
        string email = restaurantData[2];

        restaurants[id] = new Restaurant(id, name, email);
    }

    return restaurants;
}

Dictionary<string, FoodItem> LoadFoodItems(Dictionary<string, Restaurant> restaurants)
{
    Dictionary<string, FoodItem> foodItems = new Dictionary<string, FoodItem>();
    string[] data = File.ReadAllLines("fooditems.csv");

    for (int i = 1; i < data.Length; i++)
    {
        string[] parts = data[i].Split('"');
        string restaurantId;
        string name;
        string desc;
        double price;
        if (parts.Length > 1)
        {
            string[] part1 = parts[0].Split(',');
            string part2 = parts[1];
            string[] part3 = parts[2].Split(',');
            restaurantId = part1[0];
            name = part1[1];
            desc = part2;
            price = Convert.ToDouble(part3[1]);
        }
        else
        {


            string[] foodData = data[i].Split(',');
            restaurantId = foodData[0];
            name = foodData[1];
            desc = foodData[2];
            price = Convert.ToDouble(foodData[3]);
        }

        FoodItem item = new FoodItem(name, desc, price);
        foodItems[name] = item;
        if (restaurants[restaurantId].Menus.Count == 0)
        {
            restaurants[restaurantId].AddMenu(new Menu($"M-{restaurantId}-1", "Main Menu"));
            restaurants[restaurantId].Menus[0].AddFoodItem(item);
        }
        else
        {
            restaurants[restaurantId].Menus[0].AddFoodItem(item);
        }
    }
    return foodItems;
}

Dictionary<string, Customer> LoadCustomer()
{
    Dictionary<string, Customer> customers = new Dictionary<string, Customer>();
    string[] data = File.ReadAllLines("customers.csv");
    for (int i = 1; i < data.Length; i++)
    {
        string[] customerdata = data[i].Split(',');
        string name = customerdata[0];
        string email = customerdata[1];
        customers[email] = new Customer(email, name);
    }
    return customers;

}

Dictionary<int, Order> LoadOrders(Dictionary<string, Customer> customers, Dictionary<string, Restaurant> restaurants, Dictionary<string, FoodItem> foodItems)
{
    Dictionary<int, Order> orders = new Dictionary<int, Order>();
    string[] data = File.ReadAllLines("orders.csv");
    for (int i = 1; i < data.Length; i++)
    {

        string[] split = data[i].Split('"');
        string[] order = split[0].Split(",");
        int orderID = Convert.ToInt32(order[0].Trim());
        string email = order[1].Trim();
        string restaurantId = order[2].Trim();
        string date = order[3].Trim();
        string time = order[4].Trim();
        string sdatetime = date + " " + time;
        string address = order[5].Trim();
        DateTime createdDateTime = Convert.ToDateTime(order[6].Trim());
        DateTime deliveryDateTime = Convert.ToDateTime(sdatetime);
        double totalAmount = Convert.ToDouble(order[7].Trim());
        string status = order[8];
        bool paid;
        if (status == "Cancelled" || status == "Rejected")
        {
            paid = false;
        }
        else
        {
            paid = true;
        }
        string[] items = split[1].Split('|');
        Order order1 = new Order(orderID, createdDateTime, totalAmount, status, deliveryDateTime, address, "unkown", paid, customers[email], restaurants[restaurantId], "-");
        for (int j = 0; j < items.Length; j++)
        {
            string[] item = items[j].Split(',');
            int quantity;
            quantity = Convert.ToInt32(item[1]);
            string itemName = item[0];
            string desc = foodItems[itemName].ItemDesc;
            double price = foodItems[itemName].ItemPrice;
            order1.AddOrderedFoodItem(new OrderedFoodItem(itemName, desc, price, quantity));
        }
        orders[orderID] = order1;
        customers[email].AddOrder(order1);
        restaurants[restaurantId].Orders.Enqueue(order1);
    }

    return orders;

}


static void DeleteOrder(List<Order> orderList, Stack<Order> refundStack)
{
    Console.WriteLine("Delete Order");
    Console.WriteLine("============");

    // Email input validation
    Console.Write("Enter Customer Email: ");
    string email = (Console.ReadLine() ?? "").Trim();

    if (string.IsNullOrWhiteSpace(email))
    {
        Console.WriteLine("Email cannot be empty.");
        return;
    }

    // Find pending orders for customer (NO constants needed, safe for spacing/case)
    List<Order> pendingOrders = new List<Order>();

    foreach (Order o in orderList)
    {
        if (o == null)
            continue;

        if (o.Customer == null)
            continue;

        if (o.Customer.EmailAdress == null)
            continue;

        if (o.Customer.EmailAdress.Trim().ToLower() != email.Trim().ToLower())
            continue;

        if (o.OrderStatus != Order.STATUS_PENDING)
            continue;

        pendingOrders.Add(o);
    }

    if (pendingOrders.Count == 0)
    {
        Console.WriteLine("No pending orders found for this customer.");
        return;
    }

    Console.WriteLine("\nPending Orders:");
    foreach (Order o in pendingOrders)
    {
        Console.WriteLine(o.OrderId);
    }

    // Order ID validation
    Console.Write("\nEnter Order ID: ");
    string orderIdInput = (Console.ReadLine() ?? "").Trim();

    if (!int.TryParse(orderIdInput, out int orderId))
    {
        Console.WriteLine("Invalid Order ID. Must be a number.");
        return;
    }

    Order selectedOrder = null;

    foreach (Order o in pendingOrders)
    {
        if (o.OrderId == orderId)
        {
            selectedOrder = o;
            break;
        }
    }

    if (selectedOrder == null)
    {
        Console.WriteLine("Order ID not found.");
        return;
    }

    // Display order details
    Console.WriteLine($"\nCustomer: {selectedOrder.Customer.CustomerName}");
    Console.WriteLine("Ordered Items:");

    int count = 1;
    foreach (OrderedFoodItem item in selectedOrder.OrderedFoodItems)
    {
        Console.WriteLine($"{count}. {item.ItemName} - {item.QtyOrdered}");
        count++;
    }

    Console.WriteLine($"Delivery date/time: {selectedOrder.DeliveryDateTime:dd/MM/yyyy HH:mm}");
    Console.WriteLine($"Total Amount: ${selectedOrder.OrderTotal:0.00}");
    Console.WriteLine($"Order Status: {selectedOrder.OrderStatus}");

    // Confirm deletion validation
    Console.Write("\nConfirm deletion? [Y/N]: ");
    string confirm = (Console.ReadLine() ?? "").Trim().ToUpper();

    while (confirm != "Y" && confirm != "N")
    {
        Console.Write("Invalid option. Confirm deletion? [Y/N]: ");
        confirm = (Console.ReadLine() ?? "").Trim().ToUpper();
    }

    if (confirm == "Y")
    {
        // Cancel order + push to refund stack
        selectedOrder.OrderStatus = "Cancelled";
        refundStack.Push(selectedOrder);

        Console.WriteLine(
            $"Order {selectedOrder.OrderId} cancelled. " +
            $"Refund of ${selectedOrder.OrderTotal:0.00} processed.");
    }
    else
    {
        Console.WriteLine("Order deletion aborted.");
    }
}

void CreateNewOrder()
{
    Console.WriteLine("Create New Order");
    Console.WriteLine("================");
    string email;
    while (true)
    {
        Console.Write("Enter Customer Email: ");
        email = Console.ReadLine();
        if (customers.ContainsKey(email))
        {
            break;
        }
        else
        {
            Console.WriteLine("Invalid customer email");
        }
    }
    string restaurantId;
    while (true)
    {
        Console.Write("Enter Restaurant ID: ");
        restaurantId = Console.ReadLine();
        if (restaurants.ContainsKey(restaurantId))
        {
            break;
        }
        else
        {
            Console.WriteLine("Invalid Restaurant ID");
        }
    }

    DateTime deliveryDateTime;
    while (true)
    {
        Console.Write("Enter Delivery Date (dd/mm/yyyy): ");
        string date = Console.ReadLine();
        Console.Write("Enter Delivery Time (hh:mm): ");
        string time = Console.ReadLine();
        string sdatetime = date + " " + time;
        try
        {
            deliveryDateTime = Convert.ToDateTime(sdatetime);
            break;
        }
        catch
        {
            Console.WriteLine("Invalid delivery date/time. Use dd/MM/yyyy and HH:mm.");
        }
    }

    Console.Write("Enter Delivery Address: ");
    string address = Console.ReadLine();
    Console.WriteLine("");


    Console.WriteLine("Available Food Items:");
    int num = 1;
    foreach (FoodItem foodItem in restaurants[restaurantId].Menus[0].FoodItems)
    {
        Console.WriteLine($"{num}. {foodItem.ItemName} - ${foodItem.ItemPrice.ToString("0.00")}");
        num += 1;
    }


    List<OrderedFoodItem> orderedFoodItems = new List<OrderedFoodItem>();
    int itemNu;
    int quantity;
    int maxItem = restaurants[restaurantId].Menus[0].FoodItems.Count;
    while (true)
    {
        Console.Write("Enter item number (0 to finish): ");
        try
        {
            itemNu = Convert.ToInt32(Console.ReadLine());
            if (itemNu < 0 || itemNu > maxItem)
            {
                Console.WriteLine($"Invalid item number. Enter 1 to {maxItem}, or 0 to finish.");
                continue;
            }
        }
        catch
        {
            Console.WriteLine("Item number must be a number.");
            continue;
        }

        if (itemNu == 0)
        {
            break;
        }
        Console.Write("Enter quantity: ");
        try
        {
            quantity = Convert.ToInt32(Console.ReadLine());
        }
        catch
        {
            Console.WriteLine("Quantity must be a number.");
            continue;
        }

        while (true)
        {
            if (quantity > 0)
            {
                orderedFoodItems.Add(new OrderedFoodItem(restaurants[restaurantId].Menus[0].FoodItems[itemNu - 1].ItemName, restaurants[restaurantId].Menus[0].FoodItems[itemNu - 1].ItemDesc, restaurants[restaurantId].Menus[0].FoodItems[itemNu - 1].ItemPrice, quantity));
                break;
            }
            else
            {
                Console.WriteLine("Quantity must be greater than 0!");
            }
        }
    }

    if (orderedFoodItems.Count == 0)
    {
        Console.WriteLine("No items selected. Order not created.");
        return;
    }

    string requestChoice;
    string specialRequest;
    while (true)
    {
        Console.Write("Add special request? [Y/N]:");
        requestChoice = Console.ReadLine();
        if (requestChoice == "Y")
        {
            Console.Write("Please enter your special request: ");
            specialRequest = Console.ReadLine();
            break;
        }
        else if (requestChoice == "N")
        {
            specialRequest = "-";
            break;
        }
        else
        {
            Console.WriteLine("Invalid Option");
        }
    }
    double subtotal = 0;
    foreach (OrderedFoodItem item in orderedFoodItems)
    {
        subtotal += item.CalculateSubTotal();
    }
    double total = subtotal + 5;
    Console.WriteLine("");
    Console.WriteLine($"Order total: ${subtotal.ToString("0.00")} + $5.00 (delivery) = ${total.ToString("0.00")}");
    string paymentMethod;
    string paymentOption;
    while (true)
    {
        Console.Write("Proceed to payment? [Y/N]: ");
        paymentOption = Console.ReadLine();
        Console.WriteLine("");
        if (paymentOption != "Y" && paymentOption != "N")
        {
            Console.WriteLine("Invalid Option");
        }
        else
        {
            break;
        }
    }
    if (paymentOption == "Y")
    {
        while (true)
        {
            Console.Write("Payment method:\n[CC] Credit Card / [PP] PayPal / [CD] Cash on Delivery: ");
            paymentMethod = Console.ReadLine();
            if (paymentMethod == "CC" || paymentMethod == "PP" || paymentMethod == "CD")
            {
                break;
            }
            else
            {
                Console.WriteLine("Invalid Option");
            }
        }
    }
    else
    {
        Console.WriteLine("See you again!");
        return;
    }
    int maxOrderId = 0;
    foreach (Order o in orders.Values)
    {
        if (o.OrderId > maxOrderId)
        {
            maxOrderId = o.OrderId;
        }
    }

    Order order = new Order(maxOrderId + 1, DateTime.Now, subtotal, "Pending", deliveryDateTime, address, paymentMethod, true, customers[email], restaurants[restaurantId], specialRequest);
    foreach (OrderedFoodItem item in orderedFoodItems)
    {
        order.AddOrderedFoodItem(item);
    }
    restaurants[restaurantId].Orders.Enqueue(order);
    customers[email].AddOrder(order);
    orders[order.OrderId] = order;
    string items = "";
    foreach (OrderedFoodItem ofi in order.OrderedFoodItems)
    {
        items += $"{ofi.ItemName},{ofi.QtyOrdered}|";
    }
    items = items.Trim('|');
    string line;
    using (StreamWriter sw = new StreamWriter("orders.csv", true))
    {
        sw.WriteLine($"{order.OrderId},{order.Customer.EmailAdress},{order.Restaurant.RestaurantId},{order.DeliveryDateTime.ToString("dd/MM/yyyy")},{order.DeliveryDateTime.ToString("HH:mm")},{order.DeliveryAddress},{order.OrderDateTime.ToString("dd/MM/yyyy HH:mm")},{total.ToString("0.00")},{order.OrderStatus},{items}");
    }


    Console.WriteLine("");
    Console.WriteLine($"Order {order.OrderId} created successfully! Status: {order.OrderStatus}");
    Console.WriteLine("");
}

void ModifyOrder()
{
    Console.WriteLine("Modify Order");
    Console.WriteLine("============");
    string email;
    while (true)
    {
        Console.Write("Enter Customer Email: ");
        email = Console.ReadLine();
        if (customers.ContainsKey(email))
        {
            break;
        }
        else
        {
            Console.WriteLine("Invalid customer email");
        }
    }

    Console.WriteLine("Pending Orders:");
    List<Order> pendingOrders = new List<Order>();

    foreach (Order order in customers[email].Orders)
    {
        if (order.OrderStatus == "Pending")
        {
            Console.WriteLine(order.OrderId);
            pendingOrders.Add(order);
        }
    }

    if (pendingOrders.Count == 0)
    {
        Console.WriteLine("No pending orders found.");
        return;
    }

    int orderid;
    Order selectedOrder = null;

    while (true)
    {
        Console.Write("Enter OrderID: ");
        try
        {
            orderid = Convert.ToInt32(Console.ReadLine());
        }
        catch
        {
            Console.WriteLine("OrderID must be a number. Try Again!");
            continue;
        }

        foreach (Order order in pendingOrders)
        {
            if (order.OrderId == orderid)
            {
                selectedOrder = order;
                break;
            }
        }

        if (selectedOrder != null)
        {
            break;
        }
        else
        {
            Console.WriteLine("Invalid OrderID. Try Again!");
        }
    }

    Console.WriteLine("Order Items:");
    int num = 1;
    foreach (OrderedFoodItem item in selectedOrder.OrderedFoodItems)
    {
        Console.WriteLine($"{num}. {item.ItemName} - {item.QtyOrdered}");
        num++;
    }

    Console.WriteLine("Address:");
    Console.WriteLine(selectedOrder.DeliveryAddress);

    Console.WriteLine("Delivery Date/Time:");
    Console.WriteLine($"{selectedOrder.DeliveryDateTime.ToString("dd/MM/yyyy")}, {selectedOrder.DeliveryDateTime.ToString("HH:mm")}");

    string opt;
    while (true)
    {
        Console.Write("Modify: [1] Items [2] Address [3] Delivery Time: ");
        opt = Console.ReadLine();
        if (opt == "1" || opt == "2" || opt == "3")
        {
            break;
        }
        else
        {
            Console.WriteLine("Option invalid. Try Again!");
        }
    }

    double oldTotal = selectedOrder.OrderTotal;

    if (opt == "1")
    {
        string itemopt;
        while (true)
        {
            Console.Write("Modify Items: [1] Change quantity [2] Remove item [3] Add new item: ");
            itemopt = Console.ReadLine();
            if (itemopt == "1" || itemopt == "2" || itemopt == "3")
            {
                break;
            }
            else
            {
                Console.WriteLine("Option invalid. Try Again!");
            }
        }

        if (itemopt == "1")
        {
            while (true)
            {
                Console.Write("Enter item number to modify: ");

                int itemnum;
                try
                {
                    itemnum = Convert.ToInt32(Console.ReadLine());
                }
                catch
                {
                    Console.WriteLine("Invalid item number. Enter a number.");
                    continue;
                }


                if (itemnum < 1 || itemnum > selectedOrder.OrderedFoodItems.Count)
                {
                    Console.WriteLine("Invalid item number. Try Again!");
                }
                else
                {
                    int oldQty = selectedOrder.OrderedFoodItems[itemnum - 1].QtyOrdered;
                    int newqty;
                    while (true)
                    {
                        Console.Write("Enter quantity: ");
                        try
                        {
                            newqty = Convert.ToInt32(Console.ReadLine());
                        }
                        catch
                        {
                            Console.WriteLine("Quantity must be a number.");
                            continue;
                        }

                        if (newqty > 0) break;

                        Console.WriteLine("Quantity must be more than 0. Try again!");
                    }

                    selectedOrder.OrderedFoodItems[itemnum - 1].QtyOrdered = newqty;

                    selectedOrder.CalculateOrderTotal();

                    if (selectedOrder.OrderTotal > oldTotal)
                    {
                        double diff = selectedOrder.OrderTotal - oldTotal;
                        Console.WriteLine($"Additional payment required: ${diff:0.00}");

                        string payopt;
                        while (true)
                        {
                            Console.Write("Proceed to payment? [Y/N]: ");
                            payopt = Console.ReadLine().Trim().ToUpper();

                            if (payopt == "Y" || payopt == "N") break;
                            Console.WriteLine("Invalid Option. Try Again!");
                        }

                        if (payopt == "N")
                        {
                            selectedOrder.OrderedFoodItems[itemnum - 1].QtyOrdered = oldQty;
                            selectedOrder.CalculateOrderTotal();

                            Console.WriteLine("Payment not made. Order not modified.");
                            break;
                        }

                        Console.WriteLine("Payment successful.");
                    }

                    Console.WriteLine("");
                    Console.WriteLine($"Quantity updated for {selectedOrder.OrderedFoodItems[itemnum - 1].ItemName}. Quantity now: {newqty}");
                    break;
                }
            }
        }
        else if (itemopt == "2")
        {
            while (true)
            {
                Console.Write("Enter item number to remove: ");
                int itemnum;
                try
                {
                    itemnum = Convert.ToInt32(Console.ReadLine());
                }
                catch
                {
                    Console.WriteLine("Invalid item number.");
                    continue;
                }


                if (itemnum < 1 || itemnum > selectedOrder.OrderedFoodItems.Count)
                {
                    Console.WriteLine("Invalid item number. Try Again!");
                }
                else
                {
                    string removedName = selectedOrder.OrderedFoodItems[itemnum - 1].ItemName;

                    selectedOrder.RemoveOrderedFoodItem(selectedOrder.OrderedFoodItems[itemnum - 1]);
                    selectedOrder.CalculateOrderTotal();

                    Console.WriteLine("");
                    Console.WriteLine($"Item {removedName} removed.");
                    break;
                }
            }
        }
        else
        {
            Console.WriteLine("Available Food Items:");
            int itemCount = selectedOrder.Restaurant.Menus[0].FoodItems.Count;

            for (int i = 0; i < itemCount; i++)
            {
                FoodItem fooditem = selectedOrder.Restaurant.Menus[0].FoodItems[i];
                Console.WriteLine($"{i + 1}. {fooditem.ItemName} - ${fooditem.ItemPrice:0.00}");
            }

            int itemnum;
            while (true)
            {
                Console.Write("Enter item number to add: ");
                try
                {
                    itemnum = Convert.ToInt32(Console.ReadLine());
                }
                catch
                {
                    Console.WriteLine("Invalid item number.");
                    continue;
                }


                if (itemnum >= 1 && itemnum <= itemCount)
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid item number. Try again!");
                }
            }

            int qty;
            while (true)
            {
                Console.Write("Enter quantity: ");
                try
                {
                    qty = Convert.ToInt32(Console.ReadLine());
                }
                catch
                {
                    Console.WriteLine("Quantity must be a number.");
                    continue;
                }


                if (qty > 0)
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Quantity must be more than 0. Try again!");
                }
            }


            FoodItem foodItem = selectedOrder.Restaurant.Menus[0].FoodItems[itemnum - 1];
            OrderedFoodItem addedItem = new OrderedFoodItem(foodItem.ItemName, foodItem.ItemDesc, foodItem.ItemPrice, qty);
            selectedOrder.AddOrderedFoodItem(addedItem);


            selectedOrder.CalculateOrderTotal();


            if (selectedOrder.OrderTotal > oldTotal)
            {
                double diff = selectedOrder.OrderTotal - oldTotal;
                Console.WriteLine($"Additional payment required: ${diff.ToString("0.00")}");

                string payopt;
                while (true)
                {
                    Console.Write("Proceed to Payment [Y/N]: ");
                    payopt = Console.ReadLine().Trim().ToUpper();

                    if (payopt == "Y" || payopt == "N")
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Invalid Option. Try Again!");
                    }
                }

                if (payopt == "N")
                {
                    selectedOrder.OrderedFoodItems.Remove(addedItem);
                    selectedOrder.CalculateOrderTotal();

                    Console.WriteLine("Payment not made. Order not modified.");
                }
                else
                {
                    Console.WriteLine("Payment successful.");
                    Console.WriteLine($"Item {foodItem.ItemName} added.");
                }
            }

        }
    }

    else if (opt == "2")
    {
        Console.Write("Enter new Address: ");
        selectedOrder.DeliveryAddress = Console.ReadLine();
        Console.WriteLine($"Order {orderid} updated. New Address: {orders[orderid].DeliveryAddress}");
    }
    else
    {
        while (true)
        {
            Console.Write("Enter new Delivery Time (hh:mm): ");
            string time = Console.ReadLine();
            string date = selectedOrder.DeliveryDateTime.ToString("dd/MM/yyyy");
            try
            {
                selectedOrder.DeliveryDateTime = Convert.ToDateTime(date + " " + time);
                break;
            }
            catch
            {
                Console.WriteLine("Invalid time format. Use HH:mm (e.g. 14:30).");
            }
        }

        Console.WriteLine($"Order {orderid} updated. New Delivery Time: {orders[orderid].DeliveryDateTime.ToString("HH:mm")}");
    }
}

void BulkOrder()
    {
        Queue<Order> orderqueue = new Queue<Order>();
        foreach (Restaurant restaurant in restaurants.Values)
        {
            foreach (Order order in restaurant.Orders)
            {
                if (order.OrderStatus == "Pending" && order.DeliveryDateTime.Date == DateTime.Now.Date)
                {
                    orderqueue.Enqueue(order);
                }
            }
        }

        Console.WriteLine($"Total Pending Order Queues for Today: {orderqueue.Count}");

        int rejectedCount = 0;
        int preparingCount = 0;
        int processedCount;
        foreach (Order order in orderqueue)
        {
            TimeSpan diff = order.DeliveryDateTime - DateTime.Now;
            if (diff.TotalMinutes < 60)
            {
                order.OrderStatus = "Rejected";
                rejectedCount += 1;
                refundStack.Push(order);
            }
            else
            {
                order.OrderStatus = "Preparing";
                preparingCount += 1;
            }
        }
        processedCount = rejectedCount + preparingCount;
        double doubleProcessed = Convert.ToDouble(processedCount);
        double percent = (doubleProcessed / orders.Count) * 100;
        Console.WriteLine("");
        Console.WriteLine($"Total Orders Processed for Today: {processedCount}");
        Console.WriteLine($"Preparing: {preparingCount} Rejected: {rejectedCount}");
        Console.WriteLine($"Percentage of automatically processed orders against all orders: {percent.ToString("0.00")}%");
        Console.WriteLine("");


    }
}

