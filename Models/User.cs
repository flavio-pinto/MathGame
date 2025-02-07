using System.Collections.Generic;

namespace MathGame.Models;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string PasswordHash { get; set; }
    public int Score { get; set; }
    public ICollection<GameSession> GameSessions { get; set; } = new List<GameSession>();

    public string Rank => Score switch
    {
        int s when s >= 0 && s <= 10 => "Bronze",
        int s when s >= 11 && s <= 20 => "Silver",
        int s when s >= 21 && s <= 30 => "Gold",
        int s when s >= 31 && s <= 40 => "Platinum",
        int s when s >= 41 && s <= 50 => "Diamond",
        int s when s >= 51 && s <= 60 => "Master",
        int s when s >= 61 => "Grandmaster",
        _ => "Undefined" // In case the score is outside expected ranges
    };
}
