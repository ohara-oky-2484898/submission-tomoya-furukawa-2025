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

/// <summary> バトルアクションの共通インターフェース </summary>
public interface IBattleAction
{
    bool Execute();

    ActionData GetActionData();
}

/// <summary>
/// 戦うなら誰でもできる通常攻撃攻撃アクションクラス
/// 勇者、戦士はもちろん
/// 魔法使い、僧侶も攻撃できなきゃいけない
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
        Debug.Log($"AttackAction: {_attacker.Name}が{_target.Name}に攻撃");
        return _attacker.BasicAttack(_target);
    }

    public ActionData GetActionData()
    {
        return new ActionData("通常攻撃", _target); 
    }
}
/// <summary>
/// 強攻撃ができるアクションクラス
/// 勇者、戦士のため
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
        Debug.Log($"HeavyAttackAction: {_attacker.Name}が{_target.Name}に強攻撃");
        return _attacker.HeavyAttack(_target);
    }
    public ActionData GetActionData()
    {
        return new ActionData("物理の強攻撃", _target);
    }

}

/// <summary>
/// 回復ができるアクションクラス
/// 僧侶のため
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
        Debug.Log($"HealAction: {_healer.Name}が{targetNames}に回復({_spell})");
        return _healer.Heal(_spell, _targets);
    }
    public ActionData GetActionData()
    {
        return new ActionData("全体回復", _targets);
    }

}

/// <summary>
/// 回復ができるアクションクラス
/// 勇者のため
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
        Debug.Log($"SelfHealAction: {_healer.Name}が自己回復({_spell})");
        return _healer.Heal(_spell);
    }

    public ActionData GetActionData()
    {
        return new ActionData("自己回復", _healer);
    }
}

/// <summary>
/// 魔法攻撃ができるアクションクラス
/// 魔法使いのため
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
        Debug.Log($"SpellAttackAction: {_attacker.Name}が{targetNames}に攻撃({_spell})");
        return _attacker.MagicAttack(_spell, _targets);
    }

    public ActionData GetActionData()
    {
        return new ActionData("魔法攻撃", _targets);
    }
}