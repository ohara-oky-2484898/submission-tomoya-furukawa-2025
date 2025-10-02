using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class random : MonoBehaviour
{
   // [SerializeField]
    [Header("��������GameObject")]
    [SerializeField] private GameObject createPrefabRed;
    [SerializeField] private GameObject createPrefabBlue;
    [SerializeField] private GameObject createPrefabYellow;
    [SerializeField] private GameObject createPrefabGreen;
    [SerializeField]
    [Header("��������͈͍���p")]
    private Transform rangeA;
    [SerializeField]
    [Header("��������͈͉E���p")]
    private Transform rangeB;

    public int createNumMax = 0;

    private GameObject createPrefab;
    private int count = 0;

    private int nowMaxColorNum = 4;

	private void Start()
	{
        // ������������
        createNumMax = createNumMax * GameManager.Instance.gameLevel;
        GameManager.Instance.clearborder = createNumMax;
	}

	// Update is called once per frame
	void Update()
    {

        if(count < createNumMax)
        {
            // �����_���Ɍ��݂̐F�̐�����I��(0����3��4�F)
            int color = Random.Range(0, nowMaxColorNum);

            switch (color)
            {
                case 0:
                    createPrefab = createPrefabRed;
                    ++GameManager.Instance.numRedMax;
                    break;
                case 1:
                    createPrefab = createPrefabBlue;
                    ++GameManager.Instance.numBlueMax;
                    break;
                case 2:
                    createPrefab = createPrefabYellow;
                    ++GameManager.Instance.numYellowMax;
                    break;
                case 3:
                    createPrefab = createPrefabGreen;
                    ++GameManager.Instance.numGreenMax;
                    break;
            }


            // rangeA��rangeB��x���W�͈͓̔��Ń����_���Ȑ��l���쐬
            float x = Random.Range(rangeA.position.x, rangeB.position.x);
            // rangeA��rangeB��y���W�͈͓̔��Ń����_���Ȑ��l���쐬
            float y = Random.Range(1, 3);
            // rangeA��rangeB��z���W�͈͓̔��Ń����_���Ȑ��l���쐬
            float z = Random.Range(rangeA.position.z, rangeB.position.z);

            // GameObject����L�Ō��܂��������_���ȏꏊ�ɐ���
            Instantiate(createPrefab, new Vector3(x, y, z), createPrefab.transform.rotation);
        }
        count++;
    }
}