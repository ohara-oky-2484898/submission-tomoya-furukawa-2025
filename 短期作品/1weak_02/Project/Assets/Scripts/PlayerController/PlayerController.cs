using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // 入力
    private Vector2 inputMove_L; // 左プレイヤーの移動入力
    private Vector2 inputMove_R; // 右プレイヤーの移動入力

    // プレイヤーとマットのオブジェクト
    private GameObject playerObj_L; // 左プレイヤーオブジェクト
    private GameObject playerObj_R; // 右プレイヤーオブジェクト
    private GameObject matteObj;     // マットオブジェクト

    // Rigidbody
    private Rigidbody rbody_L; // 左プレイヤーのRigidbody
    private Rigidbody rbody_R; // 右プレイヤーのRigidbody

    // 移動・回転用ベクトル
    private Vector3 player_L_dir; // 左プレイヤーの移動方向
    private Vector3 player_R_dir; // 右プレイヤーの移動方向

    // 設定値
    [SerializeField] private float moveSpeed = 5.0f; // 通常移動速度
    [SerializeField] private GameObject camera; // メインカメラ
    [SerializeField] private float notMoveTime = 2.5f; // 一時停止時間
    [SerializeField] public float dashSpeed; // ダッシュ速度加算
    [SerializeField] private Texture2D arabesqueTexture; // マットに貼るテクスチャ
    [SerializeField] private Renderer matteRenderer; // マットのレンダラー
    [SerializeField] private Animator animator_L; // 左プレイヤーのアニメーター
    [SerializeField] private Animator animator_R; // 右プレイヤーのアニメーター
    [SerializeField] private StrCount str; // ゲーム開始判定用スクリプト

    // 面積・移動制御用
    private float matteY; // マットのYスケール保持
    private float maxArea; // 最大許容面積
    private float tabooArea = 100f; // 面積制限値（ゲームレベルで変化）
    private float currentArea; // 現在の面積
    private bool isAreaMax; // 面積制限フラグ
    private bool notMove = false; // 一時停止フラグ
    private Vector3 matteStartPos; // マットの初期位置

    // 状態変数
    public float vertical; // 縦幅
    public float width; // 横幅
    public float pushPower = 20f; // 引き寄せ力
    public float additionSpeed_L; // 左プレイヤーの加算速度
    public float additionSpeed_R; // 右プレイヤーの加算速度

    // 簡略化用
    private Vector3 Lpos; // 左プレイヤー位置
    private Vector3 Rpos; // 右プレイヤー位置
    private Material matteMaterial; // マットのマテリアルインスタンス

    void Start()
    {
        // プレイヤーオブジェクトとマットオブジェクトの取得
        playerObj_L = transform.GetChild(0).gameObject;
        playerObj_R = transform.GetChild(1).gameObject;
        matteObj = transform.GetChild(2).gameObject;

        // マットのスケール・位置初期化
        matteY = matteObj.transform.localScale.y;
        matteStartPos = matteObj.transform.position;

        // Rigidbody取得
        rbody_L = playerObj_L.GetComponent<Rigidbody>();
        rbody_R = playerObj_R.GetComponent<Rigidbody>();

        // マテリアルインスタンス作成
        if (matteRenderer != null)
        {
            matteMaterial = new Material(matteRenderer.material);
            matteRenderer.material = matteMaterial;
        }

        // ゲームレベルによる制限初期化
        tabooArea /= (int)GameManager.Instance.gamelevel;
        maxArea = tabooArea * 1.1f;
        GameManager.Instance.maxAreaSize = maxArea;
    }

    void Update()
    {
        // アニメーション更新
        animator_L.SetFloat("walk", inputMove_L.magnitude, 0.1f, Time.deltaTime);
        animator_R.SetFloat("walk2", inputMove_R.magnitude, 0.1f, Time.deltaTime);
        animator_L.SetBool("NotMove", notMove);
        animator_R.SetBool("NotMove", notMove);
    }

    void FixedUpdate()
    {
        if (!str.strFlag) return; // カウントダウンが終わるまで操作禁止

        // 各プレイヤーの現在位置取得
        Lpos = playerObj_L.transform.position;
        Rpos = playerObj_R.transform.position;
        Lpos.y += matteStartPos.y;

        // マットの更新
        MatteTransform();
        GameManager.Instance.nowAreaSize = currentArea;
        ChangeMatteTextureColor(currentArea);
        CheckAreaLimit();

        // プレイヤー操作
        if (!notMove)
        {
            PlayerMove();
        }
        PlayerRotation();
    }

    #region 入力関数
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

    #region プレイヤー処理
    private void PlayerMove()
    {
        // カメラ方向を基準に移動ベクトル作成
        Vector3 camForward = Vector3.Scale(camera.transform.forward, new Vector3(1, 0, 1)).normalized;
        player_L_dir = camForward * inputMove_L.y + camera.transform.right * inputMove_L.x;
        player_R_dir = camForward * inputMove_R.y + camera.transform.right * inputMove_R.x;

        // 移動速度適用
        rbody_L.velocity = player_L_dir * (moveSpeed + additionSpeed_L);
        rbody_R.velocity = player_R_dir * (moveSpeed + additionSpeed_R);
    }

    private void PlayerRotation()
    {
        // 各プレイヤーを相手プレイヤーに向ける
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

    #region Matte処理
    private void MatteTransform()
    {
        // 中心位置に配置
        Vector3 centerPos = (Lpos + Rpos) / 2f;
        matteObj.transform.position = centerPos;

        // スケーリング
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
        // 面積に応じてマットの色を変化させる
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
        // 面積が限界を超えたら強制引き寄せを開始
        if (currentArea >= maxArea)
        {
            isAreaMax = true;
            StartCoroutine(PullPlayers());
        }
    }

    private IEnumerator PullPlayers()
    {
        // プレイヤー間距離が1未満になるまで互いに引き寄せる
        while (Vector3.Distance(playerObj_L.transform.position, playerObj_R.transform.position) > 1f)
        {
            Vector3 dir = (playerObj_L.transform.position - playerObj_R.transform.position).normalized;
            rbody_L.AddForce(-dir * pushPower, ForceMode.Acceleration);
            rbody_R.AddForce(dir * pushPower, ForceMode.Acceleration);
            yield return null;
        }

        // 一定時間操作不能状態にする
        notMove = true;
        yield return new WaitForSeconds(notMoveTime);
        notMove = false;
        isAreaMax = false;
    }
    #endregion
}
