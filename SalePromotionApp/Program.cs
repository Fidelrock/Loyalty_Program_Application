using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Embarking on a jackpot!");

        // Initialize a list of customers
        var customers = new List<Customer>
        {
            new Customer("Alice", "C001"),
            new Customer("Bob", "C002"),
            new Customer("Fidel","C003"),
            new Customer("Abdul","C004"),
            new Customer("Cliff","C005"),
            new Customer("James","C007"),
            new Customer("Jackson","C008"),
            new Customer("Geofrrey","C009"),
            new Customer("Jimmy","C0010")
        };

        var loyaltyProgram = new LoyaltyProgram(customers);

        Console.WriteLine("Select an option to perform: ");

        bool loop = true;
        try
        {
            do
            {
                Console.WriteLine("1. Add Customer\n\r2. Earn points\n\r3. Redeem Points\n\r4. Transfer Points\n\r5. Buy Points\n\r6. Check for expired points\n\r7. List all customers\n\r0. Exit \n");

                string _selectOption = Console.ReadLine();

                switch (_selectOption)
                {
                    case "1": {
                           
                            

                            Console.WriteLine("Enter the name of the customer to ADD: ");
                            string name = Console.ReadLine();
                            Console.WriteLine("Enter the name of the customer to ADD: ");
                            string id = Console.ReadLine();
                             
                            new Customer(name, id);

                            Console.WriteLine($"New Customer: {name} ,ID: {id} was added successfully!!!...");

                            break;
                        }
                    case "2":
                        {
                            // Earn Points
                            loyaltyProgram.EarnPoints("C001", 150);
                            loyaltyProgram.EarnPoints("C002", 200);
                            break;
                        }
                    case "3":
                        {
                            // Redeem points
                            loyaltyProgram.RedeemPoints("C001", 50); // Alice redeems points
                            break;
                        }
                    case "4":
                        {
                            // Transfer points
                            loyaltyProgram.TransferPoints("C002", "C001", 30); // Bob transfers points to Alice
                            break;
                        }
                    case "5":
                        {
                            // Buy points
                            loyaltyProgram.BuyPoints("C001", 50); // Alice buys points
                            break;
                        }
                    case "6":
                        {
                            // Check for expired points
                            loyaltyProgram.CheckForExpiredPoints();
                            break;
                        }
                    case "7":
                        {
                            // List all customers
                            loyaltyProgram.ListAllCustomers();
                            break;
                        }
                    case "0":
                        {
                            loop = false;
                            break;
                        }
                    default:
                        {
                            Console.WriteLine("Invalid Selection!!!");
                            break;
                        }
                }
            } while (loop);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }

        Console.ReadLine();
    }
}

class Customer
{
    public string Name { get; set; }
    public string ID { get; set; }
    public int Points { get; set; }
    public DateTime LastActivity { get; set; }

    public Customer(string name, string id, int points = 0)
    {
        Name = name;
        ID = id;
        Points = points;
        LastActivity = DateTime.Now;
    }
}

class LoyaltyProgram
{
    private List<Customer> customers;
    private const int ExpirationPeriodInMonths = 12;
    private const int PointsPerDollarSpent = 1;
    private const double PointPurchaseCost = 0.002; // $0.02 per point
    private const int MaxPointsPerPurchase = 10000;

    public LoyaltyProgram(List<Customer> customers)
    {
        this.customers = customers;
    }

    // Method that enhances earning of points
    public void EarnPoints(string id, double amountSpent)
    {
        var customer = FindCustomerById(id);
        if (customer != null)
        {
            int pointsEarned = (int)(amountSpent * PointsPerDollarSpent);
            customer.Points += pointsEarned;
            customer.LastActivity = DateTime.Now;
            Console.WriteLine($"{customer.Name} earned {pointsEarned} points. Total points: {customer.Points}");
        }
    }

    // Customer redeeming points
    public void RedeemPoints(string id, int pointsToRedeem)
    {
        var customer = FindCustomerById(id);
        if (customer != null && customer.Points >= pointsToRedeem)
        {
            customer.Points -= pointsToRedeem;
            customer.LastActivity = DateTime.Now;
            Console.WriteLine($"{customer.Name} redeemed {pointsToRedeem} points. Remaining points: {customer.Points}");
        }
        else
        {
            Console.WriteLine("Insufficient points for redemption or customer not found.");
        }
    }

    // Transfer points between customers
    public void TransferPoints(string fromId, string toId, int pointsToTransfer)
    {
        var fromCustomer = FindCustomerById(fromId);
        var toCustomer = FindCustomerById(toId);

        if (fromCustomer != null && toCustomer != null && fromCustomer.Points >= pointsToTransfer)
        {
            fromCustomer.Points -= pointsToTransfer;
            toCustomer.Points += pointsToTransfer;

            fromCustomer.LastActivity = DateTime.Now;
            toCustomer.LastActivity = DateTime.Now;

            Console.WriteLine($"{fromCustomer.Name} transferred {pointsToTransfer} points to {toCustomer.Name}. New balances - {fromCustomer.Name}: {fromCustomer.Points}, {toCustomer.Name}: {toCustomer.Points}");
        }
        else
        {
            Console.WriteLine("Transfer failed. Insufficient points or customer not found.");
        }
    }

    // Method for buying points
    public void BuyPoints(string id, int pointsToBuy)
    {
        if (pointsToBuy > MaxPointsPerPurchase)
        {
            Console.WriteLine($"Cannot purchase more than {MaxPointsPerPurchase} points in a single transaction.");
            return;
        }

        var customer = FindCustomerById(id);
        if (customer != null)
        {
            double cost = pointsToBuy * PointPurchaseCost;
            customer.Points += pointsToBuy;
            customer.LastActivity = DateTime.Now;
            Console.WriteLine($"{customer.Name} purchased {pointsToBuy} points for ${cost}. New balance: {customer.Points}");
        }
        else
        {
            Console.WriteLine("Customer not found!!!...");
        }
    }

    // Check and expire points if needed
    public void CheckForExpiredPoints()
    {
        foreach (var customer in customers)
        {
            if (DateTime.Now > customer.LastActivity.AddMonths(ExpirationPeriodInMonths))
            {
                Console.WriteLine($"Points expired for {customer.Name}. Current points: {customer.Points}");
                customer.Points = 0;
            }
        }
    }

    // List all customers in the system
    public void ListAllCustomers()
    {
        Console.WriteLine("Listing all customers in the system:");
        foreach (var customer in customers)
        {
            Console.WriteLine($"Name: {customer.Name}, ID: {customer.ID}, Points: {customer.Points}, Last Activity: {customer.LastActivity}");
        }
    }

    // Find customer by ID
    private Customer FindCustomerById(string id)
    {
        return customers.FirstOrDefault(c => c.ID == id);
    }
}
