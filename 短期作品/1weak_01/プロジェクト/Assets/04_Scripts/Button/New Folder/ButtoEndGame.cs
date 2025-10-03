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

        // ボタンにリスナーを追加
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
    //    if (context.started)// 押されてる
    //    {
    //        state = state + 1 % 2;
    //    }
    //}

    // ボタンを押したときの処理
    public void OnSwichScene()
    {

        Debug.Log("切り替え");

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

    // クリックされたときに呼び出されるメソッド
    public void OnQuitButtonClick()
    {
        quitPanel.SetActive(true);
        quitButton.SetActive(false);
    }

    // Yesボタンがクリックされたときに呼び出されるメソッド
    public void QuitGameYes()
    {
        //Application.Quit();
        //ゲーム終了
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;//ゲームプレイ終了
#else
        Application.Quit();//ゲームプレイ終了
#endif
        
    }

    // Noボタンがクリックされたときに呼び出されるメソッド
    public void QuitGameNo()
    {
        quitPanel.SetActive(false);
        quitButton.SetActive(true);
    }
}
