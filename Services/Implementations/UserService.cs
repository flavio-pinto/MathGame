using System.Linq;
using MathGame.Models;
using MathGame.Repositories;
using MathGame.Services.Interfaces;

namespace MathGame.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IRepository<User> _userRepository;

        public UserService(IRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }

        // Registra un nuovo utente. Qui viene eseguita una semplice validazione e
        // l'hash della password (per ora, a scopo didattico, si limita a restituire la stringa in chiaro).
        public void RegisterUser(string username, string password)
        {
            // Potresti controllare se un utente con lo stesso username esiste già
            var existingUser = _userRepository.GetAll().FirstOrDefault(u => u.Username == username);
            if (existingUser != null)
            {
                // Qui, a seconda delle esigenze, potresti lanciare un'eccezione o gestire l'errore in altro modo.
                throw new System.Exception("User already exists");
            }

            var user = new User
            {
                Username = username,
                PasswordHash = HashPassword(password), // Chiamata al metodo di hashing
                Score = 0
            };

            _userRepository.Add(user);
            _userRepository.Save();
        }

        // Effettua il login confrontando username e password.
        // Restituisce l'utente se le credenziali sono valide, altrimenti null.
        public User? LoginUser(string username, string password)
        {
            var user = _userRepository.GetAll().FirstOrDefault(u => u.Username == username);
            if (user != null && VerifyPassword(password, user.PasswordHash))
            {
                return user;
            }

            return null;
        }

        // Recupera un utente in base al suo ID
        public User? GetUserById(int id)
        {
            return _userRepository.GetById(id);
        }

        // Metodo di hashing (per esempio, potresti usare BCrypt o un altro algoritmo in un'app reale)
        private string HashPassword(string password)
        {
            // A scopo didattico ritorniamo la password in chiaro.
            // In produzione, implementa un vero meccanismo di hashing.
            return password;
        }

        // Verifica la password confrontando l'input con l'hash memorizzato.
        private bool VerifyPassword(string inputPassword, string storedHash)
        {
            // A scopo didattico, eseguiamo un confronto semplice.
            return inputPassword == storedHash;
        }

        // Restituisce la classifica degli utenti ordinata per punteggio e rank
        public IEnumerable<User> GetLeaderboard()
        {
            return _userRepository.GetAll()
                .OrderByDescending(u => u.Score) // Ordina per punteggio decrescente
                .ThenByDescending(u => u.Rank)   // Se due utenti hanno lo stesso score, ordina per rank
                .ToList();
        }
    }
}
