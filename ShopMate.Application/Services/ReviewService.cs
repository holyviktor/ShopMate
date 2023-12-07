using Microsoft.EntityFrameworkCore;
using ShopMate.Core.Entities;
using ShopMate.Infrastructure.Data;

namespace ShopMate.Application.Services;

public class ReviewService
{
    private readonly ShopMateDbContext _dbContext;

    public ReviewService(ShopMateDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Review>> GetAllReviews(string productId)
    {
        var client = new HttpClient();
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri("https://dummyjson.com/products/" + productId),
        };
        var response = await client.SendAsync(request);
        try
        {
            response.EnsureSuccessStatusCode();
        }
        catch (HttpRequestException)
        {
            throw new Exception(response.StatusCode.ToString());
        }

        var reviews = _dbContext.Reviews.Where(x => x.ProductId == productId).ToList();
        return await Task.FromResult(reviews);
    }

    public async Task<double> GetRating(string productId)
    {
        var client = new HttpClient();
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri("https://dummyjson.com/products/" + productId),
        };
        var response = await client.SendAsync(request);
        try
        {
            response.EnsureSuccessStatusCode();
        }
        catch (HttpRequestException)
        {
            throw new Exception(response.StatusCode.ToString());
        }

        var reviews = _dbContext.Reviews.Where(x => x.ProductId == productId).ToList();
        double sumRating = 0;
        foreach (var review in reviews)
        {
            sumRating += review.Rating;
        }

        return sumRating / reviews.Count;
    }

    public async Task<List<double>> GetListRating(string[] productsId)
    {
        List<double> listRating = new List<double>();
        foreach (var productId in productsId)
        {
            listRating.Add(await GetRating(productId));
        }

        return listRating;
    }

    public async Task Add(int userId, string productId, string text, double rating)
    {
        var client = new HttpClient();
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri("https://dummyjson.com/products/" + productId),
        };
        var response = await client.SendAsync(request);
        try
        {
            response.EnsureSuccessStatusCode();
        }
        catch (HttpRequestException)
        {
            throw new Exception(response.StatusCode.ToString());
        }

        var countReviews = _dbContext.Reviews.Count(x => x.UserId == userId && x.ProductId == productId);


        if (countReviews < 5)
        {
            var review = new Review()
            {
                UserId = userId,
                ProductId = productId,
                Text = text,
                Rating = rating,
                IsVerified = IsVerified(userId, productId)
            };
            _dbContext.Reviews.Add(review);
        }
        else
        {
            throw new InvalidOperationException("ThereAreAlready5Reviews");
        }

        await _dbContext.SaveChangesAsync();
    }

    private bool IsVerified(int userId, string productId)
    {
        var orderProd = _dbContext.OrderProducts
            .Where(x => x.ProductId == productId)
            .SingleOrDefault(x => x.Order.UserAddress.UserId == userId);

        return orderProd != null ? true : false;
    }

    public async Task Delete(int userId, int id)
    {
        var review = _dbContext.Reviews.Where(x => x.UserId == userId)
            .SingleOrDefault(x => x.Id == id);
        if (review == null)
        {
            throw new InvalidOperationException("ReviewNotFound");
        }
        else
        {
            _dbContext.Reviews.Remove(review);
        }

        await _dbContext.SaveChangesAsync();
    }
}