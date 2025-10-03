using UnityEngine;
using UnityEngine.InputSystem;

public class GrapplingHook : MonoBehaviour
{
    public Camera playerCamera;
    public GameObject hookPrefab;
    public float pullForce = 50f;

    private GameObject activeHook;
    private SpringJoint joint;
    private Rigidbody playerRb;

    private InputAction shootAction;

    void Awake()
    {
        playerRb = GetComponent<Rigidbody>();

        shootAction = new InputAction(type: InputActionType.Button, binding: "<Mouse>/leftButton");
        shootAction.performed += ctx => ShootHook();
        shootAction.canceled += ctx => ReleaseHook();
    }

    void OnEnable()
    {
        shootAction.Enable();
    }

    void OnDisable()
    {
        shootAction.Disable();
    }
    void ShootHook()
    {
        if (activeHook != null) return;

        RaycastHit hit;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, 100f))
        {
            // �q�b�g�n�_�Ƀt�b�N�𐶐����A�����̓J��������
            activeHook = Instantiate(hookPrefab, hit.point, Quaternion.LookRotation(playerCamera.transform.forward));

            Hook hookScript = activeHook.GetComponent<Hook>();
            if (hookScript != null)
            {
                //hookScript.OnHooked += OnHooked;
            }
        }
        else
        {
            // �q�b�g���Ȃ���΁A�Ⴆ�΃J�����O���ɌŒ萶���i�C�Ӂj
            activeHook = Instantiate(hookPrefab, playerCamera.transform.position + playerCamera.transform.forward * 10f, Quaternion.LookRotation(playerCamera.transform.forward));

            Hook hookScript = activeHook.GetComponent<Hook>();
            if (hookScript != null)
            {
                //hookScript.OnHooked += OnHooked;
            }
        }
    }


    void ReleaseHook(InputAction.CallbackContext ctx)
    {
        ReleaseHook();
    }

    void ReleaseHook()
    {
        if (activeHook)
        {
            Hook hookScript = activeHook.GetComponent<Hook>();
            if (hookScript != null)
            {
                //hookScript.OnHooked -= OnHooked;
            }
            Destroy(activeHook);
            activeHook = null;
        }

        if (joint)
        {
            Destroy(joint);
            joint = null;
        }
    }
}
