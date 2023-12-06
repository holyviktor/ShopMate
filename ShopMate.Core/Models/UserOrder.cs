using System.ComponentModel.DataAnnotations;

namespace ShopMate.Core.Models
{
    public class UserOrder
    {
        [Required]
        [MinLength(2)]
        public string FirstName { get; set; }
        [Required]
        [MinLength(2)]
        public string LastName { get; set; }
        [Required]
        [MinLength(10)]
        [MaxLength(13)]
        public string PhoneNumber { get; set; }
        
    }
}
