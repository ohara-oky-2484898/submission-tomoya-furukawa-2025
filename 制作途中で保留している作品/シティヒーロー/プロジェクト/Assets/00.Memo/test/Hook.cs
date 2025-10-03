using UnityEngine;

public class Hook : MonoBehaviour
{
    public float speed = 40f;
    private Rigidbody rb;
    private bool hooked = false;
    private SpringJoint springJoint;  // �ڑ��̂��߂ɕێ�

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

        // �e�I�u�W�F�N�g�i�v���C���[�j�ɐڑ���ʒm
        OnHooked?.Invoke(gameObject);

        // �t�b�N�ʒu�ŌŒ�
        transform.position = collision.contacts[0].point;
    }
}
