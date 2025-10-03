using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//UI管理: ゲームのUI（ユーザーインターフェース）を管理する
//「UIManager」は、ゲームのメニューやポップアップ、
//インターフェースの切り替えなどを制御します。

//UIManagerの責任範囲とは？
//UIの表示・非表示管理: UI要素（ボタン、テキスト、ポップアップ、メニューなど）
//の表示、非表示、アニメーションなどを管理します。

//ユーザーからの入力処理の転送: ユーザーがボタンを押したときや、
//UI要素に入力を行った際に、その入力を適切なゲームロジック（他のManagerやクラス）
//に転送します。

public enum HUDAlignment
{
    Center, // 中央揃え
    Left,   // 左詰め
    Right   // 右詰め
}
/// <summary>
/// 表示非表示の管理
/// animationなどの管理
/// 入力処理、ボタンなどの入力転送
/// ゲーム振興に応じたUIの切り替え
/// </summary>
public class UIManager : Singleton<UIManager>
{
    [Header("HUDプレハブと親")]
    [SerializeField] private CharacterHUD _hudPrefab;
    [SerializeField] private RectTransform _alliesParent; // 味方HUDを配置するUIの親（HorizontalLayoutGroupなど）
    [SerializeField] private RectTransform _enemiesParent; // 敵HUD用の親
    [SerializeField] private Text _modeText;
    // 行動順表示用のTextをInspectorからセット
    [SerializeField] private Text _actionOrderText; 
    [SerializeField] private float _deathFadeDuration = 1.0f;
    [Header("Retryダイアログ")]
    [SerializeField] private GameObject _retryDialogPanel;
    [SerializeField] private Button _yesButton;
    [SerializeField] private Button _noButton;

    private bool _inputReceived = false;
    private bool _retry = false;

    private List<CharacterHUD> _activeHUDs = new List<CharacterHUD>();

    private readonly string _autoModeText = "オートモード中\n手動モードに切り替え\n「Space」key";
    private readonly string _manualModeText = "手動モード中\n「T」key：次へ\n オートモードに切り替え\n「Space」key";
    protected override void Init()
    {
        // 必要なら初期化処理
        base.Init();
        _modeText.text = _manualModeText;
        _actionOrderText.text = "ボタン入力待ち：まだ始まっていません";
        _retryDialogPanel.SetActive(false);
    }

    /// <summary>
    /// モードメッセージの切り替え
    /// </summary>
    /// <param name="isAutoMode">オートモードならtrue</param>
    public void SetModeMessage(bool isAoutMode)
	{
        // オートならtrue
        _modeText.text = isAoutMode ? _autoModeText : _manualModeText;
    }

    /// <summary>
    /// 味方・敵のHUDを生成して並べる
    /// </summary>
    public void CreateHUDs(List<IBattler> battlers)
    {
        ClearHUDs();
        // チームごとに分ける
        List<IBattler> allies = battlers.FindAll(b => b.Team == Team.Ally);
        List<IBattler> enemies = battlers.FindAll(b => b.Team == Team.Enemy);

        ArrangeHUDs(allies, _alliesParent, HUDAlignment.Left);
        ArrangeHUDs(enemies, _enemiesParent, HUDAlignment.Right);
    }

    /// <summary>
    /// 対象キャラのHUDを更新する処理
    /// HP,MPの増減
    /// </summary>
    public void UpdateHUD(IBattler battler)
    {
        // 対象キャラのHUDだけを更新する処理
        var hud = _activeHUDs.Find(h => h.Battler == battler);
        if (hud != null)
        {
            hud.UpdateUI(); // UI更新メソッド
        }
    }

	/// <summary>
	/// 全てのHUDを削除
	/// </summary>
	public void ClearHUDs()
    {
        foreach (var hud in _activeHUDs)
        {
            Destroy(hud.gameObject);
        }
        _activeHUDs.Clear();
    }

    /// <summary>
    /// 指定のバトラーの行動時のエフェクトを呼び出す
    /// </summary>
    /// <param name="battler">指定のバトラー</param>
    public void PlayFocusEffectFor(IBattler battler)
    {
        if (battler == null || !battler.IsAlive) return;
        _activeHUDs.Find(h => h.Battler == battler).PlayFocusEffect();
    }

    /// <summary>
    /// 指定のバトラーの死亡時のエフェクトを呼び出す
    /// </summary>
    /// <param name="battler">指定バトラー</param>
    public void PlayDeathEffectFor(IBattler battler)
    {
        _activeHUDs.Find(h => h.Battler == battler).PlayDeathFadeEffect(_deathFadeDuration);
    }

