using UnityEngine;

public partial class PlayerController
{
	/// <summary>
	/// 壁キック
	/// </summary>
	private class WallJumping : StateBase<PlayerController>
	{
		private float _jumpPower = 10f;
		private float _moveSpeed = 5f;
        private readonly float _wallContactIgnoreDuration = 0.2f;
        private float _wallContactIgnoreTimer = 0f;

		public WallJumping(PlayerController machine) : base(machine) { }

		public override void OnEnterState()
		{
			_machine._animator.SetBool("IsWallJump", true);
			// 壁の法線＋上方向にジャンプ力を与える
			Vector3 jumpDir = (_machine._currentWallNormal.Value + Vector3.up).normalized;
			_machine._rigidbody.linearVelocity = Vector3.zero; // 一旦速度リセット
			_machine._rigidbody.AddForce(jumpDir * _jumpPower, ForceMode.Impulse);

			//Debug.DrawRay(_machine.transform.position, jumpDir * _jumpPower, Color.red, 10.0f);

			_wallContactIgnoreTimer = _wallContactIgnoreDuration;


            _machine._isGrounded = false;
			_machine._currentWallNormal = null;
			_machine._jumpPressed = false;

			_machine.ShowNowState($"壁ジャンプ中");
		}


		public override void OnUpdate()
		{
			// 無効時間をカウントダウン
			if (_wallContactIgnoreTimer > 0)
			{
				_wallContactIgnoreTimer -= Time.deltaTime;
			}
		}
		public override void OnFixedUpdate()
		{
			// 壁判定を無視する時間なら触れていないことにする
			// 壁ジャン直後に触れている判定に座れてしまうため
			if (_wallContactIgnoreTimer > 0f)
			{
				return;
			}
			var dir = _machine.GetCameraRelativeDirection();
			_machine.HandleMovement(dir, _moveSpeed);
			_machine.HandleRotation(dir);

			_machine.CheckNearestWall();

			//Vector3 inputDir = new Vector3(_machine._moveInput.x, 0, _machine._moveInput.y).normalized;

			//// 壁の法線と入力方向の内積を計算
			//float dot = Vector3.Dot(_machine._currentWallNormal, inputDir);

			//// 内積が0.5以上（壁の裏側方向に入力している）なら壁ジャンプOK
			//if (_machine._isTouchingWall && dot > 0.5f && _machine._jumpPressed)
			//{
			//	_machine.ChangeState(new WallJumping(_machine));
			//	_machine._jumpPressed = false;

			//}



			if (_machine.IsTouchingWall)
			{
				_machine.ChangeState(PlayerState.TouchingWall);
				return;
			}

			if(_machine.IsFalling)
			{
				_machine.ChangeState(PlayerState.Falling);
			}

			//if (_machine._isGrounded)
			//{
			//	_machine.ChangeState(PlayerState.Moving);
			//}
			_machine.UpdateAnimatorParams();
		}

		public override void OnExitState()
		{
			_machine._animator.SetBool("IsWallJump", false);
		}
	}
}
