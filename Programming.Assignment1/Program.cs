using System.Collections.Generic;

class Program
{
    static void Main()
    {
        Inventory inventory = new Inventory();
        Cart cart = new Cart(inventory);

        while (true)
        {
            Console.WriteLine("1. Inventory Management");
            Console.WriteLine("2. Shop");
            Console.WriteLine("3. Exit");
            Console.Write("Select your option: ");
            int choice = int.Parse(Console.ReadLine() ?? "");

            if (choice == 1)
            {
                InventoryManagement(inventory);
            }
            else if (choice == 2)
            {
                Shopping(cart);
            }
            else if (choice == 3)
            {
                break;
            }
        }
    }

    static void InventoryManagement(Inventory inventory)
    {
        while (true)
        {
            Console.WriteLine("Inventory Management");
            Console.WriteLine("1. Create Item");
            Console.WriteLine("2. View Items");
            Console.WriteLine("3. Update Item");
            Console.WriteLine("4. Delete Item");
            Console.WriteLine("5. Back to Main Menu");
            Console.Write("Select an option: ");
            int choice = int.Parse(Console.ReadLine() ?? "");

            if (choice == 1)
            {
                inventory.CreateItem();
            }
            else if (choice == 2)
            {
                inventory.ViewItems();
            }
            else if (choice == 3)
            {
                inventory.UpdateItem();
            }
            else if (choice == 4)
            {
                inventory.DeleteItem();
            }
            else if (choice == 5)
            {
                break;
            }
        }
    }

    static void Shopping(Cart cart)
    {
        while (true)
        {
            Console.WriteLine("Shop");
            Console.WriteLine("1. Add Item to Cart");
            Console.WriteLine("2. Remove Item from Cart");
            Console.WriteLine("3. Checkout");
            Console.WriteLine("4. Back to Main Menu");
            Console.Write("Select an option: ");
            int choice = int.Parse(Console.ReadLine() ?? "");

            if (choice == 1)
            {
                cart.AddItemToCart();
            }
            else if (choice == 2)
            {
                cart.RemoveItemFromCart();
            }
            else if (choice == 3)
            {
                cart.Checkout();
            }
            else if (choice == 4)
            {
                break;
            }
        }
    }
}

class Item
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }

    public Item(int id, string name, string description, decimal price)
    {
        Id = id;
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Description = description ?? throw new ArgumentNullException(nameof(description));
        Price = price;
    }
}

class Inventory
{
    private List<Item> items = new List<Item>();
    private int nextId = 1;

    public void CreateItem()
    {
        Console.Write("Enter name: ");
        string name = Console.ReadLine() ?? "";
        Console.Write("Enter description: ");
        string description = Console.ReadLine() ?? "";
        Console.Write("Enter price: ");
        decimal price = decimal.Parse(Console.ReadLine() ?? "0");

        Item newItem = new Item(nextId++, name, description, price);
        items.Add(newItem);

        Console.WriteLine("Item created successfully.");
    }

    public void ViewItems()
    {
        Console.WriteLine("Inventory:");
        foreach (Item item in items)
        {
            Console.WriteLine($"ID: {item.Id}, Name: {item.Name}, Description: {item.Description}, Price: {item.Price:C}");
        }
    }

    public void UpdateItem()
    {
        Console.Write("Enter the ID of the item to update: ");
        int id = int.Parse(Console.ReadLine() ?? "");
        Item? item = items.Find(i => i.Id == id);

        if (item != null)
        {
            Console.Write("Enter new name (or leave blank to keep current): ");
            string? name = Console.ReadLine();
            if (!string.IsNullOrEmpty(name)) item.Name = name;

            Console.Write("Enter new description (or leave blank to keep current): ");
            string? description = Console.ReadLine();
            if (!string.IsNullOrEmpty(description)) item.Description = description;

            Console.Write("Enter new price (or leave blank to keep current): ");
            string? priceInput = Console.ReadLine();
            if (!string.IsNullOrEmpty(priceInput)) item.Price = decimal.Parse(priceInput);

            Console.WriteLine("Item updated successfully.");
        }
        else
        {
            Console.WriteLine("Item not found.");
        }
    }

    public void DeleteItem()
    {
        Console.Write("Enter the ID of the item to delete: ");
        int id = int.Parse(Console.ReadLine() ?? "");
        Item? item = items.Find(i => i.Id == id);

        if (item != null)
        {
            items.Remove(item);
            Console.WriteLine("Item deleted successfully.");
        }
        else
        {
            Console.WriteLine("Item not found.");
        }
    }

    public Item? GetItemById(int id)
    {
        return items.Find(i => i.Id == id);
    }
}

class Cart
{
    private Inventory inventory;
    private Dictionary<int, int> cartItems = new Dictionary<int, int>();

    public Cart(Inventory inventory)
    {
        this.inventory = inventory ?? throw new ArgumentNullException(nameof(inventory));
    }

    public void AddItemToCart()
    {
        Console.Write("Enter the ID of the item to add to cart: ");
        int id = int.Parse(Console.ReadLine() ?? "");

        Item? item = inventory.GetItemById(id);
        if (item != null)
        {
            if (cartItems.ContainsKey(id))
            {
                cartItems[id]++;
            }
            else
            {
                cartItems[id] = 1;
            }
            Console.WriteLine("Item added to cart successfully.");
        }
        else
        {
            Console.WriteLine("Item not found in inventory.");
        }
    }

    public void RemoveItemFromCart()
    {
        Console.Write("Enter the ID of the item to remove from cart: ");
        int id = int.Parse(Console.ReadLine() ?? "");

        if (cartItems.ContainsKey(id))
        {
            cartItems[id]--;
            if (cartItems[id] == 0)
            {
                cartItems.Remove(id);
            }
            Console.WriteLine("Item removed from cart successfully.");
        }
        else
        {
            Console.WriteLine("Item not found in cart.");
        }
    }

    public void Checkout()
    {
        decimal total = 0m;
        decimal taxRate = 0.07m; // 7% tax rate
        Console.WriteLine("Receipt:");
        foreach (var cartItem in cartItems)
        {
            Item? item = inventory.GetItemById(cartItem.Key);
            if (item != null)
            {
                decimal itemTotal = item.Price * cartItem.Value;
                Console.WriteLine($"Item: {item.Name}, Quantity: {cartItem.Value}, Price: {item.Price:C}, Total: {itemTotal:C}");
                total += itemTotal;
            }
        }

        decimal taxAmount = total * taxRate;
        decimal totalWithTax = total + taxAmount;

        Console.WriteLine($"Subtotal: {total:C}");
        Console.WriteLine($"Tax (7%): {taxAmount:C}");
        Console.WriteLine($"Total: {totalWithTax:C}");

        cartItems.Clear();
    }
}
