using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//UI�Ǘ�: �Q�[����UI�i���[�U�[�C���^�[�t�F�[�X�j���Ǘ�����
//�uUIManager�v�́A�Q�[���̃��j���[��|�b�v�A�b�v�A
//�C���^�[�t�F�[�X�̐؂�ւ��Ȃǂ𐧌䂵�܂��B

//UIManager�̐ӔC�͈͂Ƃ́H
//UI�̕\���E��\���Ǘ�: UI�v�f�i�{�^���A�e�L�X�g�A�|�b�v�A�b�v�A���j���[�Ȃǁj
//�̕\���A��\���A�A�j���[�V�����Ȃǂ��Ǘ����܂��B

//���[�U�[����̓��͏����̓]��: ���[�U�[���{�^�����������Ƃ���A
//UI�v�f�ɓ��͂��s�����ۂɁA���̓��͂�K�؂ȃQ�[�����W�b�N�i����Manager��N���X�j
//�ɓ]�����܂��B

public enum HUDAlignment
{
    Center, // ��������
    Left,   // ���l��
    Right   // �E�l��
}
/// <summary>
/// �\����\���̊Ǘ�
/// animation�Ȃǂ̊Ǘ�
/// ���͏����A�{�^���Ȃǂ̓��͓]��
/// �Q�[���U���ɉ�����UI�̐؂�ւ�
/// </summary>
public class UIManager : Singleton<UIManager>
{
    [Header("HUD�v���n�u�Ɛe")]
    [SerializeField] private CharacterHUD _hudPrefab;
    [SerializeField] private RectTransform _alliesParent; // ����HUD��z�u����UI�̐e�iHorizontalLayoutGroup�Ȃǁj
    [SerializeField] private RectTransform _enemiesParent; // �GHUD�p�̐e
    [SerializeField] private Text _modeText;
    // �s�����\���p��Text��Inspector����Z�b�g
    [SerializeField] private Text _actionOrderText; 
    [SerializeField] private float _deathFadeDuration = 1.0f;
    [Header("Retry�_�C�A���O")]
    [SerializeField] private GameObject _retryDialogPanel;
    [SerializeField] private Button _yesButton;
    [SerializeField] private Button _noButton;

    private bool _inputReceived = false;
    private bool _retry = false;

    private List<CharacterHUD> _activeHUDs = new List<CharacterHUD>();

    private readonly string _autoModeText = "�I�[�g���[�h��\n�蓮���[�h�ɐ؂�ւ�\n�uSpace�vkey";
    private readonly string _manualModeText = "�蓮���[�h��\n�uT�vkey�F����\n �I�[�g���[�h�ɐ؂�ւ�\n�uSpace�vkey";
    protected override void Init()
    {
        // �K�v�Ȃ珉��������
        base.Init();
        _modeText.text = _manualModeText;
        _actionOrderText.text = "�{�^�����͑҂��F�܂��n�܂��Ă��܂���";
        _retryDialogPanel.SetActive(false);
    }

    /// <summary>
    /// ���[�h���b�Z�[�W�̐؂�ւ�
    /// </summary>
    /// <param name="isAutoMode">�I�[�g���[�h�Ȃ�true</param>
    public void SetModeMessage(bool isAoutMode)
	{
        // �I�[�g�Ȃ�true
        _modeText.text = isAoutMode ? _autoModeText : _manualModeText;
    }

    /// <summary>
    /// �����E�G��HUD�𐶐����ĕ��ׂ�
    /// </summary>
    public void CreateHUDs(List<IBattler> battlers)
    {
        ClearHUDs();
        // �`�[�����Ƃɕ�����
        List<IBattler> allies = battlers.FindAll(b => b.Team == Team.Ally);
        List<IBattler> enemies = battlers.FindAll(b => b.Team == Team.Enemy);

        ArrangeHUDs(allies, _alliesParent, HUDAlignment.Left);
        ArrangeHUDs(enemies, _enemiesParent, HUDAlignment.Right);
    }

    /// <summary>
    /// �ΏۃL������HUD���X�V���鏈��
    /// HP,MP�̑���
    /// </summary>
    public void UpdateHUD(IBattler battler)
    {
        // �ΏۃL������HUD�������X�V���鏈��
        var hud = _activeHUDs.Find(h => h.Battler == battler);
        if (hud != null)
        {
            hud.UpdateUI(); // UI�X�V���\�b�h
        }
    }

	/// <summary>
	/// �S�Ă�HUD���폜
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
    /// �w��̃o�g���[�̍s�����̃G�t�F�N�g���Ăяo��
    /// </summary>
    /// <param name="battler">�w��̃o�g���[</param>
    public void PlayFocusEffectFor(IBattler battler)
    {
        if (battler == null || !battler.IsAlive) return;
        _activeHUDs.Find(h => h.Battler == battler).PlayFocusEffect();
    }

    /// <summary>
    /// �w��̃o�g���[�̎��S���̃G�t�F�N�g���Ăяo��
    /// </summary>
    /// <param name="battler">�w��o�g���[</param>
    public void PlayDeathEffectFor(IBattler battler)
    {
        _activeHUDs.Find(h => h.Battler == battler).PlayDeathFadeEffect(_deathFadeDuration);
    }

