using ShopMate.Core.Models;

namespace ShopMate.WebApi.Models
{
    public class OrderCreationInfo
    {
        public List<ProductBasket> Products { get; set; }
        public int CouponId { get; set; }
        public UserOrder User { get; set; }
        public double Price { get; set; }
        public double PriceDiscount { get; set; }
    }
}
