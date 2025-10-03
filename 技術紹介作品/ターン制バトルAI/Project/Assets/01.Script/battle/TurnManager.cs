using System.Collections.Generic;

/// <summary>
/// �퓬�̃^�[�������Ǘ�����N���X
/// </summary>
public class TurnManager
{
    private int _turnCount = 1;

    public int TurnCount => _turnCount;
    /// <summary>
    /// �^�[���������肵�ĕԂ�
    /// </summary>
    /// <param name="turnOrderStrategy">�^�[��������̐헪</param>
    /// <param name="allBattlers">�S�Ẵo�g���[�̃��X�g</param>
    /// <returns>���肳�ꂽ�^�[�����̃��X�g</returns>
    public List<IBattler> GetTurnOrder(ITurnOrderStrategy turnOrderStrategy, List<IBattler> allBattlers)
    {
        return turnOrderStrategy.GetTurnOrder(allBattlers);
    }

    /// <summary>
    /// �^�[����1�����߂�
    /// </summary>
    public void NextTurn() => _turnCount++;

    /// <summary>
    /// �^�[���������Z�b�g
    /// </summary>
    public void TurnReset() => _turnCount = 0;

}
