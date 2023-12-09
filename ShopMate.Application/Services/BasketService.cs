using Microsoft.EntityFrameworkCore;
using ShopMate.Core.Entities;
using ShopMate.Core.Interfaces;
using ShopMate.Infrastructure.Data;

namespace ShopMate.Application.Services
{
    public class BasketService:IBasketService
    {
        private readonly ShopMateDbContext _dbContext;
        public BasketService(ShopMateDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Basket>> GetProductsAsync(int userId, int[] productsIds)
        {
            List<Basket> basketProducts = new List<Basket>();
            foreach(var product in productsIds)
            {
                var basketProduct = await _dbContext.Baskets
                    .FirstOrDefaultAsync(b => b.UserId == userId && b.ProductId == product.ToString());
                if (basketProduct == null)
                {
                    throw new InvalidOperationException("Product is not in your basket.");
                }
                basketProducts.Add(basketProduct);
            }
            return basketProducts;
        }
        public async Task AddAsync(int userId, string productId, int count)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://dummyjson.com/products/" + productId),
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
            var basket = await _dbContext.Baskets.Where(x => x.UserId == userId)
                .SingleOrDefaultAsync(x => x.ProductId == productId);
            if (basket == null)
            {
                basket = new Basket
                {
                    UserId = userId,
                    ProductId = productId,
                    Number = count
                };
                await _dbContext.Baskets.AddAsync(basket);
            }
            else
            {
                basket.Number += count;
            }
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int userId, string productId, int count)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://dummyjson.com/products/" + productId),
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
            var basket = await _dbContext.Baskets.Where(x => x.UserId == userId)
                .SingleOrDefaultAsync(x => x.ProductId == productId);
            if (basket == null)
            {
                throw new InvalidOperationException("BasketNotFound");
            }
            if (basket.Number <= count)
            {
                _dbContext.Baskets.Remove(basket);
            }
            else
            {
                basket.Number -= count;
            }
            
            await _dbContext.SaveChangesAsync();
        }
        public async Task RemoveAsync(int userId, string productId)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://dummyjson.com/products/" + productId),
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
            var basket = await _dbContext.Baskets.Where(x => x.UserId == userId)
                .SingleOrDefaultAsync(x => x.ProductId == productId);
            if (basket == null)
            {
                throw new InvalidOperationException("BasketNotFound");
            }
            _dbContext.Baskets.Remove(basket);
            await _dbContext.SaveChangesAsync();
        }
    }
}