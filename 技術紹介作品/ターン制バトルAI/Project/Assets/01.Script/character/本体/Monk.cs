using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// �m���Ȃ�
/// �o�g�����ł���,�񕜂��邱�Ƃ��ł���
/// �s��
/// �@�g����񕜖��@�������āA�P�̉񕜂����߂Ă��閡����1�l�����Ȃ�P�̉�
/// �A�g����񕜖��@��1�����̏ꍇ���P�̉�
/// �B�S�̉񕜂����߂Ă���L��������l���ȏ�
///		���S�̉񕜋Z������ΑS�̉�
/// 
/// �C�ǂ�ɂ����Ă͂܂�Ȃ�
///		(���r���[�Ɉ�̂�������Ă���A�S�̉񕜂��g���l���̊�����̏ꍇ)
///		�܂��́A�񕜂����߂Ă�L��������̂����Ȃ�
///		MP���؂�Ă���ꍇ
///	����U��
/// </summary>
public class Monk : BattlerBase, IHealer
{
	/// <summary> �t�B�[���h </summary>
	private SpellData[] _spellData;
	private IBasicAttackStrategy _strategy;


	/// <summary> �v���p�e�B </summary>
	public SpellData[] SpellData => _spellData;
	public override IBasicAttackStrategy BasicAttackStrategy => _strategy;
	


	/// <summary> �R���X�g���N�^ </summary>
	public Monk(CharacterData data) : base(data) { _strategy = data.BasicAttackStrategy; }



	/// <summary> ���\�b�h </summary>
	public override void Init()
	{
		base.Init();
		_spellData = new SpellData[] {
            // ���O�A�����̎�ށA�_���[�W�A����l�o
            new SpellData("Monk�P�̉񕜖��@", SpellCategory.Heal, 3, false),// �P��
            new SpellData("Monk�S�̉񕜖��@", SpellCategory.Heal, 5, true),// �S��
        };
	}
	public override int GetBasicAttackStatValue() => Status.Attack;

	public override void DecideAction(List<IBattler> allBattlers)
	{
		var allies = this.GetAllies(allBattlers);

		// �S�̉񕜂��K�v�Ȗ���(HP���S�̉񕜊�ȉ��̃��X�g)
		List<IBattler> lowHpAlliesForAllHealing = GetLowHpAllies(allies, _aiCriteriaData.AllHealingCriteria);

		// �P�̉񕜂��K�v�Ȗ���(HP���P�̉񕜊�ȉ��̃��X�g)
		List<IBattler> lowHpAlliesForSingleHealing = GetLowHpAllies(allies, _aiCriteriaData.SingleHealingCriteria);



		SpellData[] usableSpells;
		// �g����񕜋Z�����邩�A�񕜂��K�v�ȃL����������
		if (this.TryGetUsableSpells(out usableSpells) && lowHpAlliesForAllHealing != null)
		{
			Debug.Log($"Monk�F�g����񕜋Z����B�S�̉񕜊�ȉ��̖������F{lowHpAlliesForAllHealing.Count}");

			// �P�̉񕜂��g������
			bool useSingleHealing = false;

			// �P�̉񕜂����߂Ă���L������1�̂����B�Ȃ�P�̉�
			if (lowHpAlliesForSingleHealing?.Count == 1)
			{
				useSingleHealing = true;
				Debug.Log("�P�̉񕜏����ɊY��: �񕜂����߂閡����1�̂̂�");
			}
			// �S�̉񕜂����߂Ă�L�������S�̉񕜂��g����̐l���ȏ�
			// �ł��A�S�̉񕜋Z���g����Ȃ�S�̉�
			else if (lowHpAlliesForAllHealing.Count >= _aiCriteriaData.RequiredTeammatesForHealing
					|| usableSpells.Length == _spellData.Length)
			{
				// �񕜂����߂閡�����������āA�S�̉񕜂̊(�l�����݂Ă�)�ȉ��Ȃ�S�̉�
				// default��2�l�ȏ㋁�߂Ă���ΑS�̉�
				useSingleHealing = false;
				Debug.Log("�S�̉񕜏����ɊY��: �����̖������񕜂�K�v�Ƃ��Ă��邩�A�g����Z������");
			}
			// �g����Z���ЂƂ���(�P�̉񕜂���)�Ȃ�P�̉�
			else if (usableSpells.Length == 1)
			{
				useSingleHealing = true;
				Debug.Log("�P�̉񕜏����ɊY��: �g����񕜋Z���ЂƂ����Ȃ�");
			}
			else
			{
				// �ǂ���̏����ɂ��Y�����Ȃ�
				// (���r���[�Ɉ�̂�������Ă���A�S�̉񕜂��g���l���̊�����̏ꍇ)
				// �� �񕜂����U������
				Debug.Log("�񕜏������ǂ�����������Ă��Ȃ����ߍU���ֈڍs");
				PerformAttackAction(allBattlers, _aiCriteriaData);
				return;
			}

			SpellData selectedSpell;
			List<IBattler> targets;

			// �P�̉񕜂��g�������ɓ��Ă͂܂��Ă�^�󂶂�Ȃ����m�F
			if (useSingleHealing && lowHpAlliesForSingleHealing != null)
			{
				// �P�̉񕜁i�񕜂����߂钆��HP����ԒႢ������I�ԁj
				var target = lowHpAlliesForSingleHealing
					.OrderBy(a => a.Status.HP)
					.First();

				selectedSpell = usableSpells[0]; // �P�̉񕜖��@���g���z��
				targets = new List<IBattler> { target };

				Debug.Log($"�P�̉񕜎g�p: {target.Name} �� {selectedSpell.SpellName}");
			}
			else
			{
				// �S�̉�
				selectedSpell = usableSpells[1];
				targets = lowHpAlliesForAllHealing;

				Debug.Log($"�S�̉񕜎g�p: �Ώۂ� {targets.Count} �l");
			}

			// �񕜃A�N�V������\��
			ReservedAction = new HealAction(this, targets, selectedSpell);
		}
		else
		{
			Debug.Log("�g����񕜖��@�Ȃ��A�܂��͉񕜑ΏۂȂ� �� �U��");
			PerformAttackAction(allBattlers, _aiCriteriaData);
		}
	}

	public bool Heal(SpellData spell, List<IBattler> targets)
	{
		// �P�̉񕜂Ȃ�
		if(spell == _spellData[0])
		{
			return this.SpellHeal(targets[0], spell);
		}
		// �����񕜂Ȃ�
		else
		{
			// ��
			foreach(IBattler target in targets)
			{
				this.SpellHeal(target, spell);
			}
			return true;
		}
	}

	/// <summary>
	/// ��U����\�񂷂�֐�
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
			Debug.Log($"Monk�F<color=red>{Name}</color>�� {target.Name} �ɍU������I");
		}
		else
		{
			Debug.LogWarning($"Monk�F<color=gray>{Name}</color>�͍U������G�������炸�A�������Ȃ������B");
		}
	}

	/// <summary>
	/// �����̒�����񕜂������𖞂������o�g���[��T���A���X�g�ŕԂ�
	/// </summary>
	/// <param name="allies">�������X�g</param>
	/// <param name="healingCriteria">�񕜊</param>
	/// <returns>���X�g��Ԃ�����l���Y�����Ȃ���� null </returns>
	private List<IBattler> GetLowHpAllies(List<IBattler> allies, float healingCriteria)
	{
		var result = allies
			.Where(a => a.Status.HP <= a.Status.MaxHP * healingCriteria)
			.ToList();

		return result.Count > 0 ? result : null;
	}


}