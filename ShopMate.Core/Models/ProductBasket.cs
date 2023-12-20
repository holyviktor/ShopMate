using System.ComponentModel.DataAnnotations;

namespace ShopMate.Core.Models
{
    public class ProductBasket
    {
        public string ProductId { get; set; }
        [Range(1, Int32.MaxValue)]
        public int Number { get; set; }
    }
}
