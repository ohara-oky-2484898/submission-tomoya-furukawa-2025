using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �E�҂Ȃ�
/// �o�g�����ł���,���U�����ł���A���ȉ񕜂��ł���
/// �s��
/// �@������HP�����ȉ񕜂̔��f�������Ă���MP������Ύ��ȉ�
/// �A����Ă��Ȃ�orMP���Ȃ��Ƃ��͍U��
/// </summary>
public class Hero : BattlerBase, IPhysicalAttacker, ISelfHealer
{	
    /// <summary> �t�B�[���h </summary>
    private IBasicAttackStrategy _strategy;
    private SpellData[] _spellData;
	

    /// <summary> �v���p�e�B </summary>
    public override IBasicAttackStrategy BasicAttackStrategy => _strategy;
	public SpellData[] SpellData => _spellData;


    /// <summary> �R���X�g���N�^ </summary>
    public Hero(CharacterData data) : base(data) { _strategy = data.BasicAttackStrategy; }



    /// <summary> ���\�b�h </summary>
    public override void Init()
	{
        base.Init();
		_spellData = new SpellData[] {
            // ���O�A�����̎�ށA�_���[�W�A����l�o,�S�̂�
            new SpellData("Hiro�F���ȉ񕜖��@", SpellCategory.Heal, 3, false),// �������g����
        };
    }

    public override int GetBasicAttackStatValue() => Status.Attack;

    public override void DecideAction(List<IBattler> allBattlers)
    {
        var enemies = this.GetEnemies(allBattlers);

        IBattler target;

        // ���݂�HP����̊����܂ō��ꂽ(���� <= �ő� * ��)
        bool shouldSelfHeal = Status.HP <= Status.MaxHP * _aiCriteriaData.SelfHealingCriteria;

        SpellData[] usableSpells;
        // �񕜂��ׂ��^�C�~���O�Ŏ��ȉ񕜋Z������Ή񕜍s��
        if (this.TryGetUsableSpells(out usableSpells) && shouldSelfHeal)
        {
            Debug.Log($"[Hero: {Name}] HP���Ⴂ���ߎ��ȉ񕜂�I��");
            ReservedAction = new SelfHealAction(this, usableSpells[0]);
        }
        // �񕜂��ׂ��^�C�~���O����Ȃ��A�܂��͎g����񕜋Z���Ȃ�(MP�؂�)�ꍇ�͍U��
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
    /// ���ȉ񕜏���
    /// </summary>
    /// <param name="spell">�񕜎���</param>
    /// <returns></returns>
    public bool Heal(SpellData spell)
    {
        // ��
        return this.SpellSelfHeal(spell);
    }
}

