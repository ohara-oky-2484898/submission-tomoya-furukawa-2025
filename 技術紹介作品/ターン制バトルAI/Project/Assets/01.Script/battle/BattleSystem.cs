using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public enum BattleResult
{
    Continue, // 勝負継続中（どちらも全滅していない）
    Win,      // プレイヤーの勝利
    Lose      // プレイヤーの敗北
}

/// <summary>
/// バトルシステムにおける共通処理を担当するクラス
/// </summary>
public static class BattleSystem
{
    // 攻撃に失敗するとき
    // ①攻撃が当たらない(今回はない)
    // ②敵がもう死んでいた(別の敵を狙う)
    // ③自分が殺された(呼ばれるのがおかしい？（呼ばれる前に弾くほうがいい)


    /// <summary>
    /// 攻撃処理の拡張メソッド
    /// </summary>
    /// <param name="attacker">攻撃者</param>
    /// <param name="target">攻撃対象</param>
    public static bool Attack(this IBattler attacker, IBattler target)
    {
        if (!attacker.IsAlive) return false;

        int damage = CalculateEffectAmount(attacker.Status.Attack);
        Debug.Log($"BattleSystem<color=yellow>{attacker.Name}</color>が<color=yellow>{target.Name}</color>を<color=red>攻撃</color>した（<color=red>{damage}ダメージ</color>）");
        return ApplyHpChange(target, -damage);
    }

    /// <summary>
    ///  基本攻撃(弱攻撃は威力半分)
    ///  弱攻撃処理
    /// </summary>
    /// <param name="attacker">攻撃者</param>
    /// <param name="target">攻撃対象</param>
    public static bool BasicAttack(this IBattler attacker, IBattler target)
    {
        if (!attacker.IsAlive) return false;

        // それぞれのバトラーの参照したいステータスの数値を参照して計算した後
        // そのままだとAttack()やSpellAttack()と同じダメージなので
        // 威力を半分にしておく
        int damage = CalculateEffectAmount(attacker.GetBasicAttackStatValue()) / 2;
        Debug.Log($"BattleSystem：<color=yellow>{attacker.Name}</color>が<color=yellow>{target.Name}</color>に<color=red>弱攻撃</color>をした（<color=red>{damage}ダメージ</color>）");
        return ApplyHpChange(target, -damage);

    }

    /// <summary>
    /// 魔法攻撃処理
    /// </summary>
    /// <param name="attacker">魔法攻撃者</param>
    /// <param name="target">魔法攻撃を受ける対象</param>
    public static bool SpellAttack(this IMagicAttacker attacker, List<IBattler> targets, SpellData data)
    {
        if (!attacker.IsAlive)return false;

        if (attacker.Status.MP < data.MpCost ||
            data.Category != SpellCategory.Attack) return false;

        // MP消費
        attacker.Status.MP -= data.MpCost;

        // UI更新を呼ぶ
        UIManager.Instance.UpdateHUD(attacker);

        // 基礎ダメージ計算
        int damage = CalculateEffectAmount(attacker.Status.Magic);
        // 基礎ダメージに全体攻撃なら、ダメージがばらけるように(分散されて低威力になるように)
        // 攻撃する人数分で割ってあげる
        // が3体以上なら3で割る(威力が低すぎるの防止)
        int applyDamage = targets.Count >= 3
            ? damage / 3
            : damage / targets.Count;

        foreach (IBattler target in targets)
        {
            ApplyHpChange(target, -applyDamage);
            Debug.Log($"BattleSystem：<color=yellow>{attacker.Name}</color>が<color=yellow>{target.Name}</color>を<color=red>魔法攻撃</color>した（<color=red>{applyDamage}ダメージ</color>）");
        }
        return true;

    }


