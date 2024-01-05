using Microsoft.AspNetCore.Mvc;

namespace ShopMate.WebApi.Models
{
    [BindProperties]
    public class CreateOrder
    {
        public string[] ProductsId { get; set; }
    }
}
