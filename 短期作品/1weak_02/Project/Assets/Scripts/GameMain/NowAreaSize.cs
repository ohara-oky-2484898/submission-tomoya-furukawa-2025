using UnityEngine;
using UnityEngine.UI;

public class NowAreaSize : MonoBehaviour
{
    [SerializeField] Text scoreText;
    [SerializeField] Text scoreText2;

    [SerializeField] PlayerController player;
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.nowAreaSize = 0;
    }

    // Update is called once per frame
    void Update()
    {
        // �����_���.ToString("F1")
        scoreText.text = $"���݂̖ʐρw {GameManager.Instance.nowAreaSize.ToString("F0")}�^{GameManager.Instance.maxAreaSize.ToString("F0")}�x";
        scoreText2.text = $" �c�y {player.vertical.ToString("F1")}�z �� ���y {player.width.ToString("F1")}�z";
    }
}
