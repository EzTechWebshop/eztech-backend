namespace EzTech.Data.ApiModels.UserApiModels.OrderApiModels;

public class CreateOrderRequest
{
    public string PaymentId { get; set; } = null!;
    public List<CreateOrderItemRequest> Products { get; set; } = null!;
    public int Total { get; set; }
    public string Date { get; set; } = null!;
}

public class CreateOrderItemRequest
{
    public int ProductId { get; set; }
    public string ProductName { get; set; } = null!;
    public string ProductDescription { get; set; } = null!;
    public decimal Price { get; set; } = 0;
    public int Quantity { get; set; } = 0;
}