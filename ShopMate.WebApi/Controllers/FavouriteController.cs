﻿using Microsoft.AspNetCore.Http;
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
        public FavouriteController(ShopMateDbContext dbContext)
        {
            _dbContext = dbContext;
            _favouriteService = new FavouriteService(_dbContext);
        }
        
        
        [HttpGet("/favourites")]
        public async Task GetAllFavourites()
        {
            int userId = 1;
            var authorisedUser = _dbContext.Users.FirstOrDefault(x => x.Id == userId);
            if (authorisedUser == null)
            {
                throw new InvalidOperationException("User is not found.");
            }
            if (!ModelState.IsValid)
            {
                throw new InvalidOperationException("Count value is not correct.");
            }

            await _favouriteService.GetAllFavourites(authorisedUser.Id);
        }
        
        
        [HttpPost("/favourite/add")]
        public async Task Add(ProductFavourite productFavourite)
        {
            int userId = 1;
            var authorisedUser = _dbContext.Users.FirstOrDefault(x => x.Id == userId);
            if (authorisedUser == null)
            {
                throw new InvalidOperationException("User is not found.");
            }
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
            var authorisedUser = _dbContext.Users.FirstOrDefault(x => x.Id == userId);
            if (authorisedUser == null)
            {
                throw new InvalidOperationException("User is not found.");
            }
            if (!ModelState.IsValid)
            {
                throw new InvalidOperationException("Count value is not correct.");
            }

            await _favouriteService.Delete(authorisedUser.Id, productFavourite.ProductId);
        }
    }
}