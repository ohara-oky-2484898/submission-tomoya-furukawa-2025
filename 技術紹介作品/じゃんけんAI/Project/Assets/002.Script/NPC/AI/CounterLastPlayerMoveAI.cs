//using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーが最後に出した手に勝つ手を出すAI
/// ずっとグーなどの時用
/// </summary>
public class CounterLastPlayerMoveAI : IJankenAI
{
    public JankenHand Decide(IReadOnlyList<JankenHistory> history)
    {

        // プレイヤーが最後に出した手を取得
        JankenHand lastPlayerHand = JankenHistoryManager.Instance.GetLastPlayerHand();

        // 予測された手に有利な手を取得する
        return JankenLogic.AdvantageousHand(lastPlayerHand);
    }
}
