using System.Text.Json;

using BisleriumApp.Data.Models;
using BisleriumApp.Data.Enums;

namespace BisleriumApp.Data.Services;

public static class AddInsService
{
    private static void SaveAll(List<AddIns> addIns)
    {
        string appDataDirectoryPath = Utils.GetAppDirectoryPath();
        string appAddInsFilePath = Utils.GetAppAddInsFilePath();

        if (!Directory.Exists(appDataDirectoryPath))
        {
            Directory.CreateDirectory(appDataDirectoryPath);
        }

        var json = JsonSerializer.Serialize(addIns);
        File.WriteAllText(appAddInsFilePath, json);
    }

    public static List<AddIns> GetAll()
    {
        string appAddInsFilePath = Utils.GetAppAddInsFilePath();
        if (!File.Exists(appAddInsFilePath))
        {
            return new List<AddIns>();
        }

        var json = File.ReadAllText(appAddInsFilePath);

        return JsonSerializer.Deserialize<List<AddIns>>(json);
    }

    public static List<AddIns> Create(Guid userId, string name, int price)
    {
        List<AddIns> addIns = GetAll();
        bool addInsNameExists = addIns.Any(x => x.Name == name);

        if (addInsNameExists)
        {
            throw new Exception("AddIns already exists.");
        }

        if (name == null || price == 0)
        {
            throw new Exception("Null value detected!");
        }

        var creator = UsersService.GetById(userId);

        if (creator.Role != Role.Admin)
        {
            throw new Exception("Not authorized.");
        }

        addIns.Add(
            new AddIns
            {
                Name = name,
                Price = price,
                CreatedBy = userId,

            }
        );
        SaveAll(addIns);
        return addIns;
    }

    public static List<AddIns> Delete(Guid id, Guid userId)
    {
        List<AddIns> addIns = GetAll();
        AddIns AddIns = addIns.FirstOrDefault(x => x.Id == id);

        if (AddIns == null)
        {
            throw new Exception("AddIns not found.");
        }

        var creator = UsersService.GetById(userId);

        if (creator.Role != Role.Admin)
        {
            throw new Exception("Not authorized.");
        }

        addIns.Remove(AddIns);
        SaveAll(addIns);

        return addIns;
    }


    public static List<AddIns> Update(Guid userId, Guid id, string name, int price)
    {
        List<AddIns> addIns = GetAll();
        AddIns addInsToUpdate = addIns.FirstOrDefault(x => x.Id == id);
        var creator = UsersService.GetById(userId);

        if (addInsToUpdate == null)
        {
            throw new Exception("AddIns not found.");
        }

        if (name.Length == 0 || price <= 0)
        {
            throw new Exception("Order invalid! Check order credentials");
        }


        if (creator.Role == Role.Admin)
        {
            addInsToUpdate.Name = name;
            addInsToUpdate.Price = price;
            SaveAll(addIns);
            return addIns;
        }
        else
        {
            throw new Exception("Not authorized");
        }
    }

    public static int GetAddInsPrice(Guid id)
    {
        List<AddIns> addIns = GetAll();
        return addIns.FirstOrDefault(x => x.Id == id).Price;
    }
    
    public static string GetAddInsName(Guid id)
    {
        List<AddIns> addIns = GetAll();
        return addIns.FirstOrDefault(x => x.Id == id).Name;
    }

}
