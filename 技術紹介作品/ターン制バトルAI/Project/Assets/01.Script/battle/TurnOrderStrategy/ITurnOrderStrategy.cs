using System.Collections.Generic;

/// <summary>
/// �^�[���J�n���̐퓬�s���������߂�C���^�t�F�[�X
/// </summary>
public interface ITurnOrderStrategy
{
    string DisplayName { get; }
    /// <summary>
    /// �󂯎�����S�o�g���[�̃��X�g����A�������Ă���o�g���[��
    /// ���鏇�Ԃɕ��ёւ������X�g��Ԃ�
    /// </summary>
    /// <param name="allBattlers">�S�o�g���[</param>
    /// <returns>�����t�����ꂽ�����o�g���[�̃��X�g</returns>
    List<IBattler> GetTurnOrder(List<IBattler> allBattlers);

    /// <summary> �헪�̕\���� </summary>
}
