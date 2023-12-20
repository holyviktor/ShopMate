namespace ShopMate.WebApi.Models;

public class UserToken
{
    public UserToken(string token)
    {
        this.token = token;
    }

    public String token { get; set; }

}