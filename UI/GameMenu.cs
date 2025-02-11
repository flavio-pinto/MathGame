using Spectre.Console;
using MathGame.Services.Interfaces;
using MathGame.Enums;
using MathGame.Models;

namespace MathGame.UI
{
    public class GameMenu
    {
        private readonly IGameSessionService _gameSessionService;
        private readonly IUserService _userService; 
        private readonly SessionMenu _sessionMenu;

        public GameMenu(IGameSessionService gameSessionService, IUserService userService, SessionMenu sessionMenu)
        {
            _gameSessionService = gameSessionService ?? throw new ArgumentNullException(nameof(gameSessionService));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));  // ✅ Controllo di null
            _sessionMenu = sessionMenu ?? throw new ArgumentNullException(nameof(sessionMenu));
        }

        public void ShowGameMenu(User user)
        {
            while (true)
            {
                AnsiConsole.Clear();
                AnsiConsole.MarkupLine($"[bold green]Welcome, {user.Username}![/] [yellow](Rank: {user.Rank})[/]");
                AnsiConsole.MarkupLine("[bold]Choose a game mode:[/]");

                var choice = AnsiConsole.Prompt(
                    new SelectionPrompt<GameMenuOption>()
                        .Title("[bold]Choose an option:[/]")
                        .AddChoices(GameMenuOption.Addition, GameMenuOption.Subtraction, GameMenuOption.Multiplication,
                                    GameMenuOption.Division, GameMenuOption.Hardcore, GameMenuOption.ViewHistory,
                                    GameMenuOption.ViewLeaderboard,
                                    GameMenuOption.Logout));

                switch (choice)
                {
                    case GameMenuOption.Logout:
                        AnsiConsole.MarkupLine("[yellow]Logged out.[/]");
                        return;

                    case GameMenuOption.ViewHistory:
                        ViewGameHistory(user);
                        continue;

                    case GameMenuOption.ViewLeaderboard:
                        ViewLeaderboard();
                        continue;

                    default:
                        StartGameSession(user, choice);
                        break;
                }
            }
        }


        private void StartGameSession(User user, GameMenuOption mode)
        {
            var gameMode = Enum.Parse<GameMode>(mode.ToString());
            var session = _gameSessionService.CreateGameSession(user.Id, gameMode);
            var currentRank = user.Rank;

            _sessionMenu.PlaySession(session);
            _gameSessionService.EndGameSession(session);

            // Mostra il nuovo rank dopo aver aggiornato il punteggio
            if (user.Rank != currentRank)
            {
                AnsiConsole.MarkupLine($"[bold green]🎉 Congratulations! You've reached rank [yellow]{user.Rank}[/]! 🎉[/]");
                AnsiConsole.MarkupLine("[bold white]Press any key to continue...[/]");
                Console.ReadKey(); // Pausa per far leggere il messaggio
            }

            AnsiConsole.MarkupLine($"[bold green]Game session completed![/]");
            AnsiConsole.MarkupLine("[bold white]Press any key to return to the menu...[/]");
            Console.ReadKey();
        }


        private void ViewGameHistory(User user)
        {
            var sessions = _gameSessionService.GetSessionsByUser(user.Id);

            if (!sessions.Any())
            {
                AnsiConsole.MarkupLine("[yellow]No game history found.[/]");
                AnsiConsole.MarkupLine("[bold white]Press any key to return...[/]");
                Console.ReadKey();
                return;
            }

            var table = new Table();
            table.AddColumn("ID");
            table.AddColumn("Mode");
            table.AddColumn("Score");
            table.AddColumn("Date");

            foreach (var session in sessions)
            {
                table.AddRow(session.Id.ToString(), session.Mode.ToString(), session.Score.ToString(), session.StartedAt.ToString("g"));
            }

            AnsiConsole.Write(table);
            AnsiConsole.MarkupLine("[bold white]Press any key to return...[/]");
            Console.ReadKey();
        }


        private void ViewLeaderboard()
        {
            var users = _userService.GetLeaderboard();

            if (!users.Any())
            {
                AnsiConsole.MarkupLine("[yellow]No users found in leaderboard.[/]");
                return;
            }

            var table = new Table();
            table.AddColumn("[bold]Rank[/]");
            table.AddColumn("[bold]Username[/]");
            table.AddColumn("[bold]Score[/]");

            foreach (var user in users)
            {
                table.AddRow(user.Rank, user.Username, user.Score.ToString());
            }

            AnsiConsole.Write(table);
            AnsiConsole.MarkupLine("[bold white]Press any key to return...[/]");
            Console.ReadKey();
        }
    }
}
