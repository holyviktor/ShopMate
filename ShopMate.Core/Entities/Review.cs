using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopMate.Core.Entities
{
    public class Review
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string ProductId { get; set; }
        public bool IsVerified { get; set; }
        public string Text { get; set; }
        public Double Rating { get; set; }
        public User User { get; set; }
        
        public DateTime date { get; set; }
    }
}
