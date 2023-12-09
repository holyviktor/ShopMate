using Microsoft.EntityFrameworkCore;
using ShopMate.Core.Entities;


namespace ShopMate.Infrastructure.Data
{
    public class ShopMateDbContext : DbContext
    {
        public ShopMateDbContext(DbContextOptions options) : base(options) { }

        public ShopMateDbContext() { }
        public DbSet<Basket> Baskets { get; set; }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<Coupon> Coupons { get; set; }
        public DbSet<Favourite> Favourites { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderProduct> OrderProducts { get; set; }
        public DbSet<Participant> Participants { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<User> Users { get;set; }
        /*public DbSet<Status> Statuses { get; set; }*/
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserAddress> UserAddresses { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ShopMateDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
