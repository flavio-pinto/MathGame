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

        public async Task Run()
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
                        await _authMenu.LoginUser();
                        break;
                    case MainMenuOption.Exit:
                        return;
                }
            }
        }
    }
}