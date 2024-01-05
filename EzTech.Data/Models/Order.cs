using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace EzTech.Data.Models;

public class Order
{
    [Key]
    public int Id { get; set; }
    [Required]
    public Guid OrderNumber { get; set; } = Guid.NewGuid();
    
    [Required]
    public OrderStatus Status { get; set; } = OrderStatus.Pending;
    public bool Active => Status is OrderStatus.Pending or OrderStatus.Processing or OrderStatus.Shipped;
    
    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? UpdatedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    
    [Required]
    [Precision(10, 2)]
    public decimal Total { get; set; }
    
    [Required]
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    
    [Required]
    public List<OrderItem> Items { get; set; } = new();
}

public enum OrderStatus
{
    Pending,
    Processing,
    Shipped,
    Completed,
    Cancelled,
    Refunded
}