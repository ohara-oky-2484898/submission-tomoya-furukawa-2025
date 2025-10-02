using UnityEngine;

public class SpinerRotate : MonoBehaviour
{
    [SerializeField] GameObject droneObj;// ドローン本体
    [SerializeField] private float maxSpeed = 30;
    [SerializeField] private Vector3 localAxis = new Vector3(0, 0, -1);

    private float nowSpeed = 0.0f;
    private DroneActionController drone;

    void Start()
    {
        if(droneObj != null)
		{
            drone = droneObj.GetComponent<DroneActionController>();
		}
    }

    void Update()
    {
        if (drone == null) return;

        // プロペラ操作中
        if (drone.spinnerFlag)
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
        // 停止中は徐々に減速
        else
        {
            --nowSpeed;
        }

        //if (drone.upFlag)

        this.transform.Rotate(localAxis * nowSpeed * Time.deltaTime);
    }
}
