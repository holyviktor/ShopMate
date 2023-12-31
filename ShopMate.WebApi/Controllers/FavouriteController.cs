﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopMate.Core.Entities;
using ShopMate.Infrastructure.Data;
using ShopMate.WebApi.Models;
using ShopMate.Application.Services;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using ShopMate.WebApi.Identity;

namespace ShopMate.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    public class FavouriteController : Controller
    {
        private readonly ShopMateDbContext _dbContext;
        private readonly FavouriteService _favouriteService;
        private readonly UserService _userService;
        private readonly IMapper _mapper;
        public FavouriteController(ShopMateDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _favouriteService = new FavouriteService(_dbContext);
            _userService = new UserService(_dbContext);
            _mapper = mapper;
        }
        
        [HttpGet("/favourites")]
        public async Task<ActionResult<List<Favourite>>> GetAllFavourites()
        {
            // int userId = 1;
            var userId = (HttpContext.User.Identity as ClaimsIdentity)?.FindFirst("userid")?.Value;
            var authorisedUser = await _userService.GetByIdAsync(Convert.ToInt32(userId));
            if (!ModelState.IsValid)
            {
                throw new InvalidOperationException("Count value is not correct.");
            }

            var favourites = await _favouriteService.GetAllFavourites(authorisedUser.Id);
            var favouritesModel = _mapper.Map<List<ProductFavourite>>(favourites);
            return Ok(favouritesModel);
        }
        
        [HttpGet("/favourite")]
        public async Task<ActionResult<Boolean>> CheckFavourite(string productId)
        {
            var userId = (HttpContext.User.Identity as ClaimsIdentity)?.FindFirst("userid")?.Value;
            var authorisedUser = await _userService.GetByIdAsync(Convert.ToInt32(userId));
            if (!ModelState.IsValid)
            {
                throw new InvalidOperationException("Count value is not correct.");
            }

            var checkFavourite = await _favouriteService.CheckFavourite(authorisedUser.Id, productId);
            return Ok(checkFavourite);
        }
        
        
        [HttpPost("/favourite/add")]
        public async Task Add(ProductFavourite productFavourite)
        {
            // int userId = 1;
            var userId = (HttpContext.User.Identity as ClaimsIdentity)?.FindFirst("userid")?.Value;

            var authorisedUser = await _userService.GetByIdAsync(Convert.ToInt32(userId));
            if (!ModelState.IsValid)
            {
                throw new InvalidOperationException("Count value is not correct.");
            }

            await _favouriteService.Add(authorisedUser.Id, productFavourite.ProductId);
           
        }

        [HttpDelete("/favourite/delete")]
        public async Task Delete(ProductFavourite productFavourite)
        {
            // int userId = 1;
            var userId = (HttpContext.User.Identity as ClaimsIdentity)?.FindFirst("userid")?.Value;

            var authorisedUser = await _userService.GetByIdAsync(Convert.ToInt32(userId));
            if (!ModelState.IsValid)
            {
                throw new InvalidOperationException("Count value is not correct.");
            }

            await _favouriteService.Delete(authorisedUser.Id, productFavourite.ProductId);
        }

        [HttpPost("/favourite/change")]
        public async Task Change(ProductFavourite productFavourite)
        {
            var userId = (HttpContext.User.Identity as ClaimsIdentity)?.FindFirst("userid")?.Value;

            var authorisedUser = await _userService.GetByIdAsync(Convert.ToInt32(userId));
            if (!ModelState.IsValid)
            {
                throw new InvalidOperationException("Count value is not correct.");
            }

            await _favouriteService.Change(authorisedUser.Id, productFavourite.ProductId);
        }
    }
}