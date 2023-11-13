using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ShopMate.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ShopMate.Infrastructure
{
    public static class RegistrationExtensions
    {
        public static void AddStorage(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.AddDbContext<ShopMateDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("ShopMateDbContext"));
            });
        }
    }
}
