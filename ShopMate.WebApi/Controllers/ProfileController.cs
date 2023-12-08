using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ShopMate.Application.Services;
using ShopMate.Core.Entities;
using ShopMate.Infrastructure.Data;
using ShopMate.WebApi.Models;

namespace ShopMate.WebApi.Controllers;

public class ProfileController:Controller
{
    private readonly ShopMateDbContext _dbContext;
    private readonly UserService _userService;
    private readonly ProfileService _profileService;
    private readonly IMapper _mapper;
    public ProfileController(ShopMateDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _userService = new UserService(_dbContext);
        _profileService = new ProfileService(_dbContext);
        _mapper = mapper;
    }
    [HttpGet("/profile")]
    public async Task<UserProfile> Index()
    {
        int userId = 1;
        var authorisedUser = await _userService.GetByIdAsync(userId);
        var profile = _mapper.Map<UserProfile>(authorisedUser);
        return profile;
    }
    [HttpPatch("/profile/edit")]
    public async Task Edit(ProfileInput profileInput)
    {
        int userId = 1;
        var authorisedUser = await _userService.GetByIdAsync(userId);
        if (!ModelState.IsValid)
        {
            throw new InvalidOperationException("Input value is not correct.");
        }

        authorisedUser.FirstName = profileInput.FirstName;
        authorisedUser.LastName = profileInput.LastName;
        authorisedUser.Password = profileInput.LastName;
        authorisedUser.PhoneNumber = profileInput.PhoneNumber;
        authorisedUser.DateBirth = profileInput.DateBirth;
        await _dbContext.SaveChangesAsync();
    }

    [HttpGet("/profile/coupons")]
    public async Task<ActionResult<List<CouponModel>>> GetUserCoupons()
    {
        int userId = 1;
        var authorisedUser = await _userService.GetByIdAsync(userId);
        var coupons = await _profileService.GetUserCoupons(authorisedUser.Id);
        
        return Ok(_mapper.Map<List<CouponModel>>(coupons));
    }
    
    [HttpGet("/profile/orders")]
    public async Task<ActionResult<List<UserOrdersModel>>> GetUserOrders(Status status)
    {
        int userId = 1;
        var authorisedUser = await _userService.GetByIdAsync(userId);
        if (!ModelState.IsValid)
        {
            throw new InvalidOperationException("Input data is not valid.");
        }
        Console.WriteLine("here1");
        var userOrders = await _profileService.GetUserOrders(authorisedUser.Id, status);
        Console.WriteLine("here2");
        
        List<UserOrdersModel> userOrdersModels = new List<UserOrdersModel>();
        foreach (var order in userOrders)
        {
  
            List<string> products = new List<string>();
         // if (order.Products != null)
         //    {
         //        foreach (var product in order.Products)
         //        {
         //            products.Add(product.ProductId);
         //        }
         //    }

            userOrdersModels.Add(new UserOrdersModel(order.Id, _mapper.Map<UserAddressModel>(order.UserAddress.Address),order.Date, order.Status, order.TotalPrice,_mapper.Map<CouponModel>(order.Coupon), products));
        }

        return Ok(userOrdersModels);
    }

    [HttpPost("/profile/orders/cancelled")]
    public async Task CancelledOrder(int orderId)
    {
        await _profileService.CancelledOrder(orderId);
    }
  
    
}