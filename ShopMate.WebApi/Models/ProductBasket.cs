using System.ComponentModel.DataAnnotations;

namespace ShopMate.WebApi.Models
{
    public class ProductBasket
    {
        public string ProductId { get; set; }
        [Range(1, Int32.MaxValue)]
        public int Count { get; set; }
    }
}
