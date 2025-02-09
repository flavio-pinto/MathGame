using System;
using System.Linq;
using MathGame.Models;
using MathGame.Enums;
using MathGame.Repositories;
using MathGame.Services.Interfaces;

namespace MathGame.Services.Implementations
{
    public class RoundService : IRoundService
    {
        private readonly IRepository<Round> _roundRepository;
        private readonly IRepository<GameSession> _gameSessionRepository;
        private readonly Random _random;

        public RoundService(IRepository<Round> roundRepository, IRepository<GameSession> gameSessionRepository)
        {
            _roundRepository = roundRepository;
            _gameSessionRepository = gameSessionRepository;
            _random = new Random();
        }

        // Crea un nuovo round per la sessione di gioco specificata.
        public Round CreateRound(int gameSessionId, int roundNumber)
        {
            // Recupera la sessione di gioco per determinare la modalità.
            var gameSession = _gameSessionRepository.GetById(gameSessionId);
            if (gameSession == null)
            {
                throw new Exception("Game session not found");
            }

            MathOperation operation;
            if (gameSession.Mode == GameMode.Hardcore)
            {
                // In modalità hardcore si impostano le operazioni in sequenza:
                // 1-5: Addition, 6-10: Subtraction, 11-15: Multiplication, 16-20: Division.
                if (roundNumber >= 1 && roundNumber <= 5)
                {
                    operation = MathOperation.Addition;
                }
                else if (roundNumber >= 6 && roundNumber <= 10)
                {
                    operation = MathOperation.Subtraction;
                }
                else if (roundNumber >= 11 && roundNumber <= 15)
                {
                    operation = MathOperation.Multiplication;
                }
                else if (roundNumber >= 16 && roundNumber <= 20)
                {
                    operation = MathOperation.Division;
                }
                else
                {
                    throw new Exception("Invalid round number for Hardcore mode");
                }
            }
            else
            {
                // Per modalità non hardcore, l'operazione viene derivata dalla modalità impostata.
                operation = (MathOperation)Enum.Parse(typeof(MathOperation), gameSession.Mode.ToString());
            }

            int number1 = 0, number2 = 0;
            double correctAnswer = 0;

            // Genera i numeri e calcola la risposta corretta in base all'operazione.
            switch (operation)
            {
                case MathOperation.Addition:
                    number1 = _random.Next(1, 101);
                    number2 = _random.Next(1, 101);
                    correctAnswer = number1 + number2;
                    break;
                case MathOperation.Subtraction:
                    number1 = _random.Next(1, 101);
                    number2 = _random.Next(1, number1 + 1); // per evitare risultati negativi
                    correctAnswer = number1 - number2;
                    break;
                case MathOperation.Multiplication:
                    number1 = _random.Next(1, 21);
                    number2 = _random.Next(1, 21);
                    correctAnswer = number1 * number2;
                    break;
                case MathOperation.Division:
                    // Per garantire un risultato intero, generiamo un divisore e poi moltiplichiamo per un fattore.
                    number2 = _random.Next(1, 21); // divisore (non zero)
                    int factor = _random.Next(1, 21);
                    number1 = number2 * factor;
                    correctAnswer = number1 / number2;
                    break;
                default:
                    throw new Exception("Unsupported operation");
            }

            var round = new Round
            {
                GameSessionId = gameSessionId,
                RoundNumber = roundNumber,
                Number1 = number1,
                Number2 = number2,
                Operation = operation,
                CorrectAnswer = correctAnswer,
                IsCorrect = false,
                TimeTaken = null
            };

            _roundRepository.Add(round);
            _roundRepository.Save();

            return round;
        }

        // Valida la risposta fornita dall'utente per il round specificato.
        public bool ValidateAnswer(Round round, int userAnswer)
        {
            bool isCorrect = ((int)round.CorrectAnswer) == userAnswer;
            round.IsCorrect = isCorrect;
            round.UserAnswer = userAnswer;

            _roundRepository.Update(round);
            _roundRepository.Save();

            return isCorrect;
        }
    }
}
