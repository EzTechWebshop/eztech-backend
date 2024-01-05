using EzTech.Data.DtoModels;

namespace EzTech.Data.ApiModels.AdminApiModels.ManagementApiModels;

public class GetOrdersApiResponse
{
    public int TotalOrders { get; set; }
    public int TotalPages { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public string? Status { get; set; }
    public DateTime? MinDate { get; set; }
    public DateTime? MaxDate { get; set; }
    public string? Sort { get; set; }
    public List<OrderDto> Orders { get; set; } = null!;
    public OrderDto? Order { get; set; }
}