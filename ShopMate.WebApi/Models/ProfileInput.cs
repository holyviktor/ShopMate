using System.ComponentModel.DataAnnotations;

namespace ShopMate.WebApi.Models;

public class ProfileInput
{
    [Required]
    [MinLength(3)]
    [MaxLength(50)]
    public string FirstName { get; set; }
    [Required]
    [MinLength(3)]
    [MaxLength(50)]
    public string LastName { get; set; }
    
    [Required]
    [MinLength(10)]
    public string PhoneNumber { get; set; }
    [Required]
    public string Email { get; set; }
    [Required]
    public DateTime DateBirth { get; set; }
}