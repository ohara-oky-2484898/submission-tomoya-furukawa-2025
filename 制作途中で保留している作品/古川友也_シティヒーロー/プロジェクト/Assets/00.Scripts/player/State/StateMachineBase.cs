using UnityEngine;
using System.Collections.Generic;

public class StateMachineBase<TMachine, TState> : MonoBehaviour 
	where TMachine : StateMachineBase<TMachine, TState> // 必ず自身を継承している
	where TState : System.Enum // 列挙子の状態が必ずある, enumをintキャストするのを省ける
{
	private StateBase<TMachine> _currentState;
	private StateBase<TMachine> _nextState;

	// 各ステート管理用
	private Dictionary<TState, StateBase<TMachine>> _stateMap = new();

	///// <summary> 現在のステート確認用 </summary>
	//public StateBase<TMachine> CurrentState => _currentState;

	private void Update()
	{
		ProcessStateTransition();
		_currentState?.OnUpdate();
	}

	private void FixedUpdate()
	{
		ProcessStateTransition();
		_currentState?.OnFixedUpdate();
	}

	/// <summary>
	/// 状態遷移の処理をする関数
	/// </summary>
	private void ProcessStateTransition()
	{
		if (_nextState != null)
		{
			// 一番最初のループのみは設定されているかわからないためチェック
			_currentState?.OnExitState();
			_currentState = _nextState;
			_currentState.OnEnterState();
			_nextState = null;
		}
	}

	/// <summary>
	/// 状態の登録
	/// </summary>
	protected void RegisterState(TState stateKey, StateBase<TMachine> state)
	{
		if (!_stateMap.ContainsKey(stateKey))
		{
			_stateMap.Add(stateKey, state);
		}
	}


	/// <summary>
	/// 状態遷移予約 (登録済みの状態を使う)
	/// </summary>
	/// <param name="stateKey">遷移したいステート</param>
	/// <returns>成功したか</returns>
	public bool ChangeState(TState stateKey)
	{
		// すでに予約済み
		if (_nextState != null) return false;
		// マップにkeyに当てはまるステートのインスタンスが存在しない
		if (!_stateMap.TryGetValue(stateKey, out var nextState)) return false;

		_nextState = nextState;
		return true;
	}
}
