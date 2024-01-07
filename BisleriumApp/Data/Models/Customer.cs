
using BisleriumApp.Data.Enums;

namespace BisleriumApp.Data.Models;

public class Customer
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string PhoneNumber { get; set; }
    public CustomerRole CustomerRole { get; set; } = CustomerRole.Normal;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public Guid CreatedBy { get; set; }
}
