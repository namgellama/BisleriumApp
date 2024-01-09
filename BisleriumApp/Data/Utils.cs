using System.Security.Cryptography;
using BisleriumApp.Data.Services;

namespace BisleriumApp.Data;

public static class Utils
{
    private const char _segmentDelimiter = ':';

    public static string HashSecret(string input)
    {
        var saltSize = 16;
        var iterations = 100_000;
        var keySize = 32;
        HashAlgorithmName algorithm = HashAlgorithmName.SHA256;
        byte[] salt = RandomNumberGenerator.GetBytes(saltSize);
        byte[] hash = Rfc2898DeriveBytes.Pbkdf2(input, salt, iterations, algorithm, keySize);

        return string.Join(
            _segmentDelimiter,
            Convert.ToHexString(hash),
            Convert.ToHexString(salt),
            iterations,
            algorithm
        );
    }

    public static bool VerifyHash(string input, string hashString)
    {
        string[] segments = hashString.Split(_segmentDelimiter);
        byte[] hash = Convert.FromHexString(segments[0]);
        byte[] salt = Convert.FromHexString(segments[1]);
        int iterations = int.Parse(segments[2]);
        HashAlgorithmName algorithm = new(segments[3]);
        byte[] inputHash = Rfc2898DeriveBytes.Pbkdf2(
            input,
            salt,
            iterations,
            algorithm,
            hash.Length
        );

        return CryptographicOperations.FixedTimeEquals(inputHash, hash);
    }

    public static string GetAppDirectoryPath()
    {
        return Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            "Bislerium"
        );
    }

    public static string GetAppUsersFilePath()
    {
        return Path.Combine(GetAppDirectoryPath(), "users.json");
    }
    public static string GetAppCustomersFilePath()
    {
        return Path.Combine(GetAppDirectoryPath(), "customers.json");
    }

    public static string GetAppCoffeesFilePath()
    {
        return Path.Combine(GetAppDirectoryPath(), "coffees.json");
    }
    
    public static string GetAppAddInsFilePath()
    {
        return Path.Combine(GetAppDirectoryPath(), "addIns.json");
    }


    public static string GetOrdersFilePath()
    {
        return Path.Combine(GetAppDirectoryPath(), "orders.json");
    }


    public static int CountWeekdays(DateTime date)
    {
        int weekdays = 0;
        for (int day = 1; day <= date.Day; day++)
        {
            DateTime _date = new DateTime(date.Year, date.Month, day);

            if (_date.DayOfWeek != DayOfWeek.Saturday && _date.DayOfWeek != DayOfWeek.Sunday)
            {
                weekdays++;
            }
        }

        return weekdays;
    }

    public static string GetUserName(Guid userId)
    {
        var creator = UsersService.GetById(userId);
        return creator == null ? "Unknown" : creator.Username;
    }


}
