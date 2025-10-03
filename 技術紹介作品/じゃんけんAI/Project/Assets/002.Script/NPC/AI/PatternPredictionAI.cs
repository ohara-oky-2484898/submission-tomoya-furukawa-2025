using System.Collections.Generic;
using UnityEngine;

public class PatternPredictionAI : IJankenAI
{
    // KeyValuePair�̑���Ƀ^�v�����g�p
    private Dictionary<(JankenHand, JankenHand), int> transitionCounts = new Dictionary<(JankenHand, JankenHand), int>();

    public JankenHand Decide(IReadOnlyList<JankenHistory> history)
    {
        if (history.Count < 2)
        {
            return (JankenHand)Random.Range(0, (int)JankenHand.Num);
        }

        // �J�E���g������
        transitionCounts.Clear();

        // �u�O��̎�v���u����̎�v�̑g�ݍ��킹���J�E���g
        for (int i = 1; i < history.Count; i++)
        {
            var prev = history[i - 1].PlayerHand;
            var curr = history[i].PlayerHand;
            var key = (prev, curr); // �^�v���Ƃ��Ċi�[

            if (!transitionCounts.ContainsKey(key))
                transitionCounts[key] = 0;

            transitionCounts[key]++;
        }

        // �Ō�̃v���C���[�̎���擾
        JankenHand lastHand = history[history.Count - 1].PlayerHand;

        // �ł������o���ꂽ�ulastHand �� ?�v�̑g�ݍ��킹��T��
        JankenHand predictedNext = JankenHand.Rock;
        int maxCount = -1;

        foreach (var kv in transitionCounts)
        {
            if (kv.Key.Item1 == lastHand && kv.Value > maxCount) // Item1�̓^�v���̍ŏ��̗v�f
            {
                predictedNext = kv.Key.Item2; // Item2�̓^�v����2�Ԗڂ̗v�f
                maxCount = kv.Value;
            }
        }

        if (maxCount == -1)
        {
            // �f�[�^�����Ȃ��ꍇ�̓����_��
            return (JankenHand)Random.Range(0, (int)JankenHand.Num);
        }

        // �\�����ꂽ��ɏ����Ԃ�
        return JankenLogic.AdvantageousHand(predictedNext);
    }
}
