using Spectre.Console;
using MathGame.Services.Interfaces;
using MathGame.Enums;
using MathGame.Models;

namespace MathGame.UI
{
    public class SessionMenu
    {
        private readonly IRoundService _roundService;

        public SessionMenu(IRoundService roundService)
        {
            _roundService = roundService;
        }

        public void PlaySession(GameSession session)
        {
            int totalRounds = session.Mode == GameMode.Hardcore ? 20 : 5;

            for (int roundNumber = 1; roundNumber <= totalRounds; roundNumber++)
            {
                PlayRound(session.Id, roundNumber);
            }
        }

        private void PlayRound(int sessionId, int roundNumber)
        {
            var round = _roundService.CreateRound(sessionId, roundNumber);
            var answer = AnsiConsole.Ask<int>($"{round.Number1} {GetOperatorSymbol(round.Operation)} {round.Number2} = ?");

            bool isCorrect = _roundService.ValidateAnswer(round, answer);
            if (isCorrect)
                AnsiConsole.MarkupLine("[green]Correct![/]");
            else
                AnsiConsole.MarkupLine($"[red]Wrong! The correct answer was {round.CorrectAnswer}.[/]");
        }

        private string GetOperatorSymbol(MathOperation operation) => operation switch
        {
            MathOperation.Addition => "+",
            MathOperation.Subtraction => "-",
            MathOperation.Multiplication => "*",
            MathOperation.Division => "/",
            _ => "?"
        };
    }
}
