using System.ComponentModel.DataAnnotations;

namespace EzTech.Data.Models;

public class Cart
{
    [Key] 
    public int Id { get; set; }
    
    [Required] 
    public List<CartItem> CartItems { get; set; } = new();
    
    [Required] 
    public int TotalQuantity => CartItems.Sum(item => item.Quantity);
    
    [Required] 
    public decimal TotalPrice => CartItems.Sum(item => item.Price);
    
    [Required] 
    public int UserId { get; set; }
    public User User { get; set; } = null!;

    public void AddItem(Product product)
    {
        var item = CartItems
            .OrderBy(u => u.Id)
            .FirstOrDefault(item => item.ProductId == product.Id);
        // In case the item is already in the cart, increase the quantity
        if (item != null)
        {
            item.Quantity++;
            return;
        }

        item = new CartItem
        {
            ProductId = product.Id,
            Product = product,
            Quantity = 1,
        };
        CartItems.Add(item);
    }

    public void RemoveItem(int cartItemId)
    {
        var item = CartItems
            .OrderBy(u => u.Id)
            .FirstOrDefault(item => item.Id == cartItemId);
        if (item == null) return;
        CartItems.Remove(item);
    }

    public void ChangeQuantity(int cartItemId, int quantity)
    {
        var item = CartItems
            .OrderBy(u => u.Id)
            .FirstOrDefault(item => item.Id == cartItemId);
        if (item == null) return;
        item.Quantity = quantity;
    }

    public void ClearCart()
    {
        CartItems.Clear();
    }
}