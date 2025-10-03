/// <summary>
/// 現在の勝敗を表示
/// </summary>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateScore : MonoBehaviour
{
    [SerializeField] Text scoreText;
    private string text = "";

	private void Update()
	{
		SetText();
	}

	public void SetText()
	{
		text = "";
		text += $"{GameManager.Instance.GameFinishLine} 本先取！\n\n";
		text += "かち：" + GameManager.Instance.winCount.ToString() + "\n";
		text += "まけ：" + GameManager.Instance.loseCount.ToString() + "\n";
		text += "ひきわけ：" + GameManager.Instance.drawCount.ToString();

		scoreText.text = text;
	}
}
