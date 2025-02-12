using MathGame.Models;

namespace MathGame.Services.Interfaces
{
    public interface IRoundService
    {
        // Inizia un nuovo round per la sessione di gioco specificata.
        Round CreateRound(int gameSessionId, int roundNumber);

        // Valida la risposta dell'utente per il round specificato, aggiornando lo stato del round.
        bool ValidateAnswer(Round round, int userAnswer);
    }
}
