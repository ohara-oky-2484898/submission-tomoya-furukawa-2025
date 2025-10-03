using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ‹L˜^‚ª‚Å‚«‚é
/// </summary>
public class JankenHistory : IJankenHistory
{
    public JankenHand PlayerHand { get; set; }
    public JankenHand NpcHand { get; set; }
    public GameResult Result { get; set; }

    public JankenHistory(JankenHand playerHand, JankenHand npcHand, GameResult result)
    {
        PlayerHand = playerHand;
        NpcHand = npcHand;
        Result = result;
    }
}

