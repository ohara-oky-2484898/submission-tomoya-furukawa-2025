/// <summary>
/// じゃんけんの流れ
/// ①出す手を決める
/// ②出す
/// ③ジャッジ
/// ④相子なら①から
/// </summary>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum JankenState
{
    Wait,       // 待ち時間(思考時間)
    Start,      // "最初はグー"
    Play,       // "じゃんけん" ? "ポン"！
    Judge,      // 勝敗判定
    Rematch,    // "あいこで" ? "しょ！"
    Exit         // おわり
}

public class JankenManager : Singleton<JankenManager>
{
    /// <summary> AIを切り替えるもの </summary>
    private PatternBasedAISelector aiSelector = new PatternBasedAISelector();


    /// <summary> さいしょはぐーなどの掛け声を移す時間 </summary>
    [SerializeField] private float nextActionIntervalTime = 1f;
    [SerializeField] private NpcHandIconDisplay npcHandIconDisplay;

    private JankenState jankenState;
    private IHandDecider player;
    private IHandDecider npc;
    private bool isPlayerWin = false;   // playerの勝ちか？

    private int turnCount = 0; // ターン数を管理


    public JankenState CurrentState => jankenState;

    protected override void Init()
    {
        base.Init();
        // 初期化
        jankenState = JankenState.Start;

        player = FindObjectOfType<JankenPlayer>();
        npc = FindObjectOfType<JankenNPC>();


        // 最初はランダムAIをセット
        (npc as JankenNPC).AI(new RandomAI());
    }

    void Update()
    {
        PlayJanken();

        Debug.Log("現在のステート：" + jankenState);
        Debug.Log("現在のAI：" + aiSelector.GetCurrentStrategy());
        Debug.Log("現在のAI：" + aiSelector.GetCurrentAI());
    }

    /// <summary>
    /// じゃんけんの進行処理
    /// </summary>
    void PlayJanken()
    {
        switch (jankenState)
        {
            case JankenState.Start: // "最初はグー"
                StartCoroutine(OnStart());
                break;

            case JankenState.Play:  // "じゃんけん"
                // ↓即切り替えでコルーチン多重実行防止！
                jankenState = JankenState.Wait;
                OnPlay();
                break;

            case JankenState.Judge: // "判定"
                // ↓即切り替えでコルーチン多重実行防止！
                jankenState = JankenState.Wait;
                StartCoroutine(OnJudge());
                break;

            case JankenState.Rematch:// "あいこで"
                // ↓即切り替えでコルーチン多重実行防止！
                jankenState = JankenState.Wait;
                OnRematch();
                break;

            case JankenState.Exit:   // ゲーム終了かどうか判別
                OnExit();
                break;
            case JankenState.Wait:
                // 選択中、選択時間何もしない
                break;
        }
	}

    protected override void OnRelease()
    {
        base.OnRelease();
        // 解放処理などが必要ならここに書く
    }



    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////
    #region 各状態の処理

    /// <summary> "最初はグー" </summary>
    IEnumerator OnStart()
    {
        UIManager.Instance.SetCallText(jankenState);  // UIに掛け声のテキストを表示させる
        yield return new WaitForSeconds(nextActionIntervalTime);
        jankenState = JankenState.Play;
    }

    /// <summary> "じゃんけん" → "ぽん！" </summary>
    void OnPlay()
    {
        //PlayJankenState(JankenState.Play);
        StartCoroutine(PlayJankenState(JankenState.Play)); // ← ??こうすればOK！
    }

