using System.Collections;

namespace BisleriumApp.Data.Enumerables;

public class CoffeeChoice : IEnumerable<KeyValuePair<string, int>>
{
    private Dictionary<string, int> coffees;

    // Constructor to initialize the dictionary
    public CoffeeChoice()
    {
        coffees = new Dictionary<string, int>();
        InitializeCoffees();
    }

    // Method to initialize coffee prices
    private void InitializeCoffees()
    {
        coffees.Add("Espresso", 120);
        coffees.Add("Latte", 180);
        coffees.Add("Cappuccino", 190);
        coffees.Add("Americano", 150);
    }

    // Implement IEnumerable interface
    public IEnumerator<KeyValuePair<string, int>> GetEnumerator()
    {
        return coffees.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public int GetCoffeePrice(string coffeeType)
    {
        if (coffees.ContainsKey(coffeeType))
        {
            return coffees[coffeeType];
        }
        else
        {
            return 0;
        }
    }
}



