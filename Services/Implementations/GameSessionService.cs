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
        private readonly IRepository<User> _userRepository; // ✅ AGGIUNTO

        public GameSessionService(IRepository<GameSession> gameSessionRepository, IRepository<User> userRepository)
        {
            _gameSessionRepository = gameSessionRepository;
            _userRepository = userRepository; // ✅ AGGIUNTO
        }

        // Crea una nuova sessione di gioco per l'utente specificato e per la modalità indicata.
        public GameSession CreateGameSession(int userId, GameMode mode)
        {
            var user = _userRepository.GetById(userId); // ✅ Ora il repository è disponibile
            if (user == null)
            {
                throw new Exception("User not found.");
            }

            var session = new GameSession
            {
                UserId = userId,
                User = user,  // ✅ Corretto: assegniamo l'utente richiesto
                Mode = mode,
                StartedAt = DateTime.Now,
                Score = 0
            };

            _gameSessionRepository.Add(session);
            _gameSessionRepository.Save();

            return session;
        }

        // Termina la sessione di gioco, impostando la data/ora di fine e salvando le modifiche.
        public void EndGameSession(GameSession session)
        {
            if (session == null) throw new ArgumentNullException(nameof(session));

            // Calcola lo score come il numero di round vinti
            session.Score = session.Rounds.Count(r => r.IsCorrect);

            // Aggiorna il punteggio totale dell'utente
            session.User.Score += session.Score;

            // Imposta la data di fine sessione
            session.EndedAt = DateTime.Now;

            // Salva le modifiche
            _gameSessionRepository.Update(session);
            _userRepository.Update(session.User);
            _gameSessionRepository.Save();
        }


        public IEnumerable<GameSession> GetSessionsByUser(int userId)
        {
            return _gameSessionRepository.GetAll()
                .Where(gs => gs.UserId == userId)
                .OrderByDescending(gs => gs.StartedAt) // Ordina dalla più recente alla più vecchia
                .ToList();
        }
    }
}
