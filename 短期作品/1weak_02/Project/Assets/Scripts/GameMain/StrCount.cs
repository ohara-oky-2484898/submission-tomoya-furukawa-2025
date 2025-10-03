using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StrCount : MonoBehaviour
{
    public Text countdownText; // UI Text�R���|�[�l���g���w��
    public float countdownTime = 3f; // �J�E���g�_�E���̎���

    public bool strFlag = false;

    void Start()
    {
        StartCoroutine(StartCountdown());
    }
    private IEnumerator StartCountdown()
    {
        float currentTime = countdownTime;

        while (currentTime > 0)
        {
            countdownText.text = currentTime.ToString("F0"); // �����Ƃ��ĕ\��
            countdownText.color = Color.white; // �J�E���g�_�E�����͔��F
            yield return new WaitForSeconds(1f); // 1�b�҂�
            currentTime--; // �J�E���g�_�E��
        }

        countdownText.text = "�X�^�[�g�I"; // �J�E���g�_�E����̃��b�Z�[�W
        countdownText.color = Color.green; // �I�����͗ΐF

        yield return new WaitForSeconds(1f); // 1�b�҂��Ă����\���ɂ���
        countdownText.gameObject.SetActive(false); // �e�L�X�g���\���ɂ���

        strFlag = true;
    }
}
