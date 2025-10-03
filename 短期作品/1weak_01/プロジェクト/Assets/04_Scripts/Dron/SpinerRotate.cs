using UnityEngine;

public class SpinerRotate : MonoBehaviour
{
    [SerializeField] GameObject droneObj;// �h���[���{��
    [Header("���v���(���O�A�E�� = false)�������v���(�E�O�A���� = true)��")]
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

        // �v���y�����쒆
        if (drone.spinnerFlag && drone.upFlag)
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
        // ���~�{�^�������Ă�Ԃ͍ŏ��l�܂łɂƂǂ߂Ă���
        else if (drone.spinnerFlag && !drone.upFlag)
        {
            --nowSpeed;
            if (nowSpeed < minSpeed)
            {
                nowSpeed = minSpeed;
            }
        }
        // �n�ʂɂ��Ă���E��~���͏��X�Ɍ���
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
