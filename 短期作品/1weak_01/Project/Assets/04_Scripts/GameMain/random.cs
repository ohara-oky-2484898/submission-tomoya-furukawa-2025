using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class random : MonoBehaviour
{
   // [SerializeField]
    [Header("生成するGameObject")]
    [SerializeField] private GameObject createPrefabRed;
    [SerializeField] private GameObject createPrefabBlue;
    [SerializeField] private GameObject createPrefabYellow;
    [SerializeField] private GameObject createPrefabGreen;
    [SerializeField]
    [Header("生成する範囲左上角")]
    private Transform rangeA;
    [SerializeField]
    [Header("生成する範囲右下角")]
    private Transform rangeB;

    public int createNumMax = 0;

    private GameObject createPrefab;
    private int count = 0;

    private int nowMaxColorNum = 4;

	private void Start()
	{
        // 生成個数を決定
        createNumMax = createNumMax * GameManager.Instance.gameLevel;
        GameManager.Instance.clearborder = createNumMax;
	}

	// Update is called once per frame
	void Update()
    {

        if(count < createNumMax)
        {
            // ランダムに現在の色の数から選ぶ(0から3で4色)
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


            // rangeAとrangeBのx座標の範囲内でランダムな数値を作成
            float x = Random.Range(rangeA.position.x, rangeB.position.x);
            // rangeAとrangeBのy座標の範囲内でランダムな数値を作成
            float y = Random.Range(1, 3);
            // rangeAとrangeBのz座標の範囲内でランダムな数値を作成
            float z = Random.Range(rangeA.position.z, rangeB.position.z);

            // GameObjectを上記で決まったランダムな場所に生成
            Instantiate(createPrefab, new Vector3(x, y, z), createPrefab.transform.rotation);
        }
        count++;
    }
}