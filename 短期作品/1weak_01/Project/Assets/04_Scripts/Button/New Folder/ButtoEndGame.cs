using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;


//public enum titleState
//{
//	game,
//    quit
//}
public class ButtoEndGame : MonoBehaviour
{
    //public bool selectFlag;
   // titleState state;
    //public GameObject gameButton;
    public GameObject quitPanel;
    public GameObject quitButton;
    public Button yesButton;
    public Button noButton;
    //public Button strButton;
    GameObject NowButton;

    private void Start()
    {
        quitPanel.SetActive(false);

        // �{�^���Ƀ��X�i�[��ǉ�
        yesButton.onClick.AddListener(QuitGameYes);
        noButton.onClick.AddListener(QuitGameNo);
        //strButton.onClick.AddListener(OnSwichScene);
        
       // state = titleState.game;
       /// NowButton = gameButton;
    }

	//private void Update()
	//{
 //       switch (state)
 //       {
 //           case titleState.game:
 //               NowButton = gameButton;
 //               break;

 //           case titleState.quit:
 //               NowButton = quitButton;
 //               break;
 //       }
        
	//}

	public string sceneName = "";

    //public void OnSwichSelect(InputAction.CallbackContext context)
    //{
    //    if (context.started)// ������Ă�
    //    {
    //        state = state + 1 % 2;
    //    }
    //}

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

    // �N���b�N���ꂽ�Ƃ��ɌĂяo����郁�\�b�h
    public void OnQuitButtonClick()
    {
        quitPanel.SetActive(true);
        quitButton.SetActive(false);
    }

    // Yes�{�^�����N���b�N���ꂽ�Ƃ��ɌĂяo����郁�\�b�h
    public void QuitGameYes()
    {
        //Application.Quit();
        //�Q�[���I��
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;//�Q�[���v���C�I��
#else
        Application.Quit();//�Q�[���v���C�I��
#endif
        
    }

    // No�{�^�����N���b�N���ꂽ�Ƃ��ɌĂяo����郁�\�b�h
    public void QuitGameNo()
    {
        quitPanel.SetActive(false);
        quitButton.SetActive(true);
    }
}
