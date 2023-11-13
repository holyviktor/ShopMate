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
        public int UserId { get; set; }
        public DateTime Date { get; set; }
        public bool isPaid { get; set; }
        public Double TotalPrice { get; set; }
        public int CouponId { get; set; }
        public User User { get; set; }
        public Coupon Coupon { get; set; }
        public ICollection<OrderProduct> Products { get; set; }
    }
}
