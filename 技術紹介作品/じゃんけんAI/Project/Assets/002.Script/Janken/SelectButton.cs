/// <summary>
/// 各アイコンのボタンにアタッチする用
/// 自分がじゃんけんのどの手なのか教える
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

	// OnClickに登録できる関数は"引数なし"のみ
	public void SelectThisHand()
    {
		//Debug.Log("ボタン押されたよ！");
        player.SetSelectHand(handToSelect);
    }
}
