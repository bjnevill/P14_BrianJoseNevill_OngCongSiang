
using S10274174G_PRG2Assignment;
using System.Runtime.CompilerServices;
using System.Security.Principal;

Dictionary<string, Restaurant> restaurants = LoadRestaurants();
Dictionary<string, FoodItem> foodItems = LoadFoodItems(restaurants);
Dictionary<string, Customer> customers = LoadCustomer();
Dictionary<int, Order> orders = LoadOrders(customers, restaurants, foodItems);

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
        Console.WriteLine("All Restaurants and Menu Items");
        Console.WriteLine("==============================");
        foreach (Restaurant restaurant in restaurants.Values)
        {
            restaurant.DisplayMenus();
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

    }
    else if (choice == 5)
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
            orderid = Convert.ToInt32(Console.ReadLine());

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
                    int itemnum = Convert.ToInt32(Console.ReadLine());

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
                            newqty = Convert.ToInt32(Console.ReadLine());
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
                    int itemnum = Convert.ToInt32(Console.ReadLine());

                    if (itemnum < 1 || itemnum > selectedOrder.OrderedFoodItems.Count)
                    {
                        Console.WriteLine("Invalid item number. Try Again!");
                    }
                    else
                    {
                        string removedName = selectedOrder.OrderedFoodItems[itemnum - 1].ItemName;

                        selectedOrder.RemoveOrderedFoodItem(selectedOrder.OrderedFoodItems[itemnum - 1]);

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
                    itemnum = Convert.ToInt32(Console.ReadLine());

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
                    qty = Convert.ToInt32(Console.ReadLine());

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
            Console.Write("Enter new Delivery Time (hh:mm): ");
            string time = Console.ReadLine();
            string date = selectedOrder.DeliveryDateTime.ToString("dd/MM/yyyy");
            selectedOrder.DeliveryDateTime = Convert.ToDateTime(date + " " + time);

            Console.WriteLine($"Order {orderid} updated. New Delivery Time: {orders[orderid].DeliveryDateTime.ToString("HH:mm")}");
        }

    }
    else if (choice == 6)
    {

    }
    else
    {
        Console.WriteLine("Inavlid Option");
    }

} while (choice != 0);


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
        int orderID = Convert.ToInt32(order[0]);
        string email = order[1];
        string restaurantId = order[2];
        string date = order[3];
        string time = order[4];
        string sdatetime = date + " " + time;
        string address = order[5];
        DateTime createdDateTime = Convert.ToDateTime(order[6]);
        DateTime deliveryDateTime = Convert.ToDateTime(sdatetime);
        double totalAmount = Convert.ToDouble(order[7]);
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
        Order order1 = new Order(orderID, createdDateTime, totalAmount, status, deliveryDateTime, address, "unkown", paid, customers[email], restaurants[restaurantId],"-");
        for (int j = 0; j < items.Length; j++)
        {
            string[] item = items[j].Split(',');
            int quantity;
            if (item.Length < 2)
            {
                quantity = 1;
            }
            else
            {
                quantity = Convert.ToInt32(item[1]);
            }
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
    Console.Write("Enter Delivery Date (dd/mm/yyyy): ");
    string date = Console.ReadLine();
    Console.Write("Enter Delivery Time (hh:mm): ");
    string time = Console.ReadLine();
    Console.Write("Enter Delivery Address: ");
    string address = Console.ReadLine();
    Console.WriteLine("");
    string sdatetime = date + " " + time;
    DateTime deliveryDateTime = Convert.ToDateTime(sdatetime);


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
    while (true)
    {
        Console.Write("Enter item number (0 to finish): ");
        itemNu = Convert.ToInt32(Console.ReadLine());
        if (itemNu == 0)
        {
            break;
        }
        Console.Write("Enter quantity: ");
        quantity = Convert.ToInt32(Console.ReadLine());
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

    Console.WriteLine("");
    Console.WriteLine($"Order {order.OrderId} created successfully! Status: {order.OrderStatus}");
    Console.WriteLine("");
}