    /// <summary> 判定 </summary>
    IEnumerator OnJudge()
    {
        GameResult gameResult;

        if (player.IsDraw(npc))
        {
            // 引き分けだった
            yield return new WaitForSeconds(nextActionIntervalTime);
            gameResult = GameResult.Draw;
            GameManager.Instance.drawCount++;

            jankenState = JankenState.Rematch;
        }
        else
        {
            // 勝敗がついた
            isPlayerWin = player.IsAdvantageous(npc);
            // 勝敗に応じた処理を書く（UI表示など）
            
            // 勝ったらWin、負けたらLiseを代入
            gameResult = isPlayerWin ? GameResult.Win : GameResult.Lose;

            if (isPlayerWin)
            {
                GameManager.Instance.winCount++;
            }
            else
            {
                GameManager.Instance.loseCount++;
            }

            // 結果を表示
            UIManager.Instance.SetResultText(isPlayerWin, true);

            yield return new WaitForSeconds(nextActionIntervalTime);

            // 結果を非表示にする
            UIManager.Instance.SetResultText(isPlayerWin, false);

            jankenState = JankenState.Exit;
        }


        // 戦績を履歴に追加
        JankenHistoryManager.Instance.AddHistory(player.SelectHand, npc.SelectHand, gameResult);

        // ゲームが進んだ後、ターン数をカウントアップ
        turnCount++;

        // プレイヤーの手の履歴に基づいてAIを切り替える
        if (turnCount >= 5)
        {
            var history = JankenHistoryManager.Instance.GetAllHistory();
            var selectedAI = aiSelector.SelectAI(history);
            (npc as JankenNPC).AI(selectedAI); // 履歴に基づいてAIを切り替え
        }
        else
        {
            // 5ターン未満はランダムAI
            (npc as JankenNPC).AI(new RandomAI());
        }

    }

    /// <summary> "あいこで" → "しょ！" </summary>
    void OnRematch()
    {
        //PlayJankenState(JankenState.Rematch);
        StartCoroutine(PlayJankenState(JankenState.Rematch));
    }

    void OnExit()
	{
        // 続きがあるか？
        if (GameManager.Instance.CheckGameFinish())
        {
            // 終了
            UIManager.Instance.EndText(isPlayerWin);
        }
        else
        {
            // まだ終わらない
            jankenState = JankenState.Start;
        }
    }

    IEnumerator PlayJankenState(JankenState state)
    {
        if (!IsPlayableState(state)) yield break;

        // "じゃんけん"　or "あいこで"
        // まだ手を決めていない
        UIManager.Instance.SetCallText(state,false);  // UIに掛け声のテキストを表示させる

        // 入力待ち、入力されたら "→" にいく
        yield return StartCoroutine(player.DecideHand());
        yield return StartCoroutine(npc.DecideHand());
        // 入力されるまでは次にはいかない

        // → "ぽん！"　or "しょ！"
        // もう手をきめた
        UIManager.Instance.SetCallText(state, true);  // UIに掛け声のテキストを表示させる
        // 敵の手を出す
        npcHandIconDisplay.ShowIcon(npc.SelectHand);

        yield return new WaitForSeconds(nextActionIntervalTime);
        // 非表示にする
        //npcHandIconDisplay.HideIcon(npc.SelectHand);
        npcHandIconDisplay.HideAllIcons();

        //yield return new WaitForSeconds(2);
        jankenState = JankenState.Judge;
    }
    #endregion
    //コルーチンメモ
    //書き方 意味
    //yield return null; 1フレーム待つ
    //yield return new WaitForSeconds(1); 指定した秒数待つ
    //yield return new WaitUntil(() => 条件); 条件が成立するまで待つ
    //yield return new WaitWhile(() => 条件); 条件が成立している間待つ
    //yield break; コルーチンを途中で終了する

    /// <summary>
    /// Play又はRematch以外で使用を意図していないため
    /// じゃんけんができるかどうかを判別するもの
    /// </summary>
    /// <param name="state">現在の状態</param>
    /// <returns>できるかどうか</returns>
    bool IsPlayableState(JankenState state)
    {
        return state == JankenState.Play || state == JankenState.Rematch;
    }

    /// <summary>
    /// AIの切り替え処理
    /// </summary>
    /// <param name="isPost5Turns">ターン数が5ターン以上かどうか</param>
    //public void SwitchAI(bool isPost5Turns)
    //{
    //    // 5ターン目以降は、パターン予測型AIに切り替える
    //    if (isPost5Turns)
    //    {
    //        (npc as JankenNPC).AI(new PatternPredictionAI()); // プレイヤーの手のパターンを予測するAI
    //    }
    //    else
    //    {
    //        (npc as JankenNPC).AI(new RandomAI()); // ランダムAI
    //    }
    //}

}
