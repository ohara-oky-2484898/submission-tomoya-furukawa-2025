using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;


/// <summary>
/// ��m�Ȃ�
/// �o�g�����ł���,���U�����ł���
/// �s��
/// �@ AI�̗D���ɂ���������
///     (����̃��[���_�� or �c���HP���Ȃ���_��)
///     �D��x�̍����G���Ђ�����U��
/// </summary>
public class Warrior : BattlerBase, IPhysicalAttacker
{
    /// <summary> �t�B�[���h </summary>
    private IBasicAttackStrategy _strategy;

    /// <summary> �v���p�e�B </summary>
    public override IBasicAttackStrategy BasicAttackStrategy => _strategy;

    /// <summary> �R���X�g���N�^ </summary>
    public Warrior(CharacterData data) : base(data) { _strategy = data.BasicAttackStrategy; }


    /// <summary> ���\�b�h </summary>

    public override int GetBasicAttackStatValue() => Status.Attack;

    public override void DecideAction(List<IBattler> allBattlers)
    {
        var enemies = this.GetEnemies(allBattlers);  // �G�L�������擾

        // �^�[�Q�b�g��I��
        IBattler target = BattleSystem.SelectTarget(enemies, _aiCriteriaData);

        if (target != null)
        {
            ReservedAction = new HeavyAttackAction(this, target);
        }
    }

    public bool HeavyAttack(IBattler target)
    {
        // �����U������
        return BattleSystem.Attack(this, target);
    }
}
