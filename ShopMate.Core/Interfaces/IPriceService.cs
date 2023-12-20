using ShopMate.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopMate.Core.Interfaces
{
    public interface IPriceService
    {
        public Task<double> GetPriceAsync(List<ProductBasket> productBaskets);
        public Task<double> ApplyCouponAsync(double price, int couponId);
        public double GetDiscountValue(double price);
    }
}
