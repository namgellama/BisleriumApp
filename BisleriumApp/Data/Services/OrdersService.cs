
using System.Globalization;
using System.Text.Json;
using BisleriumApp.Data.Enums;
using BisleriumApp.Data.Models;

namespace BisleriumApp.Data.Services;

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
    public static List<OrderItem> GetCustomerOrders(string  phoneNumber)
    {
        var orders = GetAll();
        var customerOrders = orders.Where(order => order.Customer == phoneNumber).ToList();

        return customerOrders;
    }


    public static List<OrderItem> GetParticularMonthOrders(string month)
    {
        var orders = GetAll();
        /*var currentYear = 2023;*/
        var currentYear = DateTime.Now.Year;

        var _orders = orders.Where(order => order.CreatedAt.ToString("MMMM") == month && order.CreatedAt.Date.Year == currentYear).ToList();

        return _orders;
    }
    

    public static List<OrderItem> GetParticularDayOrders(int day, int month, int year)
    {
        var orders = GetAll();

        var _orders = orders.Where(order => order.CreatedAt.Day == day && order.CreatedAt.Month == month && order.CreatedAt.Year == year).ToList();

        return _orders;
    }

    public static Dictionary<string, int> GetTop5CoffeesForMonth(string month)
    {
        var orders = GetAll();
        /*var currentYear = 2023;*/
        var currentYear = DateTime.Now.Year;

        var filteredOrders = orders
            .Where(order => order.CreatedAt.ToString("MMMM") == month && order.CreatedAt.Date.Year == currentYear)
            .ToList();

        var orderedItems = new Dictionary<string, int>();

        foreach (var order in filteredOrders)
        {
            if (orderedItems.ContainsKey(order.Coffee))
            {
                orderedItems[order.Coffee]++;
            } else
            {
                orderedItems.Add(order.Coffee, 1);
            }
        }

        var topFiveCoffees = orderedItems.OrderByDescending(item => item.Value).Take(5).
            ToDictionary(item => item.Key, item => item.Value);

        return topFiveCoffees;
    }

       public static Dictionary<string, int> GetTop5CoffeesAllTime()
    {
        var orders = GetAll();

        var orderedItems = new Dictionary<string, int>();

        foreach (var order in orders)
        {
            if (orderedItems.ContainsKey(order.Coffee))
            {
                orderedItems[order.Coffee]++;
            } else
            {
                orderedItems.Add(order.Coffee, 1);
            }
        }

        var topFiveCoffees = orderedItems.OrderByDescending(item => item.Value).Take(5).
            ToDictionary(item => item.Key, item => item.Value);

        return topFiveCoffees;
    }

    public static Dictionary<string, int> GetTop5CoffeesForParticularDay(int day, int month, int year)
    {
        var orders = GetAll();

        var filteredOrders = orders.Where(order => order.CreatedAt.Day == day && order.CreatedAt.Month == month && order.CreatedAt.Year == year).ToList();


        var orderedItems = new Dictionary<string, int>();

        foreach (var order in filteredOrders)
        {
            if (orderedItems.ContainsKey(order.Coffee))
            {
                orderedItems[order.Coffee]++;
            } else
            {
                orderedItems.Add(order.Coffee, 1);
            }
        }

        var topFiveCoffees = orderedItems.OrderByDescending(item => item.Value).Take(5).
            ToDictionary(item => item.Key, item => item.Value);

        return topFiveCoffees;
    }

    public static Dictionary<string, int> GetTop5AddInsForMonth(string month)
    {
        var orders = GetAll();
        /*var currentYear = 2023;*/
        var currentYear = DateTime.Now.Year;

        var filteredOrders = orders
            .Where(order => order.CreatedAt.ToString("MMMM") == month && order.CreatedAt.Date.Year == currentYear)
            .ToList();

        var orderedItems = new Dictionary<string, int>();

        foreach (var order in filteredOrders)
        {
            if (order.AddIn != null)
            {
                if (orderedItems.ContainsKey(order.AddIn))
                {
                    orderedItems[order.AddIn]++;
                }
                else
                {
                    orderedItems.Add(order.AddIn, 1);
                }
            }
           
        }

        var topFiveAddIns = orderedItems.OrderByDescending(item => item.Value).Take(5).
            ToDictionary(item => item.Key, item => item.Value);

        return topFiveAddIns;
    }
    
    public static Dictionary<string, int> GetTop5AddInsAllTime()
    {
        var orders = GetAll();

        var orderedItems = new Dictionary<string, int>();

        foreach (var order in orders)
        {
            if (order.AddIn != null)
            {
                if (orderedItems.ContainsKey(order.AddIn))
                {
                    orderedItems[order.AddIn]++;
                }
                else
                {
                    orderedItems.Add(order.AddIn, 1);
                }
            }
           
        }

        var topFiveAddIns = orderedItems.OrderByDescending(item => item.Value).Take(5).
            ToDictionary(item => item.Key, item => item.Value);

        return topFiveAddIns;
    }

    public static Dictionary<string, int> GetTop5AddInsForParticularDay(int day, int month, int year)
    {
        var orders = GetAll();

        var filteredOrders =  orders.Where(order => order.CreatedAt.Day == day && order.CreatedAt.Month == month && order.CreatedAt.Year == year).ToList();


        var orderedItems = new Dictionary<string, int>();

        foreach (var order in filteredOrders)
        {
            if(order.AddIn != null)
            {
                if (orderedItems.ContainsKey(order.AddIn))
                {
                    orderedItems[order.AddIn]++;
                }
                else
                {
                    orderedItems.Add(order.AddIn, 1);
                }
            }
        }
     
        var topFiveAddIns = orderedItems.OrderByDescending(item => item.Value).Take(5).
            ToDictionary(item => item.Key, item => item.Value);

        return topFiveAddIns;
    }
    
    public static int GetRevenueForMonth(string month)
    {
        var orders = GetAll();
        /*var currentYear = 2023;*/
        var currentYear = DateTime.Now.Year;

        var filteredOrders = orders
            .Where(order => order.CreatedAt.ToString("MMMM") == month && order.CreatedAt.Date.Year == currentYear)
            .ToList();

        int totalRevenue = 0;

        foreach (var order in filteredOrders)
        {
            totalRevenue += order.TotalPrice;
        }

        return totalRevenue;
    }
        
    public static int GetRevenueForParticularDay(int day, int month, int year)
    {
        var orders = GetAll();

        var filteredOrders = orders.Where(order => order.CreatedAt.Day == day && order.CreatedAt.Month == month && order.CreatedAt.Year == year).ToList();


        int totalRevenue = 0;

        foreach (var order in filteredOrders)
        {
            totalRevenue += order.TotalPrice;
        }

        return totalRevenue;
    }        

    public static int GetRevenueTillNow()
    {
        var orders = GetAll();

        int totalRevenue = 0;

        foreach (var order in orders)
        {
            totalRevenue += order.TotalPrice;
        }

        return totalRevenue;
    }


    public static List<OrderItem> GetCustomerOrdersLastMonth(string phoneNumber)
    {
        Customer customer = CustomersService.GetByPhoneNumer(phoneNumber);
        List<OrderItem> orders = OrdersService.GetAll();
        List<OrderItem> customerOrdersLastMonth = new List<OrderItem>();

        DateTime currentDate = DateTime.Now;

        DateTime firstDayOfCurrentMonth = new DateTime(currentDate.Year, currentDate.Month, 1);
        DateTime lastDayOfPreviousMonth = firstDayOfCurrentMonth.AddDays(-1);

        int lastMonth = lastDayOfPreviousMonth.Month;
        int lastMonthYear = lastDayOfPreviousMonth.Year;


        if (customer != null)
        {
            var customerOrders = orders.Where(order => order.Customer == phoneNumber);

            foreach (var customerOrder in customerOrders)
            {
                if (customerOrder.CreatedAt.Date.Month == lastMonth && customerOrder.CreatedAt.Date.Year == lastMonthYear)
                {
                    customerOrdersLastMonth.Add(customerOrder);
                }
            }
        }

        return customerOrdersLastMonth;

    }

    public static List<OrderItem> Create(string coffee, string addIn, int totalPrice, Guid userId, string phoneNumber, int coffeePrice, int addInPrice, bool isComplementary)
    {
        List<OrderItem> orders = GetAll();
        Customer customer = CustomersService.GetByPhoneNumer(phoneNumber);
        int customerOrdersLastMonthCount = CustomersService.GetCustomerOrdersLastMonthCount(phoneNumber);
        List<OrderItem> customerOrdersLastMonth = GetCustomerOrdersLastMonth(phoneNumber);

        if (phoneNumber == null || coffee == null || coffee.Length == 0 || phoneNumber.Length == 0)
        {
            throw new Exception("Order invalid! Check order credentials");
        }

        if (addIn.Length == 0)
        {
            addIn = null;
        }

        int totalOrder = 0;
        bool regularMember = false;

        DateTime currentDate = DateTime.Now;

        DateTime firstDayofCurrentMonth = new DateTime(currentDate.Year, currentDate.Month, 1);
        DateTime lastDayOfPreviousMonth = firstDayofCurrentMonth.AddDays(-1);


        int totalWeekdaysLastMonth = Utils.CountWeekdays(lastDayOfPreviousMonth);


        if (customer != null)
        {
            List<DateTime> orderDateTimesList = new List<DateTime>();

            if (customerOrdersLastMonthCount < totalWeekdaysLastMonth)
            {
                regularMember = false;
            }
            else
            {
                foreach (var order in customerOrdersLastMonth)
                {
                    if (order.CreatedAt.DayOfWeek != DayOfWeek.Saturday && order.CreatedAt.DayOfWeek != DayOfWeek.Sunday)
                    {
                        if (!orderDateTimesList.Contains(order.CreatedAt.Date)) {
                            totalOrder++;
                            orderDateTimesList.Add(order.CreatedAt.Date);
                        }
                    }
                }

                if (totalOrder >= totalWeekdaysLastMonth) 
                {
                    regularMember = true;
                } else
                {
                    regularMember = false;
                }
            }
            
        }


        orders.Add(new OrderItem
        {
            Coffee = coffee,
            AddIn = addIn,
            CreatedBy = userId,
            Customer = phoneNumber,
            CoffeePrice = coffeePrice,
            AddInPrice = addInPrice,
            TotalPrice = totalPrice,
            isComplementary = isComplementary
        }) ;

        SaveAll(orders);

        if (regularMember)
        {
            CustomersService.Update(phoneNumber, CustomerRole.Regular); 
        } else
        {
            CustomersService.Update(phoneNumber, CustomerRole.Normal);
        }


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

        if (user.Id == creator.Id || user.Role == Role.Admin)
        {
            orders.Remove(order);
            SaveAll(orders);
            return orders;
        } else
        {
            throw new Exception("Not authorized");
        }
    }



    public static List<OrderItem> Update(Guid userId, Guid id, string coffee, string addIn, string phoneNumber, int totalPrice, int coffeePrice, int addInPrice)
    {
        List<OrderItem> orders = GetAll();
        OrderItem orderToUpdate = orders.FirstOrDefault(x => x.Id == id);
        User user = UsersService.GetById(userId);

        if (orderToUpdate == null)
        {
            throw new Exception("Order not found.");
        }

        if (phoneNumber.Length == 0 || coffee.Length == 0)
        {
            throw new Exception("Order invalid! Check order credentials");
        }

        var creator = UsersService.GetById(orderToUpdate.CreatedBy);

        if (user.Id == creator.Id || user.Role.ToString() == "Admin")
        {
            orderToUpdate.Coffee = coffee;
            orderToUpdate.AddIn = addIn;
            orderToUpdate.TotalPrice = totalPrice;
            orderToUpdate.Customer = phoneNumber;
            orderToUpdate.CoffeePrice = coffeePrice;
            orderToUpdate.AddInPrice = addInPrice;
            SaveAll(orders);
            return orders;
        } else
        {
            throw new Exception("Not authorized");
        }
    }

}
