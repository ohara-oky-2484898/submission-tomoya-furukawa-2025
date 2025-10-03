using UnityEngine;
using UnityEngine.InputSystem;

public class DroneActionController : MonoBehaviour
{
    private Vector3 inputMove;
    private Vector3 cameraForward;
    private Vector3 moveForward;


    [SerializeField] public float maxSpeed = 30.0f;
    [SerializeField] public float maxUpDownSpeed = 30.0f;
    [SerializeField] private float moveSpeed = 0.0f;
    [SerializeField] private float rotationSpeed = 0.0f;
    [SerializeField] Rigidbody _rigidbody;

    // ���肵�����I�u�W�F�N�g�̃^�O���w��
    public string targetTag = "TargetObject";
    private string[] groundTags = { "PlaneRed", "PlaneBlue", "PlaneYellow", "PlaneGreen" };


    public bool holdFlag = false;   // ���̂����߂���
    public bool isCatch = false;   // ���߂邩
    public bool upFlag = false;     // Up�Ȃ̂�Down�Ȃ̂�
    public bool friezeFlag = false;     // �������Ȃ��E���W�b�gbody��؂肽��
    public bool OnTouch = false;     // 

    public bool isGround = false;

    public Vector3 moveVector3;
    public float upDownSpeed = 0.0f;

    // ��ԊǗ��p
    public bool spinnerFlag = false;// UpDown�Ȃǂ̑�������Ă��邩..�v���y�����񂷂悤
    public bool isMax = false;

    // �͂񂾃I�u�W�F�N�g�p
    private GameObject holdObj;
    private Rigidbody holdObj_rigidbody;

    private void FixedUpdate()
	{
        // �ړ�
        MainMovementsMethod();
        UpDownMove();
        HoldAction();

        _rigidbody.velocity = moveVector3;

        if (!upFlag && !OnTouch)
        {
            upDownSpeed -= Time.deltaTime;
            moveVector3.y -= upDownSpeed;
        }
    }

    private void MainMovementsMethod()
    {// �ړ�����

        //��]����
        cameraForward = Camera.main.transform.forward;
        cameraForward.y = 0; // Y�����𖳎����Đ����ʂ̕������擾
        cameraForward.Normalize(); // ���K��

        // �����L�[�̓��͒l�ƃJ�����̌�������A�ړ������ƈړ��ʂ�moveForward�ɑ��
        moveForward = (cameraForward * inputMove.y + Camera.main.transform.right * inputMove.x).normalized;

        // �L�����N�^�[�̌�������]
        if (moveForward != Vector3.zero)
        {
            Quaternion rotation = Quaternion.LookRotation(moveForward);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
        }


        if (moveForward != Vector3.zero)
        {
            //�ړ��������쐬 velocity
            moveVector3 = moveForward * moveSpeed + new Vector3(0, _rigidbody.velocity.y, 0);
            //Debug.Log("�����Ă�I");
        }
        else
        {
            // ���͂��Ȃ��Ƃ��͑��x���[���ɐݒ�
            moveVector3 = new Vector3(0, _rigidbody.velocity.y, 0);
            //Debug.Log("�Ƃ܂��Ă�I");
        }
    }

    private void HoldAction()
    {
        if (!holdFlag) return;

        // �v���C���[�̈ʒu�̉��ɔz�u������
        holdObj_rigidbody.velocity = moveVector3;
       // Debug.Log("�͂�ł��I");
    }

    private void UpDownMove()
    {
        // �v���y�����������Ă��Ȃ������㏸�����~�����Ă��Ȃ�
        if (!spinnerFlag) return;
        isMax = maxUpDownSpeed > Mathf.Abs(upDownSpeed);
       // Debug.Log($"isMax{isMax} = maxUpDownSpeed{maxUpDownSpeed}�@>�@upDownSpeed�F{upDownSpeed}");

        if (upFlag)
        {
			if (upDownSpeed < 0)
			{
                upDownSpeed = 0;
            }
            if (isMax)
            {
                upDownSpeed += (0.1f / (maxUpDownSpeed - upDownSpeed));
                moveVector3.y += upDownSpeed;// = upDownSpeed + 0.1f;
              //  Debug.Log("�㏸���I");
            }
            else
            {
                moveVector3.y = maxUpDownSpeed;
              //  Debug.Log("�㏸�����䒆�I");
            }
        }
        else
        {
            if (upDownSpeed > 0)
            {
                upDownSpeed -= Time.deltaTime ;
            }
            if (isMax)
            {
                upDownSpeed -= (0.1f / (maxUpDownSpeed - upDownSpeed));
                moveVector3.y += upDownSpeed;
              //  Debug.Log("���~���I");
            }
            else
            {
                moveVector3.y -= maxUpDownSpeed;
              // Debug.Log("���~�����䒆�I");
            }
        }
    }

    #region ���͎󂯎��֐�

    public void OnMove(InputAction.CallbackContext context)
    {// �ړ��X�e�B�b�N
        if (context.performed)
        {
            friezeFlag = false;
            //inputMove = context.ReadValue<Vector2>();
        }
        else if (context.canceled)
        {
            friezeFlag = true;
        }
        inputMove = context.ReadValue<Vector2>();
        //inputMove�� inputMove.x�ɐ��l�������Ă��� inputMove.y�ɐ��l�������Ă��� 
        //Debug.Log($"inputMove{inputMove}");
    }

    public void OnUpMove(InputAction.CallbackContext context)
    {// �㏸�{�^���������ꂽ��
        if (context.performed)// ������Ă�
        {
            spinnerFlag = true;
            upFlag = true;
        }
        else if (context.canceled)// �����ꂽ
        {
            spinnerFlag = false;
            upFlag = false;
        }
    }

    public void OnDownMove(InputAction.CallbackContext context)
    {// ���~�{�^���������ꂽ�Ƃ�
        if (context.performed)// ������Ă�
        {
            spinnerFlag = true;
        }
        else if (context.canceled)// �����ꂽ
        {
            spinnerFlag = false;
        }
    }
    public void OnHoldMove(InputAction.CallbackContext context)
    {// �͂݃{�^���������ꂽ��
       
        if (context.performed)// ������Ă�
        {
            if (isCatch)
            {
                holdFlag = true;
                holdObj_rigidbody = holdObj.GetComponent<Rigidbody>();
            }
        }
        else if (context.canceled)// �����ꂽ
        {
            holdFlag = false;
        }
    }

	private void OnTriggerEnter(Collider other)
	{
        OnTouch = true;

        // �Փ˂����I�u�W�F�N�g���擾
        GameObject collidedObject = other.gameObject;

        // ����̃I�u�W�F�N�g�ł��邩����
        if (collidedObject.CompareTag(targetTag))
        {
            isCatch = true;
            holdObj = collidedObject;

        }
        else
        {
            //collidedObject.CompareTag()
            isCatch = false;
        }
    }

	private void OnTriggerStay(Collider other)
	{
        // �Փ˂����I�u�W�F�N�g���擾
        GameObject collidedObject = other.gameObject;
        for (int i = 0; i < groundTags.Length; ++i)
        {
            if (collidedObject.CompareTag(groundTags[i]))
            {
                isGround = true;
                break;
            }
        }
    }

	private void OnTriggerExit(Collider other)
    {
        OnTouch = false;

        // ���ꂽ
        isCatch = false;
    }

    #endregion
}
