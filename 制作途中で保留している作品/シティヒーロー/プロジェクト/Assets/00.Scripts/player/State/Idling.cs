using UnityEngine;

public partial class PlayerController
{
	/// <summary>
	/// óßÇøèÛë‘Å^ì¸óÕñ≥Çµ
	/// </summary>
	private class Idling : StateBase<PlayerController>
	{
		public Idling(PlayerController mashine) : base(mashine)	{}

		public override void OnEnterState()
		{
			_machine.ShowNowState($"ÉAÉCÉhÉã");
		}
		public override void OnUpdate()
		{
			_machine._animator.SetFloat("MoveSpeed", 0f);
			_machine._rigidbody.linearVelocity = Vector3.zero;


			if (_machine.IsMoving)
			{
				_machine.ChangeState(PlayerState.Moving);
			}
			if (_machine.IsJumping)
			{
				_machine.ChangeState(PlayerState.Jumping);
			}
			//if (_machine._swingPressed)
			//{

			//}

			_machine.CheckNearestWall();
			_machine.UpdateAnimatorParams();
		}
	}
}
