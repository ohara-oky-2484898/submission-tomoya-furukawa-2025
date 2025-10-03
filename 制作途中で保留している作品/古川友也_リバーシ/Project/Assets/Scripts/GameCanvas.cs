using UnityEngine;
using TMPro;
using ReversiConstants;

public class GameCanvas : MonoBehaviour
{
    /// <summary> �Ȉ�UI�p </summary>
    [SerializeField] private TextMeshProUGUI playerCallText;
    [SerializeField] private TextMeshProUGUI currentDiskText;


    public void UpDateUI(bool isSkip, DiscColors currentColor, int turnCount, int blackNum, int whiteNum)
	{
        string skipAnnounce = isSkip ? "" : "�X�L�b�v\n";
        string nowTurnPlayerText = currentColor == DiscColors.black
            ? "- �^�[�� -\n '��'(��) "
            : "- �^�[�� -\n '��'(�Z) ";
        // �e�L�X�g�\���X�V
        playerCallText.text = skipAnnounce + nowTurnPlayerText;

        // �^�[���\���X�V
        string turn = $"{turnCount} ���\n\n";
        // �΂̐��\���X�V
        string discNum =
            $"����(�Z)\n�@�F{whiteNum}��" +
            $"����(�Z)\n�@�F{blackNum}��";
        currentDiskText.text = turn + discNum;
    }

    public void ShowGameEndText(bool isBlackWin)
	{
        playerCallText.text = isBlackWin
        ? "���̏����I"
        : "���̏����I";
    }
}