    /// <summary>
    /// �s�������X�g���󂯎��A�e�L�X�g�ɐ��`���ĕ\������
    /// </summary>
    /// <param name="turnOrder">�s�����̃��X�g</param>
    public void DisplayActionOrder(List<IBattler> turnOrder, string strategyName, int turnCount)
    {
        if (turnOrder == null || turnOrder.Count == 0)
        {
            _actionOrderText.text = "�s�����Ȃ�";
            return;
        }

        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        sb.AppendLine($"=== �s�����i{strategyName}�j�i{turnCount}���ځj===");

        int order = 1;
        foreach (var battler in turnOrder)
        {
            if (battler.ReservedAction == null) continue;
            var actionData = battler.ReservedAction.GetActionData();

            string actionName = actionData != null ? actionData.Name : "�s���ȍs��";
            string targetDisplay;

            if (actionData == null || actionData.Targets == null || actionData.Targets.Count == 0)
            {
                targetDisplay = "�Ȃ�";
            }
            else if (actionData.Targets.Count == 1)
            {
                targetDisplay = actionData.Targets[0].Name; // �P�̑Ώ�
            }
            else
            {
                targetDisplay = "�S��"; // �����Ώ�
            }

            sb.AppendLine($"{order}: {battler.Name}(�f�����F{battler.Status.Speed})�^" +
				$"�A�N�V����({actionName})\n�@�@�@�^�[�Q�b�g�F�@{targetDisplay}");
            order++;
        }

        _actionOrderText.text = sb.ToString();
    }


    /// <summary>
    /// HUD��e�I�u�W�F�N�g���ɔz�u����
    /// </summary>
    private void ArrangeHUDs(List<IBattler> teamBattlers, RectTransform parent, HUDAlignment alignment)
    {
        float spacing = 150f; // HUD�Ԃ̋����i�����j
        int count = teamBattlers.Count;

        // �e��RectTransform����J�n�ʒu���擾
        Vector2 parentPosition = parent.anchoredPosition;

        // �z�u���@�ɉ����ĊJ�n�ʒu��ݒ�
        float startX = 0;
        if (alignment == HUDAlignment.Center) // ��������
        {
            startX = -(spacing * (count - 1)) / 2f; // �e�̈ʒu����ɒ�������
        }
        else if (alignment == HUDAlignment.Left) // ���l��
        {
            startX = 0; // �ŏ��̃L������e�̈ʒu(0,0)�ɔz�u
        }
        else if (alignment == HUDAlignment.Right) // �E�l��
        {
            startX = -(spacing * (count - 1)); // �ŏ��̃L������e�̉E�[�ɔz�u
        }
        else
        {
            Debug.LogError("�s���Ȕz�u�w��ł�");
            return;
        }

        for (int i = 0; i < count; i++)
        {
            // �V����HUD�𐶐�
            CharacterHUD hud = Instantiate(_hudPrefab, parent);
            hud.Setup(teamBattlers[i]);

            // HealthGaugeEffect ���������ݒ肳��Ă��邩�`�F�b�N
            if (!hud.GaugeEffect)
            {
                Debug.LogError("HUD ���쐬����܂������AHealthGaugeEffect ���ݒ肳��Ă��܂���I");
                continue; // gaugeEffect ���ݒ肳��Ă��Ȃ��ꍇ�A����HUD���X�L�b�v
            }

            _activeHUDs.Add(hud);

            // �z�u: �e�I�u�W�F�N�g�̈ʒu����ɁA�w�肳�ꂽ�z�u���@�Ŕz�u
            RectTransform rt = hud.GetComponent<RectTransform>();

            // startX����150px�����炵�Ĕz�u
            rt.anchoredPosition = new Vector2(startX + (i * spacing), 0);
        }
    }



    /// <summary>
    /// ���g���C�_�C�A���O��\�����A���[�U�[�̓��͂�҂�
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

        // ���͂�����܂ő҂�
        yield return new WaitUntil(() => _inputReceived);

        _retryDialogPanel.SetActive(false);
        onDecision?.Invoke(_retry);
    }
}

//��F�|�[�Y��ʂ�\������
//�|�[�Y��ʂ̕\��: �Q�[�����ꎞ��~�����ۂɁA�|�[�Y���j���[��\������B

//���̃N���X�Ƃ̂����: �Q�[�����|�[�Y���ꂽ���ǂ�����**
//GameManager�i�܂���BattleManager�j���m���Ă��邪�A
//�|�[�Y��ʂ����ۂɕ\������̂�UIManager** �̐ӔC�ł��B


//��F�Q�[���I�[�o�[���̍Ď��s����
//�Q�[���I�[�o�[���ɁuRetry���܂����H�v�Ƃ���UI��\�����镔���́A
//UIManager�̎d���ł����A���ۂɃ��g���C����
//�i�Q�[���̍ăX�^�[�g���Ԃ̃��Z�b�g�j�̓Q�[�����W�b�N�̒S���ł��B

//UIManager�̐ӔC: �uRetry���܂����H�v�Ƃ����_�C�A���O�̕\��
//BattleManager�i�܂���GameManager�j�̐ӔC: ���g���C���̃Q�[���̏�Ԃ̃��Z�b�g��V�[���̍ă��[�h


//��F�Q�[���̐i�s�󋵂ɉ�����UI�̍X�V
//�Q�[�����i�s�����A�Q�[���I�[�o�[���A�|�[�Y�����A�Ȃǂ̃Q�[���̏�Ԃɉ�����
//UI��ύX����̂�UIManager�̎d���ł����A
//���̏�Ԏ��̂̓Q�[���̃��W�b�N�iGameManager��BattleManager�j�ɊǗ������܂��B
