using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class materialcheck : MonoBehaviour
{
    // 自分の欲しているタグ
    [SerializeField]public  string compareCollarTag = "DesiredTag";
    public string colorID = "DesiredTag";

    private void Awake()
	{

	}

	void OnCollisionEnter(Collision collision)
    {

        // 触れているオブジェクトのタグを取得
        if (collision.gameObject.CompareTag(compareCollarTag))
        {
            // タグが一致した時の処理
            GameManager.Instance.clearNum += 1; 
            Debug.Log("タグが一致しました！");
            PointUp();
        }
    }
	private void OnCollisionStay(Collision collision)
	{
		
	}

	private void OnCollisionExit(Collision collision)
	{
        if (collision.gameObject.CompareTag(compareCollarTag))
        {
            GameManager.Instance.clearNum -= 1;
            // タグが一致した時の処理
            Debug.Log("同じ色から離れたよ");
            PointDwun();
        }
    }

    void PointUp()
    {
        if ("PlaneRed" == compareCollarTag)
        {
            ++GameManager.Instance.numRed;
        }
        else if ("PlaneBlue" == compareCollarTag)
        {
            ++GameManager.Instance.numBlue;
        }
        else if ("PlaneYellow" == compareCollarTag)
        {
            ++GameManager.Instance.numYellow;
        }
        else if ("PlaneGreen" == compareCollarTag)
        {
            ++GameManager.Instance.numGreen;
        }
    }

    void PointDwun()
    {
		if ("PlaneRed" == compareCollarTag)
		{
            --GameManager.Instance.numRed;
		}
		else if ("PlaneBlue" == compareCollarTag)
		{
            --GameManager.Instance.numBlue;
        }
		else if ("PlaneYellow" == compareCollarTag)
		{
            --GameManager.Instance.numYellow;
        }
		else if ("PlaneGreen" == compareCollarTag)
		{
            --GameManager.Instance.numGreen;
        }
    }
}
