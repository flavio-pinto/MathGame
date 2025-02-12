using System;
using System.IO;
using System.Threading.Tasks;
using MathGame.Utilities;
using NAudio.Wave;
using Vosk;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MathGame.Services.Implementations
{
    public class VoskSpeechRecognitionService
    {
        private readonly Model _model;
        private static readonly string ModelPath = Path.Combine(Directory.GetCurrentDirectory(), "Models", "vosk-model-small-it-0.22");

        public VoskSpeechRecognitionService()
        {
            if (!Directory.Exists(ModelPath))
                throw new DirectoryNotFoundException($"Model directory not found: {ModelPath}");

            _model = new Model(ModelPath);
        }

        public async Task<string> RecordAndTranscribeAsync()
        {
            string filePath = "recorded_audio.wav";
            Console.WriteLine("🎤 Parla ora! Premi INVIO per terminare...");

            await RecordAudioAsync(filePath);

            Console.WriteLine("🔍 Tentativo di trascrizione...");
            string transcription = TranscribeAudio(filePath);

            Console.WriteLine($"📝 Trascrizione ottenuta: '{transcription}'"); // 👈 Verifica cosa viene trascritto

            File.Delete(filePath); // Cancella il file dopo l'uso
            return transcription;
        }

        private async Task RecordAudioAsync(string filePath)
        {
            using var waveIn = new WaveInEvent
            {
                WaveFormat = new WaveFormat(16000, 1) // 16kHz, Mono
            };

            using var writer = new WaveFileWriter(filePath, waveIn.WaveFormat);
            waveIn.DataAvailable += (sender, e) => writer.Write(e.Buffer, 0, e.BytesRecorded);

            Console.WriteLine("🎤 Inizio registrazione! Parla ora e premi INVIO per terminare...");

            waveIn.StartRecording();

            await Task.Run(() =>
            {
                while (!Console.KeyAvailable || Console.ReadKey(true).Key != ConsoleKey.Enter)
                {
                    Task.Delay(100).Wait(); // Attendi senza bloccare
                }
            });

            waveIn.StopRecording();
            Console.WriteLine("✅ Registrazione completata!");
        }

        private string TranscribeAudio(string filePath)
        {
            using var recognizer = new VoskRecognizer(_model, 16000.0f);
            using var waveStream = new WaveFileReader(filePath);

            byte[] buffer = new byte[4096];
            int bytesRead;
            while ((bytesRead = waveStream.Read(buffer, 0, buffer.Length)) > 0)
            {
                recognizer.AcceptWaveform(buffer, bytesRead);
            }

            // **Estrai il testo dal JSON**
            string textResult = ExtractTranscription(rawJson);

            // **Usiamo NumberConverter per trasformare il numero in cifra**
            int? number = NumberConverter.ConvertWordToNumber(textResult);
            Console.WriteLine($"🔢 Numero NON Convertito: '{textResult}'");
            Console.WriteLine($"🔢 Numero Convertito: '{number}'");

            // **Restituiamo la cifra se convertita, altrimenti il testo originale**
            string finalResult = number?.ToString() ?? textResult;

            Console.WriteLine($"📝 Testo Estratto: '{textResult}' -> Numero Convertito: '{finalResult}'");
            return finalResult;
        }

        private string ExtractTranscription(string jsonResult)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(jsonResult))
                {
                    return "⚠️ Nessun testo riconosciuto!";
                }

                // ✅ Usa System.Text.Json per analizzare il JSON
                using JsonDocument doc = JsonDocument.Parse(jsonResult);
                if (doc.RootElement.TryGetProperty("text", out JsonElement textElement))
                {
                    string extractedText = textElement.GetString()!.Trim();
                    return !string.IsNullOrWhiteSpace(extractedText) ? extractedText : "⚠️ Nessun testo riconosciuto!";
                }

                Console.WriteLine("❌ Il JSON non contiene il campo 'text'");
                return "Errore nella trascrizione";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Errore nell'estrazione della trascrizione: {ex.Message}");
                return "Errore nella trascrizione";
            }
        }
    }
}
