using System;
using System.Collections.Generic;
using MathGame.Enums; // Assume this namespace contains the GameMode enum
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MathGame.Models;

public class GameSession
{
    [Key]
    public int Id { get; set; }

    [ForeignKey("User")]
    public int UserId { get; set; }
    public required User User { get; set; }

    [Required]
    public GameMode Mode { get; set; }

    public DateTime StartedAt { get; set; } = DateTime.Now;
    public DateTime? EndedAt { get; set; }
    public int Score { get; set; } = 0;

    public ICollection<Round> Rounds { get; set; } = new List<Round>();
}
