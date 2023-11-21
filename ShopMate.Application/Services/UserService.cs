using Microsoft.EntityFrameworkCore;
using ShopMate.Core.Entities;
using ShopMate.Core.Interfaces;
using ShopMate.Infrastructure.Data;

namespace ShopMate.Application.Services;

public class UserService:IUserService
{
    private readonly ShopMateDbContext _dbContext;
    public UserService(ShopMateDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<User> GetByIdAsync(int userId)
    {
        var authorisedUser = await _dbContext.Users.SingleOrDefaultAsync(x => x.Id == userId);
        if (authorisedUser == null)
        {
            throw new InvalidOperationException("User is not found.");
        }
        return authorisedUser;
    }
}