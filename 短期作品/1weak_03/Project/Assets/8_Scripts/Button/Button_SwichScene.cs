using UnityEngine;
using UnityEngine.SceneManagement;
public class Button_SwichScene : MonoBehaviour
{
    public string sceneName = "";

    // ƒ{ƒ^ƒ“‚ğ‰Ÿ‚µ‚½‚Æ‚«‚Ìˆ—
    public void SwichScene()
    {
        if (sceneName != "")
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            int nextIndex = SceneManager.GetActiveScene().buildIndex + 1;
            if (nextIndex < SceneManager.sceneCountInBuildSettings)
            {
                SceneManager.LoadScene(nextIndex);
            }
            else 
            {
                SceneManager.LoadScene(0);
            }
        }
    }
}
