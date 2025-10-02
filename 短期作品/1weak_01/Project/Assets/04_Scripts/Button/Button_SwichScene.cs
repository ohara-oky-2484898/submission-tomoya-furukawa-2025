using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class Button_SwichScene : MonoBehaviour
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
