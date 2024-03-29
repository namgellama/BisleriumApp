﻿using System.Text.Json;

using BisleriumApp.Data.Models;
using BisleriumApp.Data.Enums;

namespace BisleriumApp.Data.Services;

public static class CoffeesService
{
    private static void SaveAll(List<Coffee> coffees)
    {
        string appDataDirectoryPath = Utils.GetAppDirectoryPath();
        string appCoffeesFilePath = Utils.GetAppCoffeesFilePath();

        if (!Directory.Exists(appDataDirectoryPath))
        {
            Directory.CreateDirectory(appDataDirectoryPath);
        }

        var json = JsonSerializer.Serialize(coffees);
        File.WriteAllText(appCoffeesFilePath, json);
    }

    public static List<Coffee> GetAll()
    {
        string appCoffeesFilePath = Utils.GetAppCoffeesFilePath();
        if (!File.Exists(appCoffeesFilePath))
        {
            return new List<Coffee>();
        }

        var json = File.ReadAllText(appCoffeesFilePath);

        return JsonSerializer.Deserialize<List<Coffee>>(json);
    }

    public static List<Coffee> Create(Guid userId, string name, int price, string username, string password)
    {
        List<Coffee> coffees = GetAll();
        bool coffeeNameExists = coffees.Any(x => name != null && x.Name.ToLower().Trim() == name.ToLower().Trim());

        if (coffeeNameExists)
        {
            throw new Exception("Coffee already exists.");
        }

        if (name == null || price <= 0 || name.Length == 0)
        {
            throw new Exception("Empty value detected!");
        }

        var creator = UsersService.GetById(userId);

        if (creator.Role != Role.Admin)
        {
            throw new Exception("Not authorized.");
        }

        User user = UsersService.Login(username, password);

        
        coffees.Add(
            new Coffee
            {
                Name = name,
                Price = price,
                CreatedBy = userId,

            }
        );
        SaveAll(coffees);
        return coffees;
    }

    public static List<Coffee> Delete(Guid id, Guid userId)
    {
        List<Coffee> coffees = GetAll();
        Coffee coffee = coffees.FirstOrDefault(x => x.Id == id);

        if (coffee == null)
        {
            throw new Exception("Coffee not found.");
        }

        var creator = UsersService.GetById(userId);

        if (creator.Role != Role.Admin)
        {
            throw new Exception("Not authorized.");
        }

        coffees.Remove(coffee);
        SaveAll(coffees);

        return coffees;
    }


    public static List<Coffee> Update(Guid userId, Guid id, string name, int price, string username, string password)
    {
        List<Coffee> coffees = GetAll();
        Coffee coffeeToUpdate = coffees.FirstOrDefault(x => x.Id == id);
        var creator = UsersService.GetById(userId);

        if (coffeeToUpdate == null)
        {
            throw new Exception("Coffee not found.");
        }

        if (name.Length == 0 || price <= 0)
        {
            throw new Exception("Order invalid! Check order credentials");
        }

        User user = UsersService.Login(username, password);

        if (creator.Role == Role.Admin)
        {
            coffeeToUpdate.Name = name;
            coffeeToUpdate.Price = price;
            SaveAll(coffees);
            return coffees;
        }
        else
        {
            throw new Exception("Not authorized");
        }
    }

    public static int GetCoffeePrice(Guid id)
    {
        List<Coffee> coffees = GetAll();
        return coffees.FirstOrDefault(x => x.Id == id).Price;
    }
    
    public static string GetCoffeeName(Guid id)
    {
        List<Coffee> coffees = GetAll();
        return coffees.FirstOrDefault(x => x.Id == id).Name;
    }

}
