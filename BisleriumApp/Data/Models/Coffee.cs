
namespace BisleriumApp.Data.Models;

public class Coffee
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; }
    public int Price { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public Guid CreatedBy { get; set; }
}
