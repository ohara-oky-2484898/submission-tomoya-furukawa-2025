/// <summary>
/// ���ʏ����Ȃ�
/// 
/// �g�����\�b�h�p
/// (this IHandDecider self, IHandDecider other)
/// ����������邱�ƂŌĂяo������
/// player.�Z�Z(npc)
/// ���̂悤�ɂ����̃��\�b�h���̂悤�Ɉ�����悤�ɂȂ�I
/// </summary>
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class JankenExtensions
{
    /// <summary>
    /// �����̎肪����ɑ΂��ėL�����ǂ����𔻒肷��g�����\�b�h
    /// </summary>
    /// <param name="self">����</param>
    /// <param name="other">����</param>
    /// <returns> ���s </returns>
    public static bool IsAdvantageous(this IHandDecider self, IHandDecider other)
    {
        return JankenLogic.IsAdvantageous(self.SelectHand, other.SelectHand);
    }

    /// <summary>
    /// �����̎�Ƒ���̎肪�������ǂ����𔻒肷��g�����\�b�h
    /// </summary>
    /// <param name="self">����</param>
    /// <param name="other">����</param>
    /// <returns> �����������ǂ��� </returns>
    public static bool IsDraw(this IHandDecider self, IHandDecider other)
    {
        return JankenLogic.IsDraw(self.SelectHand, other.SelectHand);
    }

    ///// <summary>
    ///// �肪�I�΂��܂ő҂i�R���[�`���p�j�̊g�����\�b�h
    ///// </summary>
    //public static IEnumerator WaitForHandSelection(this IHandDecider self)
    //{
    //    //if (!self.IsDecided && self is JankenNPC)
    //    //{
    //    //    yield return new WaitForSeconds(1.0f); // NPC���Y�މ��o
    //    //    self.DecideHand(); // �����Ō��肳����
    //    //}
    //    yield return new WaitUntil(() => self.IsDecided);
    //}
}

// �R���[�`��
// Unity�ɂ����鏈�������Ԃŕ������čs���d�g��
// �ʏ�̃��\�b�h�� "��C�ɂ��ׂĎ��s�����" ��
// �R���[�`�����g���Γ���̃^�C�~���O�܂ňꎞ��~���Ă���ĊJ�\

//���ʂ̃��\�b�h�F��C�ɂ���ׂ�l�i�~�܂�Ȃ��j
//�R���[�`���F�r���Łu������Ƒ҂��ĂˁI�v���Č����Ă����l

// ��{�̌`
//IEnumerator MyCoroutine()
//{
//    Debug.Log("����1");

//    // 1�b�҂�
//    yield return new WaitForSeconds(1f);

//    Debug.Log("����2");
//}

// StartCoroutine(MyCoroutine());

//������ �Ӗ�
//yield return null; 1�t���[���҂�
//yield return new WaitForSeconds(1); �w�肵���b���҂�
//yield return new WaitUntil(() => ����); ��������������܂ő҂�
//yield return new WaitWhile(() => ����); �������������Ă���ԑ҂�
//yield break; �R���[�`����r���ŏI������

//�V�[���� �g���ǂ���
//���o	UI�Ɂu3...2...1...�X�^�[�g�I�v�ƕ\��
//�A�j���[�V����	�����̊ԂɎ��ԍ����������Ƃ�
//���͑҂�	�v���C���[���{�^���������܂ŏ������~�߂����Ƃ�
//�t�F�[�h�C���E�A�E�g	���Ԃ������ē����x��ς��鏈��
//�G�̍U���p�^�[��	��莞�Ԃ��ƂɍU�����郍�W�b�N

//���ӓ_ ���e
//yield return ���Ȃ��ƈӖ����Ȃ�	���ʂ̊֐��ƕς��Ȃ��Ȃ�
//StartCoroutine ���Ȃ��Ɠ����Ȃ�	�Ăяo����Y���Ɠ����Ȃ�
//MonoBehaviour�ł����g���Ȃ�	���̃N���X�ł͊�{�g���Ȃ�
