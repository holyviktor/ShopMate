using Microsoft.AspNetCore.Mvc;
using ShopMate.Application.Services;
using ShopMate.Infrastructure.Data;
using ShopMate.WebApi.Models;

namespace ShopMate.WebApi.Controllers;
[ApiController]
public class UserController: Controller
{
    private readonly ShopMateDbContext _dbContext;
    private readonly UserService _userService;


    public UserController(ShopMateDbContext dbContext)
    {
        _dbContext = dbContext;
        _userService = new UserService(_dbContext);
    }

    [HttpPost("/signin")]
    public async Task<string> SignIn(SignInModel signInModel)
    {
        if (!ModelState.IsValid)
        {
            throw new InvalidOperationException("Count value is not correct.");
        }
        string token = await _userService.SignInUser(signInModel.Email, signInModel.Password);
        return token;
    }

    [HttpPost("/signup")]
    public async Task<string> SignUp(SignUpModel signUpModel)
    {
        if (!ModelState.IsValid)
        {
            throw new InvalidOperationException("Count value is not correct.");
        }
        string token = await _userService.SignUpUser(signUpModel.FirstName, signUpModel.LastName, signUpModel.Email, signUpModel.Password, signUpModel.DateBirth, signUpModel.PhoneNumber);
        return token;
    }
    
}