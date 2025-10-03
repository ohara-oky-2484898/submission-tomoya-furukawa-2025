using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 「素早さ」速い順で行動順を決定する戦略
/// </summary>
public class SpeedTurnOrderStrategy : ITurnOrderStrategy
{
    public string DisplayName => "素早さ速い順";

    public List<IBattler> GetTurnOrder(List<IBattler> allBattlers)
    {
        return allBattlers
            .Where(b => b.IsAlive)
            .OrderByDescending(b => b.Status.Speed)
            .ThenBy(b => Random.Range(0, 1000)) // 同じ素早さの場合はランダム順
            .ToList();
    }
}
