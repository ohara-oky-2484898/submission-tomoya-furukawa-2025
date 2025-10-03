using UnityEngine;
using UnityEngine.UI;

public class GridDrawer : MonoBehaviour
{
    [SerializeField]private Image linePrefab;

    [Header("まとめるようの親オブジェクト")]
    [SerializeField]private RectTransform pearentObj;

    [Header("グリッドの設定")]
    [SerializeField]private RectTransform gridArea; // GridLinesオブジェクトをここに指定
    [SerializeField]private int howManyDivide = 8;
    [SerializeField]private float cellSize = 50;
    [SerializeField]private Color lineColor = Color.black;
    [SerializeField]private float lineWidth = 2f;
    void Start()
    {
        linePrefab.color = lineColor;
        DrawGridLines();

    }

    void DrawGridLines()
    {
        float totalSize = howManyDivide * cellSize;

        for (int i = 0; i <= howManyDivide; i++)
        {
            // 横線
            CreateLine(
                new Vector2(gridArea.rect.width / 2, i * cellSize),
                new Vector2(totalSize, lineWidth),
                $"Horizon_Line{i+1}"
            );

            // 縦線
            CreateLine(
                new Vector2(i * cellSize, gridArea.rect.height / 2),
                new Vector2(lineWidth, totalSize),
                $"Vertical_Line{i + 1}"
            );
        }
    }

    void CreateLine(Vector2 anchoredPos, Vector2 size, string name)
    {
        Image line = Instantiate(linePrefab, pearentObj);
        RectTransform rt = line.GetComponent<RectTransform>();
        rt.anchoredPosition = anchoredPos;
        rt.sizeDelta = size;
        rt.anchorMin = new Vector2(0, 0);
        rt.anchorMax = new Vector2(0, 0);
        line.gameObject.name = name;
    }
}