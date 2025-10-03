using UnityEngine;
using UnityEngine.SceneManagement;
public enum GameLevel
{
    Level_Fast = 1,
    Level_Second,
    Level_Third
}


public class GameManager : MonoBehaviour
{
    // staticで宣言すると1つしか存在しない
    public static GameManager Instance = null;

    // 難易度
    public GameLevel gamelevel;// = GameLevel.Level_Fast;

    public float nowAreaSize;
    public float maxAreaSize;

    // スコア
    public int score = 0;
    public int highScore = 0;

    // 制限時間
    float timeLimit = 60.0f;
    public float nowtime = 0.0f;

    public bool inGame = false;

    [SerializeField] private string ResultSceneName = "ResultScene";

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
    }

    
    void Start()
    {
        // 制限時間初期化
        nowtime = timeLimit;
    }

    
    void Update()
    {
        if (inGame)
        {
            nowtime -= Time.deltaTime;
            // タイムアップ
            if (nowtime <= 0.0f)
            {
                inGame = false;
                LoadResultScene();
            }
        }
    }

    private void LoadResultScene()
    {
        // Resultシーンをロード
        SceneManager.LoadScene(ResultSceneName);
    }
}
