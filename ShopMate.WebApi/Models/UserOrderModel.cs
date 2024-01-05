using ShopMate.Core.Entities;
using ShopMate.Core.Models;

namespace ShopMate.WebApi.Models;

public class UserOrderModel
{
    public int OrderId { get; set; }
    public DateOnly Date { get; set; }
    public double CouponDiscount { get; set; }
    public double TotalPrice { get; set; }
    public List<ProductBasket> ProductBaskets { get; set; }
    public Status Status { get; set; }
    
}