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
            modelBuilder.Entity<Game>().HasData(
                new Game
                {
                    ID = -1,
                    Name = "Dota 2",
                    Price = 99
                },
                new Game
                {
                    ID = -2,
                    Name = "Microsoft Flight Simulator",
                    Price = 4356
                }
            );
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    ID = -1,
                    Username = "Admin",
                    Password = "Admin",
                    Role = User.AdminRole
                }
            );   
        }
    }
} 