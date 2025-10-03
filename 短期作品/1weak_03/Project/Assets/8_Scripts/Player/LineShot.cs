using UnityEngine;
using UnityEngine.InputSystem;

public class LineShot : MonoBehaviour
{
    [SerializeField] private Transform shotPos;
    [SerializeField] private LineRenderer lineRenderer; // LineRenderer�R���|�[�l���g
    [SerializeField] private LayerMask ignoreLayer; // �������郌�C���[�i�v���C���[�j
    public float maxDistance = 20f; // ���̍ő勗��
    public float pullSpeed = 5f; // �������鑬�x
    private Vector3 targetPoint; // ���̏I�_
    private bool isWebActive = false; // �����A�N�e�B�u���ǂ���
    private bool isLine = false; // �{�^����������Ă��邩�ǂ���

    public bool isButtonPressed = false; // �{�^����������Ă��邩�ǂ���
    [SerializeField] private TherdCam Cam;
    [SerializeField] private Animator animator;

    void Start()
    {
        // LineRenderer�̏����ݒ�
        lineRenderer.positionCount = 2; // �n�_�ƏI�_��2�̈ʒu
        lineRenderer.enabled = false; // �����͔�\��
    }

    void Update()
    {
        animator.SetBool("PullMotion", isButtonPressed);

        // �{�^����������Ă���ꍇ�́A���̏�Ԃ��`�F�b�N
        if (isButtonPressed)
        {
            FireWeb();
        }

        // �����A�N�e�B�u�ȏꍇ�́A�������鏈�������s
        if (isWebActive)
        {
            PullToTarget();
        }

        // �����������鏈��
        if (!isButtonPressed && isWebActive)
        {
            ReleaseWeb();
        }
    }

    void FireWeb()
    {
        // �{�^���������Ă���Ԃ̓��C�L���X�g���Ȃ�
        if (!isWebActive)
        {
            Vector3 startPoint = shotPos.position;  // �v���C���[�̈ʒu���n�_�ɐݒ�
            Vector3 shotDir = Cam.cam.forward;  // ��΂�������player�̎��_�̕����ɐݒ�

            Ray ray = new Ray(startPoint, shotDir);
            RaycastHit hit;

            // ���C�L���X�g�����s
            if (Physics.Raycast(ray, out hit, maxDistance, ~ignoreLayer)) // �v���C���[�𖳎�
            {
                targetPoint = hit.point; // �q�b�g�����|�C���g���I�_�ɐݒ�
                lineRenderer.SetPosition(0, startPoint);
                lineRenderer.SetPosition(1, targetPoint);
                lineRenderer.enabled = true; // ����\��
                isWebActive = true; // �����A�N�e�B�u��
            }
        }
    }

    void PullToTarget()
    {
        // �v���C���[�����̏I�_�Ɍ������Ĉړ�
        transform.position = Vector3.Lerp(transform.position, targetPoint, Time.deltaTime * pullSpeed);

        // ���̈ʒu���X�V
        lineRenderer.SetPosition(0, transform.position);
    }

    void ReleaseWeb()
    {
        lineRenderer.enabled = false; // �����\����
        isWebActive = false; // ��������
    }

    public void OnLine(InputAction.CallbackContext context)
    {
        Debug.Log("OnLine�����Ă��");
        if (context.performed)
        {
            animator.SetTrigger("pull");
            isButtonPressed = true; // �{�^���������ꂽ
        }
        else if (context.canceled)
        {
            isButtonPressed = false; // �{�^���������ꂽ
            ReleaseWeb(); // ��������
        }
    }
}
