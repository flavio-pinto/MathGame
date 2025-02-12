using MathGame.Models;

namespace MathGame.Services.Interfaces
{
    public interface IUserService
    {
        void RegisterUser(string username, string password);

        User? LoginUser(string username, string password);

        User? GetUserById(int id);

        // Ottiene gli utenti in ordine di rank
        IEnumerable<User> GetLeaderboard();
    }
}