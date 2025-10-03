using UnityEngine;
using UnityEngine.InputSystem;

public class SpringJointSample : MonoBehaviour
{
    public Camera playerCamera;
    public GameObject hookPrefab;

    private GameObject activeHook;
    private SpringJoint springJoint;
    private Rigidbody rb;

    public float springForce = 100f;
    public float damper = 5f;
    public float minDistance = 0.1f;
    public float maxDistance = 5f;

    private InputAction clickAction;

    // LayerMask��Inspector����ݒ肵�Ă��������A�����Ŏ擾���Ă�OK
    public LayerMask hookLayerMask;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();

        clickAction = new InputAction(type: InputActionType.Button, binding: "<Mouse>/leftButton");
        clickAction.performed += ctx => ShootHook();
        clickAction.Enable();

    }
    void Start()
    {
        Cursor.visible = false;                    // �J�[�\�����\���ɂ���
        Cursor.lockState = CursorLockMode.Locked; // �J�[�\������ʒ����Ƀ��b�N�iFPS���_�Ȃǂł悭�g���j
    }

    void OnDestroy()
    {
        clickAction.Disable();
    }

    void ShootHook()
    {
        Debug.Log("�V���b�g�I�I");

        // �v���C���[��Collider�����O���邽��Raycast���Ɏg�����C���[�}�X�N����v���C���[�̃��C���[�����O���Ă�OK�B
        // �����ł̓��C���[�}�X�N��"HookObj"�������܂܂�Ă���z��B

        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);

        // RaycastHit��out�Ŏ擾
        if (Physics.Raycast(ray, out RaycastHit hit, 100f, hookLayerMask))
        {
            Debug.Log("���������I");

            // �v���C���[���g�̃R���C�_�[�����O�������ꍇ��
            // if(hit.collider.gameObject == this.gameObject) return; �ȂǂŃ`�F�b�N�\

            // ���ł�hook������ꍇ�͔j��
            if (activeHook != null)
            {
                Destroy(activeHook);
                if (springJoint != null) Destroy(springJoint);
            }

            // �t�b�N�̐���
            activeHook = Instantiate(hookPrefab, hit.point, Quaternion.identity);

            // SpringJoint�̐ݒ�
            springJoint = gameObject.AddComponent<SpringJoint>();
            springJoint.connectedBody = null; // ���[���h���W�Œ�
            springJoint.connectedAnchor = hit.point;

            springJoint.spring = springForce;
            springJoint.damper = damper;
            springJoint.minDistance = minDistance;
            springJoint.maxDistance = maxDistance;
            springJoint.autoConfigureConnectedAnchor = false;
        }
    }

    // �t�b�N�������[�X���鏈���Ȃǂ͕K�v�ɉ����č���Ă�������
}
