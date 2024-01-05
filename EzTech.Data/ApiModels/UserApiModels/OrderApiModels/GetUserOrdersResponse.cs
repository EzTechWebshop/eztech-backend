using EzTech.Data.DtoModels;

namespace EzTech.Data.ApiModels.UserApiModels.OrderApiModels;

public class GetUserOrdersResponse
{
    public string Status { get; set; } = null!;
    public int TotalOrders { get; set; }
    public int PendingOrders { get; set; }
    public int ProcessingOrders { get; set; }
    public int CompletedOrders { get; set; }
    public List<OrderDto> Orders { get; set; } = null!;
}