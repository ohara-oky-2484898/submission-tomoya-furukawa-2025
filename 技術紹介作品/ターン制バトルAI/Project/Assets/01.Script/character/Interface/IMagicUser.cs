using System.Collections.Generic;

/// <summary>
/// ���@���g�����Ƃ��ł���L�����N�^�[
/// �i�E�ҁA���@�g���A�m���Ȃǁj
/// </summary>
public interface ISpellCaster : IBattler// �� IBattler ���p�����Ă���̂́AStatus ���g����������
{
    /// <summary>
    /// �g�p�\�Ȗ��@�̃f�[�^�ꗗ
    /// </summary>
    SpellData[] SpellData { get; }

    // bool CastSpell(SpellData spell, List<IBattler> targets);
}


/// <summary>
/// ���@�ŉ񕜂��ł���L�����N�^�[�i�m���j
/// </summary>
public interface IHealer : ISpellCaster
{
    /// <summary>
    /// �Ώۂ��񕜂���
    /// </summary>
    /// <param name="spell">�g�p����񕜖��@</param>
    /// <param name="targets">�񕜑Ώۂ̃��X�g</param>
    /// <returns>�񕜂ɐ���������</returns>
    bool Heal(SpellData spell, List<IBattler> targets);
}

/// <summary>
/// ���ȉ񕜂݂̂��ł���L�����N�^�[�i�E�ҁj
/// ���L�����N�^�[�ւ̉񕜂͂ł��Ȃ�
/// </summary>
public interface ISelfHealer : ISpellCaster
{
    /// <summary>
    /// �������g���񕜂���
    /// </summary>
    /// <param name="spell">�g�p����񕜖��@</param>
    /// <returns>�񕜂ɐ���������</returns>
    bool Heal(SpellData spell);
}


/// <summary>
/// ���@�ōU�����ł���L�����N�^�[�i���@�g���j
/// </summary>
public interface IMagicAttacker : ISpellCaster
{
    /// <summary>
    /// �Ώۂɖ��@�U�����s��
    /// </summary>
    /// <param name="spell">�g�p����U�����@</param>
    /// <param name="targets">�U���Ώۂ̃��X�g</param>
    /// <returns>�U���ɐ���������</returns>
    bool MagicAttack(SpellData spell, List<IBattler> targets);
}