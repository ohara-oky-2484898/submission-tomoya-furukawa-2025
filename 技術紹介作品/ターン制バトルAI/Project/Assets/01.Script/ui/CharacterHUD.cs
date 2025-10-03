using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.UI;
using DG.Tweening;

// HUD →ヘッドアップディスプレイ
// ゲーム画面上に常に表示されているプレイヤーに情報を提供するUI要素のこと
public class CharacterHUD : MonoBehaviour
{

    //- キャラクタHUD（全体を管理する親オブジェクト）                        // this.gameobject自身がそれにあたる
    //  - キャラ画像（Image）                                                // charaImageが対応
    //  - Panel（キャラクター情報の背景となる親Panel）                       // panelTransform.gameibjectが対応
    //    - HPゲージ（Image）                                                // gaugeEffectのthis.gameobjectが対応
    //      - 背景（赤 Image）                                               // gaugeEffectのフィールドhealthImageが対応
    //      - HP表示部分（緑 Image）  ← `Fill Amount`を使って動的にHPを変更 // gaugeEffectのフィールドburnImageが対応
    //      - HPのテキスト（Text: "current/max"）                            // hpTextが対応
    //    - MPのテキスト（Text: "current/max"）  ← MPの状態を表示           // mpTextが対応
    //    - 名前（Text: "名前"）  ← ポケモン名を表示                        // nameTextが対応

    // したいこと
    // charaImage.rectTransform.positionはthis.gameobjectのポジションとそのまま対応0,0になっていればいいが
    // panelTransformを変更したい
    // 敵なら右上に、味方なら左下に
    // ずらす幅は親オブジェ(this.gameobjectを基準に)
    // ｘ軸150／ｙ軸80をプラスマイナス切り替えては配置

    // キャラ画像以外の、HPゲージ名前HP,MPテキストをまとめた親オブジェクト
    [SerializeField] private RectTransform panelTransform;
    [SerializeField] private Image charaImage;
    
    [Header("名前や数値")]
    [SerializeField] private Text nameText;
    [SerializeField] private Text hpText;
    [SerializeField] private Text mpText;

    [Header("HPゲージ演出コンポーネント")]
    // この中にHPの背景赤Imageと緑Imageが設定されてあるある
    [SerializeField] private HealthGaugeEffect gaugeEffect;
    public HealthGaugeEffect GaugeEffect { get { return gaugeEffect; } set { gaugeEffect = value; } }

    private IBattler battler;
    public IBattler Battler => battler;  // ← ここを追加！

    public void Setup(IBattler battler)
    {
        this.battler = battler;
        nameText.text = battler.Name;
        charaImage.sprite = battler.Sprite;
        Debug.Log($"[Setup] battler: {battler}, 名前{battler.Name},battler.Sprite:→ {battler.Sprite}←, charaImage: {charaImage}");


        // gaugeEffect が null でないかチェック
        if (gaugeEffect == null)
        {
            Debug.LogError("gaugeEffect (HealthGaugeEffect) が設定されていません！");
            return; // gaugeEffect が設定されていない場合、処理を止める
        }

        UpdateHP(battler.Status.HP);
        UpdateMP(battler.Status.MP);


        // 配置処理（敵か味方かによる位置決め）
        AdjustPositionBasedOnCharacter();
    }

    public void UpdateUI()
    {
        UpdateHP(battler.Status.HP);
        UpdateMP(battler.Status.MP);
    }

    public void UpdateHP(int currentHP)
    {
        float rate = (float)currentHP / battler.Status.MaxHP;
        hpText.text = $"{currentHP} / {battler.Status.MaxHP}";

        // どちらかが null または Destroy 済みなら処理しない
        if (!gaugeEffect)
        {
            Debug.LogWarning("ちゃんと設定して");
            return;
        }
        gaugeEffect.SetGauge(rate);
    }

    public void UpdateMP(int currentMP)
    {
        mpText.text = $"{(int)currentMP} / {battler.Status.MaxMP}";
    }

    public void PlayFocusEffect()
    {
        // 一旦サイズを拡大してから元に戻す
        transform.DOKill(); // 既存の Tween を殺す（重複防止）
        transform.localScale = Vector3.one;

        // 拡大→戻す（0.3秒くらい）
        transform.DOScale(1.2f, 0.15f)
            .SetLoops(2, LoopType.Yoyo)
            .SetEase(Ease.OutQuad);
    }

    // 味方/敵によってUIの位置を調整
    private void AdjustPositionBasedOnCharacter()
    {
        // 例えば、battlerが敵なら右上、味方なら左下に配置
        if (battler.Team == Team.Enemy) // IBattlerにIsEnemyプロパティがあると仮定
        {
            // 敵の場合（右上）
            panelTransform.anchoredPosition = new Vector2(50f, 90f); // 右上に配置
        }
        else
        {
            // 味方の場合（左下）
            panelTransform.anchoredPosition = new Vector2(-50f, -90f); // 左下に配置
        }
    }

    /// <summary>
    /// キャラクターが死亡したときに呼び出す：キャラ画像のαを100までフェードアウト
    /// </summary>
    public void PlayDeathFadeEffect(float duration = 1.0f)
    {
        if (charaImage == null)
        {
            Debug.LogWarning("charaImageが設定されていません！");
            return;
        }

        Color currentColor = charaImage.color;
        float targetAlpha = 100f / 255f; // UnityのColorは0?1で扱う

        // DOTween で alpha を変更
        charaImage.DOFade(targetAlpha, duration)
            .SetEase(Ease.InOutQuad);
    }

}

