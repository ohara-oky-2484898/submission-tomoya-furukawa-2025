using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーの最後に出した手と同じ手を出すAI（ミラーAI）
/// 主に、ボタンの並び順で押してくる相手に使う
/// 並びが(グー、チョキ、パー)なので
/// このの場合グーの次はチョキが来るので最後に出した手と同じグーを出すと
/// チョキに勝てる
/// </summary>
public class MirrorAI : IJankenAI
{
    public JankenHand Decide(IReadOnlyList<JankenHistory> history)
    {
        // 履歴がない場合はランダムに手を出す
        if (history.Count == 0)
        {
            return (JankenHand)Random.Range(0, (int)JankenHand.Num);
        }

        // プレイヤーの最後の手を取得して返す
        return JankenHistoryManager.Instance.GetLastPlayerHand();
    }
}

