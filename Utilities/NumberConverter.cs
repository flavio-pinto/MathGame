using System;
using System.Collections.Generic;

namespace MathGame.Utilities
{
    public static class NumberConverter
    {
        private static readonly Dictionary<string, int> NumberMap = new()
        {
            { "zero", 0 }, { "uno", 1 }, { "due", 2 }, { "tre", 3 }, { "quattro", 4 },
            { "cinque", 5 }, { "sei", 6 }, { "sette", 7 }, { "otto", 8 }, { "nove", 9 },
            { "dieci", 10 }, { "undici", 11 }, { "dodici", 12 }, { "tredici", 13 }, { "quattordici", 14 },
            { "quindici", 15 }, { "sedici", 16 }, { "diciassette", 17 }, { "diciotto", 18 }, { "diciannove", 19 },
            { "venti", 20 }, { "trenta", 30 }, { "quaranta", 40 }, { "cinquanta", 50 }, { "sessanta", 60 },
            { "settanta", 70 }, { "ottanta", 80 }, { "novanta", 90 }, { "cento", 100 },
            { "duecento", 200 }, { "trecento", 300 }, { "quattrocento", 400 }, { "cinquecento", 500 },
            { "seicento", 600 }, { "settecento", 700 }, { "ottocento", 800 }, { "novecento", 900 },
            { "mille", 1000 }, { "duemila", 2000 }, { "tremila", 3000 }, { "quattromila", 4000 },
            { "cinquemila", 5000 }, { "seimila", 6000 }, { "settemila", 7000 }, { "ottomila", 8000 }, { "novemila", 9000 }
        };

        public static int? ConvertWordToNumber(string word)
        {
            if (string.IsNullOrWhiteSpace(word))
                return null;

            word = NormalizeWord(word); // Rimuove accenti e caratteri non necessari

            if (NumberMap.TryGetValue(word, out int number))
                return number;

            // Se il numero non è trovato, proviamo a scomporlo
            return ParseComplexNumber(word);
        }

        private static string NormalizeWord(string word)
        {
            // Se la parola finisce con è, rimuoviamo l'accento
            if (word.EndsWith("è"))
                word = word.Substring(0, word.Length - 1);
            return word.ToLower().Trim();
        }

        private static int? ParseComplexNumber(string word)
        {
            int total = 0;

            // Suddividiamo la parola cercando i numeri più grandi prima (mille, cento, venti...)
            foreach (var key in NumberMap.Keys)
            {
                if (word.StartsWith(key))
                {
                    int value = NumberMap[key];
                    string remaining = word.Substring(key.Length);

                    int? remainingValue = ConvertWordToNumber(remaining);
                    if (remainingValue.HasValue)
                    {
                        total = value + remainingValue.Value;
                        return total;
                    }
                }
            }

            return null; // Se non riusciamo a riconoscere il numero
        }
    }
}
