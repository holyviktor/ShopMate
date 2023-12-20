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
        [RegularExpression("^(\\+38)?(0\\d{9})$")]
        public string PhoneNumber { get; set; }
        
    }
}
