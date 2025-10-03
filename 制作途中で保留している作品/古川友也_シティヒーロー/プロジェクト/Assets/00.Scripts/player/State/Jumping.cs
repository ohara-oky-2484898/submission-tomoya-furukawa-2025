using UnityEngine;

public partial class PlayerController
{
	/// <summary>
	/// �W�����v���
	/// </summary>
	private class Jumping : StateBase<PlayerController>
	{
		// �ړ����x�^�W�����v�͂̃p�����[�^
		private float _moveSpeed = 5f;
		private float _minJumpPower = 6f;
		private float _maxJumpPower = 10f;

		// �W�����v�{�^���������A�P��������p
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
			_machine.ShowNowState("�W�����v��");
		}

		public override void OnUpdate()
		{
			// �A�j���[�^�[�X�V
			_machine.UpdateAnimatorParams();

			// ���͒��������Ԃ̑���
			_holdTime = Time.time - _jumpStartTime;

			// �W�����v�������ςȂ��ōő厞�Ԓ������狭���I�ɃW�����v
			if (_machine._jumpPressed && _holdTime > _maxJumpHoldTime)
			{
				_machine._jumpPressed = false;
				_jumpReleased = true;
			}
			// ���͓r���ŕ����ꂽ�Ƃ�
			else if (!_machine._jumpPressed)
			{
				_jumpReleased = true;
			}
		}

		public override void OnFixedUpdate()
		{
			// �ړ������E�ړ�����
			var dir = _machine.GetCameraRelativeDirection();
			//_machine.HandleMovement(dir, _moveSpeed);

			// ���O�t���������g�����Ƃŏd�͂̐ݒ�̓f�t�H���g�����ŁI
			// �W�����v���Ă��Ȃ��Ƃ���linearVelocity���㏑�������̂�h��
			_machine.HandleMovement(dir, _moveSpeed, applyGravity: _machine._isJumping);
			_machine.HandleRotation(dir);

			// �ǐڐG����
			_machine.CheckNearestWall();

			if (!_jumpReleased) return;

			// �W�����v�����F���͂��������Ă��āA�܂��W�����v���Ă��Ȃ��Ȃ�
			if (!_machine._isJumping)
			{
				_machine._isJumping = true;
				_machine._isGrounded = false;

				// ���W��������W����������
				float jumpPower = (_holdTime < _maxJumpHoldTime) ? _minJumpPower : _maxJumpPower;
				//_holdTime = Mathf.Clamp(_holdTime, 0f, _maxJumpHoldTime);
				//float jumpPower = Mathf.Lerp(_minJumpPower, _maxJumpPower, _holdTime / _maxJumpHoldTime);

				//// y�����̑��x���W�����v�͂ɍX�V
				_machine._rigidbody.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
			}
			// �����J�n����
			if (_machine._rigidbody.linearVelocity.y < -0.1f)
			{
				_machine.ChangeState(PlayerState.Falling);
				return;
			}

			//Vector3 inputDir = new Vector3(_machine._moveInput.x, 0, _machine._moveInput.y).normalized;

			//// �ǂ̖@���Ɠ��͕����̓��ς��v�Z
			//float dot = Vector3.Dot(_machine._currentWallNormal, inputDir);

			//// �ǂ̖@���Ɠ��͕������قڋt�����Ȃ���ς͕��̒l�ɂȂ�i-1�ɋ߂��j
			//// �Ⴆ�� -0.5 �ȉ��Ȃ�ǂɌ������ē��͂��Ă���Ɣ��f
			//if (dot < -0.5f)
			//{
			//	_machine._jumpPressed = false;
			//	_machine.ChangeState(new WallJumping(_machine));
			//}

			//Vector3 inputDir = new Vector3(_machine._moveInput.x, 0, _machine._moveInput.y).normalized;

			//// �ǂ̖@���Ɠ��͕����̓��ς��v�Z
			//float dot = Vector3.Dot(_machine._currentWallNormal, inputDir);

			//// ���ς�0.5�ȏ�i�ǂ̗��������ɓ��͂��Ă���j�Ȃ�ǃW�����vOK
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
			//	// �ǂ̖@���x�N�g���Ɠ��͕����̊p�x���v�Z
			//	Vector3 inputDir = new Vector3(_machine._moveInput.x, 0, _machine._moveInput.y).normalized;
			//	float angle = Vector3.Angle(_machine._currentWallNormal, inputDir);

			//	// �ǂ̖@���Ɠ��͕��������Ε����i�p�x��90�x�ȏ�j�Ȃ�ǃW�����vOK
			//	// �p�x��90�x�����Ȃ�A���͖͂@���x�N�g���Ƃقړ��������i�ǂ̊O�����j
			//	// �p�x��90�x�ȏ�Ȃ�A���͖͂@���x�N�g���Ɣ��Ε����i�ǂɌ������Ă���j
			//	if (angle < 90f)
			//	{
			//		// �ǂ̊O�����ɓ��� �� �ǃW�����v���Ȃ�
			//	}
			//	else
			//	{
			//		// �ǂɌ������ē��� �� �ǃW�����vOK
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
