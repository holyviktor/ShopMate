using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopMate.Core.Entities;
using ShopMate.Infrastructure.Data;
using ShopMate.WebApi.Models;
using ShopMate.Application.Services;
using System.Net.Http.Headers;
using System.Text;
using AutoMapper;

namespace ShopMate.WebApi.Controllers;
[ApiController]
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
        int userId = 1;
        var authorisedUser = await _userService.GetByIdAsync(userId);
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
    
    [HttpPost("/review/add")]
    public async Task Add(ReviewForAdd reviewForAdd)
    {
        int userId = 1;
        var authorisedUser = await _userService.GetByIdAsync(userId);
        if (!ModelState.IsValid)
        {
            throw new InvalidOperationException("Count value is not correct.");
        }

        await _reviewService.Add(authorisedUser.Id, reviewForAdd.ProductId, reviewForAdd.Text, reviewForAdd.Rating);
    }
    
    [HttpDelete("/review/delete")]
    public async Task Delete(int reviewId)
    {
        int userId = 1;
        var authorisedUser = await _userService.GetByIdAsync(userId);
        if (!ModelState.IsValid)
        {
            throw new InvalidOperationException("Count value is not correct.");
        }
        await _reviewService.Delete(authorisedUser.Id, reviewId);
    }
    
    
    
}