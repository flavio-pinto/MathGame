using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MathGame.Models;

namespace MathGame.Data
{
    public class MathGameContext : DbContext
    {
        private static readonly ILoggerFactory _loggerFactory = LoggerFactory.Create(builder => { builder.ClearProviders(); });
        public MathGameContext(DbContextOptions<MathGameContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<GameSession> GameSessions { get; set; }
        public DbSet<Round> Rounds { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            modelBuilder.Entity<GameSession>()
                .HasOne(gs => gs.User)
                .WithMany(u => u.GameSessions)
                .HasForeignKey(gs => gs.UserId);

            modelBuilder.Entity<Round>()
                .HasOne(r => r.GameSession)
                .WithMany(gs => gs.Rounds)
                .HasForeignKey(r => r.GameSessionId);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=localhost;Database=math_game;Trusted_Connection=True;TrustServerCertificate=True;")
                              .EnableSensitiveDataLogging(false); // Evita log con dati sensibili, ma senza gestire direttamente LogTo()
            }
        }
    }
}
