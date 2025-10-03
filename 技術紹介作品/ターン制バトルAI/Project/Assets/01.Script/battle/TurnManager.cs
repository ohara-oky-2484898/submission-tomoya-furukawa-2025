using System.Collections.Generic;

/// <summary>
/// 戦闘のターン順を管理するクラス
/// </summary>
public class TurnManager
{
    private int _turnCount = 1;

    public int TurnCount => _turnCount;
    /// <summary>
    /// ターン順を決定して返す
    /// </summary>
    /// <param name="turnOrderStrategy">ターン順決定の戦略</param>
    /// <param name="allBattlers">全てのバトラーのリスト</param>
    /// <returns>決定されたターン順のリスト</returns>
    public List<IBattler> GetTurnOrder(ITurnOrderStrategy turnOrderStrategy, List<IBattler> allBattlers)
    {
        return turnOrderStrategy.GetTurnOrder(allBattlers);
    }

    /// <summary>
    /// ターンを1つすすめる
    /// </summary>
    public void NextTurn() => _turnCount++;

    /// <summary>
    /// ターン数をリセット
    /// </summary>
    public void TurnReset() => _turnCount = 0;

}
