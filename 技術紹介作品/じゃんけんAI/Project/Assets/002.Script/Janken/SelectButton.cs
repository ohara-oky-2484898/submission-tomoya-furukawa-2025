/// <summary>
/// �e�A�C�R���̃{�^���ɃA�^�b�`����p
/// ����������񂯂�̂ǂ̎�Ȃ̂�������
/// </summary>

using UnityEngine;
using UnityEngine.Events;

public class SelectButton : MonoBehaviour
{
    [SerializeField] private JankenHand handToSelect;
    [SerializeField] private JankenPlayer player;

	private void Start()
	{
		if(player == null)
		{
			GameObject obj = GameObject.FindGameObjectWithTag("Player");
			player = obj.GetComponent<JankenPlayer>();
		}
	}

	// OnClick�ɓo�^�ł���֐���"�����Ȃ�"�̂�
	public void SelectThisHand()
    {
		//Debug.Log("�{�^�������ꂽ��I");
        player.SetSelectHand(handToSelect);
    }
}
