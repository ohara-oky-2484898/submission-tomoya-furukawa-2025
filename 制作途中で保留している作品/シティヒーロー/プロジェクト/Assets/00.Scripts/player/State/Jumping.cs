using UnityEngine;

public partial class PlayerController
{
	/// <summary>
	/// ジャンプ状態
	/// </summary>
	private class Jumping : StateBase<PlayerController>
	{
		// 移動速度／ジャンプ力のパラメータ
		private float _moveSpeed = 5f;
		private float _minJumpPower = 6f;
		private float _maxJumpPower = 10f;

		// ジャンプボタン長押し、単押し判定用
		private float _maxJumpHoldTime = 0.14f;
		private float _holdTime = 0f;
		private bool _jumpReleased = false;
		private float _jumpStartTime;

		public Jumping(PlayerController mashine) : base(mashine) { }

		public override void OnEnterState()
		{
			_jumpReleased = false;
			_jumpStartTime = Time.time;
			_machine.UpdateAnimatorParams();
			_machine.ShowNowState("ジャンプ中");
		}

		public override void OnUpdate()
		{
			// アニメーター更新
			_machine.UpdateAnimatorParams();

			// 入力長押し時間の測定
			_holdTime = Time.time - _jumpStartTime;

			// ジャンプ押しっぱなしで最大時間超えたら強制的にジャンプ
			if (_machine._jumpPressed && _holdTime > _maxJumpHoldTime)
			{
				_machine._jumpPressed = false;
				_jumpReleased = true;
			}
			// 入力途中で放されたとき
			else if (!_machine._jumpPressed)
			{
				_jumpReleased = true;
			}
		}

		public override void OnFixedUpdate()
		{
			// 移動処理・移動処理
			var dir = _machine.GetCameraRelativeDirection();
			//_machine.HandleMovement(dir, _moveSpeed);

			// 名前付き引数を使うことで重力の設定はデフォルト引数で！
			// ジャンプしていないときにlinearVelocityを上書きされるのを防ぐ
			_machine.HandleMovement(dir, _moveSpeed, applyGravity: _machine._isJumping);
			_machine.HandleRotation(dir);

			// 壁接触判定
			_machine.CheckNearestWall();

			if (!_jumpReleased) return;

			// ジャンプ処理：入力が解放されていて、まだジャンプしていないなら
			if (!_machine._isJumping)
			{
				_machine._isJumping = true;
				_machine._isGrounded = false;

				// 小ジャンか大ジャンか分岐
				float jumpPower = (_holdTime < _maxJumpHoldTime) ? _minJumpPower : _maxJumpPower;
				//_holdTime = Mathf.Clamp(_holdTime, 0f, _maxJumpHoldTime);
				//float jumpPower = Mathf.Lerp(_minJumpPower, _maxJumpPower, _holdTime / _maxJumpHoldTime);

				//// y方向の速度をジャンプ力に更新
				_machine._rigidbody.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
			}
			// 落下開始判定
			if (_machine._rigidbody.linearVelocity.y < -0.1f)
			{
				_machine.ChangeState(PlayerState.Falling);
				return;
			}

			//Vector3 inputDir = new Vector3(_machine._moveInput.x, 0, _machine._moveInput.y).normalized;

			//// 壁の法線と入力方向の内積を計算
			//float dot = Vector3.Dot(_machine._currentWallNormal, inputDir);

			//// 壁の法線と入力方向がほぼ逆向きなら内積は負の値になる（-1に近い）
			//// 例えば -0.5 以下なら壁に向かって入力していると判断
			//if (dot < -0.5f)
			//{
			//	_machine._jumpPressed = false;
			//	_machine.ChangeState(new WallJumping(_machine));
			//}

			//Vector3 inputDir = new Vector3(_machine._moveInput.x, 0, _machine._moveInput.y).normalized;

			//// 壁の法線と入力方向の内積を計算
			//float dot = Vector3.Dot(_machine._currentWallNormal, inputDir);

			//// 内積が0.5以上（壁の裏側方向に入力している）なら壁ジャンプOK
			//if (_machine._isTouchingWall && dot > 0.5f && _machine._jumpPressed)
			//{
			//	//_machine._jumpPressed = false;
			//	_machine.ChangeState(new WallJumping(_machine));
			//}


			if (_machine.IsTouchingWall)
			{
				_machine.ChangeState(PlayerState.TouchingWall);
				return;
			}

			//if (_machine._isTouchingWall && !_machine._isGrounded && _machine._jumpPressed)
			//{
			//	// 壁の法線ベクトルと入力方向の角度を計算
			//	Vector3 inputDir = new Vector3(_machine._moveInput.x, 0, _machine._moveInput.y).normalized;
			//	float angle = Vector3.Angle(_machine._currentWallNormal, inputDir);

			//	// 壁の法線と入力方向が反対方向（角度が90度以上）なら壁ジャンプOK
			//	// 角度が90度未満なら、入力は法線ベクトルとほぼ同じ方向（壁の外向き）
			//	// 角度が90度以上なら、入力は法線ベクトルと反対方向（壁に向かっている）
			//	if (angle < 90f)
			//	{
			//		// 壁の外向きに入力 → 壁ジャンプしない
			//	}
			//	else
			//	{
			//		// 壁に向かって入力 → 壁ジャンプOK
			//		_machine._jumpPressed = false;
			//		_machine.ChangeState(new WallJumping(_machine));
			//	}
			//}

		}

		public override void OnExitState()
		{
			_machine._isJumping = false;
		}
	}
}
