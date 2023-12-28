namespace ShopMate.WebApi.Models;

public class UserProfile
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public DateTime DateBirth { get; set; }
    public string PhoneNumber { get; set; }
}