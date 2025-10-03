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

    // LayerMaskをInspectorから設定してもいいし、ここで取得してもOK
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
        Cursor.visible = false;                    // カーソルを非表示にする
        Cursor.lockState = CursorLockMode.Locked; // カーソルを画面中央にロック（FPS視点などでよく使う）
    }

    void OnDestroy()
    {
        clickAction.Disable();
    }

    void ShootHook()
    {
        Debug.Log("ショット！！");

        // プレイヤーのColliderを除外するためRaycast時に使うレイヤーマスクからプレイヤーのレイヤーを除外してもOK。
        // ここではレイヤーマスクに"HookObj"だけが含まれている想定。

        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);

        // RaycastHitはoutで取得
        if (Physics.Raycast(ray, out RaycastHit hit, 100f, hookLayerMask))
        {
            Debug.Log("あたった！");

            // プレイヤー自身のコライダーを除外したい場合は
            // if(hit.collider.gameObject == this.gameObject) return; などでチェック可能

            // すでにhookがある場合は破棄
            if (activeHook != null)
            {
                Destroy(activeHook);
                if (springJoint != null) Destroy(springJoint);
            }

            // フックの生成
            activeHook = Instantiate(hookPrefab, hit.point, Quaternion.identity);

            // SpringJointの設定
            springJoint = gameObject.AddComponent<SpringJoint>();
            springJoint.connectedBody = null; // ワールド座標固定
            springJoint.connectedAnchor = hit.point;

            springJoint.spring = springForce;
            springJoint.damper = damper;
            springJoint.minDistance = minDistance;
            springJoint.maxDistance = maxDistance;
            springJoint.autoConfigureConnectedAnchor = false;
        }
    }

    // フックをリリースする処理などは必要に応じて作ってください
}
