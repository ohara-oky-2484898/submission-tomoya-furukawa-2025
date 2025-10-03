/// <summary>
/// ���݂̏��s��\��
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
		text += $"{GameManager.Instance.GameFinishLine} �{���I\n\n";
		text += "�����F" + GameManager.Instance.winCount.ToString() + "\n";
		text += "�܂��F" + GameManager.Instance.loseCount.ToString() + "\n";
		text += "�Ђ��킯�F" + GameManager.Instance.drawCount.ToString();

		scoreText.text = text;
	}
}
