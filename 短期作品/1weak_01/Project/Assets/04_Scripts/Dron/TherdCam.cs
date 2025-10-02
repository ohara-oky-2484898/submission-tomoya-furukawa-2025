using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TherdCam : MonoBehaviour
{ 
    //カメラの回転用変数
    public float cameraSensitive = 200f;
    public Transform target;
    public Vector2 pitchMinMax = new Vector2(-40,85);

    public float smoothing = 0.12f;
    private Vector3 rotationSmoothVelosity;
    private Vector3 currentRotation;

    private Vector2 look;

    private float yaw;
    private float pitch;

    //カメラ衝突時に必要な変数
    public float camDisMin = 0.5f;
    public float camDisMax = 5f;

    private Vector3 cameraDirection;
    private float cameraDistace;
    private Vector2 cameraDistanceMinMax;
    public Transform cam;

    public float SmoothTime;
    private Vector3 velocity;


    // 障害物とするレイヤー
    [SerializeField]
    private LayerMask obstacleLayer;



    // Start is called before the first frame update
    void Start()
    {
        //カメラの方向初期化
        cameraDirection = cam.transform.localPosition.normalized;
        
        //カメラの距離初期値
        cameraDistace = cameraDistanceMinMax.y;
        
        //カメラの最短距離、最長距離
        cameraDistanceMinMax = new Vector2(camDisMin, camDisMax);
    }

    // Update is called once per frame
    void Update()
    {
        // 右スティックの入力
        yaw += look.x *  cameraSensitive * Time.deltaTime;
        pitch -= look.y * cameraSensitive * Time.deltaTime;

        //右スティックの入力によりtargetを中心に回転させる。
        pitch = Mathf.Clamp(pitch, pitchMinMax.x, pitchMinMax.y);
        currentRotation = Vector3.SmoothDamp(currentRotation, new Vector3(pitch, yaw), ref rotationSmoothVelosity, smoothing);
        transform.eulerAngles = currentRotation;
        transform.position = Vector3.MoveTowards(transform.position, target.position, 0.5f );

		//　レイを視覚的に確認
		Debug.DrawLine(target.position, cam.transform.position, Color.red, 0f, true);

		//カメラとオブジェクトの衝突処理
		CheckCameraOcclusion(cam);
    }

    public void CheckCameraOcclusion(Transform cam)
    {
        Vector3 desiredCameraPossion = transform.TransformPoint(cameraDirection * cameraDistanceMinMax.y);
        RaycastHit hit;
        if (Physics.Linecast(transform.position,desiredCameraPossion,out hit, obstacleLayer))
        {
            cameraDistace = Mathf.Clamp(hit.distance,cameraDistanceMinMax.x,cameraDistanceMinMax.y);
        }
        else
        {
            cameraDistace = cameraDistanceMinMax.y;
        }

        //cam.localPosition = cameraDirection * cameraDistace;
        cam.localPosition = Vector3.SmoothDamp(cam.localPosition, cameraDirection * cameraDistace, ref velocity, SmoothTime);
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        look = context.ReadValue<Vector2>();
    }
}