using ShopMate.Core.Models;

namespace ShopMate.WebApi.Models
{
    public class OrderCreationInfo
    {
        public List<ProductBasket> Products { get; set; }
        public UserOrder User { get; set; }

    }
}
