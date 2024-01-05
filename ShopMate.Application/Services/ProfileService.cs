using Microsoft.EntityFrameworkCore;
using ShopMate.Core.Entities;
using ShopMate.Infrastructure.Data;


namespace ShopMate.Application.Services;

public class ProfileService
{
    private readonly ShopMateDbContext _dbContext;

    public ProfileService(ShopMateDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<List<Coupon>> GetUserCoupons(int userId,string statusCoupon)
    {
        if (statusCoupon == "Active")
        {
            return Task.FromResult(_dbContext.Coupons.Where(x => x.UserId == userId && x.isUsed==false && x.DateExpiration>=DateTime.Now).ToList());
        }else if (statusCoupon == "Used")
        {
            return Task.FromResult(_dbContext.Coupons.Where(x => x.UserId == userId && x.isUsed==true).ToList());
        }else if (statusCoupon == "Overdue")
        {
            return Task.FromResult(_dbContext.Coupons.Where(x => x.UserId == userId && x.DateExpiration<DateTime.Now).ToList());
        }
        return Task.FromResult(_dbContext.Coupons.Where(x => x.UserId == userId).ToList());
        
    }
    
    public Task<List<Order>> GetUserOrders(int userId, Status status)
    {
        var userOrders = _dbContext.Orders.Where(x => x.UserAddress.UserId == userId && x.Status == status).Include(x=>x.UserAddress.Address).Include(x=>x.Coupon).ToList();
        return Task.FromResult(userOrders);
    }
    
    public async Task CancelledOrder(int orderId)
    {
        var order = _dbContext.Orders.SingleOrDefault(x => x.Id == orderId);
        if (order == null)
        {
            throw new InvalidOperationException("OrderNotFound");
        } else if (order.Status != Status.Unpaid && order.Status != Status.InProcess)
        {
            throw new InvalidOperationException("Already cannot cancelled");
        }
        else
        {
            order.Status = Status.Cancelled;
        }
        await _dbContext.SaveChangesAsync();
    }

    public async Task ChangePasswordAsync(User user, string oldPassword, string newPassword)
    {
        if (user.Password == UserService.HashPassword(oldPassword))
        {
            user.Password = UserService.HashPassword(newPassword);
            await _dbContext.SaveChangesAsync();
        }
        else
        {
            throw new InvalidOperationException("Old password is incorrect!");
        }
        
    }

}