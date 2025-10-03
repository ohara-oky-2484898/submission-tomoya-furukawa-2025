using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �v���C���[�̍Ō�̎�ɕ��������o��AI
/// ��Ƀ{�^���̔z�u�E���珇�ɉ����Ă���Ƃ��Ɏg��
/// ���т�(�p�[�A�`���L�A�O�[)�Ȃ̂�
/// ���̏ꍇ�p�[�̎��̓`���L������̂Ńp�|�ɕ�����O�[
/// ��Ԃ��΃`���L�ɏ��Ă�
/// </summary>
public class LoseToLastPlayerMoveAI : IJankenAI
{
    public JankenHand Decide(IReadOnlyList<JankenHistory> history)
    {
        JankenHand lastPlayerHand = JankenHistoryManager.Instance.GetLastPlayerHand();

        // �v���C���[�̎�ɕ�����i���s���ȁj���Ԃ�
        return JankenLogic.DisadvantageousHand(lastPlayerHand);
    }
}