    /// <summary>
    /// 行動順リストを受け取り、テキストに整形して表示する
    /// </summary>
    /// <param name="turnOrder">行動順のリスト</param>
    public void DisplayActionOrder(List<IBattler> turnOrder, string strategyName, int turnCount)
    {
        if (turnOrder == null || turnOrder.Count == 0)
        {
            _actionOrderText.text = "行動順なし";
            return;
        }

        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        sb.AppendLine($"=== 行動順（{strategyName}）（{turnCount}順目）===");

        int order = 1;
        foreach (var battler in turnOrder)
        {
            if (battler.ReservedAction == null) continue;
            var actionData = battler.ReservedAction.GetActionData();

            string actionName = actionData != null ? actionData.Name : "不明な行動";
            string targetDisplay;

            if (actionData == null || actionData.Targets == null || actionData.Targets.Count == 0)
            {
                targetDisplay = "なし";
            }
            else if (actionData.Targets.Count == 1)
            {
                targetDisplay = actionData.Targets[0].Name; // 単体対象
            }
            else
            {
                targetDisplay = "全体"; // 複数対象
            }

            sb.AppendLine($"{order}: {battler.Name}(素早さ：{battler.Status.Speed})／" +
				$"アクション({actionName})\n　　　ターゲット：　{targetDisplay}");
            order++;
        }

        _actionOrderText.text = sb.ToString();
    }


    /// <summary>
    /// HUDを親オブジェクト内に配置する
    /// </summary>
    private void ArrangeHUDs(List<IBattler> teamBattlers, RectTransform parent, HUDAlignment alignment)
    {
        float spacing = 150f; // HUD間の距離（調整可）
        int count = teamBattlers.Count;

        // 親のRectTransformから開始位置を取得
        Vector2 parentPosition = parent.anchoredPosition;

        // 配置方法に応じて開始位置を設定
        float startX = 0;
        if (alignment == HUDAlignment.Center) // 中央揃え
        {
            startX = -(spacing * (count - 1)) / 2f; // 親の位置を基準に中央揃え
        }
        else if (alignment == HUDAlignment.Left) // 左詰め
        {
            startX = 0; // 最初のキャラを親の位置(0,0)に配置
        }
        else if (alignment == HUDAlignment.Right) // 右詰め
        {
            startX = -(spacing * (count - 1)); // 最初のキャラを親の右端に配置
        }
        else
        {
            Debug.LogError("不正な配置指定です");
            return;
        }

        for (int i = 0; i < count; i++)
        {
            // 新しいHUDを生成
            CharacterHUD hud = Instantiate(_hudPrefab, parent);
            hud.Setup(teamBattlers[i]);

            // HealthGaugeEffect が正しく設定されているかチェック
            if (!hud.GaugeEffect)
            {
                Debug.LogError("HUD が作成されましたが、HealthGaugeEffect が設定されていません！");
                continue; // gaugeEffect が設定されていない場合、このHUDをスキップ
            }

            _activeHUDs.Add(hud);

            // 配置: 親オブジェクトの位置を基準に、指定された配置方法で配置
            RectTransform rt = hud.GetComponent<RectTransform>();

            // startXから150pxずつずらして配置
            rt.anchoredPosition = new Vector2(startX + (i * spacing), 0);
        }
    }



    /// <summary>
    /// リトライダイアログを表示し、ユーザーの入力を待つ
    /// </summary>
    public void ShowRetry(System.Action<bool> onDecision)
    {
        StartCoroutine(ShowRetryCoroutine(onDecision));
    }

    private IEnumerator ShowRetryCoroutine(System.Action<bool> onDecision)
    {
        _retryDialogPanel.SetActive(true);
        _inputReceived = false;

        _yesButton.onClick.RemoveAllListeners();
        _noButton.onClick.RemoveAllListeners();

        _yesButton.onClick.AddListener(() =>
        {
            _retry = true;
            _inputReceived = true;
        });

        _noButton.onClick.AddListener(() =>
        {
            _retry = false;
            _inputReceived = true;
        });

        // 入力が来るまで待つ
        yield return new WaitUntil(() => _inputReceived);

        _retryDialogPanel.SetActive(false);
        onDecision?.Invoke(_retry);
    }
}

//例：ポーズ画面を表示する
//ポーズ画面の表示: ゲームが一時停止した際に、ポーズメニューを表示する。

//他のクラスとのやり取り: ゲームがポーズされたかどうかは**
//GameManager（またはBattleManager）が知っているが、
//ポーズ画面を実際に表示するのはUIManager** の責任です。


//例：ゲームオーバー時の再試行処理
//ゲームオーバー時に「Retryしますか？」というUIを表示する部分は、
//UIManagerの仕事ですが、実際にリトライ処理
//（ゲームの再スタートや状態のリセット）はゲームロジックの担当です。

//UIManagerの責任: 「Retryしますか？」というダイアログの表示
//BattleManager（またはGameManager）の責任: リトライ時のゲームの状態のリセットやシーンの再ロード


//例：ゲームの進行状況に応じたUIの更新
//ゲームが進行中か、ゲームオーバーか、ポーズ中か、などのゲームの状態に応じて
//UIを変更するのはUIManagerの仕事ですが、
//その状態自体はゲームのロジック（GameManagerやBattleManager）に管理させます。
