using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerController : MonoBehaviour
{
	/// <summary> player�ϐ� </summary>
	[SerializeField, Header("�v���C���[")]private GameObject playerObj;
	[SerializeField] private�@Animator animator;
	[SerializeField] private Rigidbody rbody;

	/// <summary> �ړ��ϐ� </summary>
	[SerializeField, Header("�ړ��ݒ�")] private float moveSpeed = 5.0f;
	[SerializeField] private float rotationSpeed = 1.0f;
	[SerializeField] private float jumpPower = 5.0f;
	[SerializeField] private float dashAnimeSpeed = 2.0f;
	[SerializeField] private float dashSpeed = 10.0f; // �_�b�V�����̑��x
	private Vector2 inputMove;
	public Vector2 readMove;
	private float originalMoveSpeed;
	private Vector3 moveForward;

	/// <summary> �J�����p�ϐ� </summary>
	[SerializeField, Header("�J����")]GameObject camera;
	Vector3 cameraForward;

	/// <summary> �t���O�ϐ� </summary>
	private bool groundFlag = false;
	private bool jumpFlag = false;
	private bool dashFlag = false;
	public bool pullFlag = false;
	public bool pushFlag = false;
	public bool isAnimetion = false;
	public bool isNotJump = false;


	/// <summary> �X�^�~�i�֘A </summary>
	[SerializeField, Header("�X�^�~�i")] private float maxStamina = 5.0f; // �ő�X�^�~�i
	[SerializeField] private float stamina = 5.0f; // ���݂̃X�^�~�i
	[SerializeField] private float staminaDrainRate = 1.0f; // �X�^�~�i������
	[SerializeField] private float staminaRechargeRate = 0.5f; // �X�^�~�i�񕜗�

	[SerializeField]kiliniti3 kilinitiCS;
	[SerializeField]LineShot lineShotCS;
		
	void Start()
	{
		originalMoveSpeed = moveSpeed; // ���̈ړ����x��ۑ�
	}

	void Update()
	{
		animator.SetBool("isNotJump", isNotJump);
		animator.SetBool("landing", groundFlag);
        animator.SetFloat("jumpupdown", rbody.velocity.y, 0.1f, Time.deltaTime);


        animator.SetFloat("walk", inputMove.magnitude, 0.1f, Time.deltaTime);

		if(kilinitiCS.isButtonPressed)
		{
			//Debug.Log("�Ƃ߂Ă�");
			animator.speed = 0;
		}
		else if (dashFlag)
		{
			animator.speed = dashAnimeSpeed;
		}
		else
		{
			animator.speed = 1.0f;
		}

		// �X�^�~�i��
		if (!dashFlag && stamina < maxStamina)
		{
			stamina += staminaRechargeRate * Time.deltaTime;
		}
	}


	private void FixedUpdate()
	{
		if (kilinitiCS.isMove)
		{
			groundFlag = false;
		}

		//�W�����v����
		if (jumpFlag)
		{
			Debug.Log("�W�����v����");
			//�A�h�t�H�[�X�ŃW�����v
			rbody.AddForce(new Vector3(0, jumpPower, 0));

			//�t���O����
			jumpFlag = false;
			groundFlag = false;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////////////////
		///
		/// �ړ�����
		/// 
		////////////////////////////////////////////////////////////////////////////////////////////////////////////
		PlayerMove();
	}



	#region ���͊֐�
	////�ړ��������쐬 velocity
	///moveVector3 = moveForward* moveSpeed + new Vector3(0, _rigidbody.velocity.y, 0);
	// �����L�[�̓��͒l�ƃJ�����̌�������A�ړ������ƈړ��ʂ�moveForward�ɑ��
	//moveForward = (cameraForward* inputMove.y + Camera.main.transform.right* inputMove.x);
	public void OnMove(InputAction.CallbackContext context)
	{
		if (context.performed && !pullFlag)
		{
			readMove = inputMove = context.ReadValue<Vector2>();
			// Debug.Log("���������Ă�");
		}
		else
		{
			readMove = inputMove = Vector2.zero;
			moveSpeed = originalMoveSpeed;
			dashFlag = false;
		}

	}

	public void OnJump(InputAction.CallbackContext context)
	{
		Debug.Log("OnJump�����Ă��");
		//�{�^���������ꂽ�@���@�n�ʂɂ����Ă���
		if (context.performed && groundFlag)
		{
			Debug.Log("jump�\");
			isNotJump = false;
			jumpFlag = true;
		}
	}

	public void OnDash(InputAction.CallbackContext context)
	{
		if (context.started && !isAnimetion && stamina > 0)
		{
			// �_�b�V�����J�n
			dashFlag = true;
			isAnimetion = true;
			//animator.SetBool("dash", dashFlag);
			animator.SetTrigger("dashMove");
			moveSpeed = dashSpeed;
			// StartCoroutine(Dash());
			isAnimetion = false;
		}
	}

	#endregion

	#region player�̏���

	private void PlayerMove()
	{
		if (kilinitiCS.isButtonPressed || lineShotCS.isButtonPressed)
		{
			// Pull���͈ړ��𖳎����A�������J�����̕����ɍ��킹��
			AlignWithCamera();

			// �󒆂ɂ���ꍇ�́AY���̑��x���ێ����AX-Z�����̑��x���[���ɂ���
			// ���S�ɐÎ~������
			rbody.velocity = Vector3.zero;

			// �d�͂𖳌��ɂ���
			rbody.useGravity = false;
			// �A�j���[�V�����̑��x���[���ɐݒ�
			animator.speed = 0;

			return;
		}
		else
		{

			rbody.useGravity = true;
			// �A�j���[�V�����̑��x���[���ɐݒ�
			animator.speed = 1.0f;
		}



		//// �J�����̕�������AX-Z���ʂ̒P�ʃx�N�g��(�P�ɐ��K��)���擾//��]����
		//cameraForward = Camera.main.transform.forward;
		//cameraForward.y = 0; // Y�����𖳎����Đ����ʂ̕������擾
		//cameraForward.Normalize(); // ���K��

		//// �����L�[�̓��͒l�ƃJ�����̌�������A�ړ������ƈړ��ʂ�moveForward�ɑ��
		//moveForward = (cameraForward * inputMove.y + Camera.main.transform.right * inputMove.x);

		cameraForward = Vector3.Scale(camera.transform.forward, new Vector3(1, 0, 1)).normalized;
		// �i�ޕ����v�Z
		moveForward = (cameraForward * inputMove.y
						+ camera.transform.right * inputMove.x);

		// �L�����N�^�[�̌�������]
		PlayerRotation(moveForward);

		// �ړ�����
		if (!kilinitiCS.isNotControl)
		{
			rbody.velocity = moveForward * moveSpeed + new Vector3(0, rbody.velocity.y, 0);
		}
	}

	private void PlayerRotation(Vector3 moveForward)
	{
		if (moveForward != Vector3.zero)
		{
			Quaternion rotation = Quaternion.LookRotation(moveForward);
			transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
			// �����[�v�֐��ɂ��ďڂ����m�肽���ꍇ�A�V�c�搶�Ɋm�F���Ă��������B
		}
	}
	private void AlignWithCamera()
	{
		// �J�����̌����ɍ��킹�ăL�����N�^�[�̌�������]
		cameraForward = Vector3.Scale(camera.transform.forward, new Vector3(1, 0, 1)).normalized;
		PlayerRotation(cameraForward); // �J�����̌����ɍ��킹�ĉ�]
	}
	#endregion

	private void OnTriggerEnter(Collider other)
	{
		//Debug.Log($"{other.name}�ɐG��Ă�");
		//�������Ă��邩�̔���
		if (other.tag == "Ground")
		{
			groundFlag = true;
			isNotJump = true;
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.tag == "Ground")
		{
			groundFlag = false;
		}
	}
}