using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private Text waitText;
    [SerializeField] private Text resultText;
    //[SerializeField] private Text exitText;
    [SerializeField] private Color winnerColor = Color.white;
    private Color loserColor = Color.white;
    //loser color
    private string inputWaitMessage = "どのてにしようか・・・";
    private string displayCallMessage = "";
    private string displayResultMessage = "";
    //private string displayExitMessage = "";

    protected override bool DestroyTargetGameObject => true;
    public string DisplayCallMessage => displayCallMessage;

    protected override void Init()
    {
        // メッセージ初期化
        waitText.text = inputWaitMessage;
        resultText.gameObject.SetActive(false);
        waitText.gameObject.SetActive(false);
        //exitText.gameObject.SetActive(false);

        loserColor = new Color(1 - winnerColor.r, 1 - winnerColor.g, 1 - winnerColor.b, winnerColor.a);
        base.Init();
    }


    private void Update()
    {

    }

    // じゃんけんぽん、あいこでしょ
    public void SetCallText(JankenState state, bool decided)
	{
        if (state == JankenState.Play)
        {
            // 三項演算子("false" or "true")(決まっていない、決まった)
            displayCallMessage = !decided ? "じゃんけん" : "ぽん！";
        }
        else if(state == JankenState.Rematch)
		{
            displayCallMessage = !decided ? "あいこで" : "しょ！";
        }
	}
    // 最初はぐー
    public void SetCallText(JankenState state)
	{
        displayCallMessage = "さいしょはグー";
    }

    public void SetResultText(bool playerWin, bool isDisplay)
    {
        displayResultMessage = playerWin ? "きみのかち！" : "きみのまけ";
        resultText.color = playerWin ? winnerColor : loserColor;
        resultText.text = displayResultMessage;
        resultText.gameObject.SetActive(isDisplay);
    }

	/// <summary>
	/// 入力待ちの表示切り替え用
	/// </summary>
	/// <param name="isDisplay">表示するか否か</param>
	public void ButtonSelectText(bool isDisplay)
    {
        waitText.gameObject.SetActive(isDisplay);
    }

    /// <summary>
    /// ゲーム終了時にメッセージを表示する用
    /// </summary>
    /// <param name="playerWin">プレイヤー視点での勝敗</param>
    public void EndText(bool playerWin)
	{
        //displayExitMessage = playerWin ? "おめでとう！\nきみのかちだ！" : "ざんねん…\nきみのまけ…";
        //      exitText.text = displayExitMessage;
        //      exitText.gameObject.SetActive(true);

        //displayResultMessage = playerWin ? "おめでとう！\nきみのかちだ！" : "ざんねん…\nきみのまけ…";
        // 色設定後にデバッグログを追加
        if (playerWin)
        {
            displayResultMessage = "おめでとう！\nきみのかちだ！";
            resultText.color = winnerColor;
            Debug.Log("Winner Color: " + winnerColor); // ここで色を確認
        }
        else
        {
            displayResultMessage = "ざんねん…\nきみのまけ…";
            resultText.color = loserColor;
            Debug.Log("Loser Color: " + loserColor); // ここで色を確認
        }


        resultText.text = displayResultMessage;
        resultText.gameObject.SetActive(true);
    }

}
