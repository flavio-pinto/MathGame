# 🧮 MathGame - Il gioco della matematica con riconoscimento vocale! 🎙️

**MathGame** è un gioco a quiz matematico sviluppato in **C# e .NET 8**, con supporto per la **risposta vocale** grazie a **Vosk Speech Recognition**! 🗣️  
Gli utenti possono risolvere operazioni matematiche e migliorare il loro **rank** in base al punteggio ottenuto. 🏆  

---

## 🚀 Funzionalità
✅ **Registrazione & Login** con gestione utenti  
✅ **5 Modalità di gioco**: Addizione, Sottrazione, Moltiplicazione, Divisione, Hardcore  
✅ **Riconoscimento vocale** per rispondere alle domande  
✅ **Rank dinamico** in base al punteggio  
✅ **Storico delle partite** per ogni utente  
✅ **Classifica generale** con tutti gli utenti  
✅ **Seeding dei dati** per testare il gioco con utenti e partite generate automaticamente  

---

## 📥 Installazione

### 1️⃣ Clona il repository
```sh
git clone https://github.com/flavio-pinto/math__game.git
cd mathgame
```

### 2️⃣ Configura il database
- Assicurati di avere **SQL Server** installato  
- Modifica il file **`appsettings.json`** e imposta la stringa di connessione, ad esempio:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=math_game;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

### 3️⃣ Installa le dipendenze
```sh
dotnet restore
```

### 4️⃣ Applica le migrazioni
```sh
dotnet ef database update
```

### 5️⃣ (Opzionale) Popola il database con dati di test
```sh
dotnet run --seed
```

---

## 🎤 Setup del riconoscimento vocale (Vosk)

### 1️⃣ Scarica il modello di lingua italiana
Scarica il modello **Vosk per l'italiano** da [qui](https://alphacephei.com/vosk/models)  
> Modello consigliato: **vosk-model-small-it-0.22**  

### 2️⃣ Posiziona il modello nella cartella `Models/`
Dopo aver estratto il modello, il percorso deve essere:
```
mathgame/
 ├── Models/
 │   ├── vosk-model-small-it-0.22/
 │   ├── (altri file del modello)
```

### 3️⃣ Assicurati che il percorso nel codice sia corretto
Nel file **`VoskSpeechRecognitionService.cs`**, la variabile `ModelPath` deve puntare alla cartella corretta:
```csharp
private static readonly string ModelPath = Path.Combine(Directory.GetCurrentDirectory(), "Models", "vosk-model-small-it-0.22");
```

---

## 🕹️ Come giocare
Avvia il gioco con:
```sh
dotnet run
```
👤 **Effettua il login** o **crea un nuovo account**  
🎮 **Scegli una modalità di gioco**  
🎤 **Rispondi con la tastiera o con la voce**  
🏆 **Scala la classifica e migliora il tuo rank!**  

---

## ⚙️ Tecnologie utilizzate
- **C# & .NET 8**  
- **Entity Framework Core** per la gestione del database  
- **SQL Server** per la memorizzazione dei dati  
- **Spectre.Console** per un'interfaccia colorata in console  
- **Vosk Speech Recognition** per il riconoscimento vocale  
- **Bogus** per generare dati fittizi nel seeding  

---

## 📊 Classifica e Rank
Durante il gioco, il punteggio accumulato permette di scalare la classifica e migliorare il proprio **rank**:

| Punteggio | Rank       |
|-----------|-----------|
| 0 - 10    | Bronze    |
| 11 - 20   | Silver    |
| 21 - 30   | Gold      |
| 31 - 40   | Platinum  |
| 41 - 50   | Diamond   |
| 51 - 60   | Master    |
| 61+       | Grandmaster |

---

## 🛠️ Possibili miglioramenti
- 📊 **Dashboard web** per visualizzare statistiche e rank  
- 🎭 **Migliore gestione dell’input vocale** per riconoscere più varianti di numeri  
- 🤖 **Modalità multiplayer** per sfide tra utenti  

---

## ⚠️ Note importanti
🔹 Il file **Vosk Model** non è incluso nel repository per evitare file di grandi dimensioni. Deve essere scaricato separatamente.  
🔹 Assicurati di configurare **SQL Server** e di avere un database attivo prima di avviare il gioco.  
🔹 Se vuoi evitare la generazione di dati fake ad ogni avvio, disabilita il seeding nel file `Program.cs`.  

---

## 📜 Licenza
Questo progetto è distribuito sotto licenza **MIT**.  
Creato da **Flavio**. 🎓💻  

---

Se hai problemi o suggerimenti, **apri una issue su GitHub!** 🚀🔥

