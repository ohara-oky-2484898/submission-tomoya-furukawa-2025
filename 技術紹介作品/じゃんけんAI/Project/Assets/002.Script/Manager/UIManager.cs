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
    private string inputWaitMessage = "�ǂ̂Ăɂ��悤���E�E�E";
    private string displayCallMessage = "";
    private string displayResultMessage = "";
    //private string displayExitMessage = "";

    protected override bool DestroyTargetGameObject => true;
    public string DisplayCallMessage => displayCallMessage;

    protected override void Init()
    {
        // ���b�Z�[�W������
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

    // ����񂯂�ۂ�A�������ł���
    public void SetCallText(JankenState state, bool decided)
	{
        if (state == JankenState.Play)
        {
            // �O�����Z�q("false" or "true")(���܂��Ă��Ȃ��A���܂���)
            displayCallMessage = !decided ? "����񂯂�" : "�ۂ�I";
        }
        else if(state == JankenState.Rematch)
		{
            displayCallMessage = !decided ? "��������" : "����I";
        }
	}
    // �ŏ��͂��[
    public void SetCallText(JankenState state)
	{
        displayCallMessage = "��������̓O�[";
    }

    public void SetResultText(bool playerWin, bool isDisplay)
    {
        displayResultMessage = playerWin ? "���݂̂����I" : "���݂̂܂�";
        resultText.color = playerWin ? winnerColor : loserColor;
        resultText.text = displayResultMessage;
        resultText.gameObject.SetActive(isDisplay);
    }

	/// <summary>
	/// ���͑҂��̕\���؂�ւ��p
	/// </summary>
	/// <param name="isDisplay">�\�����邩�ۂ�</param>
	public void ButtonSelectText(bool isDisplay)
    {
        waitText.gameObject.SetActive(isDisplay);
    }

    /// <summary>
    /// �Q�[���I�����Ƀ��b�Z�[�W��\������p
    /// </summary>
    /// <param name="playerWin">�v���C���[���_�ł̏��s</param>
    public void EndText(bool playerWin)
	{
        //displayExitMessage = playerWin ? "���߂łƂ��I\n���݂̂������I" : "����˂�c\n���݂̂܂��c";
        //      exitText.text = displayExitMessage;
        //      exitText.gameObject.SetActive(true);

        //displayResultMessage = playerWin ? "���߂łƂ��I\n���݂̂������I" : "����˂�c\n���݂̂܂��c";
        // �F�ݒ��Ƀf�o�b�O���O��ǉ�
        if (playerWin)
        {
            displayResultMessage = "���߂łƂ��I\n���݂̂������I";
            resultText.color = winnerColor;
            Debug.Log("Winner Color: " + winnerColor); // �����ŐF���m�F
        }
        else
        {
            displayResultMessage = "����˂�c\n���݂̂܂��c";
            resultText.color = loserColor;
            Debug.Log("Loser Color: " + loserColor); // �����ŐF���m�F
        }


        resultText.text = displayResultMessage;
        resultText.gameObject.SetActive(true);
    }

}
