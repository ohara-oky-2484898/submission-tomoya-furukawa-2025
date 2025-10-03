using System;
using UnityEngine;

public partial class PlayerController
{
	private class Swinging : StateBase<PlayerController>
	{
		/// <summary> アニメーションの状態取得用 </summary>
		private AnimatorStateInfo _stateInfo;
		/// <summary> 一回のみできるよう簡易処置用 </summary>
		private bool _once = false;
		/// <summary> 飛ぶ処理を実行するタイミング用(0~1進行度) </summary>
		private float _flyForceTriggerProgress = 0.17f;
		/// <summary> hookPointへの引っ張る強さ </summary>
		private float _pullForce  = 20f;

		/// <summary> 簡略化のため </summary>
		Vector3 _playerPos;
		Vector3 _hookPoint;

		/// <summary>
		/// スイングの状態
		/// </summary>
		private enum SwingState
		{
			shot,// ひっかけるためにショットしただけ
			swing,// 引っかかってスイング中
			end,// 降りるか、切れたとき
		}


		public Swinging(PlayerController machine) : base(machine) { }


		public override void OnEnterState()
		{
			_machine._rigidbody.useGravity = false;
			_machine._lineRenderer.enabled = true;
			_machine.ShowNowState($"ショット中");

			_machine._animator.SetBool("IsPressingWire", true);

			Vector3 lookDir = _machine.GetCameraRelativeDirection();
			_machine.HandleRotation(lookDir);
			_machine._isGrounded = false;
			//_machine.HandleMovement(dir, _moveSpeed, applyGravity: _machine._isJumping);
		}

		public override void OnFixedUpdate()
		{
			// ボタンが押されたままなら早期リターン
			if (_machine._swingPressed)
			{
				_machine.ShowNowState($"狙い中{_machine._swingPressed}");
				return;
			}

			if (_once)
			{
				// 落下開始判定
				if (_machine._rigidbody.linearVelocity.y < -0.1f)
				{
					_machine.ChangeState(PlayerState.Falling);
					return;
				}
				return;
			}

			// 離されたら処理を開始
			_machine._animator.SetBool("IsPressingWire", false);

			// 1. フック方向
			Vector3 dirToHook = (_hookPoint - _playerPos);
			float distanceToHook = dirToHook.magnitude;
			Vector3 dirToHookNormalized = dirToHook.normalized;

			// 2. 視点方向（カメラ前方）
			Vector3 camForward = _machine._camera.transform.forward;
			camForward.Normalize();

			// 3. 合成ベクトル（hook方向 + 視点方向 → 自然にカーブする）
			//Vector3 combinedDirection = (dirToHookNormalized + camForward).normalized;
			Vector3 combinedDirection = (dirToHookNormalized * 0.6f + camForward * 0.4f).normalized;
			/// MEMO:
			/// 複数のExitTimeを用意して遷移を切り替えるか
			/// 遷移のすべてboolではfloatで管理するのもあり。


			// 最新のステート情報取得
			_stateInfo = _machine._animator.GetCurrentAnimatorStateInfo(0);

			if (_stateInfo.IsName("Grappling.flying") && _stateInfo.normalizedTime > _flyForceTriggerProgress)
			{
				_machine._rigidbody.useGravity = true;
				Debug.Log($"{_flyForceTriggerProgress * 100}%を超えた");
				_machine._rigidbody.AddForce(combinedDirection * _pullForce, ForceMode.Impulse);
				_once = true;
				_machine.ShowNowState($"飛んだ！");
			}
			else if (_stateInfo.IsName("Grappling.flying"))
			{
				_machine.ShowNowState($"飛ぶぞ！");
				Debug.Log($"{_flyForceTriggerProgress * 100}%を超えていない：{_stateInfo.normalizedTime}");
			}
			else if (_stateInfo.IsName("Grappling.pull"))
			{
				_machine.ShowNowState($"ぐっ");
			}
			else
			{
				_machine.ShowNowState($"どこでもない");
			}
		}

		public override void OnUpdate()
		{
			_playerPos = _machine.transform.position;
			_hookPoint = _machine._hookPoint;
   //         Debug.Log($"PositionCount: {_machine._lineRenderer.positionCount}, Trying to set index: ");


   //         // 描画更新
   //         _machine._lineRenderer.SetPosition(0, _playerPos);
			//_machine._lineRenderer.SetPosition(1, _hookPoint);

			//// 入力で解除
			//if (_machine._jumpPressed)
			//{
			//	_machine._jumpPressed = false;
			//	_machine.EndSwing("ジャンプボタン押した
			//}
		}

		public override void OnExitState()
		{
			_machine._lineRenderer.enabled = false;

			if (_machine._reticleHitText != null)
			{
				_machine._reticleHitText.text = "";
			}
			_once = false;
		}
	}
}
