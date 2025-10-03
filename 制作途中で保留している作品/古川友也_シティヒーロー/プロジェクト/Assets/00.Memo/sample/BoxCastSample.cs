using UnityEngine;
using UnityEngine.InputSystem;

public class BoxCastSample : MonoBehaviour
{
    [Header("�W�����v��")]
    [SerializeField] private float jumpForce = 5f;

    [Header("�ݒu����")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundCheckDistance = 0.1f;

    [Tooltip("����Box�̃T�C�Y(�����̒���)")]
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

        // boxOrigin��Collider�̒�Ӓ������班�����ɐݒ�
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
//    [Header("�W�����v��")]
//    [SerializeField] private float jumpForce = 5f;

//    [Header("�ݒu����")]
//    [SerializeField] private LayerMask groundLayer; // �n�ʂ̃��C���[
//    [SerializeField] private float groundCheckDistance = 0.1f; // �n�ʔ��苗��

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
//        // BoxCollider�̃o�E���f�B���O�{�b�N�X�擾
//        Bounds bounds = boxCollider.bounds;

//        // BoxCast�̃T�C�Y�iX,Z������Collider�ɍ��킹�āAY�͔����j
//        boxHalfExtents = new Vector3(bounds.size.x * 0.45f, 0.05f, bounds.size.z * 0.45f);

//        // BoxCast�̔��ˈʒu�iCollider�̒�Ӓ������班�����ɂ��炷�j
//        boxOrigin = new Vector3(bounds.center.x, bounds.min.y - boxHalfExtents.y, bounds.center.z);

//        RaycastHit hit;
//        // BoxCast�̎��s�BQuaternion.identity�͉�]�Ȃ�
//        isGrounded = Physics.BoxCast(boxOrigin, boxHalfExtents, Vector3.down, out hit, Quaternion.identity, groundCheckDistance, groundLayer);

//        if (isGrounded && Input.GetButtonDown("Jumping"))
//        {
//            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
//        }

//        // �f�o�b�O�p��BoxCast�͈̔͂�Ray��\��
//        Debug.DrawRay(boxOrigin, Vector3.down * groundCheckDistance, isGrounded ? Color.green : Color.red);
//    }

//    // �ڒn����͈͂�Gizmo�ŉ���
//    void OnDrawGizmosSelected()
//    {
//        if (boxCollider == null) return;

//        Gizmos.color = isGrounded ? Color.green : Color.red;

//        // Box�͈̔͂�\��
//        Gizmos.DrawWireCube(boxOrigin, boxHalfExtents * 2f);

//        // ��������Ray��\��
//        Gizmos.DrawRay(boxOrigin, Vector3.down * groundCheckDistance);
//    }
//}
