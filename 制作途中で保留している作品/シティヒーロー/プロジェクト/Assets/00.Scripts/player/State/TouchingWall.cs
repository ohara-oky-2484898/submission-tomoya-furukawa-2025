using UnityEngine;

public partial class PlayerController
{

	/// <summary>
	/// �ǂɐG��ĐÎ~���Ă�����
	/// </summary>
	private class TouchingWall : StateBase<PlayerController>
	{
		public TouchingWall(PlayerController machine) : base(machine) { }

		/// <summary> WallJumpingState�ɓ�������p�F �ǂɂ��܂��Ă��ԂŁA�ǂƂ͋t�̕����ɓ|�� + �W�����v�{�^��</summary>
		private bool IsWallJumping
		{
			get 
			{
				// �ǂɂ��܂��Ă��Ȃ�
				if (!_machine._currentWallNormal.HasValue)
				{
					_machine.ShowNowState("�ǂɐG��ĂȂ��I");
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
                    _machine.ShowNowState($"�ǃL�b�N{(dot > 0.5f ? "�����I" : "�s��!")}");
                }
                else
                {
                    _machine.ShowNowState($"�ǂɓ\����Ă�");
                }
				Debug.Log($"�ڍׁi{(_machine._jumpPressed && dot > 0.5f ? "����" : "���s")}�j�Fdot :{dot}, jumpbutton{_machine._jumpPressed}");
                // �ǃW�����v���������������΃W�����v
                return _machine._jumpPressed && dot > 0.5f;
			}
		}

		public override void OnEnterState()
		{
			_machine._animator.SetBool("IsTouchingWall", true);


			//_machine._rigidbody.angularVelocity = Vector3.zero;
			//_machine._rigidbody.linearVelocity = Vector3.zero;
			//_machine._rigidbody.useGravity = false;
			_machine.ShowNowState($"�ǂ��܂蒆");
		}

		public override void OnFixedUpdate()
		{
			#region ��U�c���Ă���
			//_machine.WallTouchRotation(-_machine._currentWallNormal);
			//_machine.CheckNearestWall();

			//Vector3 origin = _machine.transform.position + Vector3.up * 1.0f;
			//Vector3 wallNormal = _machine._currentWallNormal.normalized;

			//// ��F�ǂ̖@�������i= wallNormal�j
			////Debug.DrawRay(origin, wallNormal * 1.5f, Color.red);

			//// �ǃW�����v�\�ȓ��͔͈́iwallNormal �}60�x�j
			////int rayCount = 12;
			////float angleRange = 120f; // �}60�x = ���v120�x
			////// -6����6�܂�
			////for (int i = -rayCount / 2; i <= rayCount / 2; i++)
			////{
			////	float angle = i * (angleRange / rayCount);
			////	Vector3 dir = Quaternion.AngleAxis(angle, Vector3.up) * wallNormal;
			////	//Debug.DrawRay(origin, dir * 1.2f, Color.green);
			////}



			//// ?? �J������̓��͕���
			//Vector3 camForward = _machine._camera.transform.forward;
			//Vector3 camRight = _machine._camera.transform.right;

			//camForward.y = 0;
			//camRight.y = 0;
			//camForward.Normalize();
			//camRight.Normalize();

			//Vector3 inputDir = (camForward * _machine._moveInput.y + camRight * _machine._moveInput.x).normalized;

			////// ?? ���݂̃X�e�B�b�N���͕�����\��
			////if (inputDir.sqrMagnitude > 0.01f)
			////{
			////	Debug.DrawRay(origin, inputDir * 1.2f, Color.blue);
			////}


			//float dot = Vector3.Dot(_machine._currentWallNormal, inputDir);

			//// �ǃW�����v���������������΃W�����v
			//if (_machine._jumpPressed && dot > 0.5f)
			#endregion

			_machine.CheckNearestWall();

			if (IsWallJumping)
			{
				_machine._jumpPressed = false;
				_machine.ChangeState(PlayerState.WallJumping);
				return;
			}

			// �n�ʂɒ�������ʏ�ړ���
			if (_machine._isGrounded)
			{
				_machine.ChangeState(PlayerState.Moving);
				return;
			}

			// ��x�ǂ͗��t������A��𗣂�������
			// �܂�,IsFalling�𖞂����ĂȂ��Ă�������ԂɂȂ��Ăق�������
			if(!_machine._currentWallNormal.HasValue)
			{
				_machine.ChangeState(PlayerState.Falling);
				return;
			}
			//Debug.Log($"{(_machine._isTouchingWall ? "�ǂɂ��Ă�" : "�ǂ��痣�ꂽ")}");
			//// �ǂ��痣�ꂽ��W�����v��ԂȂǂ�
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
