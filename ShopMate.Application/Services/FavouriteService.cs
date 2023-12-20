using ShopMate.Core.Entities;
using ShopMate.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopMate.Application.Services
{
    public class FavouriteService
    {

        private readonly ShopMateDbContext _dbContext;
        public FavouriteService(ShopMateDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task Add(int userId, string productId)
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
            var favourite = _dbContext.Favourites.Where(x => x.UserId == userId)
                .SingleOrDefault(x => x.ProductId == productId);
 
            if (favourite == null)
            {
                favourite = new Favourite
                {
                    UserId = userId,
                    ProductId = productId,
                    
                };
                _dbContext.Favourites.Add(favourite);
            }
            await _dbContext.SaveChangesAsync();
        }

        
        public Task<List<Favourite>> GetAllFavourites(int userId)
        {
            var favourites = _dbContext.Favourites.Where(x => x.UserId == userId).ToList();
            return Task.FromResult(favourites);
        }
        
        
        public async Task Delete(int userId, string productId)
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

            var favourite = _dbContext.Favourites.Where(x => x.UserId == userId)
                .SingleOrDefault(x => x.ProductId == productId);
            if (favourite == null)
            {
                throw new InvalidOperationException("FavouriteNotFound");
            } else
            {
                _dbContext.Favourites.Remove(favourite);
            }

            await _dbContext.SaveChangesAsync();
        }
    }
}
