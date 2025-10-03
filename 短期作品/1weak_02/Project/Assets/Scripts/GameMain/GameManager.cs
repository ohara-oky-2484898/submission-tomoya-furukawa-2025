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
    // static�Ő錾�����1�������݂��Ȃ�
    public static GameManager Instance = null;

    // ��Փx
    public GameLevel gamelevel;// = GameLevel.Level_Fast;

    public float nowAreaSize;
    public float maxAreaSize;

    // �X�R�A
    public int score = 0;
    public int highScore = 0;

    // ��������
    float timeLimit = 60.0f;
    public float nowtime = 0.0f;

    public bool inGame = false;

    [SerializeField] private string ResultSceneName = "ResultScene";

    void Awake()
    {
        if (Instance == null)
        {
            // Instance�����݂��Ȃ���Ύ��M��o�^����
            // ��������݂��Ȃ�����null�Ȃ�
            Instance = this;
            // �j������Ȃ��悤�ɂ���
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            // Instance�����݂���ꍇ�͏d�����Ȃ��悤�ɍ폜����
            Debug.Log("�d�������̂ō폜�I"); //�m�F�p
            Destroy(this.gameObject);
        }
    }

    
    void Start()
    {
        // �������ԏ�����
        nowtime = timeLimit;
    }

    
    void Update()
    {
        if (inGame)
        {
            nowtime -= Time.deltaTime;
            // �^�C���A�b�v
            if (nowtime <= 0.0f)
            {
                inGame = false;
                LoadResultScene();
            }
        }
    }

    private void LoadResultScene()
    {
        // Result�V�[�������[�h
        SceneManager.LoadScene(ResultSceneName);
    }
}
