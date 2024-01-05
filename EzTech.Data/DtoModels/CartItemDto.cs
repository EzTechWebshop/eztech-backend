namespace EzTech.Data.DtoModels;

public class CartItemDto
{
    public int Id { get; set; }
    public int Quantity { get; set; }
    public decimal Price => Product.Price * Quantity;
    public ProductDto Product { get; set; } = null!;
}