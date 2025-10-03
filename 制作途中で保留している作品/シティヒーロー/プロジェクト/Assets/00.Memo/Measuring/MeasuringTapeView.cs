using UnityEngine;


// スプラと同じようにする！
// ど真ん中のレティクルと
// 実際に当たっているれティックる二つを用意する！
public class MeasuringTapeView : MonoBehaviour
{
    [SerializeField] private Transform _tape; // 実際に伸びる部分
    [SerializeField] private float _speed = 10f;
    [SerializeField] private float _maxDistance = 10f;

    private MeasuringTapeModel _model;
    private Transform _baseTransform;
    // 全体の長さのうち、内部に残しておく割合（飛び出すぎないように）
    private float _innerOffsetRatio = 0.3f;

    public float Speed => _speed;

    void Start()
    {
        _model = new MeasuringTapeModel(_maxDistance);
        _baseTransform = _tape.transform;
        UpdateVisual();
    }

    void Update()
    {
        UpdateVisual();
    }

    public void ApplyLengthChange(float delta)
    {
        if (delta > 0)
            _model.Extend(delta);
        else if (delta < 0)
            _model.Retract(-delta);
    }

    private void UpdateVisual()
    {
        float length = _model.CurrentLength;
        Vector3 baseScale = _baseTransform.localScale;
        Vector3 basePos = _baseTransform.localPosition;

        // 長さがあるときは → スケールと位置を補正して伸ばす
        // テープをZ方向にだけ伸ばす
        _tape.localScale = new Vector3(baseScale.x, baseScale.y, length);
        // 5　伸びたなら2くらい
        // 10　伸びたなら3くらい
        // 20　伸びたなら6くらい
        // の数値がzに入ってほしい
        // 1のときは1でそれ以下になってほしくない
        basePos.z = Mathf.Max(length * _innerOffsetRatio, 1f);
        _tape.localPosition = basePos;

    }
}
