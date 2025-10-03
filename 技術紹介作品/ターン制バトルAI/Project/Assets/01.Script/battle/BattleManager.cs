//①戦闘開始（初期化）
//②ターン開始（プレイヤー or AI）
//③行動選択・入力受付（AIなら自動）
//④行動実行（攻撃・防御・回復など）
//⑤勝敗判定
//⑥次のターンへ移行 or 戦闘終了

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// BattleManager は戦闘順を設定・指示するだけの管理クラス
/// </summary>
public class BattleManager : Singleton<BattleManager>
{
    /// <summary> フィールド </summary>
    [Header("次の行動が始まるまでの隙間時間")]
    [SerializeField] private float _nextActionInterval = 0.7f;
    private float _nextActionIntervalOriginal;

    /// <summary>
    /// ターンの順番管理機能
	/// バトル管理するならターン管理機能を持ってる
    /// </summary>
	private TurnManager _turnManager = new TurnManager();

    /// <summary>
    /// バトラー全員のリスト
	/// IBattlerに陣営を持たせたので一括管理が可能になった
    /// </summary>
	private List<IBattler> _allBattlers;
    private StageData _nowStageData;

    /// <summary> フラグ関連 </summary>
    private bool _once = false;
    private bool _isTurnInProgress = false;
    private bool _isAutoMode = false; // ← オートかどうかのフラグ
    private bool _isRetryDecide = true;


    /// <summary> プロパティ </summary>
    public List<IBattler> AllBattlers => _allBattlers;

    /// <summary> singletonの設定 </summary>
    protected override bool DestroyTargetGameObject => true;


    protected override void Init()
	{
		base.Init();

        _nextActionIntervalOriginal = _nextActionInterval;
    }
    private void Update()
	{
		if(!_once)
		{
			// キャラの初期化
			InitializeBattlers();
			// HUD生成をUIManagerに任せる
			UIManager.Instance.CreateHUDs(_allBattlers);
			_once = true;
		}
        // Tキー（手動モードでのみターンを進める）
        if (!_isAutoMode && Input.GetKeyDown(KeyCode.T))
        {
            if(!_isTurnInProgress)
            {
                StartCoroutine(PlayTurn());
            }
            else
            {
                Debug.Log("まだターン処理中です。しばらく待ってください。");
            }
        }

        // スペースキーでオートモードの ON/OFF を切り替える
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _isAutoMode = !_isAutoMode;
            UIManager.Instance.SetModeMessage(_isAutoMode);
            Debug.Log($"オートモード: {(_isAutoMode ? "ON" : "OFF")}");
        }

        // オートモード中は勝手にターンを回す
        // オート中でかつ、ターンが終わったら
        if (_isAutoMode && !_isTurnInProgress)
        {
            StartCoroutine(PlayTurn());
        }
    }



    /// <summary>
    /// ゲームレベルをあげて再開
    /// ステージ1から2に移動するとき
    /// 初期化処理などを呼んであげる
    /// </summary>
    public void GameReStart()
    {
        _nowStageData = GameManager.Instance.NowStageData();

        // 前回のスプライトやHUDをクリア（必要なら）
        UIManager.Instance.ClearHUDs();
        //ClearAllSprites();

        // 新規キャラ再生成
        InitializeBattlers();
        UIManager.Instance.CreateHUDs(_allBattlers);
        _turnManager.TurnReset();
    }

    /// <summary>
    /// バトルのテンポを倍速にする関数
    /// 倍って言っても行動のIntervalを短くするだけ
    /// </summary>
    public void SpeedUpBattle()
    {
        _nextActionInterval = _nextActionInterval / 2;
    }

    /// <summary>
    /// バトルのテンポをリセットする関数
    /// </summary>
    public void ResetBattleSpeed()
    {
        _nextActionInterval = _nextActionIntervalOriginal;
    }



    private void InitializeBattlers()
    {
        List<IBattler> chars = new List<IBattler>();
        _nowStageData = GameManager.Instance.NowStageData();
        foreach (var character in _nowStageData.Characters)
        {
            var battler = CreateBattler(character);
            chars.Add(battler);
        }

        _allBattlers = chars;

        foreach (var battler in _allBattlers)
        {
            battler.Init();
        }
    }

    private IBattler CreateBattler(CharacterData data)
	{
		return CharacterFactory.Create(data);
	}

    /// <summary>
    /// 1ターンの処理をコルーチンで実行し、行動の間に待機を入れる
    /// </summary>
    private IEnumerator PlayTurn()
    {
        if (!_isRetryDecide) yield break;
        _isTurnInProgress = true;

        // ①全員の行動順を決める
        // 順番に並び変えたものを格納する配列
        //List<IBattler> turnOrder = _turnManager.GetTurnOrder(_nowStageData.TurnOrderStrategy, _allBattlers);
        var turnOrderStrategy = _nowStageData.TurnOrderStrategy;
        List<IBattler> turnOrder = _turnManager.GetTurnOrder(turnOrderStrategy, _allBattlers);



        // ②順番に全員の行動を決める
        foreach (var battler in turnOrder)
        {
            battler.DecideAction(_allBattlers);
        }

        // 行動順リストと決定した行動をUIに表示
        UIManager.Instance.DisplayActionOrder(turnOrder, turnOrderStrategy.DisplayName, _turnManager.TurnCount);
        yield return new WaitForSeconds(_nextActionInterval);

        // ③順番に全員の行動を実行
        // ターン開始
        Debug.Log($"===== 実行順 =====");
        foreach (var battler in turnOrder)
        {
            var list = battler.GetEnemies(_allBattlers);
            if (list.Count == 0 || !battler.IsAlive)
                continue;

            Debug.Log($"{battler.Name} の行動開始");

            UIManager.Instance.PlayFocusEffectFor(battler);

            yield return new WaitForSeconds(_nextActionInterval);

            battler.ExecuteAction();

            // 行動が見えるように1秒待機
            yield return new WaitForSeconds(_nextActionInterval);
        }

        // どちらか全滅する
        // 勝負がつくまで繰り返し実行
        if (BattleSystem.TryCheckBattleResult(_allBattlers, out BattleResult result))
        {
            // 勝負がついた
            if (result == BattleResult.Win)
            {
                if(GameManager.Instance.IsGameClear())
				{
                    // Clear処理
                    GameManager.Instance.ExitGame();
                }
            }
            else if (result == BattleResult.Lose)
            {
                _isRetryDecide = false;
                // 再挑戦するか終わるか選択させる
                UIManager.Instance.ShowRetry((bool retry) =>
                {
                    if (retry)
                    {
                        GameReStart(); // 再挑戦
                    }
                    else
                    {
                        GameManager.Instance.ExitGame(); // 終了
                    }
                    _isRetryDecide = true;
                });
            }
        }

        _isTurnInProgress　= false;
        _turnManager.NextTurn();
    }
}
