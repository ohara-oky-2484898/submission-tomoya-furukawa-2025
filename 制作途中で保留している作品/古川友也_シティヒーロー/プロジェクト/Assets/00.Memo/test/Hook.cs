using UnityEngine;

public class Hook : MonoBehaviour
{
    public float speed = 40f;
    private Rigidbody rb;
    private bool hooked = false;
    private SpringJoint springJoint;  // 接続のために保持

    public delegate void HookedDelegate(GameObject hookedObject);
    public event HookedDelegate OnHooked;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.linearVelocity = transform.forward * speed;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (hooked) return;

        hooked = true;

        rb.linearVelocity = Vector3.zero;
        rb.isKinematic = true;

        // 親オブジェクト（プレイヤー）に接続を通知
        OnHooked?.Invoke(gameObject);

        // フック位置で固定
        transform.position = collision.contacts[0].point;
    }
}
