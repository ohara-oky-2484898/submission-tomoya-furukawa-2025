using UnityEngine;
using UnityEngine.UI;

using TMPro;
using ReversiConstants;

/// <summary>
/// �ǖʂɊւ���UI�̖�����S��
/// </summary>
public class DisplayReversiBoard : MonoBehaviour
{
    /// <summary> ��� </summary>
    [SerializeField] private Image _panel;
    /// <summary> �ǖ� </summary>
    [SerializeField] private TextMeshProUGUI diskText;
    
    public Image Panel => _panel;


    /// <summary>
    /// �ǖʂ̕\��
    /// </summary>
    public void DisplayBoard(BitBoard board, bool isPlayerTurn = false)
	{
        string currentBoard = "";


        for (int row = 0; row < GameConstants.BoardRows; row++)
        {
            string line = "";
            for (int col = 0; col < GameConstants.BoardColumns; col++)
            {
                int pos = row * GameConstants.BoardColumns + col;

                if (((board.Black >> pos) & 1) != 0)
                {
                    line += "��"; // ��
                }
                else if (((board.White >> pos) & 1) != 0)
                {
                    line += "�Z"; // ��
                }
                else
                {
                    line += "��"; // ��
                }
            }
            currentBoard += line + '\n';
        }

        diskText.text = currentBoard;
    }

    public void SetPlayerMousePotision(Vector2Int position)
	{

	}

    public void ShowValidMoves(BitBoard bitBoard)
    { 
    }
}
