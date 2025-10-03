using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonSwitchScene : MonoBehaviour
{
    public string sceneName = "";


    // ƒ{ƒ^ƒ“‚ğ‰Ÿ‚µ‚½‚Æ‚«‚Ìˆ—
    public void OnSwichScene()
    {

        Debug.Log("Ø‚è‘Ö‚¦");

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
