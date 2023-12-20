using Microsoft.AspNetCore.Mvc;

namespace ShopMate.WebApi.Models
{
    [BindProperties]
    public class CreateOrder
    {
        public int[] ProductsId { get; set; }
        public int? CouponId { get; set; }
    }
}
