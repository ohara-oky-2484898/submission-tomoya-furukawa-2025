using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class SimpleJump : MonoBehaviour
{
    [Header("ジャンプ力")]
    [SerializeField] private float jumpForce = 5f; // ジャンプ力

    [Header("接地判定")]
    [SerializeField] private LayerMask groundLayer; // 地面レイヤー
    [SerializeField] private Vector3 groundCheckOffset = new Vector3(0, -0.9f, 0); // 接地判定の中心点のオフセット(キャラクターの足元に調整)
    [SerializeField] private float groundCheckRadius = 0.3f; // 接地判定の球の半径

    private Rigidbody rb;
    private bool isGrounded; // 接地フラグ

    // Gizmo表示用 (任意)
    private Vector3 groundCheckPosition;

    void Awake()
    {
        // コンポーネントを取得
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // --- 1. 接地判定 ---
        // 接地判定の中心点を計算 (キャラクターの位置 + オフセット)
        groundCheckPosition = transform.position + groundCheckOffset;

        // groundCheckPosition を中心とする半径 groundCheckRadius の球内に、
        // groundLayer に属するColliderが存在するかどうかを判定
        isGrounded = Physics.CheckSphere(groundCheckPosition, groundCheckRadius, groundLayer);

        // --- 2. ジャンプ入力検知と処理 ---
        // スペースキーが押された瞬間、かつ接地している場合
        if (Keyboard.current.spaceKey.wasPressedThisFrame && isGrounded)
        {
            // Y軸方向への速度をリセットしてから力を加える（より安定したジャンプのため、任意）
            // rb.linearVelocity = new Vector3(rb.linearVelocityX, 0, rb.linearVelocityZ);

            // Rigidbodyに上向きの力を瞬間的に加える (ForceMode.Impulse)
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }


        // 可変ジャンプ
        // --- (既存の接地判定、ジャンプ入力処理) ---

        // ジャンプボタンが離された瞬間、かつ上昇中の場合 (可変ジャンプ)
        if (Keyboard.current.spaceKey.wasReleasedThisFrame && rb.linearVelocity.y > 0)
        {
            // 上昇速度を減衰させてジャンプの高さを低くする
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.z * 0.5f);
            Debug.Log("ジャンプ途中でボタンが離されました");
        }
    }

    // (オプション) Gizmoを使って接地判定の範囲を視覚化
    void OnDrawGizmosSelected()
    {
        Gizmos.color = isGrounded ? Color.green : Color.red; // 接地してたら緑、してなければ赤
        // 接地判定の中心点を計算 (Updateと同じロジック)
        Vector3 checkPosition = transform.position + groundCheckOffset;
        Gizmos.DrawWireSphere(checkPosition, groundCheckRadius);
    }
}
