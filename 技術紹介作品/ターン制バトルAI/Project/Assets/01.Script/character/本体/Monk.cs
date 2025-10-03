using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 僧侶など
/// バトルができる,回復することができる
/// 行動
/// ①使える回復魔法があって、単体回復を求めている味方が1人だけなら単体回復
/// ②使える回復魔法が1つだけの場合も単体回復
/// ③全体回復を求めているキャラが基準人数以上
///		かつ全体回復技があれば全体回復
/// 
/// ④どれにも当てはまらない
///		(中途半端に一体だけ削られてたり、全体回復を使う人数の基準未満の場合)
///		または、回復を求めてるキャラが一体もいない
///		MPが切れている場合
///	→弱攻撃
/// </summary>
public class Monk : BattlerBase, IHealer
{
	/// <summary> フィールド </summary>
	private SpellData[] _spellData;
	private IBasicAttackStrategy _strategy;


	/// <summary> プロパティ </summary>
	public SpellData[] SpellData => _spellData;
	public override IBasicAttackStrategy BasicAttackStrategy => _strategy;
	


	/// <summary> コンストラクタ </summary>
	public Monk(CharacterData data) : base(data) { _strategy = data.BasicAttackStrategy; }



	/// <summary> メソッド </summary>
	public override void Init()
	{
		base.Init();
		_spellData = new SpellData[] {
            // 名前、呪文の種類、ダメージ、消費ＭＰ
            new SpellData("Monk単体回復魔法", SpellCategory.Heal, 3, false),// 単体
            new SpellData("Monk全体回復魔法", SpellCategory.Heal, 5, true),// 全体
        };
	}
	public override int GetBasicAttackStatValue() => Status.Attack;

	public override void DecideAction(List<IBattler> allBattlers)
	{
		var allies = this.GetAllies(allBattlers);

		// 全体回復が必要な味方(HPが全体回復基準以下のリスト)
		List<IBattler> lowHpAlliesForAllHealing = GetLowHpAllies(allies, _aiCriteriaData.AllHealingCriteria);

		// 単体回復が必要な味方(HPが単体回復基準以下のリスト)
		List<IBattler> lowHpAlliesForSingleHealing = GetLowHpAllies(allies, _aiCriteriaData.SingleHealingCriteria);



		SpellData[] usableSpells;
		// 使える回復技があるかつ、回復が必要なキャラがいる
		if (this.TryGetUsableSpells(out usableSpells) && lowHpAlliesForAllHealing != null)
		{
			Debug.Log($"Monk：使える回復技あり。全体回復基準以下の味方数：{lowHpAlliesForAllHealing.Count}");

			// 単体回復を使う条件
			bool useSingleHealing = false;

			// 単体回復を求めているキャラが1体だけ。なら単体回復
			if (lowHpAlliesForSingleHealing?.Count == 1)
			{
				useSingleHealing = true;
				Debug.Log("単体回復条件に該当: 回復を求める味方が1体のみ");
			}
			// 全体回復を求めてるキャラが全体回復を使う基準の人数以上
			// でかつ、全体回復技を使えるなら全体回復
			else if (lowHpAlliesForAllHealing.Count >= _aiCriteriaData.RequiredTeammatesForHealing
					|| usableSpells.Length == _spellData.Length)
			{
				// 回復を求める味方が複数いて、全体回復の基準(人数をみてる)以下なら全体回復
				// defaultは2人以上求めていれば全体回復
				useSingleHealing = false;
				Debug.Log("全体回復条件に該当: 複数の味方が回復を必要としているかつ、使える技が複数");
			}
			// 使える技がひとつだけ(単体回復だけ)なら単体回復
			else if (usableSpells.Length == 1)
			{
				useSingleHealing = true;
				Debug.Log("単体回復条件に該当: 使える回復技がひとつしかない");
			}
			else
			{
				// どちらの条件にも該当しない
				// (中途半端に一体だけ削られてたり、全体回復を使う人数の基準未満の場合)
				// → 回復せず攻撃する
				Debug.Log("回復条件をどちらも満たしていないため攻撃へ移行");
				PerformAttackAction(allBattlers, _aiCriteriaData);
				return;
			}

			SpellData selectedSpell;
			List<IBattler> targets;

			// 単体回復を使う条件に当てはまってる／空じゃないか確認
			if (useSingleHealing && lowHpAlliesForSingleHealing != null)
			{
				// 単体回復（回復を求める中でHPが一番低い味方を選ぶ）
				var target = lowHpAlliesForSingleHealing
					.OrderBy(a => a.Status.HP)
					.First();

				selectedSpell = usableSpells[0]; // 単体回復魔法を使う想定
				targets = new List<IBattler> { target };

				Debug.Log($"単体回復使用: {target.Name} に {selectedSpell.SpellName}");
			}
			else
			{
				// 全体回復
				selectedSpell = usableSpells[1];
				targets = lowHpAlliesForAllHealing;

				Debug.Log($"全体回復使用: 対象は {targets.Count} 人");
			}

			// 回復アクションを予約
			ReservedAction = new HealAction(this, targets, selectedSpell);
		}
		else
		{
			Debug.Log("使える回復魔法なし、または回復対象なし → 攻撃");
			PerformAttackAction(allBattlers, _aiCriteriaData);
		}
	}

	public bool Heal(SpellData spell, List<IBattler> targets)
	{
		// 単体回復なら
		if(spell == _spellData[0])
		{
			return this.SpellHeal(targets[0], spell);
		}
		// 複数回復なら
		else
		{
			// 回復
			foreach(IBattler target in targets)
			{
				this.SpellHeal(target, spell);
			}
			return true;
		}
	}

	/// <summary>
	/// 弱攻撃を予約する関数
	/// </summary>
	/// <param name="allBattlers"></param>
	/// <param name="aiCriteriaData"></param>
	private void PerformAttackAction(List<IBattler> allBattlers, AIJudgmentCriteriaData aiCriteriaData)
	{
		var enemies = this.GetEnemies(allBattlers);

		IBattler target = BattleSystem.SelectTarget(enemies, aiCriteriaData);

		if (target != null)
		{
			ReservedAction = new AttackAction(this, target);
			Debug.Log($"Monk：<color=red>{Name}</color>は {target.Name} に攻撃する！");
		}
		else
		{
			Debug.LogWarning($"Monk：<color=gray>{Name}</color>は攻撃する敵が見つからず、何もしなかった。");
		}
	}

	/// <summary>
	/// 味方の中から回復をする基準を満たしたバトラーを探し、リストで返す
	/// </summary>
	/// <param name="allies">味方リスト</param>
	/// <param name="healingCriteria">回復基準</param>
	/// <returns>リストを返すが一人も該当しなければ null </returns>
	private List<IBattler> GetLowHpAllies(List<IBattler> allies, float healingCriteria)
	{
		var result = allies
			.Where(a => a.Status.HP <= a.Status.MaxHP * healingCriteria)
			.ToList();

		return result.Count > 0 ? result : null;
	}


}