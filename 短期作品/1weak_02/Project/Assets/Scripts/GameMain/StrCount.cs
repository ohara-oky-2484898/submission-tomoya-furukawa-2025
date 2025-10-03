using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StrCount : MonoBehaviour
{
    public Text countdownText; // UI Textコンポーネントを指定
    public float countdownTime = 3f; // カウントダウンの時間

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
            countdownText.text = currentTime.ToString("F0"); // 整数として表示
            countdownText.color = Color.white; // カウントダウン中は白色
            yield return new WaitForSeconds(1f); // 1秒待つ
            currentTime--; // カウントダウン
        }

        countdownText.text = "スタート！"; // カウントダウン後のメッセージ
        countdownText.color = Color.green; // 終了時は緑色

        yield return new WaitForSeconds(1f); // 1秒待ってから非表示にする
        countdownText.gameObject.SetActive(false); // テキストを非表示にする

        strFlag = true;
    }
}
