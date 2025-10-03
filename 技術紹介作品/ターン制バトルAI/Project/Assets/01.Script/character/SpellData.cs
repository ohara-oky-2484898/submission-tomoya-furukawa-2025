using System;

/// <summary> 呪文の種類 </summary>
public enum SpellCategory
{
    Heal,
    Attack
    // 強化系も足せるように
}

/// <summary>
/// 呪文データを管理するクラス
/// </summary>
public class SpellData
{
    // プロパティ（読み取り専用）
    public string SpellName { get; }
    public SpellCategory Category { get; }
    public int MpCost { get; }
    public bool IsAllTarget { get; }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="name">呪文の名前</param>
    /// <param name="category">呪文の種類</param>
    /// <param name="cost">魔力消費量</param>
    /// <param name="isAllTarget">全体攻撃か否か</param>
    public SpellData(string name, SpellCategory category, int cost, bool isAllTarget)
    {
        SpellName = name;
        Category = category;
        MpCost = cost;
        IsAllTarget = isAllTarget;
    }


    public override string ToString() => SpellName;

}