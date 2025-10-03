using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �v���C���[�̍Ō�ɏo������Ɠ�������o��AI�i�~���[AI�j
/// ��ɁA�{�^���̕��я��ŉ����Ă��鑊��Ɏg��
/// ���т�(�O�[�A�`���L�A�p�[)�Ȃ̂�
/// ���̂̏ꍇ�O�[�̎��̓`���L������̂ōŌ�ɏo������Ɠ����O�[���o����
/// �`���L�ɏ��Ă�
/// </summary>
public class MirrorAI : IJankenAI
{
    public JankenHand Decide(IReadOnlyList<JankenHistory> history)
    {
        // �������Ȃ��ꍇ�̓����_���Ɏ���o��
        if (history.Count == 0)
        {
            return (JankenHand)Random.Range(0, (int)JankenHand.Num);
        }

        // �v���C���[�̍Ō�̎���擾���ĕԂ�
        return JankenHistoryManager.Instance.GetLastPlayerHand();
    }
}

