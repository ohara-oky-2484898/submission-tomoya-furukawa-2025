using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public class GameManager : MonoBehaviour
public class GameManager : Singleton<GameManager>
{
    private int gameFinishLine = 20;
    public int GameFinishLine => gameFinishLine;

    public int winCount = 0;
    public int loseCount = 0;
    public int drawCount = 0;
    public int playCount = 0;

    private int turnCount = 0; // 進行中のターン数

    //public bool IsFinish = false;

    protected override bool DestroyTargetGameObject => true;


    protected override void Init()
	{
        base.Init();
	}


	private void Update()
    {
        
    }

    public bool CheckGameFinish()
    {
        return winCount >= gameFinishLine || loseCount >= gameFinishLine;
    }
}
