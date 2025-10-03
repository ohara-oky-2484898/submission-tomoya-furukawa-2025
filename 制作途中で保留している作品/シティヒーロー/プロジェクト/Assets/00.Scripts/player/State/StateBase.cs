public abstract class StateBase<TMachine>
{
	/// <summary>
	/// インスタンス
	/// </summary>
	protected TMachine _machine;

	// コンストラクタ
	public StateBase(TMachine mashine) { _machine = mashine; }

	/// <summary>
	/// 入った瞬間、初期化処理など
	/// </summary>
	public virtual void OnEnterState() { }

	/// <summary>
	/// 入力処理・アニメーション制御・UI・タイマーなど
	/// Transformの変更
	/// </summary>
	public virtual void OnUpdate(){}

	/// <summary>
	/// 物理処理・Rigidbodyの操作
	/// Physicsなどの当たり判定
	/// </summary>
	public virtual void OnFixedUpdate() { }

	/// <summary>
	/// 出る瞬間、リセット処理など
	/// </summary>
	public virtual void OnExitState() { }
}
