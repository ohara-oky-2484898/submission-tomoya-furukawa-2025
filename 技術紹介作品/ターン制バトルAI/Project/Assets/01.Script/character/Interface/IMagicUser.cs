using System.Collections.Generic;

/// <summary>
/// 魔法を使うことができるキャラクター
/// （勇者、魔法使い、僧侶など）
/// </summary>
public interface ISpellCaster : IBattler// ※ IBattler を継承しているのは、Status を使いたいため
{
    /// <summary>
    /// 使用可能な魔法のデータ一覧
    /// </summary>
    SpellData[] SpellData { get; }

    // bool CastSpell(SpellData spell, List<IBattler> targets);
}


/// <summary>
/// 魔法で回復ができるキャラクター（僧侶）
/// </summary>
public interface IHealer : ISpellCaster
{
    /// <summary>
    /// 対象を回復する
    /// </summary>
    /// <param name="spell">使用する回復魔法</param>
    /// <param name="targets">回復対象のリスト</param>
    /// <returns>回復に成功したか</returns>
    bool Heal(SpellData spell, List<IBattler> targets);
}

/// <summary>
/// 自己回復のみができるキャラクター（勇者）
/// 他キャラクターへの回復はできない
/// </summary>
public interface ISelfHealer : ISpellCaster
{
    /// <summary>
    /// 自分自身を回復する
    /// </summary>
    /// <param name="spell">使用する回復魔法</param>
    /// <returns>回復に成功したか</returns>
    bool Heal(SpellData spell);
}


/// <summary>
/// 魔法で攻撃ができるキャラクター（魔法使い）
/// </summary>
public interface IMagicAttacker : ISpellCaster
{
    /// <summary>
    /// 対象に魔法攻撃を行う
    /// </summary>
    /// <param name="spell">使用する攻撃魔法</param>
    /// <param name="targets">攻撃対象のリスト</param>
    /// <returns>攻撃に成功したか</returns>
    bool MagicAttack(SpellData spell, List<IBattler> targets);
}