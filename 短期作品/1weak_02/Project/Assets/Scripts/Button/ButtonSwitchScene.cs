using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonSwitchScene : MonoBehaviour
{
    public string sceneName = "";


    // �{�^�����������Ƃ��̏���
    public void OnSwichScene()
    {

        Debug.Log("�؂�ւ�");

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
