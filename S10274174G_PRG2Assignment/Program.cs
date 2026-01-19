
using S10274174G_PRG2Assignment;

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

    }
    else if (choice == 4)
    {

    }
    else if (choice == 5)
    {

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
        double totalAmount = Convert.ToDouble(order[7]);
        string status = order[8];
        bool paid;
        if (status == "Cancelled")
        {
            paid = false;
        }
        else
        {
            paid = true;
        }
        DateTime datetime = Convert.ToDateTime(sdatetime);
        string[] items = split[1].Split('|');
        Order order1 = new Order(orderID, datetime, totalAmount, status, createdDateTime, address, "unkown", paid, customers[email], restaurants[restaurantId]);
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

