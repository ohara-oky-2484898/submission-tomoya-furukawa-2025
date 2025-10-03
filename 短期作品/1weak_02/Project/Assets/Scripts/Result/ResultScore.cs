/// <summary>
/// リザルト画面の稼いだお金表示用
/// </summary>
using UnityEngine;
using UnityEngine.UI;

public class ResultScore : MonoBehaviour
{
    [SerializeField] Text scoreText;
    // Start is called before the first frame update
    void Start()
    {
        scoreText.text = $"稼いだお金＄:{GameManager.Instance.score}";
    }
}
