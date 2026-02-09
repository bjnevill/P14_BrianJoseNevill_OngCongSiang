//==========================================================
// Student Number : S10274174G
// Student Name : Brian Jose Nevill - Questions:2,3,5,7, Advanced Feature: Bulk Processing, Bonus Feature: Favourite Orders
// Partner Name : Ong Cong Siang - Questions:1,4,6,8, Advanced Feature: Restaurant Sub Menu, Bonus Feature: Display Total
//==========================================================

using S10274174G_PRG2Assignment;
using System.Runtime.CompilerServices;
using System.Security.Principal;
using static System.Runtime.InteropServices.JavaScript.JSType;

Dictionary<string, Restaurant> restaurants = LoadRestaurants();
Dictionary<string, FoodItem> foodItems = LoadFoodItems(restaurants);
Dictionary<string, Customer> customers = LoadCustomer();
Dictionary<int, Order> orders = LoadOrders(customers, restaurants, foodItems);
LoadFavouruite();

Stack<Order> refundStack = new Stack<Order>();

Console.WriteLine("Welcome to the Gruberoo Food Delivery System");
Console.WriteLine($"{restaurants.Count} restaurants loaded!");
Console.WriteLine($"{foodItems.Count} food items loaded!");
Console.WriteLine($"{customers.Count} customers loaded!");
Console.WriteLine($"{orders.Count} orders loaded!");
Console.WriteLine("");



string choice;
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
    Console.WriteLine("9. Favourite Orders");
    Console.WriteLine("0. Exit");
    Console.Write("Enter your choice: ");
    choice = Console.ReadLine();
    Console.WriteLine("");
    if (choice == "0")
    {
        Console.WriteLine("Exiting...");
    }
    else if (choice == "1")
    {

        // Student Name : Brian Jose Nevill - Basic Feature - 3
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
    else if (choice == "2")
    {
        // Partner Name : Ong Cong Siang - Basic Feature - 1
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
    else if (choice == "3")
    {
        CreateNewOrder();
    }
    else if (choice == "4")
    {
        ProcessOrder();
    }
    else if (choice == "5")
    {
        ModifyOrder();
    }
    else if (choice == "6")
    {
        DeleteOrder(orders.Values.ToList(), refundStack);
    }
    else if (choice == "7")
    {
        BulkOrder();
    }
    else if (choice == "8")
    {
        DisplayTotals(restaurants);
    }
    else if (choice == "9")
    {
        FavouriteOrder();

    }
    else
    {
        Console.WriteLine("Inavlid Option");

    }

    Console.WriteLine("");

} while (choice != "0");
Console.WriteLine("Thank you for using Gruberoo Food Delivery System. Goodbye!");

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
SaveFavouritesToCsv();


// Student Name : Brian Jose Nevill - Bonus Feature - Favourite Orders
void SaveFavouritesToCsv()
{
    string data = "";
    string itemline;

    foreach (Customer customer in customers.Values)
    {
        foreach (Favourite fav in customer.Favourites)
        {
            string custEmail = customer.EmailAdress;
            string favouriteName = fav.FavouriteName;
            string restaurantId = fav.RestuarantId;

            string items = "";
            foreach (OrderedFoodItem ofi in fav.Items)
            {
                items += $"{ofi.ItemName},{ofi.QtyOrdered}|";
            }
            items = items.Trim('|');
            itemline = $"\"{items}\"";

            string specialRequest = fav.SpecialRequest;
            if (specialRequest == "-")
            {
                specialRequest = "-";
            }

            data += $"{custEmail},{favouriteName},{restaurantId},{itemline},{specialRequest}\n";
        }
    }

    data = data.Trim('\n');

    using (StreamWriter sw = new StreamWriter("favourite.csv", false))
    {
        sw.WriteLine("CustomerEmail,FavouriteName,RestaurantId,Items,SpecialRequest");
        sw.WriteLine(data);
    }
}

void SaveOrdersToCsv(List<Order> orderList, string filename)
{
    string data = "";
    string itemline;
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
        itemline = $"\"{items}\"";
        data += $"{orderid},{custEmail},{restaurantId},{deliveryDate},{deliveryTime},{deliveryAddress},{createdDateTime},{totalAmount},{status},{itemline}\n";
    }

    data = data.Trim('\n');
    using (StreamWriter sw = new StreamWriter(filename, false))
    {
        sw.WriteLine("OrderId,CustomerEmail,RestaurantId,DeliveryDate,DeliveryTime,DeliveryAddress,CreatedDateTime,TotalAmount,Status,Items");
        sw.WriteLine(data);
    }
}



