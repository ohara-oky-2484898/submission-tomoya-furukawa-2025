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
        // è¨êîì_ëÊàÍ.ToString("F1")
        scoreText.text = $"åªç›ÇÃñ êœÅw {GameManager.Instance.nowAreaSize.ToString("F0")}Å^{GameManager.Instance.maxAreaSize.ToString("F0")}Åx";
        scoreText2.text = $" ècÅy {player.vertical.ToString("F1")}Åz Åñ â°Åy {player.width.ToString("F1")}Åz";
    }
}
