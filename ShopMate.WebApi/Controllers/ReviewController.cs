using Microsoft.AspNetCore.Http;
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
        var userId = (HttpContext.User.Identity as ClaimsIdentity)?.FindFirst("userid")?.Value;

        var authorisedUser = await _userService.GetByIdAsync(Convert.ToInt32(userId));
        var reviews = await _reviewService.GetAllReviews(idProduct);
        var reviewsModel = _mapper.Map<List<ProductReview>>(reviews);
        for (int i = 0; i < reviews.Count; i++)
        {
            var user = _mapper.Map<UserForReview>(reviews[i].User);
            Console.WriteLine(user);
            Console.WriteLine(reviews[i].User);
            reviewsModel[i].UserForReview = user;
            reviewsModel[i].IsThisUser  = user.Email == authorisedUser.Email;
            Console.WriteLine((HttpContext.User.Identity as ClaimsIdentity)?.FindFirst("email")?.Value);
            Console.WriteLine(reviewsModel[i].IsThisUser);
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
    public async Task<ActionResult<List<ReviewProduct>>> GetListRating([FromQuery]string[] idProducts)
    {
        if (!ModelState.IsValid)
        {
            throw new InvalidOperationException("Count value is not correct.");
        }

        List<ReviewProduct> reviewProducts = new List<ReviewProduct>();

        for (int i = 0; i < idProducts.Length; i++)
        {
            ReviewProduct reviewProduct =
                new ReviewProduct(await _reviewService.GetRating(idProducts[i]), idProducts[i]);
            // Console.WriteLine(reviewProduct.ToString());
            reviewProducts.Add(reviewProduct);
            
        }
        Console.WriteLine(reviewProducts[0].ToString());
        return Ok(reviewProducts);
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