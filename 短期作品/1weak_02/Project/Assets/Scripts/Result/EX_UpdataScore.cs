/// <summary>
/// �Q�[�����̉҂��������\���p
/// </summary>
using UnityEngine;
using UnityEngine.UI;


public class EX_UpdataScore : MonoBehaviour
{
    [SerializeField] Text scoreText;
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.score = 0;
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = $"���݂̉҂���������:{GameManager.Instance.score}";
    }
}
