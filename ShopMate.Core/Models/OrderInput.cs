﻿using ShopMate.Core.Models;

namespace ShopMate.Core.Models
{
    public class OrderInput
    {
        public string[] ProductsId { get; set; }
        public UserOrder UserOrder { get; set; }
        public int? CouponId { get; set; }
        public int AddressId { get; set; }

    }
}
