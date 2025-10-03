using UnityEngine;
using UnityEngine.InputSystem;

public class BoxCastSample : MonoBehaviour
{
    [Header("ジャンプ力")]
    [SerializeField] private float jumpForce = 5f;

    [Header("設置判定")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundCheckDistance = 0.1f;

    [Tooltip("判定Boxのサイズ(半分の長さ)")]
    [SerializeField] private Vector3 boxHalfExtents = new Vector3(0.5f, 0.05f, 0.5f);

    private Rigidbody rb;
    private BoxCollider boxCollider;
    private bool isGrounded;

    private Vector3 boxOrigin;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
    }

    void Update()
    {
        Bounds bounds = boxCollider.bounds;

        // boxOriginをColliderの底辺中央から少し下に設定
        boxOrigin = new Vector3(bounds.center.x, bounds.min.y - boxHalfExtents.y, bounds.center.z);

        RaycastHit hit;
        isGrounded = Physics.BoxCast(boxOrigin, boxHalfExtents, Vector3.down, out hit, Quaternion.identity, groundCheckDistance, groundLayer);

        if (isGrounded && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        Debug.DrawRay(boxOrigin, Vector3.down * groundCheckDistance, isGrounded ? Color.green : Color.red);
    }

    void OnDrawGizmosSelected()
    {
        if (boxCollider == null) return;

        Gizmos.color = isGrounded ? Color.green : Color.red;
        Gizmos.DrawWireCube(boxOrigin, boxHalfExtents * 2f);
        Gizmos.DrawRay(boxOrigin, Vector3.down * groundCheckDistance);
    }
}


//using UnityEngine;

//public class BoxCastSample : MonoBehaviour
//{
//    [Header("ジャンプ力")]
//    [SerializeField] private float jumpForce = 5f;

//    [Header("設置判定")]
//    [SerializeField] private LayerMask groundLayer; // 地面のレイヤー
//    [SerializeField] private float groundCheckDistance = 0.1f; // 地面判定距離

//    private Rigidbody rb;
//    private BoxCollider boxCollider;
//    private bool isGrounded;

//    private Vector3 boxOrigin;
//    private Vector3 boxHalfExtents;

//    void Start()
//    {
//        rb = GetComponent<Rigidbody>();
//        boxCollider = GetComponent<BoxCollider>();
//    }

//    void Update()
//    {
//        // BoxColliderのバウンディングボックス取得
//        Bounds bounds = boxCollider.bounds;

//        // BoxCastのサイズ（X,Z方向はColliderに合わせて、Yは薄く）
//        boxHalfExtents = new Vector3(bounds.size.x * 0.45f, 0.05f, bounds.size.z * 0.45f);

//        // BoxCastの発射位置（Colliderの底辺中央から少し下にずらす）
//        boxOrigin = new Vector3(bounds.center.x, bounds.min.y - boxHalfExtents.y, bounds.center.z);

//        RaycastHit hit;
//        // BoxCastの実行。Quaternion.identityは回転なし
//        isGrounded = Physics.BoxCast(boxOrigin, boxHalfExtents, Vector3.down, out hit, Quaternion.identity, groundCheckDistance, groundLayer);

//        if (isGrounded && Input.GetButtonDown("Jumping"))
//        {
//            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
//        }

//        // デバッグ用にBoxCastの範囲とRayを表示
//        Debug.DrawRay(boxOrigin, Vector3.down * groundCheckDistance, isGrounded ? Color.green : Color.red);
//    }

//    // 接地判定範囲をGizmoで可視化
//    void OnDrawGizmosSelected()
//    {
//        if (boxCollider == null) return;

//        Gizmos.color = isGrounded ? Color.green : Color.red;

//        // Boxの範囲を表示
//        Gizmos.DrawWireCube(boxOrigin, boxHalfExtents * 2f);

//        // 下方向のRayを表示
//        Gizmos.DrawRay(boxOrigin, Vector3.down * groundCheckDistance);
//    }
//}
