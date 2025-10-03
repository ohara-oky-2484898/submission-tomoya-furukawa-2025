using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �Q�[���̐i�s�󋵂����v���C���[�̏�ԂȂǊǗ�
//�N���X���Q�[���̊J�n�A�I���A�|�[�Y�Ȃǂ𐧌䂷�邱�Ƃ�����

public class GameManager : Singleton<GameManager>
{
    /// <summary> �t�B�[���h </summary>
    [SerializeField] private AIJudgmentCriteriaData _aiCriteriaData;
    [SerializeField] private int _nowGameLevel = 4;

    private int _gameLevelMax;
    private List<StageData> _stageDatasList;

    /// <summary> �v���p�e�B </summary>
    public int NowGameLevel => _nowGameLevel - 1;
    public AIJudgmentCriteriaData AiCriteriaData() => _aiCriteriaData;
    public StageData NowStageData() => _stageDatasList[NowGameLevel];

    /// <summary> singleton�̐ݒ� </summary>
    protected override bool DestroyTargetGameObject => true;

    protected override void Init()
    {
        base.Init();



        // �f�[�^�̓ǂݍ���
        StageLoad();

        // �ő�X�e�[�W�����Z�b�g
        _gameLevelMax = _stageDatasList.Count;
    }

    /// <summary>
    /// �X�e�[�W��ǂݍ���
    /// </summary>
    private void StageLoad()
    {
        //CSV ����L�����N�^�[�f�[�^��ǂݍ���
        _stageDatasList = StageDataLoader.LoadStageData();
    }
    
    /// <summary>
    /// �Q�[�����p�����邩�ǂ����̊m�F�p
    /// �ő僌�x����Clear���Ă�����I��
    /// �܂�����Level������΃��x���A�b�v����Restart
    /// </summary>
    /// <returns>�Q�[�����I�����ǂ���</returns>
    public bool IsGameClear()
	{
        Debug.Log("GameManager�F�Q�[��Clear�I�I");
        // �Q�[��Level���ő�ɓ��B���Ă��邩�m�F����
        // ���B���Ă�����S�N������A���Ă��Ȃ��Ȃ�Level�������đ��s
        if (CheckGameFinish())
        {
            return true;
        }
        else
		{
            // �Q�[��Level��������ď������������Ă�ő��s(�ĊJ)
            NextGameLevel();
            BattleManager.Instance.GameReStart();
            return false;
        }
    }

    /// <summary>
    /// ���x���A�b�v�p
    /// </summary>
    /// <returns>���݂�Level��1up����������</returns>
    private int NextGameLevel()
	{
        Debug.Log("GameManager�F�Q�[�����x���A�b�v�I�I�@�F" + _nowGameLevel);
        return ++_nowGameLevel;
	}


    /// <summary>
    /// �ŏI���x���ɓ��B���Ă邩�m�F����
    /// �Q�[�����I�����ǂ����m�F
    /// </summary>
    /// <returns>�Q�[�����I�����邩�ǂ���</returns>
    private bool CheckGameFinish()
    {
        return _nowGameLevel == _gameLevelMax;
    }

    public void ExitGame()
	{
        // �Q�[���N���A
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
    }
}
