using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 魔法使いなど
/// バトルができる,魔法攻撃ができる
/// 行動
/// ①全体攻撃技が使えて、敵が複数いたら全体魔法攻撃
/// ②すべて使える技があったのに全体魔法攻撃を使わないときは
///     敵が単体なので強魔法攻撃
/// ③技に限りがあって、まだ敵が複数いる場合弱攻撃しか使えない
/// ④MPがないときは弱攻撃
/// </summary>
public class Mage : BattlerBase, IMagicAttacker
{
    /// <summary> フィールド </summary>
    private SpellData[] _spellData;
    private IBasicAttackStrategy _strategy;


    /// <summary> プロパティ </summary>
    public SpellData[] SpellData => _spellData;
    public override IBasicAttackStrategy BasicAttackStrategy => _strategy;


    /// <summary> コンストラクタ </summary>
    public Mage(CharacterData data) : base(data) { _strategy = data.BasicAttackStrategy; }

    /// <summary> メソッド </summary>
    public override void Init()
	{
        base.Init();
        _spellData = new SpellData[] {
            // 名前、呪文の種類、ダメージ、消費ＭＰ
            new SpellData("Mage:ファイア", SpellCategory.Attack, 3, false),// 単体
            new SpellData("Mage:ハイパーファイア", SpellCategory.Attack, 5, false),// 全体
            new SpellData("Mage:ファイアall", SpellCategory.Attack, 6, true)// 全体強攻撃
        };
    }


    public override int GetBasicAttackStatValue() => Status.Magic;


    public override void DecideAction(List<IBattler> allBattlers)
    {
        var enemies = this.GetEnemies(allBattlers);  // 敵キャラを取得

        // 現在のMPを参照して
        // 持っている呪文(_spellData)の中から使えるものだけを取り出す
        // 戻り値はつかえるものが１つでもあればTrue
        // １つもなければfalse
        SpellData[] useSpellList;
        if (this.TryGetUsableSpells(out useSpellList))
		{
            // 敵が複数いて、かつ全体魔法あるときのみ全体魔法
            // 敵が一体だったり、単体魔法しか使えない場合は単体攻撃
            bool hasMultipleEnemies = enemies.Count > 1;

            // 使える技の中で今回は全体攻撃は1つだけなのでFindを使用
            // 複数になったらwhereに変える必要あり
            SpellData spell = Array.Find(useSpellList, s => s.IsAllTarget);
            // 全体技があるかつ、複数敵がいる
            if (spell != null && hasMultipleEnemies)
            {
                CastSpell(spell, enemies);
            }
            // 全体技がなかった、または敵が単体だった
            else
			{
                // 一体にターゲットを絞って
                IBattler target = BattleSystem.SelectTarget(enemies, _aiCriteriaData);

                // 全ての技が使えるか
                bool hasAllUsableSpells = useSpellList.Length == _spellData.Length;

                // 全ての技を使えるのに全体魔法を使わずelseに来たということは
                // 敵は単体なので強攻撃魔法を使用し全力攻撃してOK
                spell = hasAllUsableSpells
                    ? useSpellList[1]   // 単体魔法(強)
                    : useSpellList[0];  // 単体魔法(弱)

                CastSpell(spell, new List<IBattler> { target });
			}
        }
        else
		{
            // 魔法が使えない場合は弱攻撃
            IBattler target = BattleSystem.SelectTarget(enemies, _aiCriteriaData);

            if (target != null)
            {
                // 弱攻撃を予約
                ReservedAction = new AttackAction(this, target);
            }
            else
            {
                // もしターゲットが見つからない場合の処理
                Debug.LogWarning($"Mage：<color=gray>{Name}は攻撃する敵が見つからず、何もしなかった。</color>");
            }
        }
    }

    public void CastSpell(SpellData spell, List<IBattler> targets)
    {
        ReservedAction = new SpellAttackAction(this, targets, spell);
    }



    /// <summary>
    /// 魔法使いの主な攻撃手段
    /// </summary>
    /// <param name="spell">使う魔法</param>
    /// <param name="targets">攻撃対象</param>
    /// <returns>攻撃成功したか</returns>
    public bool MagicAttack(SpellData spell, List<IBattler> targets)
    {
        return this.SpellAttack(targets, spell);
    }
}