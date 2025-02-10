using Bogus;
using MathGame.Data;
using MathGame.Enums;
using MathGame.Models;

namespace MathGame.Seeding
{
    public static class DatabaseSeeder
    {
        public static void SeedDatabase(MathGameContext context)
        {
            if (context.Users.Any()) return; // ✅ Evita duplicati

            var userFaker = new Faker<User>()
                .RuleFor(u => u.Username, f => f.Internet.UserName())
                .RuleFor(u => u.PasswordHash, (f, u) => u.Username) // ✅ Password = Username
                .RuleFor(u => u.Score, 0); // Inizialmente 0, poi lo aggiorniamo

            var users = userFaker.Generate(10); // ✅ Genera 10 utenti fake

            context.Users.AddRange(users);
            context.SaveChanges();

            var random = new Random();
            foreach (var user in users)
            {
                var sessionFaker = new Faker<GameSession>()
                    .RuleFor(gs => gs.UserId, user.Id)
                    .RuleFor(gs => gs.Mode, f => f.PickRandom<GameMode>())
                    .RuleFor(gs => gs.StartedAt, f => f.Date.Past(1))
                    .RuleFor(gs => gs.EndedAt, f => f.Date.Recent(30))
                    .RuleFor(gs => gs.Score, f => f.Random.Int(5, 20));

                var sessions = sessionFaker.Generate(random.Next(2, 5));

                context.GameSessions.AddRange(sessions);
                context.SaveChanges();

                foreach (var session in sessions)
                {
                    var roundCount = session.Mode == GameMode.Hardcore ? 20 : 5; // ✅ Hardcore ha 20 round

                    var roundFaker = new Faker<Round>()
                        .RuleFor(r => r.GameSessionId, session.Id)
                        .RuleFor(r => r.RoundNumber, f => f.IndexFaker + 1)
                        .RuleFor(r => r.Number1, f => f.Random.Double(1, 100))
                        .RuleFor(r => r.Number2, f => f.Random.Double(1, 100))
                        .RuleFor(r => r.Operation, f => session.Mode == GameMode.Hardcore
                            ? f.PickRandom<MathOperation>()
                            : GetOperationFromGameMode(session.Mode))
                        .RuleFor(r => r.CorrectAnswer, (f, r) => r.Operation switch
                        {
                            MathOperation.Addition => r.Number1 + r.Number2,
                            MathOperation.Subtraction => r.Number1 - r.Number2,
                            MathOperation.Multiplication => r.Number1 * r.Number2,
                            MathOperation.Division => r.Number2 == 0 ? 1 : r.Number1 / r.Number2,
                            _ => 0
                        })
                        .RuleFor(r => r.UserAnswer, f => f.Random.Bool() ? f.Random.Double(1, 200) : (double?)null)
                        .RuleFor(r => r.IsCorrect, (f, r) => r.UserAnswer == r.CorrectAnswer)
                        .RuleFor(r => r.TimeTaken, f => TimeSpan.FromSeconds(f.Random.Int(5, 30)));

                    var rounds = roundFaker.Generate(roundCount);
                    context.Rounds.AddRange(rounds);
                }

                // ✅ Aggiorna lo score totale dell'utente in base alle sue partite
                user.Score = context.GameSessions
                    .Where(gs => gs.UserId == user.Id)
                    .Sum(gs => gs.Score);
            }

            context.SaveChanges();
            Console.WriteLine("✅ Database populated with fake data, and users' scores updated!");
        }

        // Metodo helper per assegnare l'operazione corretta in base alla modalità di gioco
        private static MathOperation GetOperationFromGameMode(GameMode mode) => mode switch
        {
            GameMode.Addition => MathOperation.Addition,
            GameMode.Subtraction => MathOperation.Subtraction,
            GameMode.Multiplication => MathOperation.Multiplication,
            GameMode.Division => MathOperation.Division,
            _ => MathOperation.Addition
        };
    }
}
