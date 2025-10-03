/// <summary>
/// ゲーム中のplayer右と左の表示の向きをカメラに合わせる用
/// </summary>
using UnityEngine;

public class textdir : MonoBehaviour
{
    public Camera mainCamera; // カメラを設定するための変数

    void Start()
    {
        // メインカメラを自動的に取得する場合
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
    }

    void Update()
    {
        // 3D Textの向きをカメラに向ける
        transform.LookAt(mainCamera.transform);

        // Z軸をカメラに向けるため、Y軸を固定する
        transform.Rotate(0, 180, 0); // テキストが反転しないように180度回転
    }
}
