using System;
using MathGame.Enums;

namespace MathGame.Models;

public class Round
{
    public int Id { get; set; }
    public int GameSessionId { get; set; }
    public GameSession GameSession { get; set; }
    public int RoundNumber { get; set; }
    public double Number1 { get; set; }
    public double Number2 { get; set; }
    public MathOperation Operation { get; set; }
    public double CorrectAnswer { get; set; }
    public double? UserAnswer { get; set; }
    public bool IsCorrect { get; set; }
    public TimeSpan? TimeTaken { get; set; }
}
