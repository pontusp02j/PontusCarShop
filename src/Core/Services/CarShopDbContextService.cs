using Core.Entities.Cars;
using Core.Entities.Users;
using Core.Entities.Subscriptions;
using Microsoft.EntityFrameworkCore;
using Core.Entities.Security;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shop.Core.Services
{
    public class CarShopDbContext : DbContext
    {

        public CarShopDbContext(DbContextOptions<CarShopDbContext> options) : base(options)
        {

        }

        public DbSet<Car> Cars { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<FailedLoginAttempt> FailedLoginAttempts  { get; set; }
        public DbSet<RestrictedIpAddress> RestrictedIpAddresses { get; set; }
    }
}