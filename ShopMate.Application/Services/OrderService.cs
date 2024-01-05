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
    private readonly CouponsService _couponsService;
    
    public OrderService(ShopMateDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _priceService = new PriceService(_dbContext);
        _basketService = new BasketService(_dbContext);
        _couponsService = new CouponsService(_dbContext);
    }

    public async Task<int> CreateOrderAsync(int userId, OrderInput orderInput)
    {
        var coupon = _dbContext.Coupons.FirstOrDefault(c => c.Id == orderInput.CouponId && c.UserId == userId);

        var baskets = await _basketService.GetProductsAsync(userId, orderInput.ProductsId);
        var basketProducts = _mapper.Map<List<ProductBasket>>(baskets);

        var price = await _priceService.GetPriceAsync(basketProducts);
        var userAddress = await _dbContext.UserAddresses
            .Where(x => x.AddressId == orderInput.AddressId)
            .SingleOrDefaultAsync(x => x.UserId == userId);
        if (userAddress == null)
        {
            throw new InvalidOperationException("User address not found");
        }
        var order = new Order
        {
            UserAddressId = userAddress.Id,
            Date = DateTime.Now,
            Status = Status.Unpaid,
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
        await _dbContext.SaveChangesAsync();
        
        await CreateOrderProductsAsync(basketProducts, order.Id);
        var discount = _priceService.GetDiscountValue(price);
        await _couponsService.CreateAsync(discount, userId);
        
        _dbContext.Baskets.RemoveRange(baskets);
        
        await _dbContext.SaveChangesAsync();
        return order.Id;
    }

    private async Task CreateOrderProductsAsync(List<ProductBasket> basketProducts, int orderId)
    {
        foreach (var product in basketProducts)
        {
            var orderProduct = new OrderProduct
            {
                OrderId = orderId,
                ProductId = product.ProductId,
                Number = product.Number,
                Price = await _priceService.GetPriceAsync(new List<ProductBasket> { product })
            };
            _dbContext.OrderProducts.Add(orderProduct);
        }

        await _dbContext.SaveChangesAsync();
    }


}