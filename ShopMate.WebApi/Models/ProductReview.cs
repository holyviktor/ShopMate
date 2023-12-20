using ShopMate.Core.Entities;

namespace ShopMate.WebApi.Models;

public class ProductReview
{
    public int Id { get; set; }
    public bool IsVerified { get; set; }
    public string ProductId { get; set; }
    public string Text { get; set; }
    public Double Rating { get; set; }
    public UserForReview UserForReview { get; set; }
}