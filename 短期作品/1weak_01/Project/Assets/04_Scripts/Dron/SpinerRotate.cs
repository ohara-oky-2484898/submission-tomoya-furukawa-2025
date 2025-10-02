using UnityEngine;

public class SpinerRotate : MonoBehaviour
{
    [SerializeField] GameObject droneObj;// �h���[���{��
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

        // �v���y�����쒆
        if (drone.spinnerFlag)
        {
            // �ő呬�x�𒴂��Ă��Ȃ��Ƃ���

            if (nowSpeed < maxSpeed)
            {
                nowSpeed++;
            }
            // �����Ă�ꍇ�͍ő�l�ɐݒ�
            else
            {
                nowSpeed = maxSpeed;
            }

        }
        // ��~���͏��X�Ɍ���
        else
        {
            --nowSpeed;
        }

        //if (drone.upFlag)

        this.transform.Rotate(localAxis * nowSpeed * Time.deltaTime);
    }
}
