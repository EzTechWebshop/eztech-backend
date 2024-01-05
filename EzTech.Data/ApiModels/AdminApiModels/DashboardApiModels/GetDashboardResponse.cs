using EzTech.Data.DtoModels;

namespace EzTech.Data.ApiModels.AdminApiModels.DashboardApiModels;

public class GetDashboardResponse
{
    public int TotalActiveOrders { get; set; }
    public int PendingOrders { get; set; }
    public int ProcessingOrders { get; set; }
    public int ShippedOrders { get; set; }

    // Orders Info
    
    // Created Orders
    public int CreatedOrdersToday { get; set; } = new();
    public int CreatedOrdersThisMonth { get; set; } = new();
    public int CreatedOrdersThisYear { get; set; } = new();
    // Completed Orders
    public List<DashboardOrder> CompletedOrdersToday { get; set; } = new();
    public List<DashboardOrder> CompletedOrdersThisMonth { get; set; } = new();
    public List<DashboardOrder> CompletedOrdersThisYear { get; set; } = new();
    public List<DashboardOrder> CompletedOrdersAllTime { get; set; } = new();
    
    // Income
    public decimal IncomeToday { get; set; }
    public decimal IncomeThisMonth { get; set; }
    public decimal IncomeThisYear { get; set; }
    public decimal IncomeAllTime { get; set; }
    
    // 
    public List<OrderDto> LatestFiveActiveOrders { get; set; } = new();
    public List<OrderDto> OldestFiveActiveOrders { get; set; } = new();
}

public class DashboardOrder
{
    public int OrderId { get; set; }
    public int UserId { get; set; }
    public string Status { get; set; } = null!;
    public List<int> IdListOfItems { get; set; } = new();
    public DateTime CreatedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public decimal Total { get; set; }
}