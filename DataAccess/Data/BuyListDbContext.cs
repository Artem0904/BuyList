using System.Reflection;
using BusinessLogic.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Data
{
    public class BuyListDbContext(DbContextOptions options) : IdentityDbContext<BotUser, IdentityRole<int>, int>(options)
    {
        public DbSet<BotUser> BotUsers { get; set; }
        public DbSet<Purchase> Purchases { get; set; }
        public DbSet<Balance> Balances { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
        
    }
}
