using Spectre.Console;
using MathGame.Services.Interfaces;
using MathGame.Models;

namespace MathGame.UI
{
    public class AuthMenu
    {
        private readonly IUserService _userService;
        private readonly GameMenu _gameMenu;
        private User? _loggedInUser;

        public AuthMenu(IUserService userService, GameMenu gameMenu)
        {
            _userService = userService;
            _gameMenu = gameMenu;
        }

        public void RegisterUser()
        {
            AnsiConsole.MarkupLine("[bold cyan]Register a new account[/]");
            string username = AnsiConsole.Ask<string>("Enter username:");
            string password = AnsiConsole.Prompt(new TextPrompt<string>("Enter password:").Secret());

            try
            {
                _userService.RegisterUser(username, password);
                AnsiConsole.MarkupLine("[green]Registration successful! You can now log in.[/]");
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]Error: {ex.Message}[/]");
            }
        }

        public void LoginUser()
        {
            AnsiConsole.MarkupLine("[bold cyan]Login[/]");
            string username = AnsiConsole.Ask<string>("Enter username:");
            string password = AnsiConsole.Prompt(new TextPrompt<string>("Enter password:").Secret());

            var user = _userService.LoginUser(username, password);
            if (user != null)
            {
                _loggedInUser = user;
                AnsiConsole.MarkupLine("[green]Login successful![/]");
                _gameMenu.ShowGameMenu(_loggedInUser);
            }
            else
            {
                AnsiConsole.MarkupLine("[red]Invalid username or password.[/]");
            }
        }
    }
}
