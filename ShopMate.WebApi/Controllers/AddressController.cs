using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopMate.Application.Services;
using ShopMate.Infrastructure.Data;
using ShopMate.WebApi.Models;

namespace ShopMate.WebApi.Controllers;
[Authorize]
[ApiController]
public class AddressController:Controller
{
    private readonly ShopMateDbContext _dbContext;
    private readonly UserService _userService;
    private readonly AddressService _addressService;
    private readonly IMapper _mapper;

    public AddressController(ShopMateDbContext dbContext,  IMapper mapper)
    {
        _dbContext = dbContext;
        _userService = new UserService(_dbContext);
        _addressService = new AddressService(_dbContext);
        _mapper = mapper;
    }

    [HttpGet("/profile/addresses")]
    public async Task<ActionResult<List<UserAddressModel>>> GetAllAddresses()
    {
        // int userId = 1;
        var userId = (HttpContext.User.Identity as ClaimsIdentity)?.FindFirst("userid")?.Value;

        var authorisedUser = await _userService.GetByIdAsync(Convert.ToInt32(userId));
        var addresses = await _addressService.GetAllAddress(authorisedUser.Id);
        var addressesModel = _mapper.Map<List<UserAddressModel>>(addresses);
        return Ok(addressesModel);
    }
    
    [HttpPost("/profile/address/add")]
    public async Task AddAddress(UserAddressModel userAddressModel)
    {
        // int userId = 1;
        var userId = (HttpContext.User.Identity as ClaimsIdentity)?.FindFirst("userid")?.Value;

        var authorisedUser = await _userService.GetByIdAsync(Convert.ToInt32(userId));
        if (!ModelState.IsValid)
        {
            throw new InvalidOperationException("Count value is not correct.");
        }

        await _addressService.Add(authorisedUser.Id, userAddressModel.City, userAddressModel.Street,
            userAddressModel.House, userAddressModel.Flat);

    }
    
    [HttpDelete("/profile/address/delete")]
    public async Task Delete(int addressId)
    {
        // int userId = 1;
        var userId = (HttpContext.User.Identity as ClaimsIdentity)?.FindFirst("userid")?.Value;

        var authorisedUser = await _userService.GetByIdAsync(Convert.ToInt32(userId));
        if (!ModelState.IsValid)
        {
            throw new InvalidOperationException("Count value is not correct.");
        }

        await _addressService.Delete(authorisedUser.Id, addressId);
    }
    
    [HttpPatch("/profile/address/edit")]
    public async Task Edit(UserAddressModel userAddressModel)
    {
        // int userId = 1;
        var userId = (HttpContext.User.Identity as ClaimsIdentity)?.FindFirst("userid")?.Value;

        var authorisedUser = await _userService.GetByIdAsync(Convert.ToInt32(userId));
        if (!ModelState.IsValid)
        {
            throw new InvalidOperationException("Input value is not correct.");
        }
        Console.WriteLine(3434);

        await _addressService.Edit(authorisedUser.Id,userAddressModel.Id, userAddressModel.City, userAddressModel.Street,
            userAddressModel.House, userAddressModel.Flat, userAddressModel.Id);
    }
}