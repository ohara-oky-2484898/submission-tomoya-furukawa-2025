/// <summary>
/// �Q�[������player�E�ƍ��̕\���̌������J�����ɍ��킹��p
/// </summary>
using UnityEngine;

public class textdir : MonoBehaviour
{
    public Camera mainCamera; // �J������ݒ肷�邽�߂̕ϐ�

    void Start()
    {
        // ���C���J�����������I�Ɏ擾����ꍇ
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
    }

    void Update()
    {
        // 3D Text�̌������J�����Ɍ�����
        transform.LookAt(mainCamera.transform);

        // Z�����J�����Ɍ����邽�߁AY�����Œ肷��
        transform.Rotate(0, 180, 0); // �e�L�X�g�����]���Ȃ��悤��180�x��]
    }
}
