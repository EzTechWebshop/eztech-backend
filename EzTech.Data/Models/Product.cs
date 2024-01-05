using System.ComponentModel.DataAnnotations;
using EzTech.Data.ApiModels.AdminApiModels.ProductApiModels;
using Microsoft.EntityFrameworkCore;

namespace EzTech.Data.Models;

[Index(nameof(Name), IsUnique = true)]
public class Product
{
    public void Sell(int quantity)
    {
        Sold += quantity;
        Stock -= quantity;
    }

    [Key] 
    public int Id { get; set; }
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = null!;
    [Required]
    [MaxLength(1000)]
    public string Description { get; set; } = null!;
    [Required]
    [Precision(14, 2)]
    public decimal? Price { get; set; } = 0;
    [Required]
    [Range(0, int.MaxValue)]
    public int? Stock { get; set; } = 0;
    [Required]
    [Range(0, int.MaxValue)]
    public int? Sold { get; set; } = 0;
    
    [Required]
    [Range(0, 100)]
    public int Discount { get; set; }
    [Required]
    public bool IsDiscounted { get; set; }
    public decimal DiscountedPrice => (decimal)(Price - (Price * Discount / 100))!;
    [Required]
    public bool IsDeleted { get; set; }
    public bool IsSoldOut => Stock <= 0;
    [Required]
    [Precision(14, 2)]
    public decimal AverageRating { get; set; }
    public int RatingCount => Ratings.Count;
    [Required]
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    
    public int? ThumbnailId { get; set; }
    
    [Required]
    public List<Image> Images { get; set; } = new();
    public Image? Thumbnail => Images
        .OrderBy(u =>  u.Id)
        .FirstOrDefault(x => x.Id == ThumbnailId) ?? Images.FirstOrDefault();
    [Required]
    public List<Category> Categories { get; set; } = new();
    [Required]
    public List<Rating> Ratings { get; set; } = new();
    [Required]
    public List<CartItem> CartItems { get; set; } = new();
    [Required]
    public List<Wishlist> Wishlists { get; set; } = new();
    [Required]
    public List<OrderItem> OrderItems { get; set; } = new();
    
    [Required]
    public List<Promotion> Promotions { get; set; } = new();
    
    public void Update(UpdateProductRequest request)
    {
        Name = request.Name ?? Name;
        Description = request.Description ?? Description;
        Price = request.Price ?? Price;
        Sold = request.Sold ?? Sold;
        Stock = request.Stock ?? Stock;
        Discount = request.Discount ?? Discount;
        Categories = request.Categories ?? Categories;
    }
}