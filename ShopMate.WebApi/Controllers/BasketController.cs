using Microsoft.AspNetCore.Mvc;
using ShopMate.Infrastructure.Data;
using ShopMate.Application.Services;
using AutoMapper;
using ShopMate.Core.Models;

namespace ShopMate.WebApi.Controllers
{
    [ApiController]
    public class BasketController : Controller
    {
        private readonly ShopMateDbContext _dbContext;
        private readonly BasketService _basketService;
        private readonly UserService _userService;
        private readonly IMapper _mapper;
        public BasketController(ShopMateDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _basketService = new BasketService(_dbContext);
            _userService = new UserService(_dbContext);
            _mapper = mapper;
        }
        [HttpGet("/basket")]
        public async Task<List<ProductBasket>> Index()
        {
            int userId = 1;
            var authorisedUser = await _userService.GetByIdAsync(userId);
            var baskets = _dbContext.Baskets.Where(b => b.UserId == authorisedUser.Id).ToList();
            var productsBasket = _mapper.Map<List<ProductBasket>>(baskets);
            return productsBasket;

        }
        
        [HttpPost("/basket/add")]
        public async Task Add(ProductBasket productBasket)
        {
            int userId = 1;
            var authorisedUser = await _userService.GetByIdAsync(userId);
            if (!ModelState.IsValid)
            {
                throw new InvalidOperationException("Count value is not correct.");
            }
            await _basketService.AddAsync(authorisedUser.Id, productBasket.ProductId, productBasket.Number);
        }

        [HttpDelete("/basket/delete")]
        public async Task Delete(ProductBasket productBasket)
        {
            int userId = 1;
            var authorisedUser = await _userService.GetByIdAsync(userId);
            if (!ModelState.IsValid)
            {
                throw new InvalidOperationException("Count value is not correct.");
            }

            await _basketService.DeleteAsync(authorisedUser.Id, productBasket.ProductId, productBasket.Number);
        }
        [HttpDelete("/basket/remove")]
        public async Task Delete(string productId)
        {
            int userId = 1;
            var authorisedUser = await _userService.GetByIdAsync(userId);
            await _basketService.RemoveAsync(authorisedUser.Id, productId);
        }
        
    }
}
