











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
    if (choice == 0)
    {
        Console.WriteLine("Exiting...");
    }
    else if (choice == 1)
    {

    }
    else if (choice == 2)
    {

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