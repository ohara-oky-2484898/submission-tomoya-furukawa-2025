using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// �u�f�����v�������ōs���������肷��헪
/// </summary>
public class SpeedTurnOrderStrategy : ITurnOrderStrategy
{
    public string DisplayName => "�f����������";

    public List<IBattler> GetTurnOrder(List<IBattler> allBattlers)
    {
        return allBattlers
            .Where(b => b.IsAlive)
            .OrderByDescending(b => b.Status.Speed)
            .ThenBy(b => Random.Range(0, 1000)) // �����f�����̏ꍇ�̓����_����
            .ToList();
    }
}
