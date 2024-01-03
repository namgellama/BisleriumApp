using System.Collections;

namespace BisleriumApp.Data;

public class AddInChoice : IEnumerable<KeyValuePair<string, int>>
{
    private Dictionary<string, int> addIns;

    // Constructor to initialize the dictionary
    public AddInChoice()
    {
        addIns = new Dictionary<string, int>();
        InitializeAddIns();
    }

    private void InitializeAddIns()
    {
        addIns.Add("Cinnamon", 50);
        addIns.Add("Honey", 50);
        addIns.Add("Ginger", 30);
        addIns.Add("Chocolate", 50);
        addIns.Add("IceCream", 80);
    }

    // Implement IEnumerable interface
    public IEnumerator<KeyValuePair<string, int>> GetEnumerator()
    {
        return addIns.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public int GetAddInPrice(string addInType)
    {
        if (addIns.ContainsKey(addInType))
        {
            return addIns[addInType];
        }
        else
        {
            return 0;
        }
    }
}
