using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ShopMate.Core.Entities;
using ShopMate.Infrastructure.Data;

namespace ShopMate.Application.Services;

public class AddressService
{

    private readonly ShopMateDbContext _dbContext;


    public AddressService(ShopMateDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAddress(string city, string street, string house, string? flat)
    {
        var addressDb = _dbContext.Addresses.SingleOrDefault(x =>
            x.City == city && x.Street == street && x.House == house && x.Flat == flat);

        if (addressDb == null)
        {
            var address = new Address
            {
                City = city, Flat = flat, House = house, Street = street
            };
            _dbContext.Addresses.Add(address);
        }

        await _dbContext.SaveChangesAsync();
    }


    public async Task Add(int userId, string city, string street, string house, string? flat)

    {
        await AddAddress(city, street, house, flat);
        var address = _dbContext.Addresses.SingleOrDefault(x =>
            x.City == city && x.Street == street && x.House == house && x.Flat == flat);
        var userAddress =
            _dbContext.UserAddresses.SingleOrDefault(x => x.AddressId == address!.Id && x.UserId == userId);

        if (userAddress == null)
        {
            userAddress = new UserAddress
            {
                UserId = userId, AddressId = address!.Id
            };
            _dbContext.UserAddresses.Add(userAddress);
        }
        await _dbContext.SaveChangesAsync();
    }
    
    public async Task<List<Address>> GetAllAddress(int userId)
    {
        List<Address> addresses = new List<Address>();
        var userAddresses = _dbContext.UserAddresses.Where(x => x.UserId == userId).ToList();
        foreach (var userAddress in userAddresses)
        {
            var address = await _dbContext.Addresses.SingleOrDefaultAsync(x => x.Id == userAddress.AddressId);

            if (address != null) addresses.Add(address);
        }

        return addresses;
    }
    public async Task Delete(int userId, int addressId)
    {
        // var address = _dbContext.Addresses.SingleOrDefault(x => x.Id == addressId);
        var userAddress = _dbContext.UserAddresses.Where(x => x.UserId == userId)
            .SingleOrDefault(x => x.AddressId == addressId);

        
        // if (address == null)
        // {
        //     throw new InvalidOperationException("AddressNotFound");
        // } else
        // {
        //     _dbContext.Addresses.Remove(address);
        // }

        if (userAddress == null)
        {
            throw new InvalidOperationException("UserAddressNotFound");
        }
        else
        {
            _dbContext.UserAddresses.Remove(userAddress);
        }

        await _dbContext.SaveChangesAsync();
    }
    
    public async Task Edit(int userId, int addressPrevId, string city, string street, string house, string? flat, int prevAddressId)

    {
        await AddAddress(city, street, house, flat);
        var address = _dbContext.Addresses.SingleOrDefault(x =>
            x.City == city && x.Street == street && x.House == house && x.Flat == flat);
        var userAddressPrev = _dbContext.UserAddresses.Where(x => x.UserId == userId)
            .SingleOrDefault(x => x.AddressId == prevAddressId);
        if (userAddressPrev == null)
        {
            throw new InvalidOperationException("UserAddressNotFound");
        }
        else
        {
            _dbContext.UserAddresses.Remove(userAddressPrev);
        }
        
        var userAddress =
            _dbContext.UserAddresses.SingleOrDefault(x => x.AddressId == address!.Id && x.UserId == userId);

        if (userAddress == null)
        {
            userAddress = new UserAddress
            {
                UserId = userId, AddressId = address!.Id
            };
            _dbContext.UserAddresses.Add(userAddress);
        }
        await _dbContext.SaveChangesAsync();

        var addressPrev = _dbContext.Addresses.SingleOrDefault(x => x.Id == addressPrevId);
        Console.WriteLine(12121);
        Console.WriteLine(addressPrev);
        var userAddressList = _dbContext.UserAddresses.Where(x => x.AddressId == addressPrev!.Id);
        Console.WriteLine(userAddressList);
        if (userAddressList.IsNullOrEmpty())
        {
            Console.WriteLine(12121);
            _dbContext.Addresses.Remove(addressPrev!);
        }
        await _dbContext.SaveChangesAsync();
        
    }
}