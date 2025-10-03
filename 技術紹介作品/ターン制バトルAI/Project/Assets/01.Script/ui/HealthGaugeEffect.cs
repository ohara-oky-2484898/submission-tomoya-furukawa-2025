using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>
/// HPゲージにエフェクト付きで反映させるコンポーネント
/// </summary>
public class HealthGaugeEffect : MonoBehaviour
{
    /// <summary>HPゲージに関する変数</summary>
    [Header("HPゲージ設定")]
    [SerializeField] private Image _healthImage;
    [SerializeField] private Image _burnImage;

    /// <summary>演出に関する設定</summary>
    [Header("演出設定")]
    [SerializeField] private float _duration = 0.5f;
    [SerializeField] private float _strength = 20f;
    [SerializeField] private int _vibrate = 100;

    /// <summary>現在のHP割合</summary>
    private float _currentRate = 1f;

    /// <summary>元のローカル位置（揺れ後に戻す位置）</summary>
    private Vector3 _originalPosition;

    /// <summary>初期化処理。元の位置を記録</summary>
    private void Awake()
    {
        _originalPosition = transform.localPosition;
    }

    /// <summary>
    /// HPゲージの表示を更新し、DOTweenで演出を適用する
    /// </summary>
    /// <param name="targetRate">目標のHP割合（0?1）</param>
    public void SetGauge(float targetRate)
    {
        if (Mathf.Approximately(_currentRate, targetRate))
        {
            Debug.Log("重複してた");
            return; // 値が変わっていなければ処理しない
        }

        // どちらかが null または Destroy 済みなら処理しない
        if (!_healthImage || !_burnImage)
        {
            Debug.LogWarning("HPゲージの画像が設定されていないよ");
            return;
        }

        transform.localPosition = _originalPosition; // まず基準位置に戻す

        // OnComplete "."の前の処理が終わったら
        // 次の()の処理を行ってくださいというもの
        // ラムダ式
        _healthImage.DOFillAmount(targetRate, _duration).OnComplete(() =>
        {
            // 演出の関係上 "duration" が同じ値だとわかりづらくなるため
            // 半分の値にしておく。
            // また、すぐ縮小が始まってしまうのもわかりづらくなるため間をあけておく
            // SetDelay(0.5f) 0.5秒まってから処理する
            _burnImage.DOFillAmount(targetRate, _duration * 0.5f).SetDelay(0.5f);
        });

        // 今回この自分自身がルート(親)のオブジェクト
        // のため自身が揺れると全体が揺れる
        // DOShakePosition(三つの引数)(揺れてる時間)(揺れの強さ)(揺れ);
        transform.DOShakePosition(_duration * 0.5f, _strength, _vibrate);

        _currentRate = targetRate;
    }

    public void OnDestroy()
	{
        // DOTween の Safe Mode が、オブジェクトが破棄された後にも Tween が生きていて、
        // それにアクセスしようとしてエラーをキャッチしていたため
        // オブジェクトが消される前に、オブジェクトにバインドされた Tween を明示的に停止
        // これによって DOTween が誤って「もうないオブジェクト」にアクセスるのを防止
        DOTween.Kill(_healthImage);
        DOTween.Kill(_burnImage);

        // Unity の Destroy() は即座にオブジェクトを削除するのではなく、
        // 次のフレーム終了時に削除される仕組みがある
        // Unity では Destroy(obj) を呼んでも その場ではすぐにオブジェクトを消さず、
        // 「このフレームの終了時に削除予定」としてマークされるだけ
        // なぜか?
        // これは「即時削除してしまうと、他の処理がまだこのオブジェクトを参照している可能性がある」ため
        // これを避けるため、このフレームの最後に消えるという仕様になっている

        // バインドとは   ?参照
        // healthImage は消えても、Tween は DOTween 側に「自分の仕事がまだ終わってない」として残っている
        // でももう対象がないのでエラーになる、というわけ
    }
}

