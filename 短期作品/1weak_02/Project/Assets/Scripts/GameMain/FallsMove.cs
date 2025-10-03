using UnityEngine;

public class FallsMove : MonoBehaviour // �N���X����PascalCase�ɓ���
{
    public float speed;         // �����X�s�[�h�i�����l��Inspector�Őݒ�j
    public int point = 0;       // �L���b�`���ɉ��Z�����X�R�A

    private bool destroyFlag = false;

    [SerializeField] private GameObject pintObj; // �q�b�g�n�_�ɕ\������}�[�J�[�p�̃v���n�u
    private GameObject pointerObj;               // ���ۂɐ������ꂽ�}�[�J�[

    // �萔�F���C�̒����ƃ}�[�J�[�̃I�t�Z�b�g
    private const float RAYCAST_LENGTH = 10f;
    private static readonly Vector3 MARKER_OFFSET = new Vector3(0, 1, 0);

    private void Start()
    {
        // �����̈ʒu����^���Ƀ��C���΂��Ēn�ʂ�T��
        Vector3 from = transform.position;
        Vector3 to = Vector3.down;
        RaycastHit hit;

        if (Physics.Raycast(from, to, out hit, RAYCAST_LENGTH))
        {
            // �q�b�g�n�_�̏�����Ƀ}�[�J�[��\������
            pointerObj = Instantiate(pintObj, hit.point + MARKER_OFFSET, Quaternion.identity);
        }

        // �X�s�[�h��������iInspector��ł͑傫�߂̒l�ň����₷���j
        speed /= 1000f;
    }

    private void Update()
    {
        // �����t���O�������Ă���΁A�����ƃ}�[�J�[���폜
        if (destroyFlag)
        {
            Destroy(gameObject);
            if (pointerObj != null) Destroy(pointerObj);
            return;
        }

        // �I�u�W�F�N�g��S������1�x����]�i�����ڗp�j
        transform.Rotate(Vector3.one);

        // �I�u�W�F�N�g�����[���h��Ԃŉ������ֈړ�
        transform.Translate(Vector3.down * speed, Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        // �v���C���[�̃}�b�g�ɃL���b�`���ꂽ��
        if (other.CompareTag("Matte"))
        {
            // �X�R�A���Z���Ď���������
            GameManager.Instance.score += point;
            destroyFlag = true;
        }
        // �n�ʂ܂��͍��E�̕ǂɓ������������
        else if (other.name == "Mesh" || other.name == "LeftPlay" || other.name == "RightPlay")
        {
            destroyFlag = true;
        }
        // ���̑��̃I�u�W�F�N�g�ɓ��������ꍇ�i���O�Ɏc���Ȃ炱���j
        else
        {
            string hitObj = other.gameObject.name;
            // Debug.Log($"{hitObj}�ɓ�����܂��� �ǂ��ɓ������Ă�˂�");
        }
    }
}
