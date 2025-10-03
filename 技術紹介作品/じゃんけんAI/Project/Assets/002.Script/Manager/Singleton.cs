/// <summary>
/// シングルトン用
/// </summary>

using UnityEngine;


// Singleton
// なぜ名前がこれなの？
// MonoBehaviourSingletonという名前が多いらしいが
// MonoBehaviourを使わないなら
// static変数ですればいいよねという意図

// GameMnagerの場合
// T がGameMnagerに置き換わると考える
public class Singleton<T> : MonoBehaviour where T : Singleton<T>
	// where T : Singleton<T>
	// これでシングルトンクラスを継承しているものに限定
	// これをしないと全然関係ないものを指定できてしまう
	// すると "Instance" の値がおかしくなってしまう
{
	// 継承先でtrueにすればゲームオブジェクトごと消せる
	protected virtual bool DestroyTargetGameObject => false;

	public static T Instance { get; private set; } = null;

	/// <summary>
	/// Singletonが有効か
	/// null がどうかチェックしたいときが多いと思われる
	/// </summary>
	public static bool IsValid() => Instance != null;

	private void Awake()
	{
		if (Instance == null)
		{
			// GameMnagerのときはGameMnagerだと型がわかっていたが
			// 今回は何の型かわからないためキャストしてあげる必要がある
			Instance = this as T;
			Instance.Init();
			return;
		}

		// 本来どちらの処理でもいいが
		// 今回はAwake()の中に入れてしまっているので
		// 継承先で選べるように
		// bool変数を用意しておく
		if (DestroyTargetGameObject)
		{
			// 別にこの辺の処理は
			// 継承先でInit()の中などで実行してもいいかも
			Destroy(gameObject);
		}
		else
		{
			Destroy(this);
		}
	}

	/// <summary>
	/// 派生クラス用のAwake
	/// ここのAwake()はもう使ってしまったため
	/// 継承先で初期化処理をしたいときように
	/// バーチャルメソッドを用意
	/// </summary>
	protected virtual void Init()
	{
	}

	private void OnDestroy()
	{
		if (Instance == this)
		{
			Instance = null;
		}
		OnRelease();
	}

	/// <summary>
	/// 派生クラス用のOnDestroy
	/// </summary>
	protected virtual void OnRelease()
	{
	}
}

//public static class GameParameters
//{
//	public static int Score { get; private set; }

//	public static void AddScore(int value)
//	{
//		Score += value;
//	}

//	public static void ResetScore()
//	{
//		Score = 0;
//	}
//}