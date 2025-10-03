using System.Collections.Generic;
using UnityEngine;

public class JankenHistoryManager : Singleton<JankenHistoryManager>
{
    private List<JankenHistory> historyList = new List<JankenHistory>();

    /// <summary>
    /// ������ǉ�����
    /// </summary>
    public void AddHistory(JankenHand playerHand, JankenHand npcHand, GameResult result)
    {
        JankenHistory history = new JankenHistory(playerHand, npcHand, result);

        historyList.Add(history);
        //Debug.Log($"����ǉ�: �v���C���[({playerHand}) vs NPC({npcHand}) �� ����: {result}");
    }

    /// <summary>
    /// �������擾����i�O���Q�Ɨp�j
    /// ReadOnly(�ǂݎ���p)��n�����Ƃň��S��S��
    /// </summary>
    public IReadOnlyList<JankenHistory> GetAllHistory()
    {
        return historyList.AsReadOnly();
    }

    /// <summary>
    /// ������S�폜
    /// </summary>
    public void ClearHistory()
    {
        historyList.Clear();
        //Debug.Log("�������N���A���܂���");
    }

    // �K�v������Εۑ��⃍�[�h���ǉ��ł���i��FJSON�ŕۑ��Ȃǁj

    /// <summary>
    /// �v���C���[���Ō�ɏo��������擾����
    /// </summary>
    /// <returns>�Ō�ɏo�����v���C���[�̎�</returns>
    public JankenHand GetLastPlayerHand()
    {
        if (historyList.Count == 0)
        {
            return JankenHand.Rock; // �f�t�H���g�l��Ԃ��i�����������ꍇ�j
        }

        // C#8.0�ȍ~�g����悤�ɂȂ����炵��
        //return historyList[^1].PlayerHand;
        // �h�C���f�b�N�X�\���h�ł��g���Ȃ��̂�
        return historyList[historyList.Count - 1].PlayerHand;
    }

    protected override void Init()
    {
        base.Init();
        //Debug.Log("JankenHistoryManager ������");
    }

    protected override void OnRelease()
    {
        base.OnRelease();
        //Debug.Log("JankenHistoryManager �I��");
    }
}
