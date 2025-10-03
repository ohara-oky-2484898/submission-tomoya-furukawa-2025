using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �����Ƃ������o���ꂽ��ɏ�����o��
/// �킩��Ȃ��Ȃ������p�I
/// </summary>
public class PatternAI : IJankenAI
{
    public JankenHand Decide(IReadOnlyList<JankenHistory> history)
    {
        if (history.Count == 0)
            return (JankenHand)Random.Range(0, (int)JankenHand.Num);

        int[] handCounts = new int[(int)JankenHand.Num];

        foreach (var h in history)
        {
            handCounts[(int)h.PlayerHand]++;
        }

        // ��ԑ����o���ꂽ���T��
        int mostCommon = 0;
        for (int i = 1; i < handCounts.Length; i++)
        {
            if (handCounts[i] > handCounts[mostCommon])
                mostCommon = i;
        }

        // ���̎�ɏ�����o��
        return JankenLogic.AdvantageousHand((JankenHand)mostCommon);
    }
}

