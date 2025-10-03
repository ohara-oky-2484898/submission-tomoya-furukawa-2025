using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;


/// <summary>
/// 戦士など
/// バトルができる,強攻撃ができる
/// 行動
/// ① AIの優先基準にしたがって
///     (特定のロール狙い or 残りのHP少ないやつ狙い)
///     優先度の高い敵をひたすら攻撃
/// </summary>
public class Warrior : BattlerBase, IPhysicalAttacker
{
    /// <summary> フィールド </summary>
    private IBasicAttackStrategy _strategy;

    /// <summary> プロパティ </summary>
    public override IBasicAttackStrategy BasicAttackStrategy => _strategy;

    /// <summary> コンストラクタ </summary>
    public Warrior(CharacterData data) : base(data) { _strategy = data.BasicAttackStrategy; }


    /// <summary> メソッド </summary>

    public override int GetBasicAttackStatValue() => Status.Attack;

    public override void DecideAction(List<IBattler> allBattlers)
    {
        var enemies = this.GetEnemies(allBattlers);  // 敵キャラを取得

        // ターゲットを選択
        IBattler target = BattleSystem.SelectTarget(enemies, _aiCriteriaData);

        if (target != null)
        {
            ReservedAction = new HeavyAttackAction(this, target);
        }
    }

    public bool HeavyAttack(IBattler target)
    {
        // 物理攻撃処理
        return BattleSystem.Attack(this, target);
    }
}
