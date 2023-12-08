using Microsoft.EntityFrameworkCore;
using ShopMate.Core.Interfaces;
using ShopMate.Core.Models;
using ShopMate.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ShopMate.Application.Services
{
    public class PriceService:IPriceService
    {
        private readonly ShopMateDbContext _dbContext;
        public PriceService(ShopMateDbContext dbContext) {
            _dbContext = dbContext;
        }
        public async Task<double> GetPriceAsync(List<ProductBasket> productBaskets)
        {
            var cost = 0.0;
            var client = new HttpClient();
            
            foreach(var product in productBaskets)
            {
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri("https://dummyjson.com/products/" + product.ProductId),
                };
                var response = await client.SendAsync(request);
                try
                {
                    response.EnsureSuccessStatusCode();
                }
                catch (HttpRequestException)
                {
                    throw new Exception(response.StatusCode.ToString());
                }
                
                var content = await response.Content.ReadAsStringAsync();
                var searchedProduct = JsonSerializer.Deserialize<Product>(content);
                if (searchedProduct == null) {
                    throw new InvalidOperationException("Product is not valid.");
                }
                cost += searchedProduct.price * product.Number;
            }
            return cost;
        }
        public async Task <double> ApplyCouponAsync(double price, int couponId)
        {
            var coupon = await _dbContext.Coupons.FirstOrDefaultAsync(c => c.Id == couponId);
            if (coupon == null)
            {
                throw new InvalidOperationException("Coupon is not valid.");
            }
            if (!coupon.isUsed)
            {
                throw new InvalidOperationException("Coupon is already used.");
            }
            if (coupon.DateExpiration<DateTime.UtcNow)
            {
                throw new InvalidOperationException("Coupon is expired.");
            }
            price *= (1 - coupon.Discount / 100);

            return price;
        }

        public double GetDiscountValue(double price)
        {
            var discount = price switch
            {
                >= 2000 => 5,
                >= 1000 => 4,
                >= 500 => 3,
                >= 200 => 2,
                > 100 => 1,
                _ => 0,
            };
            return discount;
        }

    }
}
