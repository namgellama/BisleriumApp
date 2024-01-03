
namespace BisleriumApp.Data.Models;

public class Customer
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string PhoneNumber { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public Guid CreatedBy { get; set; }
}
