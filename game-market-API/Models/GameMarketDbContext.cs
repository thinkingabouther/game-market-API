using Microsoft.EntityFrameworkCore;

namespace game_market_API.Models
{
    public class GameMarketDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<GameKey> GameKeys { get; set; }
        public DbSet<PaymentSession> PaymentSessions { get; set; }
        
        public GameMarketDbContext(DbContextOptions<GameMarketDbContext> options)
            :base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            Seed(modelBuilder);
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique(true);
        }

        private void Seed(ModelBuilder modelBuilder)
        {
            var sampleUser = new User
            {
                ID = -2,
                Username = "Vasya",
                Password = "12345",
                Role = User.VendorRole
            };
            modelBuilder.Entity<User>().HasData(sampleUser);   
            modelBuilder.Entity<Game>().HasData(
                new Game
                {
                    ID = -1,
                    Name = "Dota 2",
                    Price = 99,
                    VendorID = sampleUser.ID
                },
                new Game
                {
                    ID = -2,
                    Name = "Microsoft Flight Simulator",
                    Price = 4356,
                    VendorID = sampleUser.ID
                }
            );
        }
    }
} 