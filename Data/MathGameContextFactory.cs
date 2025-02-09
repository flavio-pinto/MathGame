using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace MathGame.Data;

public class MathGameContextFactory : IDesignTimeDbContextFactory<MathGameContext>
{
    public MathGameContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<MathGameContext>();
        optionsBuilder.UseSqlServer("Server=localhost;Database=math_game;Trusted_Connection=True;TrustServerCertificate=True;");

        return new MathGameContext(optionsBuilder.Options);
    }
}