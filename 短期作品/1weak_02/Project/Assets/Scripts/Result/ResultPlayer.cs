/// <summary>
/// リザルトのplayerの向きをカメラ方向に向けるだけのもの
/// </summary>
using UnityEngine;

public class ResultPlayer : MonoBehaviour
{
    public Transform cameraTransform; // カメラのTransformをInspectorで設定

    void Start()
    {
        LookAtCamera();
    }

    private void LookAtCamera()
    {
        // プレイヤーとカメラの位置ベクトルを計算
        Vector3 direction = cameraTransform.position - transform.position;
        direction.y = 0; // y軸の回転を無視（地面に対して水平に）

        if (direction.magnitude > 0.1f) // 0ベクトルでないことを確認
        {
            Quaternion rotation = Quaternion.LookRotation(direction);
            transform.rotation = rotation; // 回転を適用
        }
    }
}
