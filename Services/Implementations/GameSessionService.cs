using System;
using System.Collections.Generic;
using System.Linq;
using MathGame.Models;
using MathGame.Enums;
using MathGame.Repositories;
using MathGame.Services.Interfaces;

namespace MathGame.Services.Implementations
{
    public class GameSessionService : IGameSessionService
    {
        private readonly IRepository<GameSession> _gameSessionRepository;

        public GameSessionService(IRepository<GameSession> gameSessionRepository)
        {
            _gameSessionRepository = gameSessionRepository;
        }

        // Crea una nuova sessione di gioco per l'utente specificato e per la modalità indicata.
        public GameSession CreateGameSession(int userId, GameMode mode)
        {
            var session = new GameSession
            {
                UserId = userId,
                Mode = mode,
                StartedAt = DateTime.Now,
                Score = 0
                // EndedAt rimane null, da impostare al termine della sessione.
            };

            _gameSessionRepository.Add(session);
            _gameSessionRepository.Save();

            return session;
        }

        // Termina la sessione di gioco, impostando la data/ora di fine e salvando le modifiche.
        public void EndGameSession(GameSession session)
        {
            session.EndedAt = DateTime.Now;
            _gameSessionRepository.Update(session);
            _gameSessionRepository.Save();
        }

        // Restituisce tutte le sessioni di gioco associate all'utente specificato.
        public IEnumerable<GameSession> GetSessionsByUser(int userId)
        {
            return _gameSessionRepository.GetAll().Where(s => s.UserId == userId);
        }
    }
}
