using UnityEngine;
using System.Collections.Generic;

public class StateMachineBase<TMachine, TState> : MonoBehaviour 
	where TMachine : StateMachineBase<TMachine, TState> // �K�����g���p�����Ă���
	where TState : System.Enum // �񋓎q�̏�Ԃ��K������, enum��int�L���X�g����̂��Ȃ���
{
	private StateBase<TMachine> _currentState;
	private StateBase<TMachine> _nextState;

	// �e�X�e�[�g�Ǘ��p
	private Dictionary<TState, StateBase<TMachine>> _stateMap = new();

	///// <summary> ���݂̃X�e�[�g�m�F�p </summary>
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
	/// ��ԑJ�ڂ̏���������֐�
	/// </summary>
	private void ProcessStateTransition()
	{
		if (_nextState != null)
		{
			// ��ԍŏ��̃��[�v�݂̂͐ݒ肳��Ă��邩�킩��Ȃ����߃`�F�b�N
			_currentState?.OnExitState();
			_currentState = _nextState;
			_currentState.OnEnterState();
			_nextState = null;
		}
	}

	/// <summary>
	/// ��Ԃ̓o�^
	/// </summary>
	protected void RegisterState(TState stateKey, StateBase<TMachine> state)
	{
		if (!_stateMap.ContainsKey(stateKey))
		{
			_stateMap.Add(stateKey, state);
		}
	}


	/// <summary>
	/// ��ԑJ�ڗ\�� (�o�^�ς݂̏�Ԃ��g��)
	/// </summary>
	/// <param name="stateKey">�J�ڂ������X�e�[�g</param>
	/// <returns>����������</returns>
	public bool ChangeState(TState stateKey)
	{
		// ���łɗ\��ς�
		if (_nextState != null) return false;
		// �}�b�v��key�ɓ��Ă͂܂�X�e�[�g�̃C���X�^���X�����݂��Ȃ�
		if (!_stateMap.TryGetValue(stateKey, out var nextState)) return false;

		_nextState = nextState;
		return true;
	}
}
