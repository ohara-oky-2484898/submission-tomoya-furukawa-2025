/// <summary>
/// 選択中のアイコンがホップアップされるスクリプト
/// </summary>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class ScaleOnSelection : UIBehaviour
{
    [SerializeField] private float Rate = 2.25f; // 拡大率
    [SerializeField] private float AnimationDuration = 0.25f; // アニメーションの時間


    // 保管用
    //private GameObject fastSelectedObject; // 現在選択中オブジェクト
    private GameObject currentSelectedObject;
    private Vector3 BaseScale;
    private GameObject previousSelectedObject = null; // もともと選択していたオブジェクト

    void Update()
    {
        // 思考中のみホップアップする
        if (JankenManager.Instance.CurrentState == JankenState.Wait)
        {
            // 現在選ばれているオブジェクトを取得
            currentSelectedObject = EventSystem.current.currentSelectedGameObject;

            if (currentSelectedObject == previousSelectedObject) return;
            // スケールを変更する処理
            HandleScaleChange(currentSelectedObject);
        }
		else
		{
			CancelScale();
		}
	}


    // スケール変更処理
    private void HandleScaleChange(GameObject currentSelectedObject)
    {
        // 前回選ばれていたオブジェクトがあれば、元のスケールに戻す
        if (previousSelectedObject != null)
        {
            SetScale(previousSelectedObject, BaseScale); // BaseScalesの正しい値を参照
        }

        // 新しく選ばれたオブジェクトがあれば、スケールを拡大
        if (currentSelectedObject != null)
        {
            BaseScale = currentSelectedObject.transform.localScale;
            SetScale(currentSelectedObject, BaseScale * Rate); // BaseScalesの値を元に拡大
        }

        // 現在選ばれているオブジェクトを更新
        previousSelectedObject = currentSelectedObject;
    }

    // スケールを設定するメソッド
    private void SetScale(GameObject obj, Vector3 targetScale)
    {
        if (obj != null)
        {
            obj.transform.DOScale(targetScale, AnimationDuration)
                .SetEase(Ease.OutBounce)
                .Play();
        }
    }

    private void CancelScale()
	{
        SetScale(previousSelectedObject, BaseScale); // BaseScalesの正しい値を参照
    }
}
