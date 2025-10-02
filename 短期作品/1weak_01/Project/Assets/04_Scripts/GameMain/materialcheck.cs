using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class materialcheck : MonoBehaviour
{
    // �����̗~���Ă���^�O
    [SerializeField]public  string compareCollarTag = "DesiredTag";
    public string colorID = "DesiredTag";

    private void Awake()
	{

	}

	void OnCollisionEnter(Collision collision)
    {

        // �G��Ă���I�u�W�F�N�g�̃^�O���擾
        if (collision.gameObject.CompareTag(compareCollarTag))
        {
            // �^�O����v�������̏���
            GameManager.Instance.clearNum += 1; 
            Debug.Log("�^�O����v���܂����I");
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
            // �^�O����v�������̏���
            Debug.Log("�����F���痣�ꂽ��");
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
