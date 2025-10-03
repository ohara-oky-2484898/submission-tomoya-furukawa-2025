using System.Collections.Generic;
using System.Linq;

/// <summary>
/// �u�f�����v���x�����ɍs���������肷��헪
/// </summary>
public class SlowestFirstTurnOrderStrategy : ITurnOrderStrategy
{
    public string DisplayName => "�f�����x����";

    public List<IBattler> GetTurnOrder(List<IBattler> allBattlers)
    {
        return allBattlers
            .Where(b => b.IsAlive)
            .OrderBy(b => b.Status.Speed) // ���̑������x�����ɕ��ׂ�
            .ToList();
    }
}
