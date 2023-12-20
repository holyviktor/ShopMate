using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using ShopMate.Core.Entities;
using ShopMate.Core.Interfaces;
using ShopMate.Infrastructure.Data;
using Newtonsoft.Json;

namespace ShopMate.Application.Services;

public class UserService : IUserService
{
    private readonly ShopMateDbContext _dbContext;

    public UserService(ShopMateDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<User> GetByIdAsync(int userId)
    {
        var authorisedUser = await _dbContext.Users.Include(x => x.Role).SingleOrDefaultAsync(x => x.Id == userId);
        if (authorisedUser == null)
        {
            throw new InvalidOperationException("User is not found.");
        }

        return authorisedUser;
    }

    public async Task<string> SignInUser(string email, string password)
    {
        var user = await _dbContext.Users.Include(user => user.Role).SingleOrDefaultAsync(x => x.Email == email);
        if (user == null)
        {
            throw new Exception("UserNoFound");
        }

        if (user.Password != HashPassword(password))
        {
            throw new Exception("WrongPassword");
        }

        var client = new HttpClient();
        var data = new TokenUser
        {
            UserId = user.Id.ToString(),
            Email = user.Email,
            Role = user.Role.Name
        };
        // string jsonData = JsonConvert.SerializeObject(data);
        var url = "https://localhost:7078/token";

        string jsonData = JsonConvert.SerializeObject(data);

        var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

        var response = await client.PostAsync(url, content);
        try
        {
            response.EnsureSuccessStatusCode();
            string responseData = await response.Content.ReadAsStringAsync();


            return responseData ?? throw new InvalidOperationException("Error");
        }
        catch (HttpRequestException)
        {
            throw new Exception(response.StatusCode.ToString());
        }

    }

    public async Task<string> SignUpUser(string firstname, string lastname, string email, string password,
        string dateBirth, string phoneNumber)
    {
        var user = await _dbContext.Users.SingleOrDefaultAsync(x => x.Email == email);
        if (user != null)
        {
            throw new Exception("UserAlreadyExist");
        }

        user = new User
        {
            FirstName = firstname,
            LastName = lastname,
            Email = email,
            Password = HashPassword(password),
            DateBirth =  DateTime.ParseExact(dateBirth, "dd.MM.yyyy", CultureInfo.InvariantCulture),
            PhoneNumber = phoneNumber,
            RoleId = 2
        };
        ValidateUser(user);
        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();


        return await SignInUser(email, password);



    }

    public void ValidateUser(User user)
    {
        if (!Regex.IsMatch(user.FirstName, @"^[a-zA-Z]+$"))
        {
            throw new Exception("WrongFirstName");
        }

        if (!Regex.IsMatch(user.LastName, @"^[a-zA-Z]+$"))
        {
            throw new Exception("WrongLastName");
        }

        try
        {
            var addr = new System.Net.Mail.MailAddress(user.Email);
            
        }
        catch
        {
            throw new Exception("WrongEmail");
        }

        if (!Regex.IsMatch(user.PhoneNumber, @"^\+\d{1,3}\d{10}$"))
        {
            throw new Exception("WrongPhoneNumber");

        }
        var today = DateTime.Today;

        
        var age = today.Year - user.DateBirth.Year;
        
        if (user.DateBirth.Date > today.AddYears(-age))
            age--;
        
        if (age< 16 )
        {
            throw new Exception("WrongDateBirth");

        }
    }
    
    public static string HashPassword(string password)
    {
        using (var sha256 = SHA256.Create())
        {
            var passwordBytes = Encoding.UTF8.GetBytes(password);
            var hashBytes = sha256.ComputeHash(passwordBytes);
            
            // Перетворення хешу в рядок для збереження в базі даних
            var hashString = Convert.ToBase64String(hashBytes);
            return hashString;
        }
    }
}

public class TokenUser
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
    }