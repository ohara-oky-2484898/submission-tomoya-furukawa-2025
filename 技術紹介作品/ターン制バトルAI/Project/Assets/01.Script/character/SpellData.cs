using System;

/// <summary> �����̎�� </summary>
public enum SpellCategory
{
    Heal,
    Attack
    // �����n��������悤��
}

/// <summary>
/// �����f�[�^���Ǘ�����N���X
/// </summary>
public class SpellData
{
    // �v���p�e�B�i�ǂݎ���p�j
    public string SpellName { get; }
    public SpellCategory Category { get; }
    public int MpCost { get; }
    public bool IsAllTarget { get; }

    /// <summary>
    /// �R���X�g���N�^
    /// </summary>
    /// <param name="name">�����̖��O</param>
    /// <param name="category">�����̎��</param>
    /// <param name="cost">���͏����</param>
    /// <param name="isAllTarget">�S�̍U�����ۂ�</param>
    public SpellData(string name, SpellCategory category, int cost, bool isAllTarget)
    {
        SpellName = name;
        Category = category;
        MpCost = cost;
        IsAllTarget = isAllTarget;
    }


    public override string ToString() => SpellName;

}