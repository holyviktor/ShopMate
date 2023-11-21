using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopMate.Core.Entities;
using ShopMate.Infrastructure.Data;
using ShopMate.WebApi.Models;
using ShopMate.Application.Services;
using System.Net.Http.Headers;
using System.Text;

namespace ShopMate.WebApi.Controllers
{
    [ApiController]
    public class BasketController : Controller
    {
        private readonly ShopMateDbContext _dbContext;
        private readonly BasketService _basketService;
        private readonly UserService _userService;
        public BasketController(ShopMateDbContext dbContext)
        {
            _dbContext = dbContext;
            _basketService = new BasketService(_dbContext);
            _userService = new UserService(_dbContext);
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
            await _basketService.AddAsync(authorisedUser.Id, productBasket.ProductId, productBasket.Count);
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

            await _basketService.DeleteAsync(authorisedUser.Id, productBasket.ProductId, productBasket.Count);
        }
    }
}
