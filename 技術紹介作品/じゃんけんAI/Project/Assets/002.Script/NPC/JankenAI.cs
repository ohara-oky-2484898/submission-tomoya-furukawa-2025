using System.Collections.Generic;

/// <summary>
/// ������߂�AI�̃C���^�t�F�[�X
/// </summary>
public interface IJankenAI
{
    /// <summary>
    /// AI�����߂����Ԃ�
    /// </summary>
    //JankenHand Decide();

    JankenHand Decide(IReadOnlyList<JankenHistory> history);
}

