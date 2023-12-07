using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ShopMate.Application.Services;
using ShopMate.Infrastructure.Data;
using ShopMate.WebApi.Models;

namespace ShopMate.WebApi.Controllers;

public class ProfileController:Controller
{
    private readonly ShopMateDbContext _dbContext;
    private readonly UserService _userService;
    private readonly IMapper _mapper;
    public ProfileController(ShopMateDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _userService = new UserService(_dbContext);
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

  
    
}