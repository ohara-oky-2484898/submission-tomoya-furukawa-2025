/// <summary>
/// ���U���g��ʂ̉҂��������\���p
/// </summary>
using UnityEngine;
using UnityEngine.UI;

public class ResultScore : MonoBehaviour
{
    [SerializeField] Text scoreText;
    // Start is called before the first frame update
    void Start()
    {
        scoreText.text = $"�҂���������:{GameManager.Instance.score}";
    }
}
