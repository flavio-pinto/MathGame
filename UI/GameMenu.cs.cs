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
                var choice = AnsiConsole.Prompt(
                    new SelectionPrompt<GameMenuOption>()
                        .Title("[bold]Choose a game mode:[/]")
                        .AddChoices(Enum.GetValues<GameMenuOption>()));

                if (choice == GameMenuOption.Logout)
                {
                    AnsiConsole.MarkupLine("[yellow]Logged out.[/]");
                    return;
                }

                StartGameSession(user, choice);
            }
        }

        private void StartGameSession(User user, GameMenuOption mode)
        {
            var gameMode = Enum.Parse<GameMode>(mode.ToString());
            var session = _gameSessionService.CreateGameSession(user.Id, gameMode);

            _sessionMenu.PlaySession(session);

            _gameSessionService.EndGameSession(session);
            AnsiConsole.MarkupLine("[bold green]Game session completed![/]");
        }
    }
}
