using ShopMate.Core.Entities;

namespace ShopMate.Core.Interfaces
{
    public interface IBasketService
    {
        public Task<List<Basket>> GetProductsAsync(int userId, int[] productsIds);
        public Task AddAsync(int userId, string productId, int count);
        public Task DeleteAsync(int userId, string productId, int count);
    }
}