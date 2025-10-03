using UnityEngine;

public class RandomFalls : MonoBehaviour
{
    public GameObject[] falls; // ランダムに生成されるオブジェクトの配列

    // 範囲の基準となる角
    [SerializeField] private Transform leftCorner;
    [SerializeField] private Transform rightCorner;

    // 実際に使う生成範囲（X・Z方向）
    public Vector2 rangeX;
    public Vector2 rangeZ;

    [SerializeField] private StrCount str;

    private int timeCounter = 0; // 経過時間（フレームでカウント）
    private int fallCount = 0;   // 生成回数などで使用（今は未使用）
    private int currentLevel = 0; // ゲームレベル（0 = 初級, 1 = 中級, 2 = 上級）
    private int pattern = 0; // 生成パターン：0 = 横並び, 1 = 縦並び

    // 定数宣言
    private const float FALL_LIFETIME = 10f;        // オブジェクトが破壊されるまでの秒数
    private const int NORMAL_SPAWN_INTERVAL = 75;   // 通常のランダム生成の間隔（フレーム数）
    private const int BONUS_SPAWN_INTERVAL = 250;   // ボーナスタイムの間隔（フレーム数）

    private void Start()
    {
        // GameManagerからゲームレベルを取得して保存
        currentLevel = (int)GameManager.Instance.gamelevel;

        // 左上と右下のPositionから生成範囲を計算して保存
        rangeX = new Vector2(leftCorner.position.x, rightCorner.position.x);
        rangeZ = new Vector2(rightCorner.position.z, leftCorner.position.z);
    }

    private void FixedUpdate()
    {
        // 条件が満たされていなければ何もしない
        if (!str.strFlag) return;

        timeCounter++;

        // 通常の生成タイミング
        if (timeCounter % NORMAL_SPAWN_INTERVAL == 0)
        {
            fallCount++;

            // ランダムで1つオブジェクトをランダムな位置に生成
            SpawnSingleFall();
        }
        // ボーナスタイムのタイミング
        else if (timeCounter % BONUS_SPAWN_INTERVAL == 0)
        {
            Debug.Log("ボーナスタイム");

            // ゲームレベルに応じた数をまとめて生成
            SpawnBonusFalls();
        }
    }

    // 通常の1つだけ生成する処理
    private void SpawnSingleFall()
    {
        GameObject fall = Instantiate(
            falls[Random.Range(0, falls.Length)],
            GetRandomPosition(),
            Quaternion.identity
        );
        // 10秒後に破壊
        Destroy(fall, FALL_LIFETIME);
    }

    // ボーナスタイム用：まとめて生成する処理
    private void SpawnBonusFalls()
    {
        // 初級 = 3個, 中級 = 4個, 上級 = 5個 という感じ
        int minObjects = 2 + currentLevel;
        int maxObjects = minObjects * 2;

        // 実際に生成する個数をランダムで決定
        int spawnCount = Random.Range(minObjects, maxObjects);
        int selectedIndex = Random.Range(0, falls.Length); // 使用するプレハブのインデックス

        // 生成開始位置をランダムに取得
        Vector3 basePos = GetRandomPosition();

        // patternによって縦か横に並べて生成
        for (int i = 0; i < spawnCount; ++i)
        {
            Vector3 spawnPos = (pattern == 0)
                ? new Vector3(basePos.x + i, basePos.y, basePos.z)   // 横に並べる
                : new Vector3(basePos.x, basePos.y, basePos.z + i); // 縦に並べる

            Instantiate(falls[selectedIndex], spawnPos, Quaternion.identity);
        }

        // パターンを交互に切り替える（0 ↔ 1）
        pattern = 1 - pattern;
    }

    // 範囲内のランダムな位置を返す
    private Vector3 GetRandomPosition()
    {
        return new Vector3(
            Random.Range(rangeX.x, rangeX.y),
            transform.position.y,
            Random.Range(rangeZ.x, rangeZ.y)
        );
    }
}
