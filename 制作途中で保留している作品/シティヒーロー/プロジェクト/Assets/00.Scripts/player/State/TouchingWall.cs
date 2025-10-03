using UnityEngine;

public partial class PlayerController
{

	/// <summary>
	/// 壁に触れて静止している状態
	/// </summary>
	private class TouchingWall : StateBase<PlayerController>
	{
		public TouchingWall(PlayerController machine) : base(machine) { }

		/// <summary> WallJumpingStateに入る条件用： 壁につかまってる状態で、壁とは逆の方向に倒す + ジャンプボタン</summary>
		private bool IsWallJumping
		{
			get 
			{
				// 壁につかまっていない
				if (!_machine._currentWallNormal.HasValue)
				{
					_machine.ShowNowState("壁に触れてない！");
					return false;
				}

				Vector3 camForward = _machine._camera.transform.forward;
				Vector3 camRight = _machine._camera.transform.right;

				camForward.y = 0;
				camRight.y = 0;
				camForward.Normalize();
				camRight.Normalize();

				Vector3 inputDir = (camForward * _machine._moveInput.y + camRight * _machine._moveInput.x).normalized;
				float dot = Vector3.Dot(_machine._currentWallNormal.Value, inputDir);

				if (_machine._jumpPressed)
				{
                    _machine.ShowNowState($"壁キック{(dot > 0.5f ? "発動！" : "不発!")}");
                }
                else
                {
                    _machine.ShowNowState($"壁に貼りついてる");
                }
				Debug.Log($"詳細（{(_machine._jumpPressed && dot > 0.5f ? "成功" : "失敗")}）：dot :{dot}, jumpbutton{_machine._jumpPressed}");
                // 壁ジャンプ条件が満たされればジャンプ
                return _machine._jumpPressed && dot > 0.5f;
			}
		}

		public override void OnEnterState()
		{
			_machine._animator.SetBool("IsTouchingWall", true);


			//_machine._rigidbody.angularVelocity = Vector3.zero;
			//_machine._rigidbody.linearVelocity = Vector3.zero;
			//_machine._rigidbody.useGravity = false;
			_machine.ShowNowState($"壁つかまり中");
		}

		public override void OnFixedUpdate()
		{
			#region 一旦残しておく
			//_machine.WallTouchRotation(-_machine._currentWallNormal);
			//_machine.CheckNearestWall();

			//Vector3 origin = _machine.transform.position + Vector3.up * 1.0f;
			//Vector3 wallNormal = _machine._currentWallNormal.normalized;

			//// 基準：壁の法線方向（= wallNormal）
			////Debug.DrawRay(origin, wallNormal * 1.5f, Color.red);

			//// 壁ジャンプ可能な入力範囲（wallNormal ±60度）
			////int rayCount = 12;
			////float angleRange = 120f; // ±60度 = 合計120度
			////// -6から6まで
			////for (int i = -rayCount / 2; i <= rayCount / 2; i++)
			////{
			////	float angle = i * (angleRange / rayCount);
			////	Vector3 dir = Quaternion.AngleAxis(angle, Vector3.up) * wallNormal;
			////	//Debug.DrawRay(origin, dir * 1.2f, Color.green);
			////}



			//// ?? カメラ基準の入力方向
			//Vector3 camForward = _machine._camera.transform.forward;
			//Vector3 camRight = _machine._camera.transform.right;

			//camForward.y = 0;
			//camRight.y = 0;
			//camForward.Normalize();
			//camRight.Normalize();

			//Vector3 inputDir = (camForward * _machine._moveInput.y + camRight * _machine._moveInput.x).normalized;

			////// ?? 現在のスティック入力方向を表示
			////if (inputDir.sqrMagnitude > 0.01f)
			////{
			////	Debug.DrawRay(origin, inputDir * 1.2f, Color.blue);
			////}


			//float dot = Vector3.Dot(_machine._currentWallNormal, inputDir);

			//// 壁ジャンプ条件が満たされればジャンプ
			//if (_machine._jumpPressed && dot > 0.5f)
			#endregion

			_machine.CheckNearestWall();

			if (IsWallJumping)
			{
				_machine._jumpPressed = false;
				_machine.ChangeState(PlayerState.WallJumping);
				return;
			}

			// 地面に着いたら通常移動へ
			if (_machine._isGrounded)
			{
				_machine.ChangeState(PlayerState.Moving);
				return;
			}

			// 一度壁は利付きから、手を離した時に
			// まだ,IsFallingを満たしてなくても落下状態になってほしいため
			if(!_machine._currentWallNormal.HasValue)
			{
				_machine.ChangeState(PlayerState.Falling);
				return;
			}
			//Debug.Log($"{(_machine._isTouchingWall ? "壁についてる" : "壁から離れた")}");
			//// 壁から離れたらジャンプ状態などへ
			//if (!_machine._isTouchingWall)
			//{
			//	_machine.ChangeState(new Jumping(_machine));
			//}

			_machine.UpdateAnimatorParams();
		}

		public override void OnExitState()
		{
			_machine._animator.SetBool("IsTouchingWall", false);
			//_machine._rigidbody.useGravity = true;
		}
	}
}
