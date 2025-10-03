using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// もっとも多く出された手に勝つ手を出す
/// わからなくなった時用！
/// </summary>
public class PatternAI : IJankenAI
{
    public JankenHand Decide(IReadOnlyList<JankenHistory> history)
    {
        if (history.Count == 0)
            return (JankenHand)Random.Range(0, (int)JankenHand.Num);

        int[] handCounts = new int[(int)JankenHand.Num];

        foreach (var h in history)
        {
            handCounts[(int)h.PlayerHand]++;
        }

        // 一番多く出された手を探す
        int mostCommon = 0;
        for (int i = 1; i < handCounts.Length; i++)
        {
            if (handCounts[i] > handCounts[mostCommon])
                mostCommon = i;
        }

        // その手に勝つ手を出す
        return JankenLogic.AdvantageousHand((JankenHand)mostCommon);
    }
}

