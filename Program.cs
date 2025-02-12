using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using MathGame.Data;
using MathGame.Repositories;
using MathGame.Services.Interfaces;
using MathGame.Services.Implementations;
using MathGame.UI;
using MathGame.Seeding;

var builder = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((context, config) =>
    {
        config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true); // ✅ Aggiunto per leggere appsettings.json
    })
    .ConfigureServices((context, services) =>
    {
        // Configura il database con Entity Framework
        services.AddDbContext<MathGameContext>(options =>
            options.UseSqlServer("Server=localhost;Database=math_game;Trusted_Connection=True;TrustServerCertificate=True;"));

        // Configura i repository
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IGameSessionService, GameSessionService>();
        services.AddScoped<IRoundService, RoundService>();

        // Configura i menu dell'interfaccia utente
        services.AddScoped<AuthMenu>();
        services.AddScoped<GameMenu>();
        services.AddScoped<SessionMenu>();
        services.AddScoped<UIManager>();
        services.AddSingleton<VoskSpeechRecognitionService>();
    })
    .Build();

// Avvia l'applicazione
await using (var scope = builder.Services.CreateAsyncScope()) // 👈 Usa CreateAsyncScope()
{
    var services = scope.ServiceProvider;

    // Ottieni il contesto del database
    var dbContext = services.GetRequiredService<MathGameContext>();

    // ✅ Esegui il seeding dei dati fake
    DatabaseSeeder.SeedDatabase(dbContext);

    // Avvia il menu UIManager
    var uiManager = services.GetRequiredService<UIManager>();
    await uiManager.Run(); // ✅ Ora funziona correttamente
}