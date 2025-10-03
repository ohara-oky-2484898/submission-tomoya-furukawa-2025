using UnityEngine;
using UnityEngine.UI;

using TMPro;
using ReversiConstants;

/// <summary>
/// ‹Ç–Ê‚ÉŠÖ‚·‚éUI‚Ì–ğŠ„‚ğ’S‚¤
/// </summary>
public class DisplayReversiBoard : MonoBehaviour
{
    /// <summary> Šî”Õ </summary>
    [SerializeField] private Image _panel;
    /// <summary> ‹Ç–Ê </summary>
    [SerializeField] private TextMeshProUGUI diskText;
    
    public Image Panel => _panel;


    /// <summary>
    /// ‹Ç–Ê‚Ì•\¦
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
                    line += "œ"; // •
                }
                else if (((board.White >> pos) & 1) != 0)
                {
                    line += "Z"; // ”’
                }
                else
                {
                    line += " "; // ‹ó‚«
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
