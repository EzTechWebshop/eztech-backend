namespace EzTech.Data.DtoModels;

public class CartDto
{
    public int Id { get; set; }
    public int TotalQuantity { get; set; }
    public decimal TotalPrice { get; set; }
    public List<CartItemDto> CartItems { get; set; } = null!;
}