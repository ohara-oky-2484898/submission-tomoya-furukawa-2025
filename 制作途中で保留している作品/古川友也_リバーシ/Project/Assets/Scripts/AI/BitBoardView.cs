using UnityEngine;
using UnityEngine.UI;

public class BitBoardView : MonoBehaviour
{
    [SerializeField] private RectTransform root;
    [SerializeField] private GameObject cellPrefab;
    [SerializeField] private Sprite blackSprite;
    [SerializeField] private Sprite whiteSprite;
    [SerializeField] private Sprite legalMoveSprite;

    private Image[] cells;

    public void InitializeBoard()
    {
        ClearBoard();
        cells = new Image[64];

        for (int i = 0; i < 64; i++)
        {
            var obj = Instantiate(cellPrefab, root);
            var image = obj.GetComponent<Image>();
            cells[i] = image;
        }
    }

    public void ClearBoard()
    {
        if (cells != null)
        {
            foreach (var cell in cells)
            {
                if (cell != null)
                    Destroy(cell.gameObject);
            }
        }
        cells = null;
    }

    public void UpdateBoard(BitBoard board)
    {
        for (int i = 0; i < 64; i++)
        {
            var pos = 1UL << i;

            if ((board.Black & pos) != 0)
            {
                cells[i].sprite = blackSprite;
                cells[i].color = Color.white;
            }
            else if ((board.White & pos) != 0)
            {
                cells[i].sprite = whiteSprite;
                cells[i].color = Color.white;
            }
            else
            {
                cells[i].sprite = null;
                cells[i].color = Color.clear;
            }
        }
    }

    public void ShowLegalMoves(BitBoard legalMoves)
    {
        for (int i = 0; i < 64; i++)
        {
            var pos = 1UL << i;
            if ((legalMoves.Black & pos) != 0)
            {
                cells[i].sprite = legalMoveSprite;
                cells[i].color = Color.white;
            }
        }
    }
}
