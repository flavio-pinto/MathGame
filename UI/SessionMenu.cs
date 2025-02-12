using Spectre.Console;
using MathGame.Services.Interfaces;
using MathGame.Enums;
using MathGame.Models;
using MathGame.Services.Implementations;
using MathGame.Utilities;

namespace MathGame.UI
{
    public class SessionMenu
    {
        private readonly IRoundService _roundService;
        private readonly VoskSpeechRecognitionService _speechRecognitionService;

        public SessionMenu(IRoundService roundService, VoskSpeechRecognitionService speechRecognitionService)
        {
            _roundService = roundService;
            _speechRecognitionService = speechRecognitionService;
        }

        public async Task PlaySession(GameSession session)
        {
            Console.WriteLine($"🔁 Modalità di gioco: {session.Mode}");
            Console.WriteLine($"🎮 Partita avviata con {session.Id}, {session.Mode}, {session.UserId}");

            int totalRounds = session.Mode == GameMode.Hardcore ? 20 : 5;

            for (int roundNumber = 1; roundNumber <= totalRounds; roundNumber++)
            {
                Console.WriteLine($"🔢 Round {roundNumber}/{totalRounds} in corso...");
                await PlayRound(session.Id, roundNumber);
            }

            Console.WriteLine("🏁 Sessione di gioco terminata!");
            Console.WriteLine("🔄 Premere un tasto per tornare al menu...");
            Console.ReadKey(); // Impedisce il ritorno immediato al menu
        }

        private async Task PlayRound(int sessionId, int roundNumber)
        {
            var round = _roundService.CreateRound(sessionId, roundNumber);

            // ✅ Mostra prima l'operazione da risolvere
            Console.WriteLine($"{round.Number1} {GetOperatorSymbol(round.Operation)} {round.Number2} = ?");

            Console.WriteLine($"🎤 Rispondi con la voce? (y/n)");
            string? useVoice = Console.ReadLine()?.Trim().ToLower();

            string response = string.Empty;

            if (useVoice == "y")
            {
                Console.WriteLine("🎙️ Registrazione in corso...");
                response = await _speechRecognitionService.RecordAndTranscribeAsync();

                Console.WriteLine($"📝 Risultato della trascrizione: '{response}'");

                if (string.IsNullOrWhiteSpace(response))
                {
                    Console.WriteLine("⚠️ Nessuna risposta vocale ricevuta, riprova.");
                    Console.ReadKey();
                    return;
                }

                // 🔄 Converti la risposta vocale in numero
                int? convertedNumber = NumberConverter.ConvertWordToNumber(response);
                if (convertedNumber.HasValue)
                {
                    response = convertedNumber.Value.ToString();
                }
            }
            else
            {
                Console.Write("✏️ Digita la tua risposta: "); // ✅ Feedback visivo per input manuale
                response = Console.ReadLine() ?? string.Empty;
            }

            if (int.TryParse(response, out int userAnswer))
            {
                bool isCorrect = _roundService.ValidateAnswer(round, userAnswer);
                Console.WriteLine(isCorrect ? "✅ Correct!" : $"❌ Wrong! The correct answer was {round.CorrectAnswer}.");
            }
            else
            {
                Console.WriteLine("❌ Input non valido.");
            }
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
