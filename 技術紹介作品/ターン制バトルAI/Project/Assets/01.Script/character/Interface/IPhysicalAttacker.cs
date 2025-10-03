
/// <summary>
/// 物理攻撃ができるキャラクターのインターフェース
/// </summary>
public interface IPhysicalAttacker : IBattler
{
    /// <summary>
    /// 対象に強めの物理攻撃を行う
    /// </summary>
    /// <param name="target">攻撃対象</param>
    /// <returns>攻撃が成功したか</returns>
    bool HeavyAttack(IBattler target);
}