using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopMate.Core.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public int UserAddressId { get; set; }
        public DateTime Date { get; set; }
        public int StatusId { get; set; }
        public Double TotalPrice { get; set; }
        public int? CouponId { get; set; }
        public UserAddress UserAddress { get; set; }
        public Coupon Coupon { get; set; }
        public ICollection<OrderProduct> Products { get; set; }
        public Status Status { get; set; }
    }
}
