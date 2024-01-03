
using System.Text.Json;

namespace BisleriumApp.Data;

public static class OrdersService
{
    private static void SaveAll(List<OrderItem> orders)
    {
        string appDataDirectoryPath = Utils.GetAppDirectoryPath();
        string ordersFilePath = Utils.GetOrdersFilePath();

        if (!Directory.Exists(appDataDirectoryPath))
        {
            Directory.CreateDirectory(appDataDirectoryPath);
        }

        var json = JsonSerializer.Serialize(orders);
        File.WriteAllText(ordersFilePath, json);
    }


    public static List<OrderItem> GetAll()
    {
        string ordersFilePath = Utils.GetOrdersFilePath();
        if (!File.Exists(ordersFilePath))
        {
            return new List<OrderItem>();
        }

        var json = File.ReadAllText(ordersFilePath);
        return JsonSerializer.Deserialize<List<OrderItem>>(json);
    }

    public static List<OrderItem> Create(string coffee, string addIn, int totalPrice, Guid userId, string username, int coffeePrice, int addInPrice)
    {
        List<OrderItem> orders = GetAll();
        orders.Add(new OrderItem
        {
            Coffee = coffee,
            AddIn = addIn,
            CreatedBy = userId,
            Customer = username,
            CoffeePrice = coffeePrice,
            AddInPrice = addInPrice,
            TotalPrice = totalPrice,
        }) ;

        SaveAll(orders);
        return orders;
    }


    public static List<OrderItem> Delete(Guid userId, Guid id)
    {
        List<OrderItem> orders = GetAll();
        User user = UsersService.GetById(userId);

        OrderItem order = orders.FirstOrDefault(x => x.Id == id);

        if (order == null)
        {
            throw new Exception("Order not found.");
        }

        var creator = UsersService.GetById(order.CreatedBy);

        if (user.Id == creator.Id || user.Role.ToString() == "Admin")
        {
            orders.Remove(order);
            SaveAll(orders);
            return orders;
        } else
        {
            throw new Exception("Not authorized");
        }
    }


    public static List<OrderItem> Update(Guid userId, Guid id, string coffee, string addIn, string username, int totalPrice, int coffeePrice, int addInPrice, bool isPaid)
    {
        List<OrderItem> orders = GetAll();
        OrderItem orderToUpdate = orders.FirstOrDefault(x => x.Id == id);
        User user = UsersService.GetById(userId);

        if (orderToUpdate == null)
        {
            throw new Exception("Order not found.");
        }

        var creator = UsersService.GetById(orderToUpdate.CreatedBy);

        if (user.Id == creator.Id || user.Role.ToString() == "Admin")
        {
            orderToUpdate.Coffee = coffee;
            orderToUpdate.AddIn = addIn;
            orderToUpdate.TotalPrice = totalPrice;
            orderToUpdate.Customer = username;
            orderToUpdate.CoffeePrice = coffeePrice;
            orderToUpdate.AddInPrice = addInPrice;
            orderToUpdate.IsPaid = isPaid;
            SaveAll(orders);
            return orders;
        } else
        {
            throw new Exception("Not authorized");
        }
    }

}
