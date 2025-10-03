using System.Collections.Generic;
using UnityEngine;

public class JankenHistoryManager : Singleton<JankenHistoryManager>
{
    private List<JankenHistory> historyList = new List<JankenHistory>();

    /// <summary>
    /// 履歴を追加する
    /// </summary>
    public void AddHistory(JankenHand playerHand, JankenHand npcHand, GameResult result)
    {
        JankenHistory history = new JankenHistory(playerHand, npcHand, result);

        historyList.Add(history);
        //Debug.Log($"履歴追加: プレイヤー({playerHand}) vs NPC({npcHand}) → 結果: {result}");
    }

    /// <summary>
    /// 履歴を取得する（外部参照用）
    /// ReadOnly(読み取り専用)を渡すことで安全を担保
    /// </summary>
    public IReadOnlyList<JankenHistory> GetAllHistory()
    {
        return historyList.AsReadOnly();
    }

    /// <summary>
    /// 履歴を全削除
    /// </summary>
    public void ClearHistory()
    {
        historyList.Clear();
        //Debug.Log("履歴をクリアしました");
    }

    // 必要があれば保存やロードも追加できる（例：JSONで保存など）

    /// <summary>
    /// プレイヤーが最後に出した手を取得する
    /// </summary>
    /// <returns>最後に出したプレイヤーの手</returns>
    public JankenHand GetLastPlayerHand()
    {
        if (historyList.Count == 0)
        {
            return JankenHand.Rock; // デフォルト値を返す（履歴が無い場合）
        }

        // C#8.0以降使えるようになったらしい
        //return historyList[^1].PlayerHand;
        // ”インデックス構文”でも使えないので
        return historyList[historyList.Count - 1].PlayerHand;
    }

    protected override void Init()
    {
        base.Init();
        //Debug.Log("JankenHistoryManager 初期化");
    }

    protected override void OnRelease()
    {
        base.OnRelease();
        //Debug.Log("JankenHistoryManager 終了");
    }
}
