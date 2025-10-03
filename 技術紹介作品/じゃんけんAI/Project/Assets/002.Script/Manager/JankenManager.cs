/// <summary>
/// ����񂯂�̗���
/// �@�o��������߂�
/// �A�o��
/// �B�W���b�W
/// �C���q�Ȃ�@����
/// </summary>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum JankenState
{
    Wait,       // �҂�����(�v�l����)
    Start,      // "�ŏ��̓O�["
    Play,       // "����񂯂�" ? "�|��"�I
    Judge,      // ���s����
    Rematch,    // "��������" ? "����I"
    Exit         // �����
}

public class JankenManager : Singleton<JankenManager>
{
    /// <summary> AI��؂�ւ������ </summary>
    private PatternBasedAISelector aiSelector = new PatternBasedAISelector();


    /// <summary> ��������͂��[�Ȃǂ̊|�������ڂ����� </summary>
    [SerializeField] private float nextActionIntervalTime = 1f;
    [SerializeField] private NpcHandIconDisplay npcHandIconDisplay;

    private JankenState jankenState;
    private IHandDecider player;
    private IHandDecider npc;
    private bool isPlayerWin = false;   // player�̏������H

    private int turnCount = 0; // �^�[�������Ǘ�


    public JankenState CurrentState => jankenState;

    protected override void Init()
    {
        base.Init();
        // ������
        jankenState = JankenState.Start;

        player = FindObjectOfType<JankenPlayer>();
        npc = FindObjectOfType<JankenNPC>();


        // �ŏ��̓����_��AI���Z�b�g
        (npc as JankenNPC).AI(new RandomAI());
    }

    void Update()
    {
        PlayJanken();

        Debug.Log("���݂̃X�e�[�g�F" + jankenState);
        Debug.Log("���݂�AI�F" + aiSelector.GetCurrentStrategy());
        Debug.Log("���݂�AI�F" + aiSelector.GetCurrentAI());
    }

    /// <summary>
    /// ����񂯂�̐i�s����
    /// </summary>
    void PlayJanken()
    {
        switch (jankenState)
        {
            case JankenState.Start: // "�ŏ��̓O�["
                StartCoroutine(OnStart());
                break;

            case JankenState.Play:  // "����񂯂�"
                // �����؂�ւ��ŃR���[�`�����d���s�h�~�I
                jankenState = JankenState.Wait;
                OnPlay();
                break;

            case JankenState.Judge: // "����"
                // �����؂�ւ��ŃR���[�`�����d���s�h�~�I
                jankenState = JankenState.Wait;
                StartCoroutine(OnJudge());
                break;

            case JankenState.Rematch:// "��������"
                // �����؂�ւ��ŃR���[�`�����d���s�h�~�I
                jankenState = JankenState.Wait;
                OnRematch();
                break;

            case JankenState.Exit:   // �Q�[���I�����ǂ�������
                OnExit();
                break;
            case JankenState.Wait:
                // �I�𒆁A�I�����ԉ������Ȃ�
                break;
        }
	}

    protected override void OnRelease()
    {
        base.OnRelease();
        // ��������Ȃǂ��K�v�Ȃ炱���ɏ���
    }



    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////
    #region �e��Ԃ̏���

    /// <summary> "�ŏ��̓O�[" </summary>
    IEnumerator OnStart()
    {
        UIManager.Instance.SetCallText(jankenState);  // UI�Ɋ|�����̃e�L�X�g��\��������
        yield return new WaitForSeconds(nextActionIntervalTime);
        jankenState = JankenState.Play;
    }

    /// <summary> "����񂯂�" �� "�ۂ�I" </summary>
    void OnPlay()
    {
        //PlayJankenState(JankenState.Play);
        StartCoroutine(PlayJankenState(JankenState.Play)); // �� ??���������OK�I
    }

