/// <summary>
/// ���U���g��player�̌������J���������Ɍ����邾���̂���
/// </summary>
using UnityEngine;

public class ResultPlayer : MonoBehaviour
{
    public Transform cameraTransform; // �J������Transform��Inspector�Őݒ�

    void Start()
    {
        LookAtCamera();
    }

    private void LookAtCamera()
    {
        // �v���C���[�ƃJ�����̈ʒu�x�N�g�����v�Z
        Vector3 direction = cameraTransform.position - transform.position;
        direction.y = 0; // y���̉�]�𖳎��i�n�ʂɑ΂��Đ����Ɂj

        if (direction.magnitude > 0.1f) // 0�x�N�g���łȂ����Ƃ��m�F
        {
            Quaternion rotation = Quaternion.LookRotation(direction);
            transform.rotation = rotation; // ��]��K�p
        }
    }
}
