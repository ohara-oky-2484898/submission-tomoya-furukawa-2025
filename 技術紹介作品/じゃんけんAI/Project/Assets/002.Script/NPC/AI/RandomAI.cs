using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// �����_���Ɏ���o��AI
/// ��ɏ��՗p
/// </summary>
public class RandomAI : IJankenAI
{
    public JankenHand Decide(IReadOnlyList<JankenHistory> history)
    {
        return (JankenHand)Random.Range(0, (int)JankenHand.Num);
    }
}
