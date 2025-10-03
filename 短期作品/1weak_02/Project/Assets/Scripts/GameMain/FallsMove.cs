using UnityEngine;

public class FallsMove : MonoBehaviour // クラス名はPascalCaseに統一
{
    public float speed;         // 落下スピード（初期値はInspectorで設定）
    public int point = 0;       // キャッチ時に加算されるスコア

    private bool destroyFlag = false;

    [SerializeField] private GameObject pintObj; // ヒット地点に表示するマーカー用のプレハブ
    private GameObject pointerObj;               // 実際に生成されたマーカー

    // 定数：レイの長さとマーカーのオフセット
    private const float RAYCAST_LENGTH = 10f;
    private static readonly Vector3 MARKER_OFFSET = new Vector3(0, 1, 0);

    private void Start()
    {
        // 自分の位置から真下にレイを飛ばして地面を探す
        Vector3 from = transform.position;
        Vector3 to = Vector3.down;
        RaycastHit hit;

        if (Physics.Raycast(from, to, out hit, RAYCAST_LENGTH))
        {
            // ヒット地点の少し上にマーカーを表示する
            pointerObj = Instantiate(pintObj, hit.point + MARKER_OFFSET, Quaternion.identity);
        }

        // スピードを微調整（Inspector上では大きめの値で扱いやすく）
        speed /= 1000f;
    }

    private void Update()
    {
        // 消去フラグが立っていれば、自分とマーカーを削除
        if (destroyFlag)
        {
            Destroy(gameObject);
            if (pointerObj != null) Destroy(pointerObj);
            return;
        }

        // オブジェクトを全方向に1度ずつ回転（見た目用）
        transform.Rotate(Vector3.one);

        // オブジェクトをワールド空間で下方向へ移動
        transform.Translate(Vector3.down * speed, Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        // プレイヤーのマットにキャッチされたら
        if (other.CompareTag("Matte"))
        {
            // スコア加算して自分を消す
            GameManager.Instance.score += point;
            destroyFlag = true;
        }
        // 地面または左右の壁に当たったら消す
        else if (other.name == "Mesh" || other.name == "LeftPlay" || other.name == "RightPlay")
        {
            destroyFlag = true;
        }
        // その他のオブジェクトに当たった場合（ログに残すならここ）
        else
        {
            string hitObj = other.gameObject.name;
            // Debug.Log($"{hitObj}に当たりました どこに当たってんねん");
        }
    }
}
