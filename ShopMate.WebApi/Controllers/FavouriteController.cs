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
    public class FavouriteController : Controller
    {
        private readonly ShopMateDbContext _dbContext;
        private readonly FavouriteService _favouriteService;
        private readonly UserService _userService;
        public FavouriteController(ShopMateDbContext dbContext)
        {
            _dbContext = dbContext;
            _favouriteService = new FavouriteService(_dbContext);
            _userService = new UserService(_dbContext);
        }
        
        
        [HttpGet("/favourites")]
        public async Task<ActionResult<List<Favourite>>> GetAllFavourites()
        {
            int userId = 1;
            var authorisedUser = await _userService.GetByIdAsync(userId);
            if (!ModelState.IsValid)
            {
                throw new InvalidOperationException("Count value is not correct.");
            }

            var favourites = await _favouriteService.GetAllFavourites(authorisedUser.Id);
            return Ok(favourites);
        }
        
        
        [HttpPost("/favourite/add")]
        public async Task Add(ProductFavourite productFavourite)
        {
            int userId = 1;
            var authorisedUser = await _userService.GetByIdAsync(userId);
            if (!ModelState.IsValid)
            {
                throw new InvalidOperationException("Count value is not correct.");
            }

            await _favouriteService.Add(authorisedUser.Id, productFavourite.ProductId);
           
        }

        [HttpDelete("/favourite/delete")]
        public async Task Delete(ProductFavourite productFavourite)
        {
            int userId = 1;
            var authorisedUser = await _userService.GetByIdAsync(userId);
            if (!ModelState.IsValid)
            {
                throw new InvalidOperationException("Count value is not correct.");
            }

            await _favouriteService.Delete(authorisedUser.Id, productFavourite.ProductId);
        }
    }
}