using System.ComponentModel.DataAnnotations;

namespace EzTech.Data.DtoModels;

public class ProductDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public int Sold { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public int Discount { get; set; }
    public bool IsDiscounted { get; set; }
    public decimal DiscountedPrice { get; set; }
    public bool IsDeleted { get; set; }
    public bool IsSoldOut { get; set; }
    public decimal AverageRating { get; set; }
    public int RatingCount { get; set; }
    public int? ThumbnailId { get; set; }
    public ImageDto? Thumbnail { get; set; }
    public List<ImageDto> Images { get; set; } = null!;
    public List<CategoryDto> Categories { get; set; } = null!;
    public List<RatingDto> Ratings { get; set; } = null!;
    public List<RatingDto> UserRatings { get; set; } = null!;
    public List<PromotionDto> Promotions { get; set; } = null!;
}