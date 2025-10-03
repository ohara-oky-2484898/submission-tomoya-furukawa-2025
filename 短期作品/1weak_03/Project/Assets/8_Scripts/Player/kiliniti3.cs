using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class kiliniti3 : MonoBehaviour
{
    [SerializeField] private Transform shotPos;
    [SerializeField] private LineRenderer lineRenderer; // LineRenderer�R���|�[�l���g
    [SerializeField] private LayerMask ignoreLayer; // �������郌�C���[�i�v���C���[�j
    public float maxLineDistance = 20f; // ���̍ő勗��
    public float launchForce = 10f; // ��ԂƂ��̗�
    private Vector3 targetPoint; // ���̏I�_
    private bool isWebActive = false; // �����A�N�e�B�u���ǂ���
  //  private bool isLine = false; // �{�^����������Ă��邩�ǂ�
    [SerializeField] private Rigidbody rb; // Rigidbody�R���|�[�l���g

    public bool isButtonPressed = false; // �{�^����������Ă��邩�ǂ���
    [SerializeField] private TherdCam Cam;
    public bool isMove = false;
    public bool isNotControl = false;

    [SerializeField] private Animator animator;


    [SerializeField] private Text actionText;// �W���J�[�\���p
    void Start()
    {
        lineRenderer.positionCount = 2; // �n�_�ƏI�_��2�̈ʒu
        lineRenderer.enabled = false; // �����͔�\��
    }

    void Update()
    {
        animator.SetBool("action", isMove);
        animator.SetBool("NotControl", isNotControl);
        //animator.SetFloat("MotionUp", rb.velocity.y);
        // �{�^����������Ă���ꍇ�́A���̏�Ԃ��`�F�b�N
        //if (isButtonPressed)
        //{
        //    FireWeb();
        //}

        if (isButtonPressed)
        {
            actionText.gameObject.SetActive(true); // �{�^����������Ă���Ԃ̓e�L�X�g��\��
         //   Debug.Log("�Ƃ߂Ă�");
        }
        else
        {
            actionText.gameObject.SetActive(false); // �{�^���������ꂽ�Ƃ��̓e�L�X�g���\��
        }

        // �{�^���������ꂽ�Ƃ��Ɏ���ݒ�
        if (!isButtonPressed && isWebActive && isMove)
        {
            ReleaseWeb();
        }

        if (isNotControl && rb.velocity.y < 0)
        {
            isNotControl = false; // ���x��������n�߂���ړ��t���O��false�ɂ���
            Debug.Log("���x��������n�߂܂����B�ړ����~���܂��B");
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
            if (Physics.Raycast(ray, out hit, maxLineDistance, ~ignoreLayer)) // �v���C���[�𖳎�
            {
                targetPoint = hit.point; // �q�b�g�����|�C���g���I�_�ɐݒ�
                lineRenderer.SetPosition(0, startPoint);
                lineRenderer.SetPosition(1, targetPoint);
                lineRenderer.enabled = true; // ����\��
                isWebActive = true; // �����A�N�e�B�u��
            }
        }
    }

    void MoveToTarget()
    {
        // �����ł͓��ɉ������Ȃ��ARigidbody�Ŕ�Ԑ�������邽��
    }

    void ReleaseWeb()
    {
        Vector3 startPoint = shotPos.position; // �v���C���[�̈ʒu���n�_�ɐݒ�
        Vector3 shotDir = Cam.cam.forward; // �{�^���𗣂����Ƃ��̃J�����̐��ʕ������擾

        // �J����������10�̒����ŐL�΂�
        targetPoint = startPoint + shotDir * 10f;
        lineRenderer.SetPosition(0, startPoint);
        lineRenderer.SetPosition(1, targetPoint);
        lineRenderer.enabled = true; // ����\��

        // ��Ԃ��߂̗͂�������
        Vector3 launchDirection = shotDir.normalized;

        // �C���p���X�Ƃ��ė͂�������
        rb.AddForce(launchDirection * launchForce, ForceMode.Impulse); // Impulse�ŗ͂�������
        isNotControl = true;
        isWebActive = true; // �����A�N�e�B�u��

        // �R���[�`�����J�n����1�b���LineRenderer���\���ɂ���
        StartCoroutine(HideLineAfterDelay(1f));
        isMove = false;
    }



    private IEnumerator HideLineAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // �w�肵�����ԑ҂�
        lineRenderer.enabled = false; // �����\����
        isWebActive = false; // ��������
    }

    public void OnLine(InputAction.CallbackContext context)
    {
       // Debug.Log("OnLine�����Ă��");
        if (context.performed)
        {
            isButtonPressed = true; // �{�^���������ꂽ
            isMove = true;
        }
        else if (context.canceled)
        {
            isButtonPressed = false; // �{�^���������ꂽ
            ReleaseWeb(); // ��������
            animator.Play("action", -1, 0f); // �A�j���[�V�������ŏ�����Đ�
        }
    }
}
