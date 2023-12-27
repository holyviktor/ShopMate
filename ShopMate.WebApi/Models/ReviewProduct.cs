namespace ShopMate.WebApi.Models;

public class ReviewProduct
{
    public ReviewProduct(double grade, string productId)
    {
        this.grade = grade;
        this.productId = productId;
    }

    public string productId { get; set; }
    public double grade {
        get;
        set;
    }
    public override string ToString()
    {
        return productId + "   " + grade;
    }
}