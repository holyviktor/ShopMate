using ShopMate.Core.Entities;
using ShopMate.Infrastructure.Data;

namespace ShopMate.Application.Services
{
    public class BasketService
    {
        private readonly ShopMateDbContext _dbContext;
        public BasketService(ShopMateDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task Add(int userId, string productId, int count)
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
            var basket = _dbContext.Baskets.Where(x => x.UserId == userId)
                .SingleOrDefault(x => x.ProductId == productId);
            if (basket == null)
            {
                basket = new Basket
                {
                    UserId = userId,
                    ProductId = productId,
                    Number = count
                };
                _dbContext.Baskets.Add(basket);
            }
            else
            {
                basket.Number += count;
            }
            await _dbContext.SaveChangesAsync();
        }

        public async Task Delete(int userId, string productId, int count)
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
            var basket = _dbContext.Baskets.Where(x => x.UserId == userId)
                .SingleOrDefault(x => x.ProductId == productId);
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

    }
}