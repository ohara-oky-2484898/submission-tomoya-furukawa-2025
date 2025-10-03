using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーの最後の手に負ける手を出すAI
/// 主にボタンの配置右から順に押してくるときに使う
/// 並びが(パー、チョキ、グー)なので
/// この場合パーの次はチョキが来るのでパ－に負けるグー
/// を返せばチョキに勝てる
/// </summary>
public class LoseToLastPlayerMoveAI : IJankenAI
{
    public JankenHand Decide(IReadOnlyList<JankenHistory> history)
    {
        JankenHand lastPlayerHand = JankenHistoryManager.Instance.GetLastPlayerHand();

        // プレイヤーの手に負ける（＝不利な）手を返す
        return JankenLogic.DisadvantageousHand(lastPlayerHand);
    }
}
