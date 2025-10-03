using UnityEngine;

public class SpinerRotate : MonoBehaviour
{
    [SerializeField] GameObject droneObj;// ドローン本体
    [Header("時計回り(左前、右後 = false)か反時計回り(右前、左後 = true)か")]
    [SerializeField] bool isCw = false;
    private float maxSpeed = 2000;
    private float minSpeed = 500;
    private Vector3 localAxis_cw = new Vector3(0, 0, -1);
    private Vector3 localAxis_ccw = new Vector3(0, 0, 1);

    private float nowSpeed = 0.0f;
    private DroneActionController drone;

    public float NowSpeed;
    private Vector3 nowRotateAxis;

    void Start()
    {
        if(droneObj != null)
		{
            drone = droneObj.GetComponent<DroneActionController>();
		}

        if(isCw)
		{
            nowRotateAxis = localAxis_cw;
		}
        else
		{
            nowRotateAxis = localAxis_ccw;
		}
    }

    void Update()
    {
        if (drone == null) return;

        // プロペラ操作中
        if (drone.spinnerFlag && drone.upFlag)
        {
            // 最大速度を超えていないときは

            if (nowSpeed < maxSpeed)
            {
                nowSpeed++;
            }
            // 超えてる場合は最大値に設定
            else
            {
                nowSpeed = maxSpeed;
            }

        }
        // 下降ボタン押してる間は最小値までにとどめておく
        else if (drone.spinnerFlag && !drone.upFlag)
        {
            --nowSpeed;
            if (nowSpeed < minSpeed)
            {
                nowSpeed = minSpeed;
            }
        }
        // 地面についている・停止中は徐々に減速
        else if(drone.isGround)
        {
            --nowSpeed;
            if(nowSpeed < 0.0f)
			{
                nowSpeed = 0.0f;
			}
        }
        NowSpeed = nowSpeed;

		this.transform.Rotate(nowRotateAxis * nowSpeed * Time.deltaTime);
    }
}
