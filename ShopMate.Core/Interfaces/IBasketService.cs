namespace ShopMate.Core.Interfaces
{
    public interface IBasketService
    {
        public Task AddAsync(int userId, string productId, int count);
        public Task DeleteAsync(int userId, string productId, int count);
    }
}