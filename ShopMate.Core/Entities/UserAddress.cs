using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopMate.Core.Entities
{
    public class UserAddress
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int AddressId { get; set; }
        public User User { get; set; }
        public Address Address { get; set; }
        public ICollection<Order> Orders { get; set; }
    }
}
