
/// <summary>
/// �����U�����ł���L�����N�^�[�̃C���^�[�t�F�[�X
/// </summary>
public interface IPhysicalAttacker : IBattler
{
    /// <summary>
    /// �Ώۂɋ��߂̕����U�����s��
    /// </summary>
    /// <param name="target">�U���Ώ�</param>
    /// <returns>�U��������������</returns>
    bool HeavyAttack(IBattler target);
}