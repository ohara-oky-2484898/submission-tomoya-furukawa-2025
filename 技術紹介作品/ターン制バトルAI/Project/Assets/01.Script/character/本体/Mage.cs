using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���@�g���Ȃ�
/// �o�g�����ł���,���@�U�����ł���
/// �s��
/// �@�S�̍U���Z���g���āA�G������������S�̖��@�U��
/// �A���ׂĎg����Z���������̂ɑS�̖��@�U�����g��Ȃ��Ƃ���
///     �G���P�̂Ȃ̂ŋ����@�U��
/// �B�Z�Ɍ��肪�����āA�܂��G����������ꍇ��U�������g���Ȃ�
/// �CMP���Ȃ��Ƃ��͎�U��
/// </summary>
public class Mage : BattlerBase, IMagicAttacker
{
    /// <summary> �t�B�[���h </summary>
    private SpellData[] _spellData;
    private IBasicAttackStrategy _strategy;


    /// <summary> �v���p�e�B </summary>
    public SpellData[] SpellData => _spellData;
    public override IBasicAttackStrategy BasicAttackStrategy => _strategy;


    /// <summary> �R���X�g���N�^ </summary>
    public Mage(CharacterData data) : base(data) { _strategy = data.BasicAttackStrategy; }

    /// <summary> ���\�b�h </summary>
    public override void Init()
	{
        base.Init();
        _spellData = new SpellData[] {
            // ���O�A�����̎�ށA�_���[�W�A����l�o
            new SpellData("Mage:�t�@�C�A", SpellCategory.Attack, 3, false),// �P��
            new SpellData("Mage:�n�C�p�[�t�@�C�A", SpellCategory.Attack, 5, false),// �S��
            new SpellData("Mage:�t�@�C�Aall", SpellCategory.Attack, 6, true)// �S�̋��U��
        };
    }


    public override int GetBasicAttackStatValue() => Status.Magic;


    public override void DecideAction(List<IBattler> allBattlers)
    {
        var enemies = this.GetEnemies(allBattlers);  // �G�L�������擾

        // ���݂�MP���Q�Ƃ���
        // �����Ă������(_spellData)�̒�����g������̂��������o��
        // �߂�l�͂�������̂��P�ł������True
        // �P���Ȃ����false
        SpellData[] useSpellList;
        if (this.TryGetUsableSpells(out useSpellList))
		{
            // �G���������āA���S�̖��@����Ƃ��̂ݑS�̖��@
            // �G����̂�������A�P�̖��@�����g���Ȃ��ꍇ�͒P�̍U��
            bool hasMultipleEnemies = enemies.Count > 1;

            // �g����Z�̒��ō���͑S�̍U����1�����Ȃ̂�Find���g�p
            // �����ɂȂ�����where�ɕς���K�v����
            SpellData spell = Array.Find(useSpellList, s => s.IsAllTarget);
            // �S�̋Z�����邩�A�����G������
            if (spell != null && hasMultipleEnemies)
            {
                CastSpell(spell, enemies);
            }
            // �S�̋Z���Ȃ������A�܂��͓G���P�̂�����
            else
			{
                // ��̂Ƀ^�[�Q�b�g���i����
                IBattler target = BattleSystem.SelectTarget(enemies, _aiCriteriaData);

                // �S�Ă̋Z���g���邩
                bool hasAllUsableSpells = useSpellList.Length == _spellData.Length;

                // �S�Ă̋Z���g����̂ɑS�̖��@���g�킸else�ɗ����Ƃ������Ƃ�
                // �G�͒P�̂Ȃ̂ŋ��U�����@���g�p���S�͍U������OK
                spell = hasAllUsableSpells
                    ? useSpellList[1]   // �P�̖��@(��)
                    : useSpellList[0];  // �P�̖��@(��)

                CastSpell(spell, new List<IBattler> { target });
			}
        }
        else
		{
            // ���@���g���Ȃ��ꍇ�͎�U��
            IBattler target = BattleSystem.SelectTarget(enemies, _aiCriteriaData);

            if (target != null)
            {
                // ��U����\��
                ReservedAction = new AttackAction(this, target);
            }
            else
            {
                // �����^�[�Q�b�g��������Ȃ��ꍇ�̏���
                Debug.LogWarning($"Mage�F<color=gray>{Name}�͍U������G�������炸�A�������Ȃ������B</color>");
            }
        }
    }

    public void CastSpell(SpellData spell, List<IBattler> targets)
    {
        ReservedAction = new SpellAttackAction(this, targets, spell);
    }



    /// <summary>
    /// ���@�g���̎�ȍU����i
    /// </summary>
    /// <param name="spell">�g�����@</param>
    /// <param name="targets">�U���Ώ�</param>
    /// <returns>�U������������</returns>
    public bool MagicAttack(SpellData spell, List<IBattler> targets)
    {
        return this.SpellAttack(targets, spell);
    }
}