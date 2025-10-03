using System.Collections.Generic;
using System;

/// <summary>
/// キャラクターの職業（Role）に応じて適切な IBattler を生成するファクトリー
/// </summary>
public static class CharacterFactory
{
    // Action →　戻り値がない関数(デリゲート)
    // Func →　戻り値のある関数をデリゲートに　→　Func<引数、戻り値>
    // 今回はコンストラクタのキャラデータを引数に戻り値はインタフェース(IBattler)を継承したキャラ達
    private static readonly Dictionary<string, Func<CharacterData, IBattler>> roleMap =
        new Dictionary<string, Func<CharacterData, IBattler>>(StringComparer.OrdinalIgnoreCase)
        {
            //{ "Hero", data => new Hero(data.Name, data.Status, ParseTeam(data.Team), data.BasicAttackStrategy) },
            // ↑この長いのを↓CharacterDataを受け取るコンストラクタを用意して可読性をあげた
            { "Hero", charaData => new Hero(charaData) },
            { "Warrior", charaData => new Warrior(charaData) },
            { "Monk", charaData => new Monk(charaData) },
            { "Mage", charaData => new Mage(charaData) },
            //{ "Ninja", data => new Ninja(data) },
            // 追加：Ninjaなど新しい職業を足すだけでOK！
        };

    /// <summary>
    /// 辞書から受け取ったデータに対応しているものを返す
    /// </summary>
    /// <param name="data">キャラのデータ</param>
    /// <returns>生成したもの</returns>
    /// <exception cref="ArgumentException"></exception>
    public static IBattler Create(CharacterData charaData)
    {
        if (roleMap.TryGetValue(charaData.Role, out var constructor))
        {
            return constructor(charaData);
        }

        throw new ArgumentException($"未対応の役職: {charaData.Role}");
    }
}

