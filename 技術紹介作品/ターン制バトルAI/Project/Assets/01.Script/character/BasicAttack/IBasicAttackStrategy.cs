using UnityEngine;

/// <summary>
/// キャラクターが持つ「基本攻撃（ジャブなど）」の戦略インターフェース
/// 妖怪バスターズやポケモンユナイト
/// のA攻撃のような使用にしたい
/// つまり、キャラによって攻撃力参照なのか、魔力参照なのかわけたい
/// キャラごとに一発攻撃、三段攻撃、遠距離攻撃などを分けたい
/// </summary>
public interface IBasicAttackStrategy
{
    /// <summary>
    /// 基本攻撃を実行する
    /// </summary>
    /// <param name="attacker">攻撃するキャラ</param>
    /// <param name="target">攻撃対象</param>
    /// <returns>攻撃が成功したか</returns>
    bool Execute(IBattler attacker, IBattler target);
}

/// <summary>
/// シンプルな近距離一段攻撃
/// </summary>
public class SingleHitAttackStrategy : IBasicAttackStrategy
{
    public bool Execute(IBattler attacker, IBattler target)
    {
        int damage = BattleSystem.CalculateEffectAmount(attacker.GetBasicAttackStatValue()) / 2;
        Debug.Log($"IBasicAttackStrategy：{attacker.Name}の単発攻撃！ {target.Name} に {damage} ダメージ");
        return BattleSystem.ApplyHpChange(target, -damage);
    }
}


/// <summary>
/// コンボ攻撃
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
        Debug.Log($"IBasicAttackStrategy：{attacker.Name}のコンボ攻撃！ {target.Name} に合計 {totalDamage} ダメージ");
        return BattleSystem.ApplyHpChange(target, -totalDamage);
    }
}

/// <summary>
/// 遠距離攻撃
/// </summary>
public class RangedAttackStrategy : IBasicAttackStrategy
{

    public bool Execute(IBattler attacker, IBattler target)
    {
        int damage = BattleSystem.CalculateEffectAmount(attacker.GetBasicAttackStatValue()) / 2 + 2;
        Debug.Log($"IBasicAttackStrategy：{attacker.Name}の遠距離攻撃！ {target.Name} に {damage} ダメージ");
        return BattleSystem.ApplyHpChange(target, -damage);
    }
}


