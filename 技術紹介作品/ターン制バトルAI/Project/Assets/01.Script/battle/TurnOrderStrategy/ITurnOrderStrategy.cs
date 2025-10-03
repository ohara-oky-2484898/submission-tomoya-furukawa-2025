using System.Collections.Generic;

/// <summary>
/// ターン開始時の戦闘行動順を決めるインタフェース
/// </summary>
public interface ITurnOrderStrategy
{
    string DisplayName { get; }
    /// <summary>
    /// 受け取った全バトラーのリストから、生存しているバトラーを
    /// ある順番に並び替えたリストを返す
    /// </summary>
    /// <param name="allBattlers">全バトラー</param>
    /// <returns>順序付けされた生存バトラーのリスト</returns>
    List<IBattler> GetTurnOrder(List<IBattler> allBattlers);

    /// <summary> 戦略の表示名 </summary>
}
