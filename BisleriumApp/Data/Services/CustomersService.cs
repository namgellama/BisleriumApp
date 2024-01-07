using System.Text.Json;

using BisleriumApp.Data.Models;

namespace BisleriumApp.Data.Services;

public static class CustomersService
{
    private static void SaveAll(List<Customer> customers)
    {
        string appDataDirectoryPath = Utils.GetAppDirectoryPath();
        string appCustomersFilePath = Utils.GetAppCustomersFilePath();

        if (!Directory.Exists(appDataDirectoryPath))
        {
            Directory.CreateDirectory(appDataDirectoryPath);
        }

        var json = JsonSerializer.Serialize(customers);
        File.WriteAllText(appCustomersFilePath, json);
    }

    public static List<Customer> GetAll()
    {
        string appCustomersFilePath = Utils.GetAppCustomersFilePath();
        if (!File.Exists(appCustomersFilePath))
        {
            return new List<Customer>();
        }

        var json = File.ReadAllText(appCustomersFilePath);

        return JsonSerializer.Deserialize<List<Customer>>(json);
    }

     public static int GetTotalOrders(string customerPhone)
    {
        List<OrderItem> orders = OrdersService.GetAll();

        var ordersList = orders.FindAll(x => x.Customer == customerPhone);
        var filteredOrdersList = ordersList.FindAll(x => x.isComplementary == false);

        return filteredOrdersList.Count;
    }


    public static OrderItem GetLastOrder(string customerPhone)
    {
        List<OrderItem> orders = OrdersService.GetAll();

        var lastOrderItem = orders.Where(x => x.Customer == customerPhone).OrderByDescending(x => x.CreatedAt).FirstOrDefault();

        return lastOrderItem;
    }

    public static List<Customer> Create(Guid userId, string phoneNumber)
    {
        List<Customer> customers = GetAll();
        bool customerNameExists = customers.Any(x => x.PhoneNumber == phoneNumber);

        if (customerNameExists)
        {
            throw new Exception("Customer already exists.");
        }

        customers.Add(
            new Customer
            {
                PhoneNumber = phoneNumber,
                CreatedBy = userId
            }
        );
        SaveAll(customers);
        return customers;
    }

    public static Customer GetByPhoneNumer(string phoneNumber)
    {
        List<Customer> customers = GetAll();
        return customers.FirstOrDefault(x => x.PhoneNumber == phoneNumber);
    }

    public static List<Customer> Delete(Guid id)
    {
        List<Customer> customers = GetAll();
        Customer customer = customers.FirstOrDefault(x => x.Id == id);

        if (customer == null)
        {
            throw new Exception("Customer not found.");
        }

        // TodosService.DeleteByUserId(id);
        customers.Remove(customer);
        SaveAll(customers);

        return customers;
    }
}
