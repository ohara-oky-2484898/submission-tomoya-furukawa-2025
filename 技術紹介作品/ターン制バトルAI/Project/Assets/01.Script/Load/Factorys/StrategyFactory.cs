using System;

/// <summary>
/// 順番決めの種類や攻撃の種類をまとめたもの
/// 種類が増えたらここに足せばOK
/// ファクトリーパターン(生成を外部に委譲)
/// ハードとソフトの関係みたいなハードに対応したソフトを作るよね
/// </summary>
public static class StrategyFactory
{
    // ターン順戦略を取得する
    public static ITurnOrderStrategy GetTurnOrderStrategy(string strategyType)
    {
        switch (strategyType)
        {
            case "AllyThenEnemy":
                return new AllyThenEnemyTurnOrderStrategy();
            case "Speed":
                return new SpeedTurnOrderStrategy();
            //case "SlowestFirst":
            //    return new SlowestFirstTurnOrderStrategy();
            default:
                throw new ArgumentException($"未対応のターン順戦略: {strategyType}");
        }
    }

    // 攻撃戦略を取得する
    public static IBasicAttackStrategy GetAttackStrategy(string attackType)
    {
        switch (attackType)
        {
            case "SingleHit":
                return new SingleHitAttackStrategy();
            case "Combo":
                return new ComboAttackStrategy();
            case "Ranged":
                return new RangedAttackStrategy();
            default:
                throw new ArgumentException($"未対応の攻撃タイプ: {attackType}");
        }
    }
}