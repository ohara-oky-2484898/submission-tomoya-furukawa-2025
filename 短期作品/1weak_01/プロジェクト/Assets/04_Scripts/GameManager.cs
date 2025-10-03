using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // staticで宣言すると1つしか存在しない
    public static GameManager Instance = null;

    // 難易度これに比例して生成個数が決まる
    public int gameLevel = 1;
    // 現在の目標達成個数
    public int clearNum= 0;
    // クリアボーダー(最大個数)
    public int clearborder = 0;

    // それぞれの達成個数
    public int numRed = 0;
    public int numBlue = 0;
    public int numYellow = 0;
    public int numGreen = 0;

    // それぞれの生成個数の保管用
    public int numRedMax = 0;
    public int numBlueMax = 0;
    public int numYellowMax = 0;
    public int numGreenMax = 0;

    SwitchScene switchScene;


    private bool IsClear = false;
    void Awake()
    {
        if (Instance == null)
        {
            // Instanceが存在しなければ自信を登録する
            // 一つしか存在しないからnullなら
            Instance = this;
            // 破棄されないようにする
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            // Instanceが存在する場合は重複しないように削除する
            Debug.Log("重複したので削除！"); //確認用
            Destroy(this.gameObject);
        }

        switchScene = GetComponent<SwitchScene>();
    }
    
    void Update()
    {
        if (IsClear) return;

        if (clearborder == clearNum)
		{
            switchScene.ClearSwitchScene();
            Debug.Log($"GameClear:クリア目標{clearborder}クリアした数{clearNum}");
            IsClear = true;
        }

    }
}
