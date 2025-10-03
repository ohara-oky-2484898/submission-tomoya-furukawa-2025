using System.Collections.Generic;
using System.Linq;

/// <summary>
/// �^�[���̍s�������u�������G�v�̏��Ɍ��肷��헪
/// </summary>
public class AllyThenEnemyTurnOrderStrategy : ITurnOrderStrategy
{
    public string DisplayName => "�u�������G�v��";
    public List<IBattler> GetTurnOrder(List<IBattler> allBattlers)
    {
        // �����`�[���̐����o�g���[���擾
        var allies = allBattlers.Where(b => b.Team == Team.Ally && b.IsAlive).ToList();

        // �G�`�[���̐����o�g���[���擾
        var enemies = allBattlers.Where(b => b.Team == Team.Enemy && b.IsAlive).ToList();

        // �������ɁA���̌�ɓG�𑱂��ĕԂ�
        return allies.Concat(enemies).ToList();
    }

}
