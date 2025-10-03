using UnityEngine;

public partial class PlayerController
{
	/// <summary>
	/// óéâ∫íÜ
	/// </summary>
	private class Falling : StateBase<PlayerController>
	{
		public Falling(PlayerController machine) : base(machine) { }

		private float fallSpeed = 5f;

		public override void OnEnterState()
		{
			_machine.ShowNowState("óéâ∫íÜ");
			_machine._animator.SetBool("IsFalling", true);
			_machine._rigidbody.useGravity = false;
			_machine._isGrounded = false;

			Vector3 velocity = _machine._rigidbody.linearVelocity;
			velocity.y = -0.1f;
			_machine._rigidbody.linearVelocity = velocity;
		}

		public override void OnFixedUpdate()
		{
			Vector3 moveDir = _machine.GetCameraRelativeDirection();

			// é©ëOÇÃèdóÕÇ≈óéâ∫
			_machine.HandleMovement(moveDir, fallSpeed, gravity: -9.81f);
			_machine.HandleRotation(moveDir);

			_machine.CheckNearestWall();

			if (_machine._isGrounded)
			{
				_machine.ChangeState(PlayerState.Idling);
			}
			if (_machine.IsTouchingWall)
			{
				_machine.ChangeState(PlayerState.TouchingWall);
			}

			_machine.UpdateAnimatorParams();
		}


		public override void OnExitState()
		{
			_machine._animator.SetBool("IsFalling", false);
			_machine._rigidbody.useGravity = true;
		}
	}
}