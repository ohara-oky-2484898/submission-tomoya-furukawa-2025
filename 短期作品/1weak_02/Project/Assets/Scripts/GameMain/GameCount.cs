using UnityEngine;
using UnityEngine.UI;

public class GameCount : MonoBehaviour
{
    [SerializeField] Text countText;
    [SerializeField] StrCount str;
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.score = 0;
    }

    // Update is called once per frame
    void Update()
    {
        GameManager.Instance.inGame = str.strFlag;

        countText.text = $"Žc‚èŽžŠÔ:{GameManager.Instance.nowtime.ToString("F1")}•b";
    }
}
