//using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �v���C���[���Ō�ɏo������ɏ�����o��AI
/// �����ƃO�[�Ȃǂ̎��p
/// </summary>
public class CounterLastPlayerMoveAI : IJankenAI
{
    public JankenHand Decide(IReadOnlyList<JankenHistory> history)
    {

        // �v���C���[���Ō�ɏo��������擾
        JankenHand lastPlayerHand = JankenHistoryManager.Instance.GetLastPlayerHand();

        // �\�����ꂽ��ɗL���Ȏ���擾����
        return JankenLogic.AdvantageousHand(lastPlayerHand);
    }
}
