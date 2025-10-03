using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.UI;
using DG.Tweening;

// HUD ���w�b�h�A�b�v�f�B�X�v���C
// �Q�[����ʏ�ɏ�ɕ\������Ă���v���C���[�ɏ���񋟂���UI�v�f�̂���
public class CharacterHUD : MonoBehaviour
{

    //- �L�����N�^HUD�i�S�̂��Ǘ�����e�I�u�W�F�N�g�j                        // this.gameobject���g������ɂ�����
    //  - �L�����摜�iImage�j                                                // charaImage���Ή�
    //  - Panel�i�L�����N�^�[���̔w�i�ƂȂ�ePanel�j                       // panelTransform.gameibject���Ή�
    //    - HP�Q�[�W�iImage�j                                                // gaugeEffect��this.gameobject���Ή�
    //      - �w�i�i�� Image�j                                               // gaugeEffect�̃t�B�[���hhealthImage���Ή�
    //      - HP�\�������i�� Image�j  �� `Fill Amount`���g���ē��I��HP��ύX // gaugeEffect�̃t�B�[���hburnImage���Ή�
    //      - HP�̃e�L�X�g�iText: "current/max"�j                            // hpText���Ή�
    //    - MP�̃e�L�X�g�iText: "current/max"�j  �� MP�̏�Ԃ�\��           // mpText���Ή�
    //    - ���O�iText: "���O"�j  �� �|�P��������\��                        // nameText���Ή�

    // ����������
    // charaImage.rectTransform.position��this.gameobject�̃|�W�V�����Ƃ��̂܂ܑΉ�0,0�ɂȂ��Ă���΂�����
    // panelTransform��ύX������
    // �G�Ȃ�E��ɁA�����Ȃ獶����
    // ���炷���͐e�I�u�W�F(this.gameobject�����)
    // ����150�^����80���v���X�}�C�i�X�؂�ւ��Ă͔z�u

    // �L�����摜�ȊO�́AHP�Q�[�W���OHP,MP�e�L�X�g���܂Ƃ߂��e�I�u�W�F�N�g
    [SerializeField] private RectTransform panelTransform;
    [SerializeField] private Image charaImage;
    
    [Header("���O�␔�l")]
    [SerializeField] private Text nameText;
    [SerializeField] private Text hpText;
    [SerializeField] private Text mpText;

    [Header("HP�Q�[�W���o�R���|�[�l���g")]
    // ���̒���HP�̔w�i��Image�Ɨ�Image���ݒ肳��Ă��邠��
    [SerializeField] private HealthGaugeEffect gaugeEffect;
    public HealthGaugeEffect GaugeEffect { get { return gaugeEffect; } set { gaugeEffect = value; } }

    private IBattler battler;
    public IBattler Battler => battler;  // �� ������ǉ��I

    public void Setup(IBattler battler)
    {
        this.battler = battler;
        nameText.text = battler.Name;
        charaImage.sprite = battler.Sprite;
        Debug.Log($"[Setup] battler: {battler}, ���O{battler.Name},battler.Sprite:�� {battler.Sprite}��, charaImage: {charaImage}");


        // gaugeEffect �� null �łȂ����`�F�b�N
        if (gaugeEffect == null)
        {
            Debug.LogError("gaugeEffect (HealthGaugeEffect) ���ݒ肳��Ă��܂���I");
            return; // gaugeEffect ���ݒ肳��Ă��Ȃ��ꍇ�A�������~�߂�
        }

        UpdateHP(battler.Status.HP);
        UpdateMP(battler.Status.MP);


        // �z�u�����i�G���������ɂ��ʒu���߁j
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

        // �ǂ��炩�� null �܂��� Destroy �ς݂Ȃ珈�����Ȃ�
        if (!gaugeEffect)
        {
            Debug.LogWarning("�����Ɛݒ肵��");
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
        // ��U�T�C�Y���g�債�Ă��猳�ɖ߂�
        transform.DOKill(); // ������ Tween ���E���i�d���h�~�j
        transform.localScale = Vector3.one;

        // �g�偨�߂��i0.3�b���炢�j
        transform.DOScale(1.2f, 0.15f)
            .SetLoops(2, LoopType.Yoyo)
            .SetEase(Ease.OutQuad);
    }

    // ����/�G�ɂ����UI�̈ʒu�𒲐�
    private void AdjustPositionBasedOnCharacter()
    {
        // �Ⴆ�΁Abattler���G�Ȃ�E��A�����Ȃ獶���ɔz�u
        if (battler.Team == Team.Enemy) // IBattler��IsEnemy�v���p�e�B������Ɖ���
        {
            // �G�̏ꍇ�i�E��j
            panelTransform.anchoredPosition = new Vector2(50f, 90f); // �E��ɔz�u
        }
        else
        {
            // �����̏ꍇ�i�����j
            panelTransform.anchoredPosition = new Vector2(-50f, -90f); // �����ɔz�u
        }
    }

    /// <summary>
    /// �L�����N�^�[�����S�����Ƃ��ɌĂяo���F�L�����摜�̃���100�܂Ńt�F�[�h�A�E�g
    /// </summary>
    public void PlayDeathFadeEffect(float duration = 1.0f)
    {
        if (charaImage == null)
        {
            Debug.LogWarning("charaImage���ݒ肳��Ă��܂���I");
            return;
        }

        Color currentColor = charaImage.color;
        float targetAlpha = 100f / 255f; // Unity��Color��0?1�ň���

        // DOTween �� alpha ��ύX
        charaImage.DOFade(targetAlpha, duration)
            .SetEase(Ease.InOutQuad);
    }

}

