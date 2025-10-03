using System;

/// <summary>
/// ���Ԍ��߂̎�ނ�U���̎�ނ��܂Ƃ߂�����
/// ��ނ��������炱���ɑ�����OK
/// �t�@�N�g���[�p�^�[��(�������O���ɈϏ�)
/// �n�[�h�ƃ\�t�g�̊֌W�݂����ȃn�[�h�ɑΉ������\�t�g�������
/// </summary>
public static class StrategyFactory
{
    // �^�[�����헪���擾����
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
                throw new ArgumentException($"���Ή��̃^�[�����헪: {strategyType}");
        }
    }

    // �U���헪���擾����
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
                throw new ArgumentException($"���Ή��̍U���^�C�v: {attackType}");
        }
    }
}