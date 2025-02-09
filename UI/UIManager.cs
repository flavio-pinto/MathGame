//using Spectre.Console;
//using MathGame.Services.Interfaces;
//using MathGame.Enums;

//namespace MathGame.UI
//{
//    public class UIManager
//    {
//        private readonly IUserService _userService;
//        private readonly IGameSessionService _gameSessionService;
//        private readonly IRoundService _roundService;
//        private User? _loggedInUser;

//        public UIManager(IUserService userService, IGameSessionService gameSessionService, IRoundService roundService)
//        {
//            _userService = userService;
//            _gameSessionService = gameSessionService;
//            _roundService = roundService;
//        }

//        public void Run()
//        {
//            while (true)
//            {
//                var choice = AnsiConsole.Prompt(
//                    new SelectionPrompt<MainMenuOption>()
//                        .Title("[bold]Welcome to Math Game![/] Choose an option:")
//                        .AddChoices(Enum.GetValues<MainMenuOption>()));

//                switch (choice)
//                {
//                    case MainMenuOption.Register:
//                        RegisterUser();
//                        break;
//                    case MainMenuOption.Login:
//                        LoginUser();
//                        break;
//                    case MainMenuOption.Exit:
//                        return;
//                }
//            }
//        }

//        private void RegisterUser()
//        {
//            AnsiConsole.MarkupLine("[bold cyan]Register a new account[/]");
//            string username = AnsiConsole.Ask<string>("Enter username:");
//            string password = AnsiConsole.Prompt(new TextPrompt<string>("Enter password:").Secret());

//            try
//            {
//                _userService.RegisterUser(username, password);
//                AnsiConsole.MarkupLine("[green]Registration successful! You can now log in.[/]");
//            }
//            catch (Exception ex)
//            {
//                AnsiConsole.MarkupLine($"[red]Error: {ex.Message}[/]");
//            }
//        }

//        private void LoginUser()
//        {
//            AnsiConsole.MarkupLine("[bold cyan]Login[/]");
//            string username = AnsiConsole.Ask<string>("Enter username:");
//            string password = AnsiConsole.Prompt(new TextPrompt<string>("Enter password:").Secret());

//            var user = _userService.LoginUser(username, password);
//            if (user != null)
//            {
//                _loggedInUser = user;
//                AnsiConsole.MarkupLine("[green]Login successful![/]");
//                ShowGameMenu();
//            }
//            else
//            {
//                AnsiConsole.MarkupLine("[red]Invalid username or password.[/]");
//            }
//        }

//        private void ShowGameMenu()
//        {
//            while (true)
//            {
//                var choice = AnsiConsole.Prompt(
//                    new SelectionPrompt<GameMenuOption>()
//                        .Title("[bold]Choose a game mode:[/]")
//                        .AddChoices(Enum.GetValues<GameMenuOption>()));

//                if (choice == GameMenuOption.Logout)
//                {
//                    _loggedInUser = null;
//                    AnsiConsole.MarkupLine("[yellow]Logged out.[/]");
//                    return;
//                }

//                StartGameSession(choice);
//            }
//        }

//        private void StartGameSession(GameMenuOption mode)
//        {
//            if (_loggedInUser == null) return;

//            var gameMode = Enum.Parse<GameMode>(mode.ToString());
//            var session = _gameSessionService.CreateGameSession(_loggedInUser.Id, gameMode);

//            for (int roundNumber = 1; roundNumber <= (gameMode == GameMode.Hardcore ? 20 : 5); roundNumber++)
//            {
//                PlayRound(session.Id, roundNumber);
//            }

//            _gameSessionService.EndGameSession(session);
//            AnsiConsole.MarkupLine("[bold green]Game session completed![/]");
//        }

//        private void PlayRound(int sessionId, int roundNumber)
//        {
//            var round = _roundService.CreateRound(sessionId, roundNumber);
//            var answer = AnsiConsole.Ask<int>($"{round.Number1} {GetOperatorSymbol(round.Operation)} {round.Number2} = ?");

//            bool isCorrect = _roundService.ValidateAnswer(round, answer);
//            if (isCorrect)
//                AnsiConsole.MarkupLine("[green]Correct![/]");
//            else
//                AnsiConsole.MarkupLine($"[red]Wrong! The correct answer was {round.CorrectAnswer}.[/]");
//        }

//        private string GetOperatorSymbol(MathOperation operation) => operation switch
//        {
//            MathOperation.Addition => "+",
//            MathOperation.Subtraction => "-",
//            MathOperation.Multiplication => "*",
//            MathOperation.Division => "/",
//            _ => "?"
//        };
//    }
//}

using Spectre.Console;
using MathGame.Services.Interfaces;
using MathGame.Enums;

namespace MathGame.UI
{
    public class UIManager
    {
        private readonly AuthMenu _authMenu;
        private readonly GameMenu _gameMenu;

        public UIManager(AuthMenu authMenu, GameMenu gameMenu)
        {
            _authMenu = authMenu;
            _gameMenu = gameMenu;
        }

        public void Run()
        {
            while (true)
            {
                var choice = AnsiConsole.Prompt(
                    new SelectionPrompt<MainMenuOption>()
                        .Title("[bold]Welcome to Math Game![/] Choose an option:")
                        .AddChoices(Enum.GetValues<MainMenuOption>()));

                switch (choice)
                {
                    case MainMenuOption.Register:
                        _authMenu.RegisterUser();
                        break;
                    case MainMenuOption.Login:
                        _authMenu.LoginUser();
                        break;
                    case MainMenuOption.Exit:
                        return;
                }
            }
        }
    }
}
