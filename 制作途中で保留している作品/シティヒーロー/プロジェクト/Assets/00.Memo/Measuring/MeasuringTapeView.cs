using UnityEngine;


// �X�v���Ɠ����悤�ɂ���I
// �ǐ^�񒆂̃��e�B�N����
// ���ۂɓ������Ă����e�B�b�N����p�ӂ���I
public class MeasuringTapeView : MonoBehaviour
{
    [SerializeField] private Transform _tape; // ���ۂɐL�т镔��
    [SerializeField] private float _speed = 10f;
    [SerializeField] private float _maxDistance = 10f;

    private MeasuringTapeModel _model;
    private Transform _baseTransform;
    // �S�̂̒����̂����A�����Ɏc���Ă��������i��яo�����Ȃ��悤�Ɂj
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

        // ����������Ƃ��� �� �X�P�[���ƈʒu��␳���ĐL�΂�
        // �e�[�v��Z�����ɂ����L�΂�
        _tape.localScale = new Vector3(baseScale.x, baseScale.y, length);
        // 5�@�L�т��Ȃ�2���炢
        // 10�@�L�т��Ȃ�3���炢
        // 20�@�L�т��Ȃ�6���炢
        // �̐��l��z�ɓ����Ăق���
        // 1�̂Ƃ���1�ł���ȉ��ɂȂ��Ăق����Ȃ�
        basePos.z = Mathf.Max(length * _innerOffsetRatio, 1f);
        _tape.localPosition = basePos;

    }
}
