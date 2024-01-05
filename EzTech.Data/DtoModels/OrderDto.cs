using EzTech.Data.Models;

namespace EzTech.Data.DtoModels;

public class OrderDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public Guid OrderNumber { get; set; }
    public OrderStatus Status { get; set; }
    public bool Active { get; set; }
    public string StatusName => Status.ToString();
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime CompletedAt { get; set; }
    public decimal Total { get; set; }
    public List<OrderItemDto> Items { get; set; } = null!;
}