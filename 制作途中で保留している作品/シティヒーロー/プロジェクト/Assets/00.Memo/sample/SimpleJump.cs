using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class SimpleJump : MonoBehaviour
{
    [Header("�W�����v��")]
    [SerializeField] private float jumpForce = 5f; // �W�����v��

    [Header("�ڒn����")]
    [SerializeField] private LayerMask groundLayer; // �n�ʃ��C���[
    [SerializeField] private Vector3 groundCheckOffset = new Vector3(0, -0.9f, 0); // �ڒn����̒��S�_�̃I�t�Z�b�g(�L�����N�^�[�̑����ɒ���)
    [SerializeField] private float groundCheckRadius = 0.3f; // �ڒn����̋��̔��a

    private Rigidbody rb;
    private bool isGrounded; // �ڒn�t���O

    // Gizmo�\���p (�C��)
    private Vector3 groundCheckPosition;

    void Awake()
    {
        // �R���|�[�l���g���擾
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // --- 1. �ڒn���� ---
        // �ڒn����̒��S�_���v�Z (�L�����N�^�[�̈ʒu + �I�t�Z�b�g)
        groundCheckPosition = transform.position + groundCheckOffset;

        // groundCheckPosition �𒆐S�Ƃ��锼�a groundCheckRadius �̋����ɁA
        // groundLayer �ɑ�����Collider�����݂��邩�ǂ����𔻒�
        isGrounded = Physics.CheckSphere(groundCheckPosition, groundCheckRadius, groundLayer);

        // --- 2. �W�����v���͌��m�Ə��� ---
        // �X�y�[�X�L�[�������ꂽ�u�ԁA���ڒn���Ă���ꍇ
        if (Keyboard.current.spaceKey.wasPressedThisFrame && isGrounded)
        {
            // Y�������ւ̑��x�����Z�b�g���Ă���͂�������i�����肵���W�����v�̂��߁A�C�Ӂj
            // rb.linearVelocity = new Vector3(rb.linearVelocityX, 0, rb.linearVelocityZ);

            // Rigidbody�ɏ�����̗͂��u�ԓI�ɉ����� (ForceMode.Impulse)
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }


        // �σW�����v
        // --- (�����̐ڒn����A�W�����v���͏���) ---

        // �W�����v�{�^���������ꂽ�u�ԁA���㏸���̏ꍇ (�σW�����v)
        if (Keyboard.current.spaceKey.wasReleasedThisFrame && rb.linearVelocity.y > 0)
        {
            // �㏸���x�����������ăW�����v�̍�����Ⴍ����
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.z * 0.5f);
            Debug.Log("�W�����v�r���Ń{�^����������܂���");
        }
    }

    // (�I�v�V����) Gizmo���g���Đڒn����͈̔͂����o��
    void OnDrawGizmosSelected()
    {
        Gizmos.color = isGrounded ? Color.green : Color.red; // �ڒn���Ă���΁A���ĂȂ���ΐ�
        // �ڒn����̒��S�_���v�Z (Update�Ɠ������W�b�N)
        Vector3 checkPosition = transform.position + groundCheckOffset;
        Gizmos.DrawWireSphere(checkPosition, groundCheckRadius);
    }
}