// Partner Name : Ong Cong Siang - Bonus Feature - Disyplay Total 
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

// Partner Name : Ong Cong Siang - Advanced Feature - Restaurarant Sub Menu
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

// Partner Name : Ong Cong Siang - Basic Feature - 4
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

// Partner Name : Ong Cong Siang - Basic Feature - 4
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

// Student Name : Brian Jose Nevill - Basic Feature - 2
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

// Student Name : Brian Jose Nevill - Basic Feature - 2
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

// Student Name : Brian Jose Nevill - Bonus Feature - Favourite Orders
void LoadFavouruite()
{

    string[] data = File.ReadAllLines("favourite.csv");
    for (int i = 1; i < data.Length; i++)
    {
        string[] split = data[i].Split('"');
        if (split.Length < 2)
        {
            continue;
        }

        string[] favData = split[0].Split(',');
        string email = favData[0];
        string name = favData[1];
        string id = favData[2];
        string specialRequest = split[2];
        string[] items = split[1].Split('|');
        Favourite favOrder = new Favourite(name, id, specialRequest);
        for (int j = 0; j < items.Length; j++)
        {
            string[] item = items[j].Split(',');
            int quantity;
            quantity = Convert.ToInt32(item[1]);
            string itemName = item[0];
            string desc = foodItems[itemName].ItemDesc;
            double price = foodItems[itemName].ItemPrice;
            favOrder.Items.Add(new OrderedFoodItem(itemName, desc, price, quantity));
        }
        customers[email].Favourites.Add(favOrder);

    }

}


