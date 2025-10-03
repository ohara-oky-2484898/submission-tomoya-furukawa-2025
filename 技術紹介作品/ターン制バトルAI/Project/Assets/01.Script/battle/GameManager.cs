using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ゲームの進行状況すかプレイヤーの状態など管理
//クラスがゲームの開始、終了、ポーズなどを制御することが多い

public class GameManager : Singleton<GameManager>
{
    /// <summary> フィールド </summary>
    [SerializeField] private AIJudgmentCriteriaData _aiCriteriaData;
    [SerializeField] private int _nowGameLevel = 4;

    private int _gameLevelMax;
    private List<StageData> _stageDatasList;

    /// <summary> プロパティ </summary>
    public int NowGameLevel => _nowGameLevel - 1;
    public AIJudgmentCriteriaData AiCriteriaData() => _aiCriteriaData;
    public StageData NowStageData() => _stageDatasList[NowGameLevel];

    /// <summary> singletonの設定 </summary>
    protected override bool DestroyTargetGameObject => true;

    protected override void Init()
    {
        base.Init();



        // データの読み込み
        StageLoad();

        // 最大ステージ数をセット
        _gameLevelMax = _stageDatasList.Count;
    }

    /// <summary>
    /// ステージを読み込む
    /// </summary>
    private void StageLoad()
    {
        //CSV からキャラクターデータを読み込む
        _stageDatasList = StageDataLoader.LoadStageData();
    }
    
    /// <summary>
    /// ゲームが継続するかどうかの確認用
    /// 最大レベルをClearしていたら終了
    /// まだ次のLevelがあればレベルアップしてRestart
    /// </summary>
    /// <returns>ゲームが終了かどうか</returns>
    public bool IsGameClear()
	{
        Debug.Log("GameManager：ゲームClear！！");
        // ゲームLevelが最大に到達しているか確認して
        // 到達していたら全クリ判定、していないならLevelをあげて続行
        if (CheckGameFinish())
        {
            return true;
        }
        else
		{
            // ゲームLevelをあげるて初期化処理を呼んで続行(再開)
            NextGameLevel();
            BattleManager.Instance.GameReStart();
            return false;
        }
    }

    /// <summary>
    /// レベルアップ用
    /// </summary>
    /// <returns>現在のLevelを1upさせたもの</returns>
    private int NextGameLevel()
	{
        Debug.Log("GameManager：ゲームレベルアップ！！　：" + _nowGameLevel);
        return ++_nowGameLevel;
	}


    /// <summary>
    /// 最終レベルに到達してるか確認して
    /// ゲームが終了かどうか確認
    /// </summary>
    /// <returns>ゲームが終了するかどうか</returns>
    private bool CheckGameFinish()
    {
        return _nowGameLevel == _gameLevelMax;
    }

    public void ExitGame()
	{
        // ゲームクリア
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
    }
}
