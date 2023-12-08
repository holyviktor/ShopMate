namespace ShopMate.WebApi.Models;

public class CouponModel
{
    public int Id { get; set; }
    public Double Discount { get; set; }
    public DateTime DateExpiration { get; set; }
}