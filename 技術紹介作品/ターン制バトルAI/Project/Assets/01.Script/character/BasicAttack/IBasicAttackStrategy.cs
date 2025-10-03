using UnityEngine;

/// <summary>
/// �L�����N�^�[�����u��{�U���i�W���u�Ȃǁj�v�̐헪�C���^�[�t�F�[�X
/// �d���o�X�^�[�Y��|�P�������i�C�g
/// ��A�U���̂悤�Ȏg�p�ɂ�����
/// �܂�A�L�����ɂ���čU���͎Q�ƂȂ̂��A���͎Q�ƂȂ̂��킯����
/// �L�������ƂɈꔭ�U���A�O�i�U���A�������U���Ȃǂ𕪂�����
/// </summary>
public interface IBasicAttackStrategy
{
    /// <summary>
    /// ��{�U�������s����
    /// </summary>
    /// <param name="attacker">�U������L����</param>
    /// <param name="target">�U���Ώ�</param>
    /// <returns>�U��������������</returns>
    bool Execute(IBattler attacker, IBattler target);
}

/// <summary>
/// �V���v���ȋߋ�����i�U��
/// </summary>
public class SingleHitAttackStrategy : IBasicAttackStrategy
{
    public bool Execute(IBattler attacker, IBattler target)
    {
        int damage = BattleSystem.CalculateEffectAmount(attacker.GetBasicAttackStatValue()) / 2;
        Debug.Log($"IBasicAttackStrategy�F{attacker.Name}�̒P���U���I {target.Name} �� {damage} �_���[�W");
        return BattleSystem.ApplyHpChange(target, -damage);
    }
}


/// <summary>
/// �R���{�U��
/// </summary>
public class ComboAttackStrategy : IBasicAttackStrategy
{
    public bool Execute(IBattler attacker, IBattler target)
    {
        int totalDamage = 0;
        for (int i = 0; i < 3; i++)
        {
            int hit = BattleSystem.CalculateEffectAmount(attacker.GetBasicAttackStatValue()) / 4;
            totalDamage += hit;
        }
        Debug.Log($"IBasicAttackStrategy�F{attacker.Name}�̃R���{�U���I {target.Name} �ɍ��v {totalDamage} �_���[�W");
        return BattleSystem.ApplyHpChange(target, -totalDamage);
    }
}

/// <summary>
/// �������U��
/// </summary>
public class RangedAttackStrategy : IBasicAttackStrategy
{

    public bool Execute(IBattler attacker, IBattler target)
    {
        int damage = BattleSystem.CalculateEffectAmount(attacker.GetBasicAttackStatValue()) / 2 + 2;
        Debug.Log($"IBasicAttackStrategy�F{attacker.Name}�̉������U���I {target.Name} �� {damage} �_���[�W");
        return BattleSystem.ApplyHpChange(target, -damage);
    }
}


