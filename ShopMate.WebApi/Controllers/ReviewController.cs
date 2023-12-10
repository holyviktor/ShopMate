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

namespace ShopMate.WebApi.Controllers;
[ApiController]
[Authorize]
public class ReviewController : Controller
{
    private readonly ShopMateDbContext _dbContext;
    private readonly ReviewService _reviewService;
    private readonly UserService _userService;
    private readonly IMapper _mapper;
    
    public ReviewController(ShopMateDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _reviewService= new ReviewService(_dbContext);
        _userService = new UserService(_dbContext);
        _mapper = mapper;
    }
    
    [HttpGet("/reviews")]
    public async Task<ActionResult<List<Review>>> GetAllReviews(string idProduct)
    {
        
        if (!ModelState.IsValid)
        {
            throw new InvalidOperationException("Count value is not correct.");
        }

        var reviews = await _reviewService.GetAllReviews(idProduct);
        var reviewsModel = _mapper.Map<List<ProductReview>>(reviews);
        for (int i = 0; i < reviews.Count; i++)
        {
            var user = _mapper.Map<UserForReview>(reviews[i].User);
            reviewsModel[i].UserForReview = user;
        }
        return Ok(reviewsModel);
    }
    
    
    [HttpGet("/getRating")]
    public async Task<ActionResult<double>> GetRating(string idProduct)
    {
        if (!ModelState.IsValid)
        {
            throw new InvalidOperationException("Count value is not correct.");
        }

        var rating = await _reviewService.GetRating(idProduct);
        
        return Ok(rating);
    }
    
    [HttpGet("/getListRating")]
    public async Task<ActionResult<List<double>>> GetListRating([FromQuery]string[] idProducts)
    {
        if (!ModelState.IsValid)
        {
            throw new InvalidOperationException("Count value is not correct.");
        }

        var rating = await _reviewService.GetListRating(idProducts);
        
        return Ok(rating);
    }
    
    [HttpPost("/review/add")]
    public async Task Add(ReviewForAdd reviewForAdd)
    {
        // int userId = 1;
        var userId = (HttpContext.User.Identity as ClaimsIdentity)?.FindFirst("userid")?.Value;

        var authorisedUser = await _userService.GetByIdAsync(Convert.ToInt32(userId));
        if (!ModelState.IsValid)
        {
            throw new InvalidOperationException("Count value is not correct.");
        }

        await _reviewService.Add(authorisedUser.Id, reviewForAdd.ProductId, reviewForAdd.Text, reviewForAdd.Rating);
    }
    
    [HttpDelete("/review/delete")]
    public async Task Delete(int reviewId)
    {
        // int userId = 1;
        var userId = (HttpContext.User.Identity as ClaimsIdentity)?.FindFirst("userid")?.Value;

        var authorisedUser = await _userService.GetByIdAsync(Convert.ToInt32(userId));
        if (!ModelState.IsValid)
        {
            throw new InvalidOperationException("Count value is not correct.");
        }
        await _reviewService.Delete(authorisedUser.Id, reviewId);
    }
    
    
    
}