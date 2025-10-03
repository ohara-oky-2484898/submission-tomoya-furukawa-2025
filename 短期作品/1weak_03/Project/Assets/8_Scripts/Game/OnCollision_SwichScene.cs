using UnityEngine;
using UnityEngine.SceneManagement;

public class OnCollision_SwichScene : MonoBehaviour
{
    public string tagName;   // 目標タグ
    public string sceneName; // シーン名

    private void OnTriggerEnter(Collider other)
    {
        // 衝突したものが目標タグだったら
        if (other.gameObject.tag == tagName)
        {
            if (sceneName != "")
            {
                // シーン名があれば切り替える
                SceneManager.LoadScene(sceneName);
            }
            else
            {
                // シーン名がなければ次のシーンへ切り替える
                int nextIndex = SceneManager.GetActiveScene().buildIndex + 1;
                if (nextIndex < SceneManager.sceneCountInBuildSettings)
                {
                    SceneManager.LoadScene(nextIndex);
                }
                else
                {
                    // 次のシーンがなければ最初のシーンへ
                    SceneManager.LoadScene(0);
                }
            }
        }
    }
}
