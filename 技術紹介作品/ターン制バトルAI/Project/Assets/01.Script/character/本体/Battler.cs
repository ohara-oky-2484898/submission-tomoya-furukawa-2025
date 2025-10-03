using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


// virtual��abstract�̎g������
// default�łȂɂ����������������Ȃ�
// virtual
// �S�ĂɌʂŎ����������Ăق����Ȃ�
// abstract

/// <summary>
/// �o�g���̊�{�i���ہj�N���X
/// </summary>
public abstract class BattlerBase : IBattler
{
    /// <summary>
    /// �ǂ̔h���N���X�ɂ����ʂ���v���p�e�B
    /// �S�Ẵo�g���[�ɋ��ʂ�����̂Ƃ���virtual�ɂ���
    /// �e�N���X�ň�x�ݒ肷��΂悢
    /// �K���h����Ō��߂Ăق������̂�abstract
    /// </summary>

    /// <summary> �t�B�[���h </summary>
    protected AIJudgmentCriteriaData _aiCriteriaData;
    // �s���\�񂵂����̂��ꎞ�ۑ����锠
    private IBattleAction _reservedAction;

    /// <summary> �v���p�e�B </summary>
    public string Name { get; }
    public CharacterStatus Status { get;}
    public Sprite Sprite { get;}
    public bool IsAlive => Status.HP > 0;
    public Team Team { get;}   // �������G���A�����̐w�c
    public Role Roll { get;}

    // �L��������(���@�g���͔��͂Ȗ��@�U�����^�E�҂͂����ƍU�������Ăق���)
    // ���ǎQ�Ɛ悪�����Ȃ͕̂�(���@�Ȃ̂ɍU���Q�ƁH)�Ȃ̂ł��ꂼ��Ō��߂���悤��
    // �ʏ�U���̃_���[�W�v�Z�p�ɃX�e�[�^�X�̎Q�ƕ����擾����֐�
    public abstract int GetBasicAttackStatValue();
    public abstract IBasicAttackStrategy BasicAttackStrategy { get; }

    public IBattleAction ReservedAction
    {
        get => _reservedAction;
        // ReservedAction��protected setter�Ŕh������Z�b�g�\��
        protected set => _reservedAction = value;
    }


    /// <summary>
    /// �R���X�g���N�^
    /// �ǂ݂��񂾃L�����N�^�f�[�^�����̂܂ܓn����instance�ł���悤�ɗp��
    /// </summary>
    /// <param name="data">�L�����f�[�^</param>
    public BattlerBase(CharacterData data)
    {
        Name = data.Name;
        Status = new CharacterStatus(data.Status);
        Team = ParseTeam(data.Team);
        Sprite = data.Sprite;
        Roll = ParseRoll(data.Role);
    }

    /// <summary> ���\�b�h </summary>
    public virtual void Init()
    {
        // �f�t�H���g�ł͓��ɉ������Ȃ��ݒ肾��
        _aiCriteriaData =  GameManager.Instance.AiCriteriaData();  // AI���f��f�[�^���擾
    }

    public bool BasicAttack(IBattler target)
    { 
        // �p����Ŏ����������ꂼ��̊�{�U���̃p�^�[���ǂꂩ�Ă�
        return BasicAttackStrategy.Execute(this, target); 
    }

    /// <summary> �s�������߂� </summary>
    /// <param name="allBattlers">�S�o�g���[</param>
    public abstract void DecideAction(List<IBattler> allBattlers);

    /// <summary> �s�������s </summary>
    /// �Ȃ�炩�ŁA���s�����Ƃ��͎�U���ɐ؂�ւ�
    //public virtual void ExecuteAction()
    public void ExecuteAction()
    {
        if (!IsAlive)
        {
            Debug.Log($"{Name} �͂��łɎ��S���Ă��邽�߁A�s���ł��܂���B");
            return;
        }

        Debug.Log($"[DEBUG] {Name} �� ExecuteAction ���Ă΂ꂽ");

        if (ReservedAction == null)
        {
            Debug.LogWarning($"[DEBUG] {Name} �� ReservedAction �� null ������");
            return;
        }

        try
        {
            if (ReservedAction.Execute())
            {
                Debug.LogWarning($"{Name} ���\��s�������s�����I");
                ReservedAction = null;
            }
            else
            {
                Debug.LogWarning($"{Name} ���\��s�������s���������s�����I");
                //Debug.LogWarning($"{Name} �̍s���͎��s�������߁A�t�H�[���o�b�N�U�������݂܂��B");
                //var enemies = this.GetEnemies(BattleManager.Instance.AllBattlers)
                //                  .Where(b => b.IsAlive)
                //                  .ToList();

                //var fallbackTarget = BattleSystem.SelectTarget(enemies, _aiCriteriaData);
                //if (fallbackTarget != null)
                //{
                //    BasicAttack(fallbackTarget);
                //}
                //else
                //{
                //    Debug.LogWarning($"{Name} �͍U���Ώۂ������炸�A�������Ȃ������B");
                //}

                ReservedAction = null;
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"[ERROR] {Name} �� ExecuteAction ���s���ɗ�O����: {ex.Message}\n{ex.StackTrace}");
        }
    }
    public virtual void Dead()
    {
        Debug.Log($"{Name} has fallen!");
    }


    /// <summary> private���\�b�h </summary>
    
    /// <summary>
    /// ������("Ally")����񋓌^(Team.Ally)�ɕϊ�����֐�
    /// </summary>
    /// <param name="team"></param>
    /// <returns></returns>
    private Team ParseTeam(string team)
    {
        // (enum�w��)Enum.Parse(enum�̌^�Avalue�A�啶������������ʂ��Ȃ�)
        // false�Ȃ犮�S��v�̂�
        return (Team)Enum.Parse(typeof(Team), team, ignoreCase: true);
    }
    private Role ParseRoll(string roll)
    {
        // (enum�w��)Enum.Parse(enum�̌^�Avalue�A�啶������������ʂ��Ȃ�)
        // false�Ȃ犮�S��v�̂�
        return (Role)Enum.Parse(typeof(Role), roll, ignoreCase: true);
    }
}