// Partner Name : Ong Cong Siang - Basic Feature - 8
void DeleteOrder(List<Order> orderList, Stack<Order> refundStack)
{
    Console.WriteLine("Delete Order");
    Console.WriteLine("============");

    Console.Write("Enter Customer Email: ");
    string email = (Console.ReadLine() ?? "").Trim();

    if (string.IsNullOrWhiteSpace(email))
    {
        Console.WriteLine("Email cannot be empty.");
        return;
    }

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

    Console.Write("\nConfirm deletion? [Y/N]: ");
    string confirm = (Console.ReadLine() ?? "").Trim().ToUpper();

    while (confirm != "Y" && confirm != "N")
    {
        Console.Write("Invalid option. Confirm deletion? [Y/N]: ");
        confirm = (Console.ReadLine() ?? "").Trim().ToUpper();
    }

    if (confirm == "Y")
    {
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


// Student Name : Brian Jose Nevill - Basic Feature - 5
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
    string itemline;
    foreach (OrderedFoodItem ofi in order.OrderedFoodItems)
    {
        items += $"{ofi.ItemName},{ofi.QtyOrdered}|";
    }
    items = items.Trim('|');
    itemline = $"\"{items}\"";
    string line;
    using (StreamWriter sw = new StreamWriter("orders.csv", true))
    {
        sw.WriteLine($"{order.OrderId},{order.Customer.EmailAdress},{order.Restaurant.RestaurantId},{order.DeliveryDateTime.ToString("dd/MM/yyyy")},{order.DeliveryDateTime.ToString("HH:mm")},{order.DeliveryAddress},{order.OrderDateTime.ToString("dd/MM/yyyy HH:mm")},{total.ToString("0.00")},{order.OrderStatus},{items}");
    }


    Console.WriteLine("");
    Console.WriteLine($"Order {order.OrderId} created successfully! Status: {order.OrderStatus}");
    Console.WriteLine("");
}


// Student Name : Brian Jose Nevill - Basic Feature - 7
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

                        if (newqty > 0)
                        {
                            break;
                        }

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


// Student Name : Brian Jose Nevill - Advanced Feature - Bulk Order Processing
void BulkOrder()
{
    Queue<Order> orderqueue = new Queue<Order>();
    foreach (Restaurant restaurant in restaurants.Values)
    {
        foreach (Order order in restaurant.Orders)
        {
            if (order.OrderStatus == "Pending" && order.DeliveryDateTime.Date ==  DateTime.Now.Date)
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
        int diff = order.DeliveryDateTime.Subtract(DateTime.Now).Minutes;
        if (diff < 60)
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

// Student Name : Brian Jose Nevill - Bonus Feature - Favourite Orders
void FavouriteOrder()
{
    Customer customer;
    while (true)
    {
        Console.Write("Enter customer email:");
        string email = Console.ReadLine();
        if (customers.ContainsKey(email))
        {
            customer = customers[email];
            break;
        }
        else
        {
            Console.WriteLine("Invalid Email.Please Try Again");
        }
    }
    Console.WriteLine("");
    Console.WriteLine("1.Add Favourite Order");
    Console.WriteLine("2.Reorder Favourite Order");
    Console.WriteLine("3.Delete Favourite Order");
    Console.Write("Choose Option:");
    string opt = Console.ReadLine();
    if (opt == "2")
    {
        if (customer.Favourites.Count > 0)
        {
            int num = 1;
            foreach (Favourite fav in customer.Favourites)
            {
                Console.WriteLine($"{num}.{fav.FavouriteName}");
                num += 1;
            }
            int chose;
            Favourite chosen;
            while (true)
            {
                Console.WriteLine("");
                Console.Write("Choose Order to Order: ");
                try
                {
                    chose = Convert.ToInt32(Console.ReadLine());
                    if (chose < 1 || chose > customer.Favourites.Count)
                    {
                        Console.WriteLine("Invalid item number. Try Again!");
                    }
                    chosen = customer.Favourites[chose - 1];
                    break;
                }
                catch
                {
                    Console.WriteLine("Invalid Option");
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

            double subtotal = 0;
            foreach (OrderedFoodItem item in chosen.Items)
            {
                subtotal += item.CalculateSubTotal();
            }
            double total = subtotal + 5;
            Console.WriteLine("");
            Console.WriteLine($"Order total: ${subtotal.ToString("0.00")} + $5.00 (delivery) = ${total.ToString("0.00")}");
            string paymentMethod = "-";
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

            Order order = new Order(maxOrderId + 1, DateTime.Now, total, "Pending", deliveryDateTime, address, paymentMethod, true, customer, restaurants[chosen.RestuarantId], chosen.SpecialRequest);

            foreach (OrderedFoodItem item in chosen.Items)
            {
                order.AddOrderedFoodItem(new OrderedFoodItem(item.ItemName, item.ItemDesc, item.ItemPrice, item.QtyOrdered));
            }

            restaurants[chosen.RestuarantId].Orders.Enqueue(order);
            customer.AddOrder(order);
            orders[order.OrderId] = order;
            string items = "";
            string itemline;
            foreach (OrderedFoodItem ofi in order.OrderedFoodItems)
            {
                items += $"{ofi.ItemName},{ofi.QtyOrdered}|";
            }
            items = items.Trim('|');
            itemline = $"\"{items}\"";
            string line;
            using (StreamWriter sw = new StreamWriter("orders.csv", true))
            {
                sw.WriteLine($"{order.OrderId},{order.Customer.EmailAdress},{order.Restaurant.RestaurantId},{order.DeliveryDateTime.ToString("dd/MM/yyyy")},{order.DeliveryDateTime.ToString("HH:mm")},{order.DeliveryAddress},{order.OrderDateTime.ToString("dd/MM/yyyy HH:mm")},{total.ToString("0.00")},{order.OrderStatus},{items}");
            }


            Console.WriteLine("");
            Console.WriteLine($"Order {order.OrderId} created successfully! Status: {order.OrderStatus}");
            Console.WriteLine("");

        }
        else
        {
            Console.WriteLine("Custoemr has no favourite orders.");
            return;
        }
    }

    else if (opt == "3")
    {
        if (customer.Favourites.Count == 0)
        {
            Console.WriteLine("Customer has no favourite orders.");
            return;
        }

        Console.WriteLine("Favourite Orders");
        Console.WriteLine("================");
        for (int i = 0; i < customer.Favourites.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {customer.Favourites[i].FavouriteName}");
        }

        int choice;
        while (true)
        {
            try
            {
                Console.Write("Please enter a number: ");
                choice = Convert.ToInt32(Console.ReadLine());
               
            }
            catch
            {
                Console.WriteLine("Please enter a number.");
                continue;
            }

            if (choice < 1 || choice > customer.Favourites.Count)
            {
                Console.WriteLine("Invalid selection.");
                continue;
            }
            else
            {
                break;
            }
        }

        while (true)
        {
            Console.Write($"Confirm delete '{customer.Favourites[choice - 1].FavouriteName}'? [Y/N]: ");
            string confirm = Console.ReadLine();

            if (confirm == "Y")
            {
                customer.Favourites.RemoveAt(choice - 1);
                Console.WriteLine("Favourite order deleted.");
                break;
            }
            else if (confirm == "N")
            {
                Console.WriteLine("Deletion Cancelled.");
                return;
            }
            else
            {
                Console.WriteLine("Please provide valid input Y or N");
            }
        }
    }
    else if (opt == "1")
    {
        if (customer.Orders.Count == 0)
        {
            Console.WriteLine("Customer has no orders to favourite.");
            return;
        }

        Console.WriteLine("Customer Orders");
        Console.WriteLine("===============");
        foreach (Order o in customer.Orders)
        {
            string items = "";

            foreach (OrderedFoodItem item in o.OrderedFoodItems)
            {
                items += $"{item.ItemName} x{item.QtyOrdered}, ";
            }

            items = items.TrimEnd(',', ' ');

            Console.WriteLine($"{o.OrderId} - {o.Restaurant.RestaurantName} [{items}]");
        }


        int orderId;
        Order selectedOrder;
        while (true)
        {
            Console.Write("Enter Order ID to save as favourite: ");
            try
            {
                orderId = Convert.ToInt32(Console.ReadLine());
            }
            catch
            {
                Console.WriteLine("Order ID must be a number.");
                continue;
            }

            selectedOrder = null;

            foreach (Order o in customer.Orders)
            {
                if (o.OrderId == orderId)
                {
                    selectedOrder = o;
                    break;
                }
            }

            if (selectedOrder == null)
            {
                Console.WriteLine("Invalid Order ID. Try again.");
            }
            else
            {
                break;
            }
        }

        Console.Write("Enter Favourite Name: ");
        string favName = Console.ReadLine();
        if (favName == "")
        {
            favName = $"Favourite-{selectedOrder.OrderId}";
        }

        Favourite fav = new Favourite(favName, selectedOrder.Restaurant.RestaurantId, selectedOrder.SpecialRequest);

        foreach (OrderedFoodItem item in selectedOrder.OrderedFoodItems)
        {
            fav.Items.Add(new OrderedFoodItem(item.ItemName, item.ItemDesc, item.ItemPrice, item.QtyOrdered));
        }

        customer.Favourites.Add(fav);

        Console.WriteLine($"Favourite order '{fav.FavouriteName}' added successfully.");
    }

    else
    {
        Console.WriteLine("Inavlid Option. Try Again.");
    }

}

// Partner Name : Ong Cong Siang - Basic Feature - 6
void ProcessOrder()
{
    Console.WriteLine("Process Order");
    Console.WriteLine("=============");

    string restaurantId;
    while (true)
    {
        Console.Write("Enter Restaurant ID: ");
        restaurantId = Console.ReadLine();

        if (restaurants.ContainsKey(restaurantId))
            break;

        Console.WriteLine("Invalid Restaurant ID.");
    }

    Queue<Order> orderQueue = restaurants[restaurantId].Orders;

    if (orderQueue.Count == 0)
    {
        Console.WriteLine("No orders found for this restaurant.");
        return;
    }

    int count = orderQueue.Count;

    for (int i = 0; i < count; i++)
    {
        Order order = orderQueue.Dequeue();

        if (order.OrderStatus != Order.STATUS_PENDING &&
            order.OrderStatus != Order.STATUS_CANCELLED &&
            order.OrderStatus != Order.STATUS_PREPARING)
        {
            orderQueue.Enqueue(order);
            continue;
        }

        order.DisplayForProcessing();

        string option;
        while (true)
        {
            Console.Write("[C]onfirm / [R]eject / [S]kip / [D]eliver: ");
            option = Console.ReadLine().Trim().ToUpper();

            if (option == "C" || option == "R" || option == "S" || option == "D")
                break;

            Console.WriteLine("Invalid option.");
        }

        if (option == "C")
        {
            if (order.OrderStatus == Order.STATUS_PENDING)
            {
                order.OrderStatus = Order.STATUS_PREPARING;
                Console.WriteLine($"Order {order.OrderId} confirmed. Status: Preparing");
            }
            else
            {
                Console.WriteLine("Order cannot be confirmed.");
            }
        }
        else if (option == "R")
        {
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
        }
        else if (option == "S")
        {
            if (order.OrderStatus == Order.STATUS_CANCELLED)
            {
                Console.WriteLine("Order skipped.");
            }
            else
            {
                Console.WriteLine("Only cancelled orders can be skipped.");
            }
        }
        else // D
        {
            if (order.OrderStatus == Order.STATUS_PREPARING)
            {
                order.OrderStatus = Order.STATUS_DELIVERED;
                Console.WriteLine($"Order {order.OrderId} delivered successfully.");
            }
            else
            {
                Console.WriteLine("Order cannot be delivered.");
            }
        }

        orderQueue.Enqueue(order);
        Console.WriteLine("");
    }
}
