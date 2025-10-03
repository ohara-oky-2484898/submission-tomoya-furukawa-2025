using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ActionData
{
    public string Name { get; }
    public List<IBattler> Targets { get; }

    public ActionData(string name, List<IBattler> targets)
    {
        Name = name;
        Targets = targets ?? new List<IBattler>();
    }
    public ActionData(string name, IBattler target)
    {
        Name = name;
        Targets = new List<IBattler> {target };
    }
}

/// <summary> �o�g���A�N�V�����̋��ʃC���^�[�t�F�[�X </summary>
public interface IBattleAction
{
    bool Execute();

    ActionData GetActionData();
}

/// <summary>
/// �키�Ȃ�N�ł��ł���ʏ�U���U���A�N�V�����N���X
/// �E�ҁA��m�͂������
/// ���@�g���A�m�����U���ł��Ȃ��Ⴂ���Ȃ�
/// </summary>
public class AttackAction : IBattleAction
{
    private readonly IBattler _attacker;
    private readonly IBattler _target;

    public AttackAction(IBattler attacker, IBattler target)
    {
        _attacker = attacker;
        _target = target;
    }

    public bool Execute()
    {
        Debug.Log($"AttackAction: {_attacker.Name}��{_target.Name}�ɍU��");
        return _attacker.BasicAttack(_target);
    }

    public ActionData GetActionData()
    {
        return new ActionData("�ʏ�U��", _target); 
    }
}
/// <summary>
/// ���U�����ł���A�N�V�����N���X
/// �E�ҁA��m�̂���
/// </summary>
public class HeavyAttackAction : IBattleAction
{
    private readonly IPhysicalAttacker _attacker;
    private readonly IBattler _target;

    public HeavyAttackAction(IPhysicalAttacker attacker, IBattler target)
    {
        _attacker = attacker;
        _target = target;
    }

    public bool Execute()
    {
        Debug.Log($"HeavyAttackAction: {_attacker.Name}��{_target.Name}�ɋ��U��");
        return _attacker.HeavyAttack(_target);
    }
    public ActionData GetActionData()
    {
        return new ActionData("�����̋��U��", _target);
    }

}

/// <summary>
/// �񕜂��ł���A�N�V�����N���X
/// �m���̂���
/// </summary>
public class HealAction : IBattleAction
{
    private readonly IHealer _healer;
    private readonly List<IBattler> _targets;
    private readonly SpellData _spell;

    public HealAction(IHealer healer, List<IBattler> targets, SpellData spell)
    {
        _healer = healer;
        _targets = targets;
        _spell = spell;
    }

    public bool Execute()
    {
        string targetNames = string.Join(", ", _targets.Select(t => t.Name));
        Debug.Log($"HealAction: {_healer.Name}��{targetNames}�ɉ�({_spell})");
        return _healer.Heal(_spell, _targets);
    }
    public ActionData GetActionData()
    {
        return new ActionData("�S�̉�", _targets);
    }

}

/// <summary>
/// �񕜂��ł���A�N�V�����N���X
/// �E�҂̂���
/// </summary>
public class SelfHealAction : IBattleAction
{
    private readonly ISelfHealer _healer;
    private readonly SpellData _spell;

    public SelfHealAction(ISelfHealer healer, SpellData spell)
    {
        _healer = healer;
        _spell = spell;
    }

    public bool Execute()
    {
        Debug.Log($"SelfHealAction: {_healer.Name}�����ȉ�({_spell})");
        return _healer.Heal(_spell);
    }

    public ActionData GetActionData()
    {
        return new ActionData("���ȉ�", _healer);
    }
}

/// <summary>
/// ���@�U�����ł���A�N�V�����N���X
/// ���@�g���̂���
/// </summary>
public class SpellAttackAction : IBattleAction
{
    private readonly IMagicAttacker _attacker;
    private readonly List<IBattler> _targets;
    private readonly SpellData _spell;

    public SpellAttackAction(IMagicAttacker attacker, List<IBattler> targets, SpellData spell)
    {
        _attacker = attacker;
        _targets = targets;
        _spell = spell;
    }

    public bool Execute()
    {
        string targetNames = string.Join(", ", _targets.Select(t => t.Name));
        Debug.Log($"SpellAttackAction: {_attacker.Name}��{targetNames}�ɍU��({_spell})");
        return _attacker.MagicAttack(_spell, _targets);
    }

    public ActionData GetActionData()
    {
        return new ActionData("���@�U��", _targets);
    }
}