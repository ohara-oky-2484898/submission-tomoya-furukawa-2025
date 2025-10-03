//�@�퓬�J�n�i�������j
//�A�^�[���J�n�i�v���C���[ or AI�j
//�B�s���I���E���͎�t�iAI�Ȃ玩���j
//�C�s�����s�i�U���E�h��E�񕜂Ȃǁj
//�D���s����
//�E���̃^�[���ֈڍs or �퓬�I��

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// BattleManager �͐퓬����ݒ�E�w�����邾���̊Ǘ��N���X
/// </summary>
public class BattleManager : Singleton<BattleManager>
{
    /// <summary> �t�B�[���h </summary>
    [Header("���̍s�����n�܂�܂ł̌��Ԏ���")]
    [SerializeField] private float _nextActionInterval = 0.7f;
    private float _nextActionIntervalOriginal;

    /// <summary>
    /// �^�[���̏��ԊǗ��@�\
	/// �o�g���Ǘ�����Ȃ�^�[���Ǘ��@�\�������Ă�
    /// </summary>
	private TurnManager _turnManager = new TurnManager();

    /// <summary>
    /// �o�g���[�S���̃��X�g
	/// IBattler�ɐw�c�����������̂ňꊇ�Ǘ����\�ɂȂ���
    /// </summary>
	private List<IBattler> _allBattlers;
    private StageData _nowStageData;

    /// <summary> �t���O�֘A </summary>
    private bool _once = false;
    private bool _isTurnInProgress = false;
    private bool _isAutoMode = false; // �� �I�[�g���ǂ����̃t���O
    private bool _isRetryDecide = true;


    /// <summary> �v���p�e�B </summary>
    public List<IBattler> AllBattlers => _allBattlers;

    /// <summary> singleton�̐ݒ� </summary>
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
			// �L�����̏�����
			InitializeBattlers();
			// HUD������UIManager�ɔC����
			UIManager.Instance.CreateHUDs(_allBattlers);
			_once = true;
		}
        // T�L�[�i�蓮���[�h�ł̂݃^�[����i�߂�j
        if (!_isAutoMode && Input.GetKeyDown(KeyCode.T))
        {
            if(!_isTurnInProgress)
            {
                StartCoroutine(PlayTurn());
            }
            else
            {
                Debug.Log("�܂��^�[���������ł��B���΂炭�҂��Ă��������B");
            }
        }

        // �X�y�[�X�L�[�ŃI�[�g���[�h�� ON/OFF ��؂�ւ���
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _isAutoMode = !_isAutoMode;
            UIManager.Instance.SetModeMessage(_isAutoMode);
            Debug.Log($"�I�[�g���[�h: {(_isAutoMode ? "ON" : "OFF")}");
        }

        // �I�[�g���[�h���͏���Ƀ^�[������
        // �I�[�g���ł��A�^�[�����I�������
        if (_isAutoMode && !_isTurnInProgress)
        {
            StartCoroutine(PlayTurn());
        }
    }



    /// <summary>
    /// �Q�[�����x���������čĊJ
    /// �X�e�[�W1����2�Ɉړ�����Ƃ�
    /// �����������Ȃǂ��Ă�ł�����
    /// </summary>
    public void GameReStart()
    {
        _nowStageData = GameManager.Instance.NowStageData();

        // �O��̃X�v���C�g��HUD���N���A�i�K�v�Ȃ�j
        UIManager.Instance.ClearHUDs();
        //ClearAllSprites();

        // �V�K�L�����Đ���
        InitializeBattlers();
        UIManager.Instance.CreateHUDs(_allBattlers);
        _turnManager.TurnReset();
    }

    /// <summary>
    /// �o�g���̃e���|��{���ɂ���֐�
    /// �{���Č����Ă��s����Interval��Z�����邾��
    /// </summary>
    public void SpeedUpBattle()
    {
        _nextActionInterval = _nextActionInterval / 2;
    }

    /// <summary>
    /// �o�g���̃e���|�����Z�b�g����֐�
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
    /// 1�^�[���̏������R���[�`���Ŏ��s���A�s���̊Ԃɑҋ@������
    /// </summary>
    private IEnumerator PlayTurn()
    {
        if (!_isRetryDecide) yield break;
        _isTurnInProgress = true;

        // �@�S���̍s���������߂�
        // ���Ԃɕ��ѕς������̂��i�[����z��
        //List<IBattler> turnOrder = _turnManager.GetTurnOrder(_nowStageData.TurnOrderStrategy, _allBattlers);
        var turnOrderStrategy = _nowStageData.TurnOrderStrategy;
        List<IBattler> turnOrder = _turnManager.GetTurnOrder(turnOrderStrategy, _allBattlers);



        // �A���ԂɑS���̍s�������߂�
        foreach (var battler in turnOrder)
        {
            battler.DecideAction(_allBattlers);
        }

        // �s�������X�g�ƌ��肵���s����UI�ɕ\��
        UIManager.Instance.DisplayActionOrder(turnOrder, turnOrderStrategy.DisplayName, _turnManager.TurnCount);
        yield return new WaitForSeconds(_nextActionInterval);

        // �B���ԂɑS���̍s�������s
        // �^�[���J�n
        Debug.Log($"===== ���s�� =====");
        foreach (var battler in turnOrder)
        {
            var list = battler.GetEnemies(_allBattlers);
            if (list.Count == 0 || !battler.IsAlive)
                continue;

            Debug.Log($"{battler.Name} �̍s���J�n");

            UIManager.Instance.PlayFocusEffectFor(battler);

            yield return new WaitForSeconds(_nextActionInterval);

            battler.ExecuteAction();

            // �s����������悤��1�b�ҋ@
            yield return new WaitForSeconds(_nextActionInterval);
        }

        // �ǂ��炩�S�ł���
        // ���������܂ŌJ��Ԃ����s
        if (BattleSystem.TryCheckBattleResult(_allBattlers, out BattleResult result))
        {
            // ����������
            if (result == BattleResult.Win)
            {
                if(GameManager.Instance.IsGameClear())
				{
                    // Clear����
                    GameManager.Instance.ExitGame();
                }
            }
            else if (result == BattleResult.Lose)
            {
                _isRetryDecide = false;
                // �Ē��킷�邩�I��邩�I��������
                UIManager.Instance.ShowRetry((bool retry) =>
                {
                    if (retry)
                    {
                        GameReStart(); // �Ē���
                    }
                    else
                    {
                        GameManager.Instance.ExitGame(); // �I��
                    }
                    _isRetryDecide = true;
                });
            }
        }

        _isTurnInProgress�@= false;
        _turnManager.NextTurn();
    }
}
