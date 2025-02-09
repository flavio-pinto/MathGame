using Microsoft.EntityFrameworkCore;
using MathGame.Models;

namespace MathGame.Data;

public class MathGameContext : DbContext
{
    // Constructor accepting DbContextOptions for dependency injection
    public MathGameContext(DbContextOptions<MathGameContext> options)
        : base(options)
    {
    }

    // DbSets representing the tables in the database
    public DbSet<User> Users { get; set; }
    public DbSet<GameSession> GameSessions { get; set; }
    public DbSet<Round> Rounds { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer("Server=localhost;Database=biblioteca_leader;Trusted_Connection=True;TrustServerCertificate=True;");
        }
    }

    // Override OnModelCreating to configure model relationships, keys, etc.
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}