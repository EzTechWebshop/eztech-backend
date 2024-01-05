using AutoMapper;
using EzTech.Data.DtoModels;
using EzTech.Data.Models;

namespace EzTech.Data;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // DTO
        CreateMap<Category, CategoryDto>().ReverseMap();
        CreateMap<Product, ProductDto>().ReverseMap();
        CreateMap<Image, ImageDto>().ReverseMap();
        CreateMap<Promotion, PromotionDto>().ReverseMap();
        CreateMap<Rating, RatingDto>().ReverseMap();
        CreateMap<Wishlist, WishlistDto>().ReverseMap();
        CreateMap<Cart, CartDto>().ReverseMap();
        CreateMap<CartItem, CartItemDto>().ReverseMap();
        CreateMap<Order, OrderDto>().ReverseMap();
        CreateMap<OrderItem, OrderItemDto>().ReverseMap();
        CreateMap<Faq, FaqDto>().ReverseMap();
        CreateMap<User, UserDto>().ReverseMap();
        CreateMap<WebsiteInfo, WebsiteInfoDto>().ReverseMap();
        CreateMap<WebsiteInfoText, WebsiteInfoTopic>().ReverseMap();
    }
}