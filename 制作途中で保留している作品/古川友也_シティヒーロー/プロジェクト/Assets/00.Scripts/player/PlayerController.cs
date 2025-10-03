using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using System.Collections.Generic;

// 遷移と状態がそれぞれ決まっていることが有限オートマトン

public partial class PlayerController : StateMachineBase<PlayerController, PlayerController.PlayerState>
{
	[Header("Controllerの設定")]
	public InputActionReference action;

	[Header("playerの設定")]
	[SerializeField] private Animator _animator;
	private Rigidbody _rigidbody;
	private Camera _camera;
	private Vector2 _moveInput;
	private bool _isGrounded = false;
	private float _rotationSpeed = 10f;

	/// <summary>
	/// 壁判定用
	/// </summary>
	[Header("壁アクション用の設定")]
	[SerializeField] private LayerMask _wallLayer;
	/// <summary>
	/// 壁につかまるかどうかの判別するための入力方向閾値(内積)
	/// 壁の法線の逆方向に対してスティックがどれくらい向いているか、その値とこの値を使って判別
	/// 小さくなるほど、入力の遊びの幅が広くなる(0だと壁に対して真横に倒しても判定吸われるということ)
	/// </summary>
	private float _wallGrabInputThreshold = 0.7f;	//45度くらい
	/// <summary> 壁に触れたと検知する最大の距離 </summary>
	private float _detectionMaxDistance = 0.5f;
	private float _sphereRadius = 0.48f;
	private float _sphereHeightOffset = 0.8f;
	private Vector3? _currentWallNormal = null; // 接触している壁の法線ベクトル


	[Header("Swing／Grappleアクション用の設定")]
	[SerializeField] private LineRenderer _lineRenderer;
	[SerializeField] private LayerMask _hookableLayer;
	[SerializeField] private float _maxHookDistance = 30f;
	//[SerializeField] private float _swingForce = 20f;
	private Vector3 _hookPoint;
	private bool _isSwinging = false;

	/// <summary>
	/// デバック／簡易UI用
	/// </summary>
	[Header("UI用の設定")]
	[SerializeField] private TextMeshProUGUI _text;
	[SerializeField] private TextMeshProUGUI _reticleCenterText;
	[SerializeField] private TextMeshProUGUI _reticleHitText;

	/// <summary>
	/// 状態管理用
	/// </summary>
	PlayerState _nowState;
	private bool _jumpPressed = false;
	//private bool _dashPressed = false;
	private bool _swingPressed = false;
	private bool _isJumping = false;


	// 条件変更したいとき一括変更できる！
	/// <summary> MoveStateに入る条件用：左スティックの入力が少しでもあれる </summary>
	private bool IsMoving => _moveInput.sqrMagnitude > 0.01f;
	/// <summary> JumpStateに入る条件用：地面に足がついているかつ、ボタンが押された </summary>
	private bool IsJumping => _jumpPressed && _isGrounded;
	/// <summary> DashingStateに入る条件用： ボタンが押さたかつ、クールが上がってたら</summary>
	//private bool IsDashing => _dashPressed;
	/// <summary> FallingStateに入る条件用： 垂直方向の数値がマイナスの値になったら</summary>
	private bool IsFalling => _rigidbody.linearVelocity.y < -0.1f;
	/// <summary> GrapplingStateに入る条件用： ボタンが押さたかつ、クールが上がってたら</summary>
	private bool IsGrappling => false;
	/// <summary> SwingingStateに入る条件用： ボタンが押さたかつ、クールが上がってたら</summary>
	private bool IsSwinging => false;
	/// <summary> TouchingWallStateに入る条件用： 壁に触れている状態で、壁側にスティックを倒したら</summary>
	private bool IsTouchingWall
	{
		get 
		{
			// 値を持っていない → 壁に触れていない
			if (!_currentWallNormal.HasValue) return false;

			Vector3 camForward = _camera.transform.forward;
			Vector3 camRight = _camera.transform.right;

			camForward.y = 0;
			camRight.y = 0;
			camForward.Normalize();
			camRight.Normalize();

			Vector3 inputDir = (camForward * _moveInput.y + camRight * _moveInput.x).normalized;

			// 壁の法線の逆と入力方向の内積をとってその内積が閾値より大きければなら壁張りつき判定
			float dot = Vector3.Dot(-_currentWallNormal.Value, inputDir);

			// 1が壁の法線逆方向と全く同じ方向に倒してる、0が垂直に倒してる
			return dot > _wallGrabInputThreshold;
		}
	}

	public enum PlayerState
	{
		Idling = 0,
		Moving,
		Jumping,
		Dashing,
		Falling,
		Grappling,
		Swinging,
		WallJumping,
		TouchingWall,
	}

	private void Awake()
	{
		_rigidbody = GetComponent<Rigidbody>();
		_camera = Camera.main;

		StateInit();
		Debug.Log("ステート準備完了");
		InitReticleUI();
	}

	private void Start()
	{

		// 初期状態に遷移
		if (ChangeState(PlayerState.Idling))
		{
			Debug.Log("ステート初期化完了");
		}
		else
		{
			Debug.Log("ステート初期化できなかった");
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.CompareTag("Ground"))
		{
			_isGrounded = true;
			_jumpPressed = false;
		}
	}

	/// <summary>
	/// 各ステートのインスタンスを生成し、StateMachineBase に登録
	/// </summary>
	private void StateInit()
	{
		RegisterState(PlayerState.Idling, new Idling(this));
		RegisterState(PlayerState.Moving, new Moving(this));
		RegisterState(PlayerState.Jumping, new Jumping(this));
		RegisterState(PlayerState.Dashing, new Dashing(this));
		RegisterState(PlayerState.Falling, new Falling(this));
		RegisterState(PlayerState.Grappling, new Grappling(this));
		RegisterState(PlayerState.Swinging, new Swinging(this));
		RegisterState(PlayerState.WallJumping, new WallJumping(this));
		RegisterState(PlayerState.TouchingWall, new TouchingWall(this));
	}

	private void TryStartSwing()
	{
		Ray ray = _camera.ViewportPointToRay(new Vector3(0.5f, 0.5f));
		if (Physics.Raycast(ray, out RaycastHit hit, _maxHookDistance, _hookableLayer))
		{
			_hookPoint = hit.point;
			_isSwinging = true;
			_swingPressed = true;

			ChangeState(PlayerState.Swinging);
		}
	}
	private void EndSwing(string text)
	{
		Debug.Log("グラップル原因:"+text);
		_isSwinging = false;
		_swingPressed = false;
	}

	public void OnSwing(InputAction.CallbackContext context)
	{
		if (context.performed && !_isSwinging)
		{
			TryStartSwing();
		}
		else if (context.canceled)
		{
			EndSwing("ボタンキャンセルした");
		}
	}
	public void OnGrapple(InputAction.CallbackContext context)
	{
		if (context.performed && !_isSwinging)
		{
			TryStartSwing();
		}
		else if (context.canceled)
		{
			EndSwing("ボタンキャンセルした");
		}
	}


	public void OnMove(InputAction.CallbackContext context)
	{
		
		if (context.performed)
		{
			_moveInput = context.ReadValue<Vector2>();
		}
		else if(context.canceled)
		{
			_moveInput = Vector2.zero;
		}
	}

	// ジャンプアクションの入力を受け取る
	public void OnJump(InputAction.CallbackContext context)
	{
		if (context.performed && !_isJumping)
		{
			_jumpPressed = true;
		}
		else if (context.canceled)
		{
			_jumpPressed = false;
		}
	}

	// ダッシュアクションの入力を受け取る
	public void OnDash(InputAction.CallbackContext context)
	{
		if (context.performed)
		{
			//_dashPressed = true;
		}
	}

	private void InitReticleUI()
	{
		if (_reticleCenterText != null)
		{
			_reticleCenterText.text = "+";
			_reticleCenterText.alignment = TextAlignmentOptions.Center;
			_reticleCenterText.horizontalAlignment = HorizontalAlignmentOptions.Center;
			_reticleCenterText.verticalAlignment = VerticalAlignmentOptions.Middle;
			_reticleCenterText.rectTransform.anchoredPosition = Vector2.zero;
		}

		if (_reticleHitText != null)
		{
			_reticleHitText.text = "";
			_reticleHitText.alignment = TextAlignmentOptions.Center;
			_reticleHitText.horizontalAlignment = HorizontalAlignmentOptions.Center;
			_reticleHitText.verticalAlignment = VerticalAlignmentOptions.Middle;

			Vector3 dir = this.transform.forward;
			Vector3 worldPos = transform.position + dir * _maxHookDistance;
			Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);
			_reticleHitText.rectTransform.position = screenPos;
		}
	}
	private void OnDrawGizmosSelected()
	{
		// OverlapSphere と同じ位置・サイズで球体を描画
		Vector3 sphereCenter = transform.position + Vector3.up * _sphereHeightOffset;

		Gizmos.color = Color.cyan;  // デバッグ用の色（見やすい色なら自由に変えてOK）
		Gizmos.DrawWireSphere(sphereCenter, _sphereRadius);
	}


	/// <summary>
	/// 壁に触れているか確認する
	/// 触れている場合一番近い壁の法線を取得する
	/// </summary>
	private void CheckNearestWall()
	{
		// 初期化
		_currentWallNormal = null;

		// 中心点の計算
		Vector3 sphereCenter = transform.position + Vector3.up * _sphereHeightOffset;

		// 範囲内の壁を全て取得
		Collider[] hits = Physics.OverlapSphere(sphereCenter, _sphereRadius, _wallLayer);

		// 壁に触れたと判別するための最大値を設定しておく
		float closestDistance = _detectionMaxDistance;
		Collider closestCollider = null;

		// 指定の距離以内にある最も近い壁を選ぶ
		foreach (var hit in hits)
		{
			// 距離 = 中心点からヒットした壁の表面上で指定した(今回は中心点)場所に一番近いポイント
			float distance = Vector3.Distance(sphereCenter, hit.ClosestPoint(sphereCenter));
			// 設定された距離より近かったら設定を更新していく
			if (distance < closestDistance)
			{
				closestDistance = distance;
				closestCollider = hit;
			}
		}

		// 壁に触れたとみなす壁があれば触れたときの処理をする
		if (closestCollider != null)
		{
			// 一番近いコライダーの接点の法線を取得
			Vector3 closestPoint = closestCollider.ClosestPoint(sphereCenter);
			Vector3 dir = (closestPoint - sphereCenter).normalized;

			if (Physics.Raycast(sphereCenter, dir, out RaycastHit hitInfo, _detectionMaxDistance + _sphereRadius, _wallLayer))
			{
				_currentWallNormal = hitInfo.normal;
				Debug.DrawRay(hitInfo.point, hitInfo.normal * 0.3f, Color.green);
			}
		}
	}

	private void DrawWireSphere(Vector3 position, float radius, Color color)
	{
		float segments = 16;
		float angle = 360f / segments;

		// XZ平面
		for (int i = 0; i < segments; i++)
		{
			Vector3 p1 = position + Quaternion.Euler(0, i * angle, 0) * (Vector3.forward * radius);
			Vector3 p2 = position + Quaternion.Euler(0, (i + 1) * angle, 0) * (Vector3.forward * radius);
			Debug.DrawLine(p1, p2, color, 0f);
		}

		// XY平面
		for (int i = 0; i < segments; i++)
		{
			Vector3 p1 = position + Quaternion.Euler(i * angle, 0, 0) * (Vector3.up * radius);
			Vector3 p2 = position + Quaternion.Euler((i + 1) * angle, 0, 0) * (Vector3.up * radius);
			Debug.DrawLine(p1, p2, color, 0f);
		}

		// YZ平面
		for (int i = 0; i < segments; i++)
		{
			Vector3 p1 = position + Quaternion.Euler(0, 0, i * angle) * (Vector3.right * radius);
			Vector3 p2 = position + Quaternion.Euler(0, 0, (i + 1) * angle) * (Vector3.right * radius);
			Debug.DrawLine(p1, p2, color, 0f);
		}
	}

	public void HandleMovement(Vector3 moveDir, float moveSpeed, float gravity = -9.81f, bool applyGravity = true)
	{
		Vector3 velocity = Vector3.zero;

		// 横方向の移動（カメラ基準）
		if (moveDir.sqrMagnitude > 0.01f)
		{
			Vector3 horizontalVelocity = moveDir.normalized * moveSpeed;
			velocity.x = horizontalVelocity.x;
			velocity.z = horizontalVelocity.z;
		}
		// 空中で急停止防止
		else
		{
			velocity.x = _rigidbody.linearVelocity.x;
			velocity.z = _rigidbody.linearVelocity.z;
		}
		//// Y方向に手動重力を加える(次の速度 = 現在の速度 + 加速度 + Δt)
		//velocity.y = _rigidbody.linearVelocity.y + gravity * Time.fixedDeltaTime;
		// 重力を加えるかどうか
		if (applyGravity)
		{
			velocity.y = _rigidbody.linearVelocity.y + gravity * Time.fixedDeltaTime;
		}
		else
		{
			velocity.y = _rigidbody.linearVelocity.y;
		}


		_rigidbody.linearVelocity = velocity;
	}


	/// <summary>
	/// 指定の方向に回転させる関数
	/// </summary>
	/// <param name="moveDir">回転させたい方向</param>
	public void HandleRotation(Vector3 moveDir)
	{
		if (moveDir.sqrMagnitude > 0.01f)
		{
			Quaternion targetRotation = Quaternion.LookRotation(moveDir);
			transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
		}
	}

	/// <summary>
	/// カメラ基準になるように入力方向を設定し、取得する関数
	/// </summary>
	/// <returns>カメラ基準の移動方向</returns>
	public Vector3 GetCameraRelativeDirection()
	{
		Transform camTransform = _camera.transform;
		Vector3 camForward = camTransform.forward;
		Vector3 camRight = camTransform.right;

		camForward.y = 0;
		camRight.y = 0;
		camForward.Normalize();
		camRight.Normalize();

		return camForward * _moveInput.y + camRight * _moveInput.x;
	}




	private void UpdateAnimatorParams()
	{
		_animator.SetFloat("YVelocity", _rigidbody.linearVelocity.y);
		_animator.SetBool("IsGrounded", _isGrounded);
		_animator.SetBool("IsJumping", !_isGrounded); // 空中ならジャンプ中とみなす
	}

	public void WallTouchRotation(Vector3 dir)
	{
		// 回転
		Quaternion targetRotation = Quaternion.LookRotation(dir);
		transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10f * Time.deltaTime);
	}

	public void DrawReticle()
	{
		Vector3 origin = _camera.transform.position;
		Vector3 direction = _camera.transform.forward;

		RaycastHit hit;
		Vector3 screenPoint;

		// プレイヤーのレイヤーを除外
		int layerMask = ~(1 << LayerMask.NameToLayer("Player"));

		// Raycastでターゲット取得
		if (Physics.Raycast(origin, direction, out hit, _maxHookDistance, layerMask))
		{
			_hookPoint = hit.point;
			screenPoint = _camera.WorldToScreenPoint(hit.point);

			// スクリーン座標がカメラの前面にあるかチェック
			if (screenPoint.z > 0)
			{
				if (_reticleHitText != null)
				{
					_reticleHitText.rectTransform.position = screenPoint;

					// Hook可能なレイヤーかどうか確認
					if (((1 << hit.collider.gameObject.layer) & _hookableLayer) != 0)
					{
						_reticleHitText.text = "※"; // Hook可能
						_reticleHitText.color = Color.red;
					}
					else
					{
						_reticleHitText.text = "×"; // Hook不可能
						_reticleHitText.color = Color.gray;
					}
				}
			}
			else
			{
				// カメラの後方にある場合は非表示
				if (_reticleHitText != null)
				{
					_reticleHitText.text = "";
				}
			}
		}
		else
		{
			// Rayが何もヒットしなかった場合
			if (_reticleHitText != null)
			{
				_reticleHitText.text = "";
			}
		}
	}

	/// <summary>
	/// デバック用現在の状態をテキストに反映させる
	/// </summary>
	/// <param name="state"></param>
	public void ShowNowState(string state)
	{
		_text.text = $"現在の状態:{state}";
	}


	// メモ
	//Rigidbody rb = _rigidbody;
	//bool OnSlope()
	//{
	//	float playerHeight = this.gameObject.transform.lossyScale.y;
	//	float maxSlopeAngle = 45f;
	//	if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit slopeHit, playerHeight * 0.5f * 0.3f))
	//	{
	//		float angle = Vector3.Angle(Vector3.up, slopeHit.normal);

	//		return angle < maxSlopeAngle && angle != 0;
	//	}

	//	return false;
	//}


	//bool wallRight;
	//bool wallLeft;
	//RaycastHit rightWallhit;
	//RaycastHit leftWallhit;
	//float wallCheckDistans = 0.5f;
	//LayerMask whatIsWall;
	//void CheckForWall()
	//{
	//	wallRight = Physics.Raycast(transform.position, transform.right, out rightWallhit, wallCheckDistans, whatIsWall);
	//	wallLeft = Physics.Raycast(transform.position, -transform.right, out leftWallhit, wallCheckDistans, whatIsWall);
	//}

	//void WallRUnningMovement()
	//{

	//	// 法線ベクトルを使うか
	//	// ずりベクトルを使うか要件等
	//	rb.useGravity = false;
	//	rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

	//	Vector3 wallNormal = wallRight ? rightWallhit.normal : leftWallhit.normal;

	//	Vector3 wallForward = Vector3.Cross(wallNormal, transform.up);

	//	rb.AddForce(wallForward * 20f, ForceMode.Impulse);
	//}
}
