using UnityEngine;
using UnityEngine.UI;

public class GridDrawer : MonoBehaviour
{
    [SerializeField]private Image linePrefab;

    [Header("�܂Ƃ߂�悤�̐e�I�u�W�F�N�g")]
    [SerializeField]private RectTransform pearentObj;

    [Header("�O���b�h�̐ݒ�")]
    [SerializeField]private RectTransform gridArea; // GridLines�I�u�W�F�N�g�������Ɏw��
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
            // ����
            CreateLine(
                new Vector2(gridArea.rect.width / 2, i * cellSize),
                new Vector2(totalSize, lineWidth),
                $"Horizon_Line{i+1}"
            );

            // �c��
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