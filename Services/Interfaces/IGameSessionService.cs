using MathGame.Models;
using MathGame.Enums;
using System.Collections.Generic;

namespace MathGame.Services.Interfaces
{
    public interface IGameSessionService
    {
        // Crea partita per l'utente specificato e per la modalità indicata.
        GameSession CreateGameSession(int userId, GameMode mode);

        void EndGameSession(GameSession session);

        IEnumerable<GameSession> GetSessionsByUser(int userId);
    }
}
