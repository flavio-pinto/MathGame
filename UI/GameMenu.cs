using Spectre.Console;
using MathGame.Services.Interfaces;
using MathGame.Enums;
using MathGame.Models;

namespace MathGame.UI
{
    public class GameMenu
    {
        private readonly IGameSessionService _gameSessionService;
        private readonly SessionMenu _sessionMenu;

        public GameMenu(IGameSessionService gameSessionService, SessionMenu sessionMenu)
        {
            _gameSessionService = gameSessionService;
            _sessionMenu = sessionMenu;
        }

        public void ShowGameMenu(User user)
        {
            while (true)
            {
                AnsiConsole.Clear(); // Pulisce la console per una visualizzazione più pulita
                AnsiConsole.MarkupLine($"[bold green]Welcome, {user.Username}![/] [yellow](Rank: {user.Rank})[/]");
                AnsiConsole.MarkupLine("[bold]Choose a game mode:[/]");

                var choice = AnsiConsole.Prompt(
                    new SelectionPrompt<GameMenuOption>()
                        .Title("[bold]Choose an option:[/]")
                        .AddChoices(GameMenuOption.Addition, GameMenuOption.Subtraction, GameMenuOption.Multiplication,
                                    GameMenuOption.Division, GameMenuOption.Hardcore, GameMenuOption.ViewHistory, GameMenuOption.Logout));

                if (choice == GameMenuOption.Logout)
                {
                    AnsiConsole.MarkupLine("[yellow]Logged out.[/]");
                    return;
                }
                else if (choice == GameMenuOption.ViewHistory)
                {
                    ViewGameHistory(user);
                    continue;
                }

                StartGameSession(user, choice);
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
        }
    }
}