    /// <summary> ���� </summary>
    IEnumerator OnJudge()
    {
        GameResult gameResult;

        if (player.IsDraw(npc))
        {
            // ��������������
            yield return new WaitForSeconds(nextActionIntervalTime);
            gameResult = GameResult.Draw;
            GameManager.Instance.drawCount++;

            jankenState = JankenState.Rematch;
        }
        else
        {
            // ���s������
            isPlayerWin = player.IsAdvantageous(npc);
            // ���s�ɉ����������������iUI�\���Ȃǁj
            
            // ��������Win�A��������Lise����
            gameResult = isPlayerWin ? GameResult.Win : GameResult.Lose;

            if (isPlayerWin)
            {
                GameManager.Instance.winCount++;
            }
            else
            {
                GameManager.Instance.loseCount++;
            }

            // ���ʂ�\��
            UIManager.Instance.SetResultText(isPlayerWin, true);

            yield return new WaitForSeconds(nextActionIntervalTime);

            // ���ʂ��\���ɂ���
            UIManager.Instance.SetResultText(isPlayerWin, false);

            jankenState = JankenState.Exit;
        }


        // ��т𗚗��ɒǉ�
        JankenHistoryManager.Instance.AddHistory(player.SelectHand, npc.SelectHand, gameResult);

        // �Q�[�����i�񂾌�A�^�[�������J�E���g�A�b�v
        turnCount++;

        // �v���C���[�̎�̗����Ɋ�Â���AI��؂�ւ���
        if (turnCount >= 5)
        {
            var history = JankenHistoryManager.Instance.GetAllHistory();
            var selectedAI = aiSelector.SelectAI(history);
            (npc as JankenNPC).AI(selectedAI); // �����Ɋ�Â���AI��؂�ւ�
        }
        else
        {
            // 5�^�[�������̓����_��AI
            (npc as JankenNPC).AI(new RandomAI());
        }

    }

    /// <summary> "��������" �� "����I" </summary>
    void OnRematch()
    {
        //PlayJankenState(JankenState.Rematch);
        StartCoroutine(PlayJankenState(JankenState.Rematch));
    }

    void OnExit()
	{
        // ���������邩�H
        if (GameManager.Instance.CheckGameFinish())
        {
            // �I��
            UIManager.Instance.EndText(isPlayerWin);
        }
        else
        {
            // �܂��I���Ȃ�
            jankenState = JankenState.Start;
        }
    }

    IEnumerator PlayJankenState(JankenState state)
    {
        if (!IsPlayableState(state)) yield break;

        // "����񂯂�"�@or "��������"
        // �܂�������߂Ă��Ȃ�
        UIManager.Instance.SetCallText(state,false);  // UI�Ɋ|�����̃e�L�X�g��\��������

        // ���͑҂��A���͂��ꂽ�� "��" �ɂ���
        yield return StartCoroutine(player.DecideHand());
        yield return StartCoroutine(npc.DecideHand());
        // ���͂����܂ł͎��ɂ͂����Ȃ�

        // �� "�ۂ�I"�@or "����I"
        // ����������߂�
        UIManager.Instance.SetCallText(state, true);  // UI�Ɋ|�����̃e�L�X�g��\��������
        // �G�̎���o��
        npcHandIconDisplay.ShowIcon(npc.SelectHand);

        yield return new WaitForSeconds(nextActionIntervalTime);
        // ��\���ɂ���
        //npcHandIconDisplay.HideIcon(npc.SelectHand);
        npcHandIconDisplay.HideAllIcons();

        //yield return new WaitForSeconds(2);
        jankenState = JankenState.Judge;
    }
    #endregion
    //�R���[�`������
    //������ �Ӗ�
    //yield return null; 1�t���[���҂�
    //yield return new WaitForSeconds(1); �w�肵���b���҂�
    //yield return new WaitUntil(() => ����); ��������������܂ő҂�
    //yield return new WaitWhile(() => ����); �������������Ă���ԑ҂�
    //yield break; �R���[�`����r���ŏI������

    /// <summary>
    /// Play����Rematch�ȊO�Ŏg�p���Ӑ}���Ă��Ȃ�����
    /// ����񂯂񂪂ł��邩�ǂ����𔻕ʂ������
    /// </summary>
    /// <param name="state">���݂̏��</param>
    /// <returns>�ł��邩�ǂ���</returns>
    bool IsPlayableState(JankenState state)
    {
        return state == JankenState.Play || state == JankenState.Rematch;
    }

    /// <summary>
    /// AI�̐؂�ւ�����
    /// </summary>
    /// <param name="isPost5Turns">�^�[������5�^�[���ȏォ�ǂ���</param>
    //public void SwitchAI(bool isPost5Turns)
    //{
    //    // 5�^�[���ڈȍ~�́A�p�^�[���\���^AI�ɐ؂�ւ���
    //    if (isPost5Turns)
    //    {
    //        (npc as JankenNPC).AI(new PatternPredictionAI()); // �v���C���[�̎�̃p�^�[����\������AI
    //    }
    //    else
    //    {
    //        (npc as JankenNPC).AI(new RandomAI()); // �����_��AI
    //    }
    //}

}
