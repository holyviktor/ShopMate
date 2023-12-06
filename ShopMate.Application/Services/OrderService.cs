using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ShopMate.Core.Entities;
using ShopMate.Core.Models;
using ShopMate.Infrastructure.Data;
using ShopMate.Core.Interfaces;

namespace ShopMate.Application.Services;

public class OrderService : IOrderService
{
    private readonly ShopMateDbContext _dbContext;
    private readonly PriceService _priceService;
    private readonly IMapper _mapper;
    private readonly BasketService _basketService;

    public OrderService(ShopMateDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _priceService = new PriceService(_dbContext);
        _basketService = new BasketService(_dbContext);
    }

    public async Task CreateOrderAsync(int userId, OrderInput orderInput)
    {
        var coupon = _dbContext.Coupons.FirstOrDefault(c => c.Id == orderInput.CouponId && c.UserId == userId);
        var basketProducts = _mapper.Map<List<ProductBasket>>
            (await _basketService.GetProductsAsync(userId, orderInput.ProductsId));

        var price = await _priceService.GetPriceAsync(basketProducts);
        var userAddress = await _dbContext.UserAddresses
            .Where(x => x.AddressId == orderInput.AddressId)
            .SingleOrDefaultAsync(x => x.UserId == userId);
        if (userAddress == null)
        {
            throw new InvalidOperationException("User address not found");
        }

        var status = await _dbContext.Statuses.FindAsync(1);
        if (status == null)
        {
            status = new Status
            {
                Name = "Active"
            };
            _dbContext.Statuses.Add(status);
            await _dbContext.SaveChangesAsync();
        }

        var order = new Order
        {
            UserAddressId = userAddress.Id,
            Date = DateTime.Now,
            StatusId = status.Id,
            CouponId = null
        };
        if (coupon != null)
        {
            coupon.isUsed = true;
            price = await _priceService.ApplyCouponAsync(price, coupon.Id);
            order.CouponId = coupon.Id;
        }

        order.TotalPrice = price;
        _dbContext.Orders.Add(order);
        var discount = _priceService.GetDiscountValue(price);
        if (discount > 0)
        {
            var newCoupon = new Coupon
            {
                Discount = discount,
                isUsed = false,
                DateExpiration = DateTime.Now.AddYears(1),
                UserId = userId,
            };
            _dbContext.Coupons.Add(newCoupon);
        }

        await _dbContext.SaveChangesAsync();
    }
}