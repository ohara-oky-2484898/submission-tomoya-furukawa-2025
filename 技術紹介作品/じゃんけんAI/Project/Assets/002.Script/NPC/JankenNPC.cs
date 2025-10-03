/// <summary>
/// ����񂯂��NPC
/// </summary>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// NPC
/// �@��̌��肪�ł���
/// �A���̎���o�����Ƃ��ł���
/// �B��̌����IJankenAI���g���i�����Ă���j
/// </summary>
public class JankenNPC : MonoBehaviour, IHandDecider
{
    [SerializeField] private float waitDecidedTime = 0.5f;

    // �Ō�Ɍ��߂�ꂽ��
    private JankenHand decidedHand;
    private IJankenAI ai;
    public void AI(IJankenAI newAI) => ai = newAI;
    
    // ���߂����ǂ���
    private bool isDecided;

    // --- IHandDecider �̃v���p�e�B����

    public JankenHand SelectHand => decidedHand;
	//public bool IsDecided => isDecided;

	private void Start()
	{
         // ����AI�������_���ɐݒ�
        ai = new RandomAI();
        //jankenNPC.SetAI(new CounterLastPlayerMoveAI());
    }

	/// <summary>
	/// ������߂�
	/// </summary>
	/// <returns></returns>
	public IEnumerator DecideHand()
    {
        isDecided = false;

        // 0.5�b�قǔY�މ��o
        yield return new WaitForSeconds(waitDecidedTime);

        // AI�Ŏ�����߂�
        //decidedHand = Decide();
        var history = JankenHistoryManager.Instance.GetAllHistory();
        decidedHand = ai.Decide(history);
    }

    //// --- IJankenAI �̎���

    ///// <summary>
    ///// GameAI( ������߂� )
    ///// </summary>
    ///// <returns>���܂�����</returns>
    //public JankenHand Decide()
    //{
    //    // 
    //    // Dictionary<�ȁA>
    //    // �I�����Ҍ��݂̕�][�ǂ̎肩��n�܂��].Decide
    //    // �����œ����肵���o���Ȃ����肾�Ƃ�����
    //    // �����p�^�[��   �Ƃ������O��t�����Ƃ���

    //    // PlayerHand= npc[�����p�^�[��][�O�[].Decide
    //    // ��
    //    // ����
    //    // �������̂�I�ԕȂ������ăO�[�Ȃ�O�[�΂���o���Ă��܂��Ȃ�
    //    // �p�[���o���Ώ��Ă�
    //    // �݂����ȕ��ɏ�������
    //    Debug.Log($"��΂ꂽ");
    //    int decideNumber = 0;

    //    var dataHistory = JankenHistoryManager.Instance.GetAllHistory();

    //    // ����񂯂�AI
    //    // ���ȏ�f�[�^������΁A�Q�l�ɂ��߂�
    //    if (dataHistory.Count > 0)
    //    {
    //        var last = dataHistory[dataHistory.Count - 1];

    //        switch (last.PlayerHand)
    //        {
    //            case JankenHand.Rock:
    //                decideNumber = (int)JankenHand.Paper;
    //                break;
    //            case JankenHand.Paper:
    //                decideNumber = (int)JankenHand.Scissors;
    //                break;
    //            case JankenHand.Scissors:
    //                decideNumber = (int)JankenHand.Rock;
    //                break;
    //        }
    //    }
    //    else
    //    {
    //        // ���Ղ̓����_��
    //        // JankenHand.Num�͎�̑����i3�j��\���̂ŁA0�`2�̃����_���l���g���ăL���X�g
    //        decideNumber = Random.Range(0, (int)JankenHand.Num);
    //    }

    //    isDecided = true;
    //    return (JankenHand)decideNumber;
    //}





    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////

    // �{����DI��Factory�p�^�[���Œ���
}

//| ����          | �N���X                  | �����b�g                                 |
//| --------      | -------------------     | ---------------------                    |
//| ������߂�헪| `IJankenAI` +�h���N���X | AI��ύX�E�g�����₷���Ȃ�               |
//| NPC���o�E�A�g | `JankenNPC`             | �\����AI�����m�ɕ�������A�e�X�g���₷�� |
//| ���W�b�N�Ǘ�  | `JankenManager`         | �X�e�[�g����ɏW���ł���                 |

