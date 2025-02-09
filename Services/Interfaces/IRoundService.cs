using MathGame.Models;

namespace MathGame.Services.Interfaces
{
    public interface IRoundService
    {
        // Initializes a new round for a given game session.
        Round CreateRound(int gameSessionId, int roundNumber);

        // Validates the user's answer for a given round, updating the round's state.
        bool ValidateAnswer(Round round, int userAnswer);

        // Optionally, other methods specific to round management can be added here.
    }
}
