using System.Collections.Generic;
using System.Linq;

/// <summary>
/// u‘f‘‚³v‚ª’x‚¢‡‚És“®‡‚ğŒˆ’è‚·‚éí—ª
/// </summary>
public class SlowestFirstTurnOrderStrategy : ITurnOrderStrategy
{
    public string DisplayName => "‘f‘‚³’x‚¢‡";

    public List<IBattler> GetTurnOrder(List<IBattler> allBattlers)
    {
        return allBattlers
            .Where(b => b.IsAlive)
            .OrderBy(b => b.Status.Speed) // ‘«‚Ì‘¬‚³‚ª’x‚¢‡‚É•À‚×‚é
            .ToList();
    }
}
