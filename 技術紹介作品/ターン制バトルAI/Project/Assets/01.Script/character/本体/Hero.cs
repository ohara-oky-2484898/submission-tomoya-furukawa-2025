using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 勇者など
/// バトルができる,強攻撃ができる、自己回復ができる
/// 行動
/// ①自分のHPが自己回復の判断基準より削られていてMPがあれば自己回復
/// ②削られていないorMPがないときは攻撃
/// </summary>
public class Hero : BattlerBase, IPhysicalAttacker, ISelfHealer
{	
    /// <summary> フィールド </summary>
    private IBasicAttackStrategy _strategy;
    private SpellData[] _spellData;
	

    /// <summary> プロパティ </summary>
    public override IBasicAttackStrategy BasicAttackStrategy => _strategy;
	public SpellData[] SpellData => _spellData;


    /// <summary> コンストラクタ </summary>
    public Hero(CharacterData data) : base(data) { _strategy = data.BasicAttackStrategy; }



    /// <summary> メソッド </summary>
    public override void Init()
	{
        base.Init();
		_spellData = new SpellData[] {
            // 名前、呪文の種類、ダメージ、消費ＭＰ,全体か
            new SpellData("Hiro：自己回復魔法", SpellCategory.Heal, 3, false),// 自分自身を回復
        };
    }

    public override int GetBasicAttackStatValue() => Status.Attack;

    public override void DecideAction(List<IBattler> allBattlers)
    {
        var enemies = this.GetEnemies(allBattlers);

        IBattler target;

        // 現在のHPが基準の割合まで削られた(現在 <= 最大 * ％)
        bool shouldSelfHeal = Status.HP <= Status.MaxHP * _aiCriteriaData.SelfHealingCriteria;

        SpellData[] usableSpells;
        // 回復すべきタイミングで自己回復技があれば回復行動
        if (this.TryGetUsableSpells(out usableSpells) && shouldSelfHeal)
        {
            Debug.Log($"[Hero: {Name}] HPが低いため自己回復を選択");
            ReservedAction = new SelfHealAction(this, usableSpells[0]);
        }
        // 回復すべきタイミングじゃない、または使える回復技がない(MP切れ)場合は攻撃
        else
        {
            target = BattleSystem.SelectTarget(enemies, _aiCriteriaData);

            if (target != null)
            {
                ReservedAction = new HeavyAttackAction(this, target);
            }
        }
    }


    public bool HeavyAttack(IBattler target)
    {
        return BattleSystem.Attack(this, target);
    }

    /// <summary>
    /// 自己回復処理
    /// </summary>
    /// <param name="spell">回復呪文</param>
    /// <returns></returns>
    public bool Heal(SpellData spell)
    {
        // 回復
        return this.SpellSelfHeal(spell);
    }
}