    /// <summary>
    /// 回復処理
    /// </summary>
    /// <param name="healer">回復魔法使用者</param>
    /// <param name="target">回復対象</param>
    public static bool SpellHeal(this IHealer healer, IBattler target, SpellData data)
	{
        if (!healer.IsAlive) return false;

        // 50 < 50
        if (healer.Status.MP < data.MpCost ||
            data.Category != SpellCategory.Heal) return false;


        // MP消費
        healer.Status.MP -= data.MpCost;
        // UI更新を呼ぶ
        UIManager.Instance.UpdateHUD(healer);


        int healAmount = CalculateEffectAmount(healer.Status.Magic);
        Debug.Log($"BattleSystem：<color=yellow>{healer.Name}</color>が<color=yellow>{target.Name}</color>を<color=green>回復</color>した（<color=green>{healAmount}回復</color>）");
        return ApplyHpChange(target, healAmount);

    }

    /// <summary>
    /// 自己回復処理
    /// </summary>
    /// <param name="healer">回復魔法使用者</param>
    public static bool SpellSelfHeal(this ISelfHealer healer, SpellData data)
    {
        if (!healer.IsAlive) return false;

		if (healer.Status.MP < data.MpCost
            || data.Category != SpellCategory.Heal )
            return false;

        // MP消費
        healer.Status.MP -= data.MpCost;
        // UI更新を呼ぶ
        UIManager.Instance.UpdateHUD(healer);

        int healAmount = CalculateEffectAmount(healer.Status.Magic);
        Debug.Log($"BattleSystem：<color=yellow>{healer.Name}</color>が<color=yellow>自身</color>を<color=green>回復</color>した（<color=green>{healAmount}回復</color>）");
        return ApplyHpChange(healer, healAmount);

    }

    /// <summary>
    /// 魔力・攻撃力から効果量を計算（ダメージ・回復共通）
    /// </summary>
    public static int CalculateEffectAmount(int power)
    {
        int baseValue = power / 2;
        int randomBonus = Random.Range(0, baseValue + 1);
        return baseValue + randomBonus + 1;
    }

    /// <summary>
    /// HPを変動を適応させる処理（正なら回復、負ならダメージ）
    /// </summary>
    /// <param name="target">対象バトラー</param>
    /// <param name="amount">変動量（正: 回復、負: ダメージ）</param>
    public static bool ApplyHpChange(IBattler target, int amount)
    {
        // MEMO：もし死んでる味方を回復したい場合は見直さないと
        if (!target.IsAlive) return false;

        Debug.Log($"BattleSystem：適応前<color=yellow>{target.Name}</color>の<color=blue>残りHP {target.Status.HP}/{target.Status.MaxHP}</color>");
        target.Status.HP = Mathf.Clamp(target.Status.HP + amount, 0, target.Status.MaxHP);
        Debug.Log($"BattleSystem：適応後<color=yellow>{target.Name}</color>の<color=blue>残りHP {target.Status.HP}/{target.Status.MaxHP}</color>");

        // ここでUI更新を呼ぶ（例）
        UIManager.Instance.UpdateHUD(target);

        // HPが0以下であれば死亡処理
        if (target.Status.HP <= 0)
        {
            Debug.Log($"BattleSystem：<color=red><b>{target.Name}が死んだ</b></color>");


            // 死亡後のUI更新が必要なら、ここでも呼ぶか
            //UIManager.Instance.OnDestroy(_target);
            UIManager.Instance.PlayDeathEffectFor(target);
            target.Dead();
        }
        return true;
    }

    /// <summary>
    /// このバトラーに対する敵のリストを取得
    /// </summary>
    /// <param name="currentBattler">現在のバトラー（アクター）</param>
    /// <param name="allBattlers">全バトラー</param>
    /// <returns>現在のバトラーに対する敵のリスト</returns>
    public static List<IBattler> GetEnemies(this IBattler currentBattler, List<IBattler> allBattlers)
    {
        return allBattlers.Where(b => b.Team != currentBattler.Team && b.IsAlive).ToList();
    }

    /// <summary>
    /// このバトラーに対する味方のリストを取得
    /// </summary>
    /// <param name="currentBattler">現在のバトラー（アクター）</param>
    /// <param name="allBattlers">全バトラー</param>
    /// <returns>現在のバトラーに対する味方のリスト</returns>
    public static List<IBattler> GetAllies(this IBattler currentBattler, List<IBattler> allBattlers)
    {
        return allBattlers.Where(b => b.Team == currentBattler.Team && b.IsAlive).ToList();
    }

