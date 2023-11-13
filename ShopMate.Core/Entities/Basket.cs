using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopMate.Core.Entities
{
    public class Basket
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string ProductId { get; set; }
        public int Number { get; set; }
        public User User { get; set; }

    }
}
