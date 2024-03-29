﻿using System.ComponentModel.DataAnnotations;

namespace BisleriumApp.Data.Models;

public class OrderItem
{
    public Guid Id { get; set; } = Guid.NewGuid();
    [Required(ErrorMessage = "Please provide the coffee name.")]
    public string Coffee { get; set; }
    public string AddIn { get; set; }
    public int CoffeePrice { get; set; }
    public int AddInPrice { get; set; }
    public int TotalPrice {get; set;}
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public string Customer { get; set; }
    public Guid CreatedBy { get; set; }

    public bool isComplementary { get; set; }
    
}
