using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerController : MonoBehaviour
{
	/// <summary> player変数 </summary>
	[SerializeField, Header("プレイヤー")]private GameObject playerObj;
	[SerializeField] private　Animator animator;
	[SerializeField] private Rigidbody rbody;

	/// <summary> 移動変数 </summary>
	[SerializeField, Header("移動設定")] private float moveSpeed = 5.0f;
	[SerializeField] private float rotationSpeed = 1.0f;
	[SerializeField] private float jumpPower = 5.0f;
	[SerializeField] private float dashAnimeSpeed = 2.0f;
	[SerializeField] private float dashSpeed = 10.0f; // ダッシュ時の速度
	private Vector2 inputMove;
	public Vector2 readMove;
	private float originalMoveSpeed;
	private Vector3 moveForward;

	/// <summary> カメラ用変数 </summary>
	[SerializeField, Header("カメラ")]GameObject camera;
	Vector3 cameraForward;

	/// <summary> フラグ変数 </summary>
	private bool groundFlag = false;
	private bool jumpFlag = false;
	private bool dashFlag = false;
	public bool pullFlag = false;
	public bool pushFlag = false;
	public bool isAnimetion = false;
	public bool isNotJump = false;


	/// <summary> スタミナ関連 </summary>
	[SerializeField, Header("スタミナ")] private float maxStamina = 5.0f; // 最大スタミナ
	[SerializeField] private float stamina = 5.0f; // 現在のスタミナ
	[SerializeField] private float staminaDrainRate = 1.0f; // スタミナ減少率
	[SerializeField] private float staminaRechargeRate = 0.5f; // スタミナ回復率

	[SerializeField]kiliniti3 kilinitiCS;
	[SerializeField]LineShot lineShotCS;
		
	void Start()
	{
		originalMoveSpeed = moveSpeed; // 元の移動速度を保存
	}

	void Update()
	{
		animator.SetBool("isNotJump", isNotJump);
		animator.SetBool("landing", groundFlag);
        animator.SetFloat("jumpupdown", rbody.velocity.y, 0.1f, Time.deltaTime);


        animator.SetFloat("walk", inputMove.magnitude, 0.1f, Time.deltaTime);

		if(kilinitiCS.isButtonPressed)
		{
			//Debug.Log("とめてる");
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

		// スタミナ回復
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

		//ジャンプ処理
		if (jumpFlag)
		{
			Debug.Log("ジャンプした");
			//アドフォースでジャンプ
			rbody.AddForce(new Vector3(0, jumpPower, 0));

			//フラグ処理
			jumpFlag = false;
			groundFlag = false;
		}


		////////////////////////////////////////////////////////////////////////////////////////////////////////////
		///
		/// 移動処理
		/// 
		////////////////////////////////////////////////////////////////////////////////////////////////////////////
		PlayerMove();
	}



	#region 入力関数
	////移動処理を作成 velocity
	///moveVector3 = moveForward* moveSpeed + new Vector3(0, _rigidbody.velocity.y, 0);
	// 方向キーの入力値とカメラの向きから、移動方向と移動量をmoveForwardに代入
	//moveForward = (cameraForward* inputMove.y + Camera.main.transform.right* inputMove.x);
	public void OnMove(InputAction.CallbackContext context)
	{
		if (context.performed && !pullFlag)
		{
			readMove = inputMove = context.ReadValue<Vector2>();
			// Debug.Log("左動かしてる");
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
		Debug.Log("OnJump押してるよ");
		//ボタンが押された　かつ　地面にたっている
		if (context.performed && groundFlag)
		{
			Debug.Log("jump可能");
			isNotJump = false;
			jumpFlag = true;
		}
	}

	public void OnDash(InputAction.CallbackContext context)
	{
		if (context.started && !isAnimetion && stamina > 0)
		{
			// ダッシュを開始
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

	#region playerの処理

	private void PlayerMove()
	{
		if (kilinitiCS.isButtonPressed || lineShotCS.isButtonPressed)
		{
			// Pull中は移動を無視し、向きをカメラの方向に合わせる
			AlignWithCamera();

			// 空中にいる場合は、Y軸の速度を維持し、X-Z方向の速度をゼロにする
			// 完全に静止させる
			rbody.velocity = Vector3.zero;

			// 重力を無効にする
			rbody.useGravity = false;
			// アニメーションの速度をゼロに設定
			animator.speed = 0;

			return;
		}
		else
		{

			rbody.useGravity = true;
			// アニメーションの速度をゼロに設定
			animator.speed = 1.0f;
		}



		//// カメラの方向から、X-Z平面の単位ベクトル(１に正規化)を取得//回転処理
		//cameraForward = Camera.main.transform.forward;
		//cameraForward.y = 0; // Y成分を無視して水平面の方向を取得
		//cameraForward.Normalize(); // 正規化

		//// 方向キーの入力値とカメラの向きから、移動方向と移動量をmoveForwardに代入
		//moveForward = (cameraForward * inputMove.y + Camera.main.transform.right * inputMove.x);

		cameraForward = Vector3.Scale(camera.transform.forward, new Vector3(1, 0, 1)).normalized;
		// 進む方向計算
		moveForward = (cameraForward * inputMove.y
						+ camera.transform.right * inputMove.x);

		// キャラクターの向きを回転
		PlayerRotation(moveForward);

		// 移動処理
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
			// ↑ラープ関数について詳しく知りたい場合、澤田先生に確認してください。
		}
	}
	private void AlignWithCamera()
	{
		// カメラの向きに合わせてキャラクターの向きを回転
		cameraForward = Vector3.Scale(camera.transform.forward, new Vector3(1, 0, 1)).normalized;
		PlayerRotation(cameraForward); // カメラの向きに合わせて回転
	}
	#endregion

	private void OnTriggerEnter(Collider other)
	{
		//Debug.Log($"{other.name}に触れてる");
		//足がついているかの判定
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