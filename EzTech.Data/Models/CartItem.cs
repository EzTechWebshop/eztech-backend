using System.ComponentModel.DataAnnotations;

namespace EzTech.Data.Models;

public class CartItem
{
    [Key] 
    public int Id { get; set; }
    
    [Required] 
    [Range(1, int.MaxValue)] 
    public int Quantity { get; set; } = 1;
    
    [Required] 
    public decimal Price => Product.Price!.Value * Quantity;
    
    [Required] 
    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;
    
    [Required] 
    public int CartId { get; set; }
    public Cart Cart { get; set; } = null!;
}