using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Select : MonoBehaviour
{
	[SerializeField]Button fast;
	[SerializeField]Button second;
	[SerializeField]Button third;
	[SerializeField] Button title;
	public string titleName = "";
	public string gameName = "";

	private void Start()
	{
		// ボタンにリスナーを追加
		fast.onClick.AddListener(Fast);
		second.onClick.AddListener(Second);
		third.onClick.AddListener(Third);

		title.onClick.AddListener(OnSwichScene);
	}

	void Fast()
	{
		GameManager.Instance.gamelevel = GameLevel.Level_Fast;
		SwichScene();
	}
	void Second()
	{
		GameManager.Instance.gamelevel = GameLevel.Level_Second;
		SwichScene();
	}
	void Third()
	{
		GameManager.Instance.gamelevel = GameLevel.Level_Third;
		SwichScene();
	}

	public void OnSwichScene()
	{

		Debug.Log("切り替え");

		if (titleName != "")
		{
			SceneManager.LoadScene(titleName);
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

	public void SwichScene()
	{

		if (gameName != "")
		{
			SceneManager.LoadScene(gameName);
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
