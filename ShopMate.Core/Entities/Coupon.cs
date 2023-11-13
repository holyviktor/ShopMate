using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopMate.Core.Entities
{
    public class Coupon
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public Double Discount { get; set; }
        public DateTime DateExpiration { get; set; }
        public bool isUsed { get; set; }
        public User User { get; set; }
        public Order? Order { get; set; }
    }
}
