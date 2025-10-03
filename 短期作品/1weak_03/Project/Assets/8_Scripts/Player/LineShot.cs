using UnityEngine;
using UnityEngine.InputSystem;

public class LineShot : MonoBehaviour
{
    [SerializeField] private Transform shotPos;
    [SerializeField] private LineRenderer lineRenderer; // LineRendererコンポーネント
    [SerializeField] private LayerMask ignoreLayer; // 無視するレイヤー（プレイヤー）
    public float maxDistance = 20f; // 糸の最大距離
    public float pullSpeed = 5f; // 引っ張る速度
    private Vector3 targetPoint; // 糸の終点
    private bool isWebActive = false; // 糸がアクティブかどうか
    private bool isLine = false; // ボタンが押されているかどうか

    public bool isButtonPressed = false; // ボタンが押されているかどうか
    [SerializeField] private TherdCam Cam;
    [SerializeField] private Animator animator;

    void Start()
    {
        // LineRendererの初期設定
        lineRenderer.positionCount = 2; // 始点と終点の2つの位置
        lineRenderer.enabled = false; // 初期は非表示
    }

    void Update()
    {
        animator.SetBool("PullMotion", isButtonPressed);

        // ボタンが押されている場合は、糸の状態をチェック
        if (isButtonPressed)
        {
            FireWeb();
        }

        // 糸がアクティブな場合は、引っ張る処理を実行
        if (isWebActive)
        {
            PullToTarget();
        }

        // 糸を解除する処理
        if (!isButtonPressed && isWebActive)
        {
            ReleaseWeb();
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
            if (Physics.Raycast(ray, out hit, maxDistance, ~ignoreLayer)) // プレイヤーを無視
            {
                targetPoint = hit.point; // ヒットしたポイントを終点に設定
                lineRenderer.SetPosition(0, startPoint);
                lineRenderer.SetPosition(1, targetPoint);
                lineRenderer.enabled = true; // 糸を表示
                isWebActive = true; // 糸がアクティブに
            }
        }
    }

    void PullToTarget()
    {
        // プレイヤーを糸の終点に向かって移動
        transform.position = Vector3.Lerp(transform.position, targetPoint, Time.deltaTime * pullSpeed);

        // 糸の位置を更新
        lineRenderer.SetPosition(0, transform.position);
    }

    void ReleaseWeb()
    {
        lineRenderer.enabled = false; // 糸を非表示に
        isWebActive = false; // 糸を解除
    }

    public void OnLine(InputAction.CallbackContext context)
    {
        Debug.Log("OnLine押してるよ");
        if (context.performed)
        {
            animator.SetTrigger("pull");
            isButtonPressed = true; // ボタンが押された
        }
        else if (context.canceled)
        {
            isButtonPressed = false; // ボタンが離された
            ReleaseWeb(); // 糸を解除
        }
    }
}
