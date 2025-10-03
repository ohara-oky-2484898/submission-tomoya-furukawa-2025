using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Player
/// �@��̌��肪�ł���
/// �A���̎���o�����Ƃ��ł���
/// 
/// �B���͂��邱�Ƃ��ł���(���͂����邱�ƂőI���A����)
/// </summary>
public class JankenPlayer : MonoBehaviour, IHandDecider
{
    // �Ō�Ɍ��߂�ꂽ��
    private JankenHand decidedHand;
    // ���߂����ǂ���
    private bool isDecided;

    // --- IHandDecider �̃v���p�e�B����

    public JankenHand SelectHand => decidedHand;
    //public bool IsDecided => isDecided;

    /// <summary>
    /// ������߂�
    /// </summary>
    /// <returns></returns>
    public IEnumerator DecideHand()
    {

        isDecided = false;

        // �肪�I�΂��܂ő҂���
        UIManager.Instance.ButtonSelectText(true);

        yield return new WaitUntil(() => isDecided);//����); ��������������܂ő҂�

        // UI���͑҂��Ƃ��A�{�^���̃N���b�N�҂��Ƃ���\��
        UIManager.Instance.ButtonSelectText(false);
    }

    /// <summary>
    /// �܂�������߂Ă��Ȃ��Ƃ�������߂�
    /// �{�^������ݒ肵�Ă�
    /// </summary>
    /// <param name="hand"></param>
    public void SetSelectHand(JankenHand hand)
    {
        //Debug.Log("�I���Ă΂ꂽ��I");
        // �I���\�̏ꍇ�̂�
        if (!isDecided)
		{
			decidedHand = hand;
            isDecided = true;// ���肵��
        }
    }

    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////
}
