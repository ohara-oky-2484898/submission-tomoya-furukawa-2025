using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class ButtoControl : MonoBehaviour
{
    public Button strButton;

    public GameObject quitPanel;
    public GameObject quitButton;
    public Button yesButton;
    public Button noButton;

    private UIControls controls;

    private void Awake()
    {
        controls = new UIControls();

        ResetTitleScene();
    }

    private void OnEnable()
    {
        controls.UI.Enable();
        controls.UI.Quit.performed += OnQuitPerformed;
    }

    private void OnDisable()
    {
        controls.UI.Quit.performed -= OnQuitPerformed;
        controls.UI.Disable();
    }

    private void Start()
    {
        quitPanel.SetActive(false);
        yesButton.onClick.AddListener(QuitGameYes);
        noButton.onClick.AddListener(QuitGameNo);
    }

    private void OnQuitPerformed(InputAction.CallbackContext context)
    {
        if (!quitPanel.activeSelf)
        {
            quitPanel.SetActive(true);
            quitButton.SetActive(false);

            // ↓Yesボタンを選択状態にする
            EventSystem.current.SetSelectedGameObject(yesButton.gameObject);
        }
        else
        {
            quitPanel.SetActive(false);
            quitButton.SetActive(true);

            // ↓Quitボタンに戻す
            EventSystem.current.SetSelectedGameObject(quitButton);
        }
    }

    public void QuitGameYes()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void QuitGameNo()
    {
        quitPanel.SetActive(false);
        quitButton.SetActive(true);

        // ↓Quitボタンを再び選択
        EventSystem.current.SetSelectedGameObject(quitButton);
    }

    public void ResetTitleScene()
    {
        // スタートボタンを選択状態にする
        EventSystem.current.SetSelectedGameObject(strButton.gameObject);
    }
}
