using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // ����
    private Vector2 inputMove_L; // ���v���C���[�̈ړ�����
    private Vector2 inputMove_R; // �E�v���C���[�̈ړ�����

    // �v���C���[�ƃ}�b�g�̃I�u�W�F�N�g
    private GameObject playerObj_L; // ���v���C���[�I�u�W�F�N�g
    private GameObject playerObj_R; // �E�v���C���[�I�u�W�F�N�g
    private GameObject matteObj;     // �}�b�g�I�u�W�F�N�g

    // Rigidbody
    private Rigidbody rbody_L; // ���v���C���[��Rigidbody
    private Rigidbody rbody_R; // �E�v���C���[��Rigidbody

    // �ړ��E��]�p�x�N�g��
    private Vector3 player_L_dir; // ���v���C���[�̈ړ�����
    private Vector3 player_R_dir; // �E�v���C���[�̈ړ�����

    // �ݒ�l
    [SerializeField] private float moveSpeed = 5.0f; // �ʏ�ړ����x
    [SerializeField] private GameObject camera; // ���C���J����
    [SerializeField] private float notMoveTime = 2.5f; // �ꎞ��~����
    [SerializeField] public float dashSpeed; // �_�b�V�����x���Z
    [SerializeField] private Texture2D arabesqueTexture; // �}�b�g�ɓ\��e�N�X�`��
    [SerializeField] private Renderer matteRenderer; // �}�b�g�̃����_���[
    [SerializeField] private Animator animator_L; // ���v���C���[�̃A�j���[�^�[
    [SerializeField] private Animator animator_R; // �E�v���C���[�̃A�j���[�^�[
    [SerializeField] private StrCount str; // �Q�[���J�n����p�X�N���v�g

    // �ʐρE�ړ�����p
    private float matteY; // �}�b�g��Y�X�P�[���ێ�
    private float maxArea; // �ő勖�e�ʐ�
    private float tabooArea = 100f; // �ʐϐ����l�i�Q�[�����x���ŕω��j
    private float currentArea; // ���݂̖ʐ�
    private bool isAreaMax; // �ʐϐ����t���O
    private bool notMove = false; // �ꎞ��~�t���O
    private Vector3 matteStartPos; // �}�b�g�̏����ʒu

    // ��ԕϐ�
    public float vertical; // �c��
    public float width; // ����
    public float pushPower = 20f; // �����񂹗�
    public float additionSpeed_L; // ���v���C���[�̉��Z���x
    public float additionSpeed_R; // �E�v���C���[�̉��Z���x

    // �ȗ����p
    private Vector3 Lpos; // ���v���C���[�ʒu
    private Vector3 Rpos; // �E�v���C���[�ʒu
    private Material matteMaterial; // �}�b�g�̃}�e���A���C���X�^���X

    void Start()
    {
        // �v���C���[�I�u�W�F�N�g�ƃ}�b�g�I�u�W�F�N�g�̎擾
        playerObj_L = transform.GetChild(0).gameObject;
        playerObj_R = transform.GetChild(1).gameObject;
        matteObj = transform.GetChild(2).gameObject;

        // �}�b�g�̃X�P�[���E�ʒu������
        matteY = matteObj.transform.localScale.y;
        matteStartPos = matteObj.transform.position;

        // Rigidbody�擾
        rbody_L = playerObj_L.GetComponent<Rigidbody>();
        rbody_R = playerObj_R.GetComponent<Rigidbody>();

        // �}�e���A���C���X�^���X�쐬
        if (matteRenderer != null)
        {
            matteMaterial = new Material(matteRenderer.material);
            matteRenderer.material = matteMaterial;
        }

        // �Q�[�����x���ɂ�鐧��������
        tabooArea /= (int)GameManager.Instance.gamelevel;
        maxArea = tabooArea * 1.1f;
        GameManager.Instance.maxAreaSize = maxArea;
    }

    void Update()
    {
        // �A�j���[�V�����X�V
        animator_L.SetFloat("walk", inputMove_L.magnitude, 0.1f, Time.deltaTime);
        animator_R.SetFloat("walk2", inputMove_R.magnitude, 0.1f, Time.deltaTime);
        animator_L.SetBool("NotMove", notMove);
        animator_R.SetBool("NotMove", notMove);
    }

    void FixedUpdate()
    {
        if (!str.strFlag) return; // �J�E���g�_�E�����I���܂ő���֎~

        // �e�v���C���[�̌��݈ʒu�擾
        Lpos = playerObj_L.transform.position;
        Rpos = playerObj_R.transform.position;
        Lpos.y += matteStartPos.y;

        // �}�b�g�̍X�V
        MatteTransform();
        GameManager.Instance.nowAreaSize = currentArea;
        ChangeMatteTextureColor(currentArea);
        CheckAreaLimit();

        // �v���C���[����
        if (!notMove)
        {
            PlayerMove();
        }
        PlayerRotation();
    }

    #region ���͊֐�
    public void OnLeftPlayerMove(InputAction.CallbackContext context)
    {
        inputMove_L = (context.performed && !isAreaMax) ? context.ReadValue<Vector2>() : Vector2.zero;
    }

    public void OnRightPlayerMove(InputAction.CallbackContext context)
    {
        inputMove_R = (context.performed && !isAreaMax) ? context.ReadValue<Vector2>() : Vector2.zero;
    }

    public void OnLeftPlayerDash(InputAction.CallbackContext context)
    {
        if (context.performed) additionSpeed_L += dashSpeed;
        else if (context.canceled) additionSpeed_L -= dashSpeed;
    }

    public void OnRightPlayerDash(InputAction.CallbackContext context)
    {
        if (context.performed) additionSpeed_R += dashSpeed;
        else if (context.canceled) additionSpeed_R -= dashSpeed;
    }
    #endregion

    #region �v���C���[����
    private void PlayerMove()
    {
        // �J������������Ɉړ��x�N�g���쐬
        Vector3 camForward = Vector3.Scale(camera.transform.forward, new Vector3(1, 0, 1)).normalized;
        player_L_dir = camForward * inputMove_L.y + camera.transform.right * inputMove_L.x;
        player_R_dir = camForward * inputMove_R.y + camera.transform.right * inputMove_R.x;

        // �ړ����x�K�p
        rbody_L.velocity = player_L_dir * (moveSpeed + additionSpeed_L);
        rbody_R.velocity = player_R_dir * (moveSpeed + additionSpeed_R);
    }

    private void PlayerRotation()
    {
        // �e�v���C���[�𑊎�v���C���[�Ɍ�����
        Vector3 dir_L = (Rpos - Lpos).normalized;
        Vector3 dir_R = (Lpos - Rpos).normalized;
        dir_L.y = 0;
        dir_R.y = 0;

        Quaternion rot_L = Quaternion.LookRotation(dir_L, Vector3.up);
        Quaternion rot_R = Quaternion.LookRotation(dir_R, Vector3.up);
        playerObj_L.transform.rotation = rot_L;
        playerObj_R.transform.rotation = rot_R;
    }
    #endregion

    #region Matte����
    private void MatteTransform()
    {
        // ���S�ʒu�ɔz�u
        Vector3 centerPos = (Lpos + Rpos) / 2f;
        matteObj.transform.position = centerPos;

        // �X�P�[�����O
        float padding = 1.5f;
        float scaleX = Mathf.Abs(Lpos.x - Rpos.x) + padding * Mathf.Sign(Rpos.x - Lpos.x);
        float scaleZ = Mathf.Abs(Lpos.z - Rpos.z) + padding * Mathf.Sign(Lpos.z - Rpos.z);
        matteObj.transform.localScale = new Vector3(scaleX, matteY, scaleZ);
        vertical = Mathf.Abs(scaleZ);
        width = Mathf.Abs(scaleX);
        currentArea = vertical * width;
    }

    private void ChangeMatteTextureColor(float areaSize)
    {
        // �ʐςɉ����ă}�b�g�̐F��ω�������
        if (matteRenderer == null) return;

        Color newColor = Color.red;
        if (areaSize > tabooArea / 2f) newColor = Color.green;
        else if (areaSize > tabooArea / 4f) newColor = Color.yellow;
        if (areaSize >= maxArea) newColor = Color.black;

        matteMaterial.SetTexture("_MainTex", arabesqueTexture);
        matteRenderer.material.color = newColor;
    }

    private void CheckAreaLimit()
    {
        // �ʐς����E�𒴂����狭�������񂹂��J�n
        if (currentArea >= maxArea)
        {
            isAreaMax = true;
            StartCoroutine(PullPlayers());
        }
    }

    private IEnumerator PullPlayers()
    {
        // �v���C���[�ԋ�����1�����ɂȂ�܂Ō݂��Ɉ����񂹂�
        while (Vector3.Distance(playerObj_L.transform.position, playerObj_R.transform.position) > 1f)
        {
            Vector3 dir = (playerObj_L.transform.position - playerObj_R.transform.position).normalized;
            rbody_L.AddForce(-dir * pushPower, ForceMode.Acceleration);
            rbody_R.AddForce(dir * pushPower, ForceMode.Acceleration);
            yield return null;
        }

        // ��莞�ԑ���s�\��Ԃɂ���
        notMove = true;
        yield return new WaitForSeconds(notMoveTime);
        notMove = false;
        isAreaMax = false;
    }
    #endregion
}
