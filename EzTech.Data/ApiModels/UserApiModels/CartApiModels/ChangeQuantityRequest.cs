namespace EzTech.Data.ApiModels.UserApiModels.CartApiModels;

public class ChangeQuantityRequest
{
    public int CartItemId { get; set; }
    public int Quantity { get; set; }
}