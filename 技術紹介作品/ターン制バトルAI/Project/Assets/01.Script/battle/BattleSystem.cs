using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public enum BattleResult
{
    Continue, // �����p�����i�ǂ�����S�ł��Ă��Ȃ��j
    Win,      // �v���C���[�̏���
    Lose      // �v���C���[�̔s�k
}

/// <summary>
/// �o�g���V�X�e���ɂ����鋤�ʏ�����S������N���X
/// </summary>
public static class BattleSystem
{
    // �U���Ɏ��s����Ƃ�
    // �@�U����������Ȃ�(����͂Ȃ�)
    // �A�G����������ł���(�ʂ̓G��_��)
    // �B�������E���ꂽ(�Ă΂��̂����������H�i�Ă΂��O�ɒe���ق�������)


    /// <summary>
    /// �U�������̊g�����\�b�h
    /// </summary>
    /// <param name="attacker">�U����</param>
    /// <param name="target">�U���Ώ�</param>
    public static bool Attack(this IBattler attacker, IBattler target)
    {
        if (!attacker.IsAlive) return false;

        int damage = CalculateEffectAmount(attacker.Status.Attack);
        Debug.Log($"BattleSystem<color=yellow>{attacker.Name}</color>��<color=yellow>{target.Name}</color>��<color=red>�U��</color>�����i<color=red>{damage}�_���[�W</color>�j");
        return ApplyHpChange(target, -damage);
    }

    /// <summary>
    ///  ��{�U��(��U���͈З͔���)
    ///  ��U������
    /// </summary>
    /// <param name="attacker">�U����</param>
    /// <param name="target">�U���Ώ�</param>
    public static bool BasicAttack(this IBattler attacker, IBattler target)
    {
        if (!attacker.IsAlive) return false;

        // ���ꂼ��̃o�g���[�̎Q�Ƃ������X�e�[�^�X�̐��l���Q�Ƃ��Čv�Z������
        // ���̂܂܂���Attack()��SpellAttack()�Ɠ����_���[�W�Ȃ̂�
        // �З͂𔼕��ɂ��Ă���
        int damage = CalculateEffectAmount(attacker.GetBasicAttackStatValue()) / 2;
        Debug.Log($"BattleSystem�F<color=yellow>{attacker.Name}</color>��<color=yellow>{target.Name}</color>��<color=red>��U��</color>�������i<color=red>{damage}�_���[�W</color>�j");
        return ApplyHpChange(target, -damage);

    }

    /// <summary>
    /// ���@�U������
    /// </summary>
    /// <param name="attacker">���@�U����</param>
    /// <param name="target">���@�U�����󂯂�Ώ�</param>
    public static bool SpellAttack(this IMagicAttacker attacker, List<IBattler> targets, SpellData data)
    {
        if (!attacker.IsAlive)return false;

        if (attacker.Status.MP < data.MpCost ||
            data.Category != SpellCategory.Attack) return false;

        // MP����
        attacker.Status.MP -= data.MpCost;

        // UI�X�V���Ă�
        UIManager.Instance.UpdateHUD(attacker);

        // ��b�_���[�W�v�Z
        int damage = CalculateEffectAmount(attacker.Status.Magic);
        // ��b�_���[�W�ɑS�̍U���Ȃ�A�_���[�W���΂炯��悤��(���U����Ē�З͂ɂȂ�悤��)
        // �U������l�����Ŋ����Ă�����
        // ��3�̈ȏ�Ȃ�3�Ŋ���(�З͂��Ⴗ����̖h�~)
        int applyDamage = targets.Count >= 3
            ? damage / 3
            : damage / targets.Count;

        foreach (IBattler target in targets)
        {
            ApplyHpChange(target, -applyDamage);
            Debug.Log($"BattleSystem�F<color=yellow>{attacker.Name}</color>��<color=yellow>{target.Name}</color>��<color=red>���@�U��</color>�����i<color=red>{applyDamage}�_���[�W</color>�j");
        }
        return true;

    }


    /// <summary>
    /// �񕜏���
    /// </summary>
    /// <param name="healer">�񕜖��@�g�p��</param>
    /// <param name="target">�񕜑Ώ�</param>
    public static bool SpellHeal(this IHealer healer, IBattler target, SpellData data)
	{
        if (!healer.IsAlive) return false;

        // 50 < 50
        if (healer.Status.MP < data.MpCost ||
            data.Category != SpellCategory.Heal) return false;


        // MP����
        healer.Status.MP -= data.MpCost;
        // UI�X�V���Ă�
        UIManager.Instance.UpdateHUD(healer);


        int healAmount = CalculateEffectAmount(healer.Status.Magic);
        Debug.Log($"BattleSystem�F<color=yellow>{healer.Name}</color>��<color=yellow>{target.Name}</color>��<color=green>��</color>�����i<color=green>{healAmount}��</color>�j");
        return ApplyHpChange(target, healAmount);

    }

    /// <summary>
    /// ���ȉ񕜏���
    /// </summary>
    /// <param name="healer">�񕜖��@�g�p��</param>
    public static bool SpellSelfHeal(this ISelfHealer healer, SpellData data)
    {
        if (!healer.IsAlive) return false;

		if (healer.Status.MP < data.MpCost
            || data.Category != SpellCategory.Heal )
            return false;

        // MP����
        healer.Status.MP -= data.MpCost;
        // UI�X�V���Ă�
        UIManager.Instance.UpdateHUD(healer);

        int healAmount = CalculateEffectAmount(healer.Status.Magic);
        Debug.Log($"BattleSystem�F<color=yellow>{healer.Name}</color>��<color=yellow>���g</color>��<color=green>��</color>�����i<color=green>{healAmount}��</color>�j");
        return ApplyHpChange(healer, healAmount);

    }

    /// <summary>
    /// ���́E�U���͂�����ʗʂ��v�Z�i�_���[�W�E�񕜋��ʁj
    /// </summary>
    public static int CalculateEffectAmount(int power)
    {
        int baseValue = power / 2;
        int randomBonus = Random.Range(0, baseValue + 1);
        return baseValue + randomBonus + 1;
    }

    /// <summary>
    /// HP��ϓ���K�������鏈���i���Ȃ�񕜁A���Ȃ�_���[�W�j
    /// </summary>
    /// <param name="target">�Ώۃo�g���[</param>
    /// <param name="amount">�ϓ��ʁi��: �񕜁A��: �_���[�W�j</param>
    public static bool ApplyHpChange(IBattler target, int amount)
    {
        // MEMO�F��������ł閡�����񕜂������ꍇ�͌������Ȃ���
        if (!target.IsAlive) return false;

        Debug.Log($"BattleSystem�F�K���O<color=yellow>{target.Name}</color>��<color=blue>�c��HP {target.Status.HP}/{target.Status.MaxHP}</color>");
        target.Status.HP = Mathf.Clamp(target.Status.HP + amount, 0, target.Status.MaxHP);
        Debug.Log($"BattleSystem�F�K����<color=yellow>{target.Name}</color>��<color=blue>�c��HP {target.Status.HP}/{target.Status.MaxHP}</color>");

        // ������UI�X�V���Ăԁi��j
        UIManager.Instance.UpdateHUD(target);

        // HP��0�ȉ��ł���Ύ��S����
        if (target.Status.HP <= 0)
        {
            Debug.Log($"BattleSystem�F<color=red><b>{target.Name}������</b></color>");


            // ���S���UI�X�V���K�v�Ȃ�A�����ł��ĂԂ�
            //UIManager.Instance.OnDestroy(_target);
            UIManager.Instance.PlayDeathEffectFor(target);
            target.Dead();
        }
        return true;
    }

    /// <summary>
    /// ���̃o�g���[�ɑ΂���G�̃��X�g���擾
    /// </summary>
    /// <param name="currentBattler">���݂̃o�g���[�i�A�N�^�[�j</param>
    /// <param name="allBattlers">�S�o�g���[</param>
    /// <returns>���݂̃o�g���[�ɑ΂���G�̃��X�g</returns>
    public static List<IBattler> GetEnemies(this IBattler currentBattler, List<IBattler> allBattlers)
    {
        return allBattlers.Where(b => b.Team != currentBattler.Team && b.IsAlive).ToList();
    }

    /// <summary>
    /// ���̃o�g���[�ɑ΂��閡���̃��X�g���擾
    /// </summary>
    /// <param name="currentBattler">���݂̃o�g���[�i�A�N�^�[�j</param>
    /// <param name="allBattlers">�S�o�g���[</param>
    /// <returns>���݂̃o�g���[�ɑ΂��閡���̃��X�g</returns>
    public static List<IBattler> GetAllies(this IBattler currentBattler, List<IBattler> allBattlers)
    {
        return allBattlers.Where(b => b.Team == currentBattler.Team && b.IsAlive).ToList();
    }

    /// <summary>
    /// ���s����ǂ��炩�A�S�ł��Ă��Ȃ����m�F�p
    /// ����̐݌v�Ȃ���������͂Ȃ����̂Ƃ���
    /// </summary>
    /// <param name="battlers">�S�o�g���[</param>
    /// <returns>�퓬�I�����ǂ���</returns>
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
    /// �ł�HP�����Ȃ��G���擾����
    /// </summary>
    /// <param name="enemies">�G�̃��X�g</param>
    /// <returns>HP���ł��Ⴂ�G�B�Y�������Ȃ��ꍇ�� null</returns>
    public static IBattler GetLowHpEnemy(List<IBattler> enemies)
    {
        return enemies
            .Where(e => e.IsAlive)
            .OrderBy(e => e.Status.HP)
            .FirstOrDefault();
    }


    /// <summary>
    /// ���݂� MP �Ŏg�p�\�Ȏ������X�g���擾����
    /// </summary>
    /// <param name="caster">�������g���L�����N�^�[�iISpellCaster�j</param>
    /// <param name="availableSpells">�g��������̃��X�g�� out �ŕԂ�</param>
    /// <returns>�g���������1�ł������ true�A�Ȃ���� false</returns>
    public static bool TryGetUsableSpells(this ISpellCaster caster, out SpellData[] availableSpells)
    {
        availableSpells = caster.SpellData
            .Where(spell => caster.Status.MP >= spell.MpCost)
            .ToArray();

        return availableSpells.Length > 0;
    }

    /// <summary>
    /// �ł��D�揇�ʂ��������[���̃L�������擾
    /// �D��x�������ꍇ�AHP���Ⴂ�L������Ԃ�
    /// </summary>
    /// <param name="enemies">�G�̃��X�g</param>
    /// <param name="aiCriteriaData">���f��f�[�^</param>
    /// <returns>�ł��D�悳��郍�[�������L����</returns>
    public static IBattler GetHighestPriorityRoleCharacter(List<IBattler> enemies, AIJudgmentCriteriaData aiCriteriaData)
    {
        // �D��x���Ⴂ�i�����D��x�j���[����D�悷��
        var highestPriorityRoleEnemy = enemies
            .Where(e => e.IsAlive)
            .OrderBy(e => aiCriteriaData.GetRolePriority(e.Roll))  // ���[���D��x���擾���ă\�[�g
            .ThenBy(e => e.Status.HP)  // �����D��x�̏ꍇ��HP���Ⴂ����I��
            .FirstOrDefault();

        return highestPriorityRoleEnemy;
    }

    public static IBattler SelectTarget(List<IBattler> enemies, AIJudgmentCriteriaData aiCriteriaData)
    {
        if (aiCriteriaData.PrioritizeLowHpEnemyOverRole)
        {
            Debug.Log("[BattleSystem] HP�D��Ń^�[�Q�b�g�I��");
            var lowHpEnemy = GetLowHpEnemy(enemies);
            if (lowHpEnemy != null)
            {
                Debug.Log($"[BattleSystem] HP���ł����Ȃ��G��: {lowHpEnemy.Name} (HP: {lowHpEnemy.Status.HP})");
            }
            else
            {
                Debug.Log("[BattleSystem] HP���ł����Ȃ��G�͌�����܂���ł���");
            }
            return lowHpEnemy; // HP�D��
        }
        else
        {
            Debug.Log("[BattleSystem] ���[���D��Ń^�[�Q�b�g�I��");
            var highestPriorityEnemy = GetHighestPriorityRoleCharacter(enemies, aiCriteriaData);
            if (highestPriorityEnemy != null)
            {
                Debug.Log($"[BattleSystem] ���[���D��őI�΂ꂽ�G��: {highestPriorityEnemy.Name} (Role: {highestPriorityEnemy.Roll}, HP: {highestPriorityEnemy.Status.HP})");
            }
            else
            {
                Debug.Log("[BattleSystem] ���[���D��Ń^�[�Q�b�g��������܂���ł���");
            }
            return highestPriorityEnemy; // ���[���D��
        }
    }
}
