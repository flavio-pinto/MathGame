using MathGame.Models;

namespace MathGame.Services.Interfaces
{
    public interface IUserService
    {
        // Registers a new user with the provided username and password.
        void RegisterUser(string username, string password);

        // Logs in the user and returns the corresponding User model if credentials are valid.
        User? LoginUser(string username, string password);

        // Retrieves a user by its unique identifier.
        User? GetUserById(int id);
    }
}