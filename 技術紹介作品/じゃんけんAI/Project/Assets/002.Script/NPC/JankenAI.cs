using System.Collections.Generic;

/// <summary>
/// 手を決めるAIのインタフェース
/// </summary>
public interface IJankenAI
{
    /// <summary>
    /// AIが決めた手を返す
    /// </summary>
    //JankenHand Decide();

    JankenHand Decide(IReadOnlyList<JankenHistory> history);
}

