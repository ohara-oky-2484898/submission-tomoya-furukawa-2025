using System.Collections.Generic;

public enum BattleOrders
{
	AllyThenEnemy,  // 味方から敵（味方が先に行動、次に敵が行動）
	Speed           // 素早さ順（SPDの高いキャラクターが先に行動）
}


public class StageData
{
    public int StageNum { get;  }  // ステージ番号
    //public string TurnOrder { get; set; }  // 行動順（"AllyThenEnemy" か "Speed"）
    public ITurnOrderStrategy TurnOrderStrategy { get;  }  // 並び替えの種類
    public List<CharacterData> Characters { get;  }  // このステージのキャラクターリスト

    // 引数付きコンストラクタの追加
    public StageData(int stageNum, ITurnOrderStrategy turnOrderStrategy, List<CharacterData> characters)
    {
        StageNum = stageNum;
        TurnOrderStrategy = turnOrderStrategy;
        //  null合体演算子
        Characters = characters ?? new List<CharacterData>();  // charactersがnullであれば空のリストを代入
    }
	public StageData(StageData data)
	{
		StageNum = data.StageNum;
		TurnOrderStrategy = data.TurnOrderStrategy;
		Characters = data.Characters ?? new List<CharacterData>();
	}
}