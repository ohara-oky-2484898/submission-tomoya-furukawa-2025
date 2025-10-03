using System;
using UnityEngine;

public partial class PlayerController
{
	private class Swinging : StateBase<PlayerController>
	{
		/// <summary> �A�j���[�V�����̏�Ԏ擾�p </summary>
		private AnimatorStateInfo _stateInfo;
		/// <summary> ���݂̂ł���悤�ȈՏ��u�p </summary>
		private bool _once = false;
		/// <summary> ��ԏ��������s����^�C�~���O�p(0~1�i�s�x) </summary>
		private float _flyForceTriggerProgress = 0.17f;
		/// <summary> hookPoint�ւ̈������鋭�� </summary>
		private float _pullForce  = 20f;

		/// <summary> �ȗ����̂��� </summary>
		Vector3 _playerPos;
		Vector3 _hookPoint;

		/// <summary>
		/// �X�C���O�̏��
		/// </summary>
		private enum SwingState
		{
			shot,// �Ђ������邽�߂ɃV���b�g��������
			swing,// �����������ăX�C���O��
			end,// �~��邩�A�؂ꂽ�Ƃ�
		}


		public Swinging(PlayerController machine) : base(machine) { }


		public override void OnEnterState()
		{
			_machine._rigidbody.useGravity = false;
			_machine._lineRenderer.enabled = true;
			_machine.ShowNowState($"�V���b�g��");

			_machine._animator.SetBool("IsPressingWire", true);

			Vector3 lookDir = _machine.GetCameraRelativeDirection();
			_machine.HandleRotation(lookDir);
			_machine._isGrounded = false;
			//_machine.HandleMovement(dir, _moveSpeed, applyGravity: _machine._isJumping);
		}

		public override void OnFixedUpdate()
		{
			// �{�^���������ꂽ�܂܂Ȃ瑁�����^�[��
			if (_machine._swingPressed)
			{
				_machine.ShowNowState($"�_����{_machine._swingPressed}");
				return;
			}

			if (_once)
			{
				// �����J�n����
				if (_machine._rigidbody.linearVelocity.y < -0.1f)
				{
					_machine.ChangeState(PlayerState.Falling);
					return;
				}
				return;
			}

			// �����ꂽ�珈�����J�n
			_machine._animator.SetBool("IsPressingWire", false);

			// 1. �t�b�N����
			Vector3 dirToHook = (_hookPoint - _playerPos);
			float distanceToHook = dirToHook.magnitude;
			Vector3 dirToHookNormalized = dirToHook.normalized;

			// 2. ���_�����i�J�����O���j
			Vector3 camForward = _machine._camera.transform.forward;
			camForward.Normalize();

			// 3. �����x�N�g���ihook���� + ���_���� �� ���R�ɃJ�[�u����j
			//Vector3 combinedDirection = (dirToHookNormalized + camForward).normalized;
			Vector3 combinedDirection = (dirToHookNormalized * 0.6f + camForward * 0.4f).normalized;
			/// MEMO:
			/// ������ExitTime��p�ӂ��đJ�ڂ�؂�ւ��邩
			/// �J�ڂ̂��ׂ�bool�ł�float�ŊǗ�����̂�����B


			// �ŐV�̃X�e�[�g���擾
			_stateInfo = _machine._animator.GetCurrentAnimatorStateInfo(0);

			if (_stateInfo.IsName("Grappling.flying") && _stateInfo.normalizedTime > _flyForceTriggerProgress)
			{
				_machine._rigidbody.useGravity = true;
				Debug.Log($"{_flyForceTriggerProgress * 100}%�𒴂���");
				_machine._rigidbody.AddForce(combinedDirection * _pullForce, ForceMode.Impulse);
				_once = true;
				_machine.ShowNowState($"��񂾁I");
			}
			else if (_stateInfo.IsName("Grappling.flying"))
			{
				_machine.ShowNowState($"��Ԃ��I");
				Debug.Log($"{_flyForceTriggerProgress * 100}%�𒴂��Ă��Ȃ��F{_stateInfo.normalizedTime}");
			}
			else if (_stateInfo.IsName("Grappling.pull"))
			{
				_machine.ShowNowState($"����");
			}
			else
			{
				_machine.ShowNowState($"�ǂ��ł��Ȃ�");
			}
		}

		public override void OnUpdate()
		{
			_playerPos = _machine.transform.position;
			_hookPoint = _machine._hookPoint;
   //         Debug.Log($"PositionCount: {_machine._lineRenderer.positionCount}, Trying to set index: ");


   //         // �`��X�V
   //         _machine._lineRenderer.SetPosition(0, _playerPos);
			//_machine._lineRenderer.SetPosition(1, _hookPoint);

			//// ���͂ŉ���
			//if (_machine._jumpPressed)
			//{
			//	_machine._jumpPressed = false;
			//	_machine.EndSwing("�W�����v�{�^��������
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
