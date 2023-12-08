using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopMate.Application.Services;
using ShopMate.Core.Entities;
using ShopMate.Core.Models;
using ShopMate.Infrastructure.Data;
using ShopMate.WebApi.Models;

namespace ShopMate.WebApi.Controllers
{
    [ApiController]
    public class OrderController : Controller
    {
        private readonly ShopMateDbContext _dbContext;
        private readonly UserService _userService;
        private readonly PriceService _priceService;
        private readonly BasketService _basketService;
        private readonly OrderService _orderService;
        private readonly IMapper _mapper;
        public OrderController(ShopMateDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _userService = new UserService(_dbContext);
            _priceService = new PriceService(_dbContext);
            _basketService = new BasketService(_dbContext);
            _orderService = new OrderService(_dbContext, _mapper);
        }
        [HttpGet]
        [Route("/orders/create")]
        public async Task<OrderCreationInfo> Create([FromQuery]CreateOrder orderInfo)
        {
            var userId = 1;
            var authorisedUser = await _userService.GetByIdAsync(userId);
            var orderCreationInfo = _mapper.Map<OrderCreationInfo>(orderInfo);

            orderCreationInfo.User = _mapper.Map<UserOrder>(authorisedUser);

            var basketProducts = _mapper.Map<List<ProductBasket>>
                (await _basketService.GetProductsAsync(authorisedUser.Id, orderInfo.ProductsId));
            orderCreationInfo.Products = basketProducts;
            var price = await _priceService.GetPriceAsync(basketProducts);
            orderCreationInfo.Price = price;
      
            if (orderInfo.CouponId != null)
            {
                var priceDiscount = await _priceService.ApplyCouponAsync(price, (int)orderInfo.CouponId);
                orderCreationInfo.PriceDiscount = priceDiscount;
            }
            orderCreationInfo.PriceDiscount = price;

            return orderCreationInfo;
        }

        [HttpPost]
        [Route("/order/create")]
        public async Task CreateOrder(OrderInput orderInput)
        {
            if (!ModelState.IsValid)
            {
                throw new InvalidOperationException("Input data is not valid.");
            }
            int userId = 1;
            var authorisedUser = await _userService.GetByIdAsync(userId);
            await _orderService.CreateOrderAsync(authorisedUser.Id, orderInput);
            authorisedUser.FirstName = orderInput.UserOrder.FirstName;
            authorisedUser.LastName = orderInput.UserOrder.LastName;
            // need to validate phone number
            authorisedUser.PhoneNumber = orderInput.UserOrder.PhoneNumber;
            await _dbContext.SaveChangesAsync();
        }
        
        

    }
}
