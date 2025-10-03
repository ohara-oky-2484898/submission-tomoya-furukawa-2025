using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class kiliniti3 : MonoBehaviour
{
    [SerializeField] private Transform shotPos;
    [SerializeField] private LineRenderer lineRenderer; // LineRendererコンポーネント
    [SerializeField] private LayerMask ignoreLayer; // 無視するレイヤー（プレイヤー）
    public float maxLineDistance = 20f; // 糸の最大距離
    public float launchForce = 10f; // 飛ぶときの力
    private Vector3 targetPoint; // 糸の終点
    private bool isWebActive = false; // 糸がアクティブかどうか
  //  private bool isLine = false; // ボタンが押されているかどう
    [SerializeField] private Rigidbody rb; // Rigidbodyコンポーネント

    public bool isButtonPressed = false; // ボタンが押されているかどうか
    [SerializeField] private TherdCam Cam;
    public bool isMove = false;
    public bool isNotControl = false;

    [SerializeField] private Animator animator;


    [SerializeField] private Text actionText;// 標準カーソル用
    void Start()
    {
        lineRenderer.positionCount = 2; // 始点と終点の2つの位置
        lineRenderer.enabled = false; // 初期は非表示
    }

    void Update()
    {
        animator.SetBool("action", isMove);
        animator.SetBool("NotControl", isNotControl);
        //animator.SetFloat("MotionUp", rb.velocity.y);
        // ボタンが押されている場合は、糸の状態をチェック
        //if (isButtonPressed)
        //{
        //    FireWeb();
        //}

        if (isButtonPressed)
        {
            actionText.gameObject.SetActive(true); // ボタンが押されている間はテキストを表示
         //   Debug.Log("とめてる");
        }
        else
        {
            actionText.gameObject.SetActive(false); // ボタンが離されたときはテキストを非表示
        }

        // ボタンが離されたときに糸を設定
        if (!isButtonPressed && isWebActive && isMove)
        {
            ReleaseWeb();
        }

        if (isNotControl && rb.velocity.y < 0)
        {
            isNotControl = false; // 高度が下がり始めたら移動フラグをfalseにする
            Debug.Log("高度が下がり始めました。移動を停止します。");
        }
    }

    void FireWeb()
    {
        // ボタンを押している間はレイキャストしない
        if (!isWebActive)
        {
            Vector3 startPoint = shotPos.position;  // プレイヤーの位置を始点に設定
            Vector3 shotDir = Cam.cam.forward;  // 飛ばす方向をplayerの視点の方向に設定

            Ray ray = new Ray(startPoint, shotDir);
            RaycastHit hit;

            // レイキャストを実行
            if (Physics.Raycast(ray, out hit, maxLineDistance, ~ignoreLayer)) // プレイヤーを無視
            {
                targetPoint = hit.point; // ヒットしたポイントを終点に設定
                lineRenderer.SetPosition(0, startPoint);
                lineRenderer.SetPosition(1, targetPoint);
                lineRenderer.enabled = true; // 糸を表示
                isWebActive = true; // 糸がアクティブに
            }
        }
    }

    void MoveToTarget()
    {
        // ここでは特に何もしない、Rigidbodyで飛ぶ制御をするため
    }

    void ReleaseWeb()
    {
        Vector3 startPoint = shotPos.position; // プレイヤーの位置を始点に設定
        Vector3 shotDir = Cam.cam.forward; // ボタンを離したときのカメラの正面方向を取得

        // カメラ方向に10の長さで伸ばす
        targetPoint = startPoint + shotDir * 10f;
        lineRenderer.SetPosition(0, startPoint);
        lineRenderer.SetPosition(1, targetPoint);
        lineRenderer.enabled = true; // 糸を表示

        // 飛ぶための力を加える
        Vector3 launchDirection = shotDir.normalized;

        // インパルスとして力を加える
        rb.AddForce(launchDirection * launchForce, ForceMode.Impulse); // Impulseで力を加える
        isNotControl = true;
        isWebActive = true; // 糸がアクティブに

        // コルーチンを開始して1秒後にLineRendererを非表示にする
        StartCoroutine(HideLineAfterDelay(1f));
        isMove = false;
    }



    private IEnumerator HideLineAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // 指定した時間待つ
        lineRenderer.enabled = false; // 糸を非表示に
        isWebActive = false; // 糸を解除
    }

    public void OnLine(InputAction.CallbackContext context)
    {
       // Debug.Log("OnLine押してるよ");
        if (context.performed)
        {
            isButtonPressed = true; // ボタンが押された
            isMove = true;
        }
        else if (context.canceled)
        {
            isButtonPressed = false; // ボタンが離された
            ReleaseWeb(); // 糸を解除
            animator.Play("action", -1, 0f); // アニメーションを最初から再生
        }
    }
}
