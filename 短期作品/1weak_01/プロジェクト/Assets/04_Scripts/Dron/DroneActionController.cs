using UnityEngine;
using UnityEngine.InputSystem;

public class DroneActionController : MonoBehaviour
{
    private Vector3 inputMove;
    private Vector3 cameraForward;
    private Vector3 moveForward;


    [SerializeField] public float maxSpeed = 30.0f;
    [SerializeField] public float maxUpDownSpeed = 30.0f;
    [SerializeField] private float moveSpeed = 0.0f;
    [SerializeField] private float rotationSpeed = 0.0f;
    [SerializeField] Rigidbody _rigidbody;

    // 判定したいオブジェクトのタグを指定
    public string targetTag = "TargetObject";
    private string[] groundTags = { "PlaneRed", "PlaneBlue", "PlaneYellow", "PlaneGreen" };


    public bool holdFlag = false;   // ものをつかめたか
    public bool isCatch = false;   // つかめるか
    public bool upFlag = false;     // UpなのかDownなのか
    public bool friezeFlag = false;     // 動きがない・リジットbodyを切りたい
    public bool OnTouch = false;     // 

    public bool isGround = false;

    public Vector3 moveVector3;
    public float upDownSpeed = 0.0f;

    // 状態管理用
    public bool spinnerFlag = false;// UpDownなどの操作をしているか..プロペラを回すよう
    public bool isMax = false;

    // 掴んだオブジェクト用
    private GameObject holdObj;
    private Rigidbody holdObj_rigidbody;

    private void FixedUpdate()
	{
        // 移動
        MainMovementsMethod();
        UpDownMove();
        HoldAction();

        _rigidbody.velocity = moveVector3;

        if (!upFlag && !OnTouch)
        {
            upDownSpeed -= Time.deltaTime;
            moveVector3.y -= upDownSpeed;
        }
    }

    private void MainMovementsMethod()
    {// 移動処理

        //回転処理
        cameraForward = Camera.main.transform.forward;
        cameraForward.y = 0; // Y成分を無視して水平面の方向を取得
        cameraForward.Normalize(); // 正規化

        // 方向キーの入力値とカメラの向きから、移動方向と移動量をmoveForwardに代入
        moveForward = (cameraForward * inputMove.y + Camera.main.transform.right * inputMove.x).normalized;

        // キャラクターの向きを回転
        if (moveForward != Vector3.zero)
        {
            Quaternion rotation = Quaternion.LookRotation(moveForward);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
        }


        if (moveForward != Vector3.zero)
        {
            //移動処理を作成 velocity
            moveVector3 = moveForward * moveSpeed + new Vector3(0, _rigidbody.velocity.y, 0);
            //Debug.Log("動いてる！");
        }
        else
        {
            // 入力がないときは速度をゼロに設定
            moveVector3 = new Vector3(0, _rigidbody.velocity.y, 0);
            //Debug.Log("とまってる！");
        }
    }

    private void HoldAction()
    {
        if (!holdFlag) return;

        // プレイヤーの位置の下に配置したい
        holdObj_rigidbody.velocity = moveVector3;
       // Debug.Log("掴んでるよ！");
    }

    private void UpDownMove()
    {
        // プロペラが加速していない＝＝上昇も下降もしていない
        if (!spinnerFlag) return;
        isMax = maxUpDownSpeed > Mathf.Abs(upDownSpeed);
       // Debug.Log($"isMax{isMax} = maxUpDownSpeed{maxUpDownSpeed}　>　upDownSpeed：{upDownSpeed}");

        if (upFlag)
        {
			if (upDownSpeed < 0)
			{
                upDownSpeed = 0;
            }
            if (isMax)
            {
                upDownSpeed += (0.1f / (maxUpDownSpeed - upDownSpeed));
                moveVector3.y += upDownSpeed;// = upDownSpeed + 0.1f;
              //  Debug.Log("上昇中！");
            }
            else
            {
                moveVector3.y = maxUpDownSpeed;
              //  Debug.Log("上昇中制御中！");
            }
        }
        else
        {
            if (upDownSpeed > 0)
            {
                upDownSpeed -= Time.deltaTime ;
            }
            if (isMax)
            {
                upDownSpeed -= (0.1f / (maxUpDownSpeed - upDownSpeed));
                moveVector3.y += upDownSpeed;
              //  Debug.Log("下降中！");
            }
            else
            {
                moveVector3.y -= maxUpDownSpeed;
              // Debug.Log("下降中制御中！");
            }
        }
    }

    #region 入力受け取り関数

    public void OnMove(InputAction.CallbackContext context)
    {// 移動スティック
        if (context.performed)
        {
            friezeFlag = false;
            //inputMove = context.ReadValue<Vector2>();
        }
        else if (context.canceled)
        {
            friezeFlag = true;
        }
        inputMove = context.ReadValue<Vector2>();
        //inputMoveは inputMove.xに数値が入っている inputMove.yに数値が入っている 
        //Debug.Log($"inputMove{inputMove}");
    }

    public void OnUpMove(InputAction.CallbackContext context)
    {// 上昇ボタンが押された時
        if (context.performed)// 押されてる
        {
            spinnerFlag = true;
            upFlag = true;
        }
        else if (context.canceled)// 離された
        {
            spinnerFlag = false;
            upFlag = false;
        }
    }

    public void OnDownMove(InputAction.CallbackContext context)
    {// 下降ボタンが押されたとき
        if (context.performed)// 押されてる
        {
            spinnerFlag = true;
        }
        else if (context.canceled)// 離された
        {
            spinnerFlag = false;
        }
    }
    public void OnHoldMove(InputAction.CallbackContext context)
    {// 掴みボタンが押された時
       
        if (context.performed)// 押されてる
        {
            if (isCatch)
            {
                holdFlag = true;
                holdObj_rigidbody = holdObj.GetComponent<Rigidbody>();
            }
        }
        else if (context.canceled)// 離された
        {
            holdFlag = false;
        }
    }

	private void OnTriggerEnter(Collider other)
	{
        OnTouch = true;

        // 衝突したオブジェクトを取得
        GameObject collidedObject = other.gameObject;

        // 特定のオブジェクトであるか判定
        if (collidedObject.CompareTag(targetTag))
        {
            isCatch = true;
            holdObj = collidedObject;

        }
        else
        {
            //collidedObject.CompareTag()
            isCatch = false;
        }
    }

	private void OnTriggerStay(Collider other)
	{
        // 衝突したオブジェクトを取得
        GameObject collidedObject = other.gameObject;
        for (int i = 0; i < groundTags.Length; ++i)
        {
            if (collidedObject.CompareTag(groundTags[i]))
            {
                isGround = true;
                break;
            }
        }
    }

	private void OnTriggerExit(Collider other)
    {
        OnTouch = false;

        // 離れた
        isCatch = false;
    }

    #endregion
}
