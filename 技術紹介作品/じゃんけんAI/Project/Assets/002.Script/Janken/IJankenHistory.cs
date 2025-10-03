using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ‡Œ‹‰Ê
/// </summary>
public enum GameResult
{
    Win = 0,
    Lose,
    Draw,
}

public interface IJankenHistory
{
    JankenHand PlayerHand { get; set; }
    JankenHand NpcHand { get; set; }
    GameResult Result { get; set; } // "Win", "Lose", "Draw"
}
