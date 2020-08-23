using Microsoft.EntityFrameworkCore;

namespace game_market_API.Models
{
    public class GameMarketDbContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<GameKey> GameKeys { get; set; }
        public DbSet<PaymentSession> PaymentSessions { get; set; }
        public DbSet<Vendor> Vendors { get; set; }
        
        public GameMarketDbContext(DbContextOptions<GameMarketDbContext> options)
            :base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Game>().HasData
            (
                new Game
                {
                    ID = 1,
                    Name = "Dota 2",
                    Price = 99
                },
                new Game
                {
                    ID = 2,
                    Name = "Microsoft Flight Simulator",
                    Price = 4356
                }
            );
        }
    }
} 