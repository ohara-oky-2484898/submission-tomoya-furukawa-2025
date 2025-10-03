using System.Collections.Generic;
using UnityEngine;

public class PatternPredictionAI : IJankenAI
{
    // KeyValuePairの代わりにタプルを使用
    private Dictionary<(JankenHand, JankenHand), int> transitionCounts = new Dictionary<(JankenHand, JankenHand), int>();

    public JankenHand Decide(IReadOnlyList<JankenHistory> history)
    {
        if (history.Count < 2)
        {
            return (JankenHand)Random.Range(0, (int)JankenHand.Num);
        }

        // カウント初期化
        transitionCounts.Clear();

        // 「前回の手」→「今回の手」の組み合わせをカウント
        for (int i = 1; i < history.Count; i++)
        {
            var prev = history[i - 1].PlayerHand;
            var curr = history[i].PlayerHand;
            var key = (prev, curr); // タプルとして格納

            if (!transitionCounts.ContainsKey(key))
                transitionCounts[key] = 0;

            transitionCounts[key]++;
        }

        // 最後のプレイヤーの手を取得
        JankenHand lastHand = history[history.Count - 1].PlayerHand;

        // 最も多く出された「lastHand → ?」の組み合わせを探す
        JankenHand predictedNext = JankenHand.Rock;
        int maxCount = -1;

        foreach (var kv in transitionCounts)
        {
            if (kv.Key.Item1 == lastHand && kv.Value > maxCount) // Item1はタプルの最初の要素
            {
                predictedNext = kv.Key.Item2; // Item2はタプルの2番目の要素
                maxCount = kv.Value;
            }
        }

        if (maxCount == -1)
        {
            // データが少ない場合はランダム
            return (JankenHand)Random.Range(0, (int)JankenHand.Num);
        }

        // 予測された手に勝つ手を返す
        return JankenLogic.AdvantageousHand(predictedNext);
    }
}
