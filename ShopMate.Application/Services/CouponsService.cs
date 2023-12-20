using ShopMate.Core.Entities;
using ShopMate.Infrastructure.Data;

namespace ShopMate.Application.Services;

public class CouponsService
{
    private readonly ShopMateDbContext _dbContext;

    public CouponsService(ShopMateDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task CreateAsync(double discount, int userId)
    {
        if (discount > 0)
        {
            var newCoupon = new Coupon
            {
                Discount = discount,
                isUsed = false,
                DateExpiration = DateTime.Now.AddYears(1),
                UserId = userId,
            };
            await _dbContext.Coupons.AddAsync(newCoupon);
            await _dbContext.SaveChangesAsync();
        }
    }
}