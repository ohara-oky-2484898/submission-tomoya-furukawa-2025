using System.Collections.Generic;
using System;

/// <summary>
/// �L�����N�^�[�̐E�ƁiRole�j�ɉ����ēK�؂� IBattler �𐶐�����t�@�N�g���[
/// </summary>
public static class CharacterFactory
{
    // Action ���@�߂�l���Ȃ��֐�(�f���Q�[�g)
    // Func ���@�߂�l�̂���֐����f���Q�[�g�Ɂ@���@Func<�����A�߂�l>
    // ����̓R���X�g���N�^�̃L�����f�[�^�������ɖ߂�l�̓C���^�t�F�[�X(IBattler)���p�������L�����B
    private static readonly Dictionary<string, Func<CharacterData, IBattler>> roleMap =
        new Dictionary<string, Func<CharacterData, IBattler>>(StringComparer.OrdinalIgnoreCase)
        {
            //{ "Hero", data => new Hero(data.Name, data.Status, ParseTeam(data.Team), data.BasicAttackStrategy) },
            // �����̒����̂���CharacterData���󂯎��R���X�g���N�^��p�ӂ��ĉǐ���������
            { "Hero", charaData => new Hero(charaData) },
            { "Warrior", charaData => new Warrior(charaData) },
            { "Monk", charaData => new Monk(charaData) },
            { "Mage", charaData => new Mage(charaData) },
            //{ "Ninja", data => new Ninja(data) },
            // �ǉ��FNinja�ȂǐV�����E�Ƃ𑫂�������OK�I
        };

    /// <summary>
    /// ��������󂯎�����f�[�^�ɑΉ����Ă�����̂�Ԃ�
    /// </summary>
    /// <param name="data">�L�����̃f�[�^</param>
    /// <returns>������������</returns>
    /// <exception cref="ArgumentException"></exception>
    public static IBattler Create(CharacterData charaData)
    {
        if (roleMap.TryGetValue(charaData.Role, out var constructor))
        {
            return constructor(charaData);
        }

        throw new ArgumentException($"���Ή��̖�E: {charaData.Role}");
    }
}

