using System.Collections.Generic;

public enum BattleOrders
{
	AllyThenEnemy,  // ��������G�i��������ɍs���A���ɓG���s���j
	Speed           // �f�������iSPD�̍����L�����N�^�[����ɍs���j
}


public class StageData
{
    public int StageNum { get;  }  // �X�e�[�W�ԍ�
    //public string TurnOrder { get; set; }  // �s�����i"AllyThenEnemy" �� "Speed"�j
    public ITurnOrderStrategy TurnOrderStrategy { get;  }  // ���ёւ��̎��
    public List<CharacterData> Characters { get;  }  // ���̃X�e�[�W�̃L�����N�^�[���X�g

    // �����t���R���X�g���N�^�̒ǉ�
    public StageData(int stageNum, ITurnOrderStrategy turnOrderStrategy, List<CharacterData> characters)
    {
        StageNum = stageNum;
        TurnOrderStrategy = turnOrderStrategy;
        //  null���̉��Z�q
        Characters = characters ?? new List<CharacterData>();  // characters��null�ł���΋�̃��X�g����
    }
	public StageData(StageData data)
	{
		StageNum = data.StageNum;
		TurnOrderStrategy = data.TurnOrderStrategy;
		Characters = data.Characters ?? new List<CharacterData>();
	}
}