/// <summary>
/// ����񂯂�̃��[���Ɋւ������
/// </summary>
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class JankenLogic
{

    /// <summary>
    /// ����񂯂�̏��s�����߂邾���̂���
    /// </summary>
    /// <param name="myHand">�����̎�</param>
    /// <param name="opponentHand">����̎�</param>
    /// <returns> ���s </returns>
    public static bool IsAdvantageous(JankenHand myHand, JankenHand opponentHand)
    {
        switch (myHand)
        {
            case JankenHand.Rock:
                // �������O�[�̎��A���肪�`���L�Ȃ�L��
                return opponentHand == JankenHand.Scissors;
            case JankenHand.Scissors:
                // �������`���L�̎��A���肪�p�[�Ȃ�L�������
                return opponentHand == JankenHand.Paper;
            case JankenHand.Paper:
                // �������p�[�̎��A���肪�O�[�Ȃ�L�������
                return opponentHand == JankenHand.Rock;

            default:
                // ��O
                return false;
        }
        //return false;
    }

    /// <summary>
    /// �����������ǂ������߂邾���̂���
    /// </summary>
    /// <param name="player">�����̎�</param>
    /// <param name="opponent">����̎�</param>
    /// <returns> �����������ǂ��� </returns>
    public static bool IsDraw(JankenHand myHand, JankenHand opponentHand)
    {
        return myHand == opponentHand;
    }


    /// <summary>
    /// �󂯎������ɑ΂��ėL���Ȏ��Ԃ�
    /// </summary>
    /// <param name="hand">�󂯎���</param>
    /// <returns>�󂯎������ɗL���Ȏ�</returns>
    public static JankenHand AdvantageousHand(JankenHand hand)
    {
        switch (hand)
        {
            case JankenHand.Rock:
                return JankenHand.Paper;    // �O�[�ɂ̓p�[���L��
            case JankenHand.Paper:
                return JankenHand.Scissors; // �p�[�ɂ̓`���L���L��
            case JankenHand.Scissors:
                return JankenHand.Rock;     // �`���L�ɂ̓O�[���L��
            default:
                return hand;  // ���̏ꍇ�i���蓾�Ȃ����ꉞ�j
        }
    }
    
    
    /// <summary>
    /// �󂯎������ɑ΂��ĕs���Ȏ��Ԃ�
    /// </summary>
    /// <param name="hand">�󂯎���</param>
    /// <returns>�󂯎������ɕs���Ȏ�</returns>
    public static JankenHand DisadvantageousHand(JankenHand hand)
    {
        switch (hand)
        {
            case JankenHand.Rock:
                return JankenHand.Scissors;    // �O�[�ɂ̓`���L���s��
            case JankenHand.Paper:
                return JankenHand.Rock; // �p�[�ɂ̓O�[���s��
            case JankenHand.Scissors:
                return JankenHand.Paper;     // �`���L�ɂ̓p�[���s��
            default:
                return hand;  // ���̏ꍇ�i���蓾�Ȃ����ꉞ�j
        }
    }



}

