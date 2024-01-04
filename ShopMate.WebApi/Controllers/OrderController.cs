using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ShopMate.Application.Services;
using ShopMate.Core.Entities;
using ShopMate.Core.Models;
using ShopMate.Infrastructure.Data;
using ShopMate.WebApi.Models;

namespace ShopMate.WebApi.Controllers
{
    [ApiController]
    [Authorize]
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
        [Route("/orders/info")]
        public async Task<OrderCreationInfo> Create([FromQuery] CreateOrder orderInfo)
        {
            // var userId = 1;
            var userId = (HttpContext.User.Identity as ClaimsIdentity)?.FindFirst("userid")?.Value;

            var authorisedUser = await _userService.GetByIdAsync(Convert.ToInt32(userId));
            var orderCreationInfo = _mapper.Map<OrderCreationInfo>(orderInfo);

            orderCreationInfo.User = _mapper.Map<UserOrder>(authorisedUser);

            var basketProducts = _mapper.Map<List<ProductBasket>>
                (await _basketService.GetProductsAsync(authorisedUser.Id, orderInfo.ProductsId));
            orderCreationInfo.Products = basketProducts;

            return orderCreationInfo;
        }

        [HttpPost]
        [Route("/order/create")]
        public async Task<int> CreateOrder(OrderInput orderInput)
        {
            if (!ModelState.IsValid)
            {
                throw new InvalidOperationException("Input data is not valid.");
            }

            var userId = (HttpContext.User.Identity as ClaimsIdentity)?.FindFirst("userid")?.Value;

            var authorisedUser = await _userService.GetByIdAsync(Convert.ToInt32(userId));
            int orderId = await _orderService.CreateOrderAsync(authorisedUser.Id, orderInput);
            authorisedUser.FirstName = orderInput.UserOrder.FirstName;
            authorisedUser.LastName = orderInput.UserOrder.LastName;
            authorisedUser.PhoneNumber = orderInput.UserOrder.PhoneNumber;
            await _dbContext.SaveChangesAsync();
            return orderId;
        }

        [HttpGet]
        [Route("/order/{id:int}")]
        public async Task<UserOrderModel> Order(int id)
        {
            var userId = (HttpContext.User.Identity as ClaimsIdentity)?.FindFirst("userid")?.Value;

            var authorisedUser = await _userService.GetByIdAsync(Convert.ToInt32(userId));

            var userOrder = await _dbContext.Orders.Include(o => o.UserAddress)
                .Where(o => o.Id == id && o.UserAddress.UserId == authorisedUser.Id)
                .Include(o=>o.Coupon)
                .Include(o=>o.Products)
                .SingleOrDefaultAsync();
            if (userOrder == null)
            {
                throw new Exception("Order is not found");
            }

            var orderProducts = _mapper.Map<List<ProductBasket>>(userOrder.Products);
            
            var userOrderModel = new UserOrderModel
            {
                OrderId = userOrder.Id,
                ProductBaskets = orderProducts,
                CouponDiscount = userOrder.Coupon != null ? userOrder.Coupon.Discount:0.0,
                Date = DateOnly.FromDateTime(userOrder.Date),
                TotalPrice = userOrder.TotalPrice,
                Status = userOrder.Status
            };
            return userOrderModel;
        }
        
        [HttpGet]
        [Route("/orders")]
        public async Task<List<UserOrderModel>> Orders()
        {
            var userId = (HttpContext.User.Identity as ClaimsIdentity)?.FindFirst("userid")?.Value;

            var authorisedUser = await _userService.GetByIdAsync(Convert.ToInt32(userId));

            var userOrders = await _dbContext.Orders.Include(o => o.UserAddress)
                .Where(o => o.UserAddress.UserId == authorisedUser.Id)
                .Include(o=>o.Coupon)
                .Include(o=>o.Products)
                .ToListAsync();
            if (userOrders.IsNullOrEmpty())
            {
                throw new Exception("Orders are not found");
            }

            var orders = new List<UserOrderModel>();

            foreach (var order in userOrders)
            {
                var orderProducts = _mapper.Map<List<ProductBasket>>(order.Products);
            
                var userOrderModel = new UserOrderModel
                {
                    OrderId = order.Id,
                    ProductBaskets = orderProducts,
                    CouponDiscount = order.Coupon != null ? order.Coupon.Discount:0.0,
                    Date = DateOnly.FromDateTime(order.Date),
                    TotalPrice = order.TotalPrice,
                    Status = order.Status
                };
                orders.Add(userOrderModel);
            }
            return orders;
        }
        
        [HttpGet]
        [Route("/orders/status")]
        public async Task<List<UserOrderModel>> OrdersByStatus(Status status)
        {
            var userId = (HttpContext.User.Identity as ClaimsIdentity)?.FindFirst("userid")?.Value;

            var authorisedUser = await _userService.GetByIdAsync(Convert.ToInt32(userId));

            var userOrders = await _dbContext.Orders.Include(o => o.UserAddress)
                .Where(o => o.UserAddress.UserId == authorisedUser.Id)
                .Where(o=>o.Status == status)
                .Include(o=>o.Coupon)
                .Include(o=>o.Products)
                .ToListAsync();
            var orders = new List<UserOrderModel>();
            if (userOrders.IsNullOrEmpty())
            {
                return orders;
            }

            foreach (var order in userOrders)
            {
                var orderProducts = _mapper.Map<List<ProductBasket>>(order.Products);
            
                var userOrderModel = new UserOrderModel
                {
                    OrderId = order.Id,
                    ProductBaskets = orderProducts,
                    CouponDiscount = order.Coupon != null ? order.Coupon.Discount:0.0,
                    Date = DateOnly.FromDateTime(order.Date),
                    TotalPrice = order.TotalPrice,
                    Status = order.Status
                };
                orders.Add(userOrderModel);
            }
            return orders;
        }
    }
}