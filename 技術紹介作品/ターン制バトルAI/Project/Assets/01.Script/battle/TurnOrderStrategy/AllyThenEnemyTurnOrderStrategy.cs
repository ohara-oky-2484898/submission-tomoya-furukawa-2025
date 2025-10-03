using System.Collections.Generic;
using System.Linq;

/// <summary>
/// ターンの行動順を「味方→敵」の順に決定する戦略
/// </summary>
public class AllyThenEnemyTurnOrderStrategy : ITurnOrderStrategy
{
    public string DisplayName => "「味方→敵」順";
    public List<IBattler> GetTurnOrder(List<IBattler> allBattlers)
    {
        // 味方チームの生存バトラーを取得
        var allies = allBattlers.Where(b => b.Team == Team.Ally && b.IsAlive).ToList();

        // 敵チームの生存バトラーを取得
        var enemies = allBattlers.Where(b => b.Team == Team.Enemy && b.IsAlive).ToList();

        // 味方を先に、その後に敵を続けて返す
        return allies.Concat(enemies).ToList();
    }

}
