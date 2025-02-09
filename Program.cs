using MathGame;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MathGame.Data;
using MathGame.Repositories;
using MathGame.Services.Interfaces;
using MathGame.Services.Implementations;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        // Registrazione del DbContext. Qui utilizziamo il metodo OnConfiguring già definito in MathGameContext.
        services.AddDbContext<MathGameContext>();

        // Registrazione del repository generico
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

        // Registrazione dei servizi di business
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IGameSessionService, GameSessionService>();
        services.AddScoped<IRoundService, RoundService>();

        // Eventuali altre registrazioni (es. logging, configurazioni custom, ecc.)
    })
    .Build();

// Creiamo uno scope per utilizzare i servizi registrati
using (var scope = host.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    // Esempio di recupero e utilizzo di un servizio
    var userService = services.GetRequiredService<IUserService>();

    // Qui potresti avviare il flusso della tua applicazione,
    // ad esempio, gestendo login, registrazione, menu interattivi, ecc.
    // Oppure, se preferisci, puoi passare il controllo a un'apposita classe di orchestrazione.
}

host.Run();  // Se necessario, oppure termina l'applicazione dopo l'esecuzione del flusso.
