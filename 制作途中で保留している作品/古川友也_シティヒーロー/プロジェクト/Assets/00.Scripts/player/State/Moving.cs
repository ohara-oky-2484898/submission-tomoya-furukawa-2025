using UnityEngine;

public partial class PlayerController
{
	/// <summary>
	/// ˆÚ“®’†
	/// </summary>
	private class Moving : StateBase<PlayerController>
	{
		private float _moveSpeed = 5f;

		public Moving(PlayerController machine) : base(machine) { }
		public override void OnEnterState()
		{
			_machine.ShowNowState($"ˆÚ“®’†");
		}
		public override void OnFixedUpdate()
		{
			//_machine.HandleMovementAndRotation(_moveSpeed);	
			var dir = _machine.GetCameraRelativeDirection();
			_machine.HandleMovement(dir, _moveSpeed);
			_machine.HandleRotation(dir);

			_machine._animator.SetFloat("MoveSpeed", _machine._moveInput.magnitude);

			if (!_machine.IsMoving)
			{
				_machine.ChangeState(PlayerState.Idling);
			}
			if (_machine.IsJumping)
			{
				_machine.ChangeState(PlayerState.Jumping);
			}

			_machine.UpdateAnimatorParams();

			_machine.DrawReticle();
		}
	}
}
