using MathGame.Models;
using MathGame.Enums;
using System.Collections.Generic;

namespace MathGame.Services.Interfaces
{
    public interface IGameSessionService
    {
        // Creates a new game session for the specified user and game mode.
        GameSession CreateGameSession(int userId, GameMode mode);

        // Ends the provided game session (e.g., setting the EndedAt and final score).
        void EndGameSession(GameSession session);

        // Retrieves all game sessions associated with a given user.
        IEnumerable<GameSession> GetSessionsByUser(int userId);
    }
}