    /// <summary>
    /// 勝敗判定どちらか、全滅していないか確認用
    /// 今回の設計なら引き分けはないものとする
    /// </summary>
    /// <param name="battlers">全バトラー</param>
    /// <returns>戦闘終了かどうか</returns>
    public static bool TryCheckBattleResult(List<IBattler> battlers, out BattleResult result)
    {
        bool alliesAlive = battlers.Any(b => b.IsAlive && b.Team == Team.Ally);
        bool enemiesAlive = battlers.Any(b => b.IsAlive && b.Team == Team.Enemy);

        if (!alliesAlive)
        {
            result = BattleResult.Lose;
            return true;
        }

        if (!enemiesAlive)
        {
            result = BattleResult.Win;
            return true;
        }

        result = BattleResult.Continue;
        return false;
    }

    /// <summary>
    /// 最もHPが少ない敵を取得する
    /// </summary>
    /// <param name="enemies">敵のリスト</param>
    /// <returns>HPが最も低い敵。該当がいない場合は null</returns>
    public static IBattler GetLowHpEnemy(List<IBattler> enemies)
    {
        return enemies
            .Where(e => e.IsAlive)
            .OrderBy(e => e.Status.HP)
            .FirstOrDefault();
    }


    /// <summary>
    /// 現在の MP で使用可能な呪文リストを取得する
    /// </summary>
    /// <param name="caster">呪文を使うキャラクター（ISpellCaster）</param>
    /// <param name="availableSpells">使える呪文のリストを out で返す</param>
    /// <returns>使える呪文が1つでもあれば true、なければ false</returns>
    public static bool TryGetUsableSpells(this ISpellCaster caster, out SpellData[] availableSpells)
    {
        availableSpells = caster.SpellData
            .Where(spell => caster.Status.MP >= spell.MpCost)
            .ToArray();

        return availableSpells.Length > 0;
    }

    /// <summary>
    /// 最も優先順位が高いロールのキャラを取得
    /// 優先度が同じ場合、HPが低いキャラを返す
    /// </summary>
    /// <param name="enemies">敵のリスト</param>
    /// <param name="aiCriteriaData">判断基準データ</param>
    /// <returns>最も優先されるロールを持つキャラ</returns>
    public static IBattler GetHighestPriorityRoleCharacter(List<IBattler> enemies, AIJudgmentCriteriaData aiCriteriaData)
    {
        // 優先度が低い（高い優先度）ロールを優先する
        var highestPriorityRoleEnemy = enemies
            .Where(e => e.IsAlive)
            .OrderBy(e => aiCriteriaData.GetRolePriority(e.Roll))  // ロール優先度を取得してソート
            .ThenBy(e => e.Status.HP)  // 同じ優先度の場合はHPが低い方を選択
            .FirstOrDefault();

        return highestPriorityRoleEnemy;
    }

    public static IBattler SelectTarget(List<IBattler> enemies, AIJudgmentCriteriaData aiCriteriaData)
    {
        if (aiCriteriaData.PrioritizeLowHpEnemyOverRole)
        {
            Debug.Log("[BattleSystem] HP優先でターゲット選択");
            var lowHpEnemy = GetLowHpEnemy(enemies);
            if (lowHpEnemy != null)
            {
                Debug.Log($"[BattleSystem] HPが最も少ない敵は: {lowHpEnemy.Name} (HP: {lowHpEnemy.Status.HP})");
            }
            else
            {
                Debug.Log("[BattleSystem] HPが最も少ない敵は見つかりませんでした");
            }
            return lowHpEnemy; // HP優先
        }
        else
        {
            Debug.Log("[BattleSystem] ロール優先でターゲット選択");
            var highestPriorityEnemy = GetHighestPriorityRoleCharacter(enemies, aiCriteriaData);
            if (highestPriorityEnemy != null)
            {
                Debug.Log($"[BattleSystem] ロール優先で選ばれた敵は: {highestPriorityEnemy.Name} (Role: {highestPriorityEnemy.Roll}, HP: {highestPriorityEnemy.Status.HP})");
            }
            else
            {
                Debug.Log("[BattleSystem] ロール優先でターゲットが見つかりませんでした");
            }
            return highestPriorityEnemy; // ロール優先
        }
    }
}
