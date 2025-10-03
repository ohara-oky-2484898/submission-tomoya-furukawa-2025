using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>
/// HP�Q�[�W�ɃG�t�F�N�g�t���Ŕ��f������R���|�[�l���g
/// </summary>
public class HealthGaugeEffect : MonoBehaviour
{
    /// <summary>HP�Q�[�W�Ɋւ���ϐ�</summary>
    [Header("HP�Q�[�W�ݒ�")]
    [SerializeField] private Image _healthImage;
    [SerializeField] private Image _burnImage;

    /// <summary>���o�Ɋւ���ݒ�</summary>
    [Header("���o�ݒ�")]
    [SerializeField] private float _duration = 0.5f;
    [SerializeField] private float _strength = 20f;
    [SerializeField] private int _vibrate = 100;

    /// <summary>���݂�HP����</summary>
    private float _currentRate = 1f;

    /// <summary>���̃��[�J���ʒu�i�h���ɖ߂��ʒu�j</summary>
    private Vector3 _originalPosition;

    /// <summary>�����������B���̈ʒu���L�^</summary>
    private void Awake()
    {
        _originalPosition = transform.localPosition;
    }

    /// <summary>
    /// HP�Q�[�W�̕\�����X�V���ADOTween�ŉ��o��K�p����
    /// </summary>
    /// <param name="targetRate">�ڕW��HP�����i0?1�j</param>
    public void SetGauge(float targetRate)
    {
        if (Mathf.Approximately(_currentRate, targetRate))
        {
            Debug.Log("�d�����Ă�");
            return; // �l���ς���Ă��Ȃ���Ώ������Ȃ�
        }

        // �ǂ��炩�� null �܂��� Destroy �ς݂Ȃ珈�����Ȃ�
        if (!_healthImage || !_burnImage)
        {
            Debug.LogWarning("HP�Q�[�W�̉摜���ݒ肳��Ă��Ȃ���");
            return;
        }

        transform.localPosition = _originalPosition; // �܂���ʒu�ɖ߂�

        // OnComplete "."�̑O�̏������I�������
        // ����()�̏������s���Ă��������Ƃ�������
        // �����_��
        _healthImage.DOFillAmount(targetRate, _duration).OnComplete(() =>
        {
            // ���o�̊֌W�� "duration" �������l���Ƃ킩��Â炭�Ȃ邽��
            // �����̒l�ɂ��Ă����B
            // �܂��A�����k�����n�܂��Ă��܂��̂��킩��Â炭�Ȃ邽�ߊԂ������Ă���
            // SetDelay(0.5f) 0.5�b�܂��Ă��珈������
            _burnImage.DOFillAmount(targetRate, _duration * 0.5f).SetDelay(0.5f);
        });

        // ���񂱂̎������g�����[�g(�e)�̃I�u�W�F�N�g
        // �̂��ߎ��g���h���ƑS�̂��h���
        // DOShakePosition(�O�̈���)(�h��Ă鎞��)(�h��̋���)(�h��);
        transform.DOShakePosition(_duration * 0.5f, _strength, _vibrate);

        _currentRate = targetRate;
    }

    public void OnDestroy()
	{
        // DOTween �� Safe Mode ���A�I�u�W�F�N�g���j�����ꂽ��ɂ� Tween �������Ă��āA
        // ����ɃA�N�Z�X���悤�Ƃ��ăG���[���L���b�`���Ă�������
        // �I�u�W�F�N�g���������O�ɁA�I�u�W�F�N�g�Ƀo�C���h���ꂽ Tween �𖾎��I�ɒ�~
        // ����ɂ���� DOTween ������āu�����Ȃ��I�u�W�F�N�g�v�ɃA�N�Z�X��̂�h�~
        DOTween.Kill(_healthImage);
        DOTween.Kill(_burnImage);

        // Unity �� Destroy() �͑����ɃI�u�W�F�N�g���폜����̂ł͂Ȃ��A
        // ���̃t���[���I�����ɍ폜�����d�g�݂�����
        // Unity �ł� Destroy(obj) ���Ă�ł� ���̏�ł͂����ɃI�u�W�F�N�g���������A
        // �u���̃t���[���̏I�����ɍ폜�\��v�Ƃ��ă}�[�N����邾��
        // �Ȃ���?
        // ����́u�����폜���Ă��܂��ƁA���̏������܂����̃I�u�W�F�N�g���Q�Ƃ��Ă���\��������v����
        // ���������邽�߁A���̃t���[���̍Ō�ɏ�����Ƃ����d�l�ɂȂ��Ă���

        // �o�C���h�Ƃ�   ?�Q��
        // healthImage �͏����Ă��ATween �� DOTween ���Ɂu�����̎d�����܂��I����ĂȂ��v�Ƃ��Ďc���Ă���
        // �ł������Ώۂ��Ȃ��̂ŃG���[�ɂȂ�A�Ƃ����킯
    }
}

