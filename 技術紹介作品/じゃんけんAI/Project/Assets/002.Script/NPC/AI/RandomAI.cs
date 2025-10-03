using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// ƒ‰ƒ“ƒ_ƒ€‚Éè‚ğo‚·AI
/// å‚É˜”Õ—p
/// </summary>
public class RandomAI : IJankenAI
{
    public JankenHand Decide(IReadOnlyList<JankenHistory> history)
    {
        return (JankenHand)Random.Range(0, (int)JankenHand.Num);
    }
}
