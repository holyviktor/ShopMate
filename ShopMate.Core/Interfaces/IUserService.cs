using ShopMate.Core.Entities;

namespace ShopMate.Core.Interfaces;

public interface IUserService
{
    public Task<User> GetByIdAsync(int userId);

}