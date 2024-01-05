using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace EzTech.Data.Models;

public class OrderItem
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public string ProductName { get; set; } = null!;
    [Required]
    public string ProductDescription { get; set; } = null!;
    [Required]
    [Precision(10, 2)]
    public decimal Price { get; set; }
    [Required]
    public int Quantity { get; set; }
    
    public decimal Total => Price * Quantity;
    
    [Required]
    // Information stored above in case product is deleted, or changed
    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;
    [Required]
    public int OrderId { get; set; }
    public Order Order { get; set; } = null!;
}