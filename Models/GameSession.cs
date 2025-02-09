using System;
using System.Collections.Generic;
using MathGame.Enums; // Assume this namespace contains the GameMode enum

namespace MathGame.Models;

public class GameSession
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public User User { get; set; } = null!; // Il null-forgiving operator evita il warning
    public GameMode Mode { get; set; }
    public DateTime StartedAt { get; set; }
    public DateTime? EndedAt { get; set; }
    public int Score { get; set; }
    public ICollection<Round> Rounds { get; set; } = new List<Round>();
}
