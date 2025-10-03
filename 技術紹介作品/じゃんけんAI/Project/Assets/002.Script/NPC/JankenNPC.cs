/// <summary>
/// じゃんけんのNPC
/// </summary>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// NPC
/// ①手の決定ができる
/// ②その手を出すことができる
/// ③手の決定はIJankenAIを使う（持っている）
/// </summary>
public class JankenNPC : MonoBehaviour, IHandDecider
{
    [SerializeField] private float waitDecidedTime = 0.5f;

    // 最後に決められた手
    private JankenHand decidedHand;
    private IJankenAI ai;
    public void AI(IJankenAI newAI) => ai = newAI;
    
    // 決めたかどうか
    private bool isDecided;

    // --- IHandDecider のプロパティ実装

    public JankenHand SelectHand => decidedHand;
	//public bool IsDecided => isDecided;

	private void Start()
	{
         // 初期AIをランダムに設定
        ai = new RandomAI();
        //jankenNPC.SetAI(new CounterLastPlayerMoveAI());
    }

	/// <summary>
	/// 手を決める
	/// </summary>
	/// <returns></returns>
	public IEnumerator DecideHand()
    {
        isDecided = false;

        // 0.5秒ほど悩む演出
        yield return new WaitForSeconds(waitDecidedTime);

        // AIで手を決める
        //decidedHand = Decide();
        var history = JankenHistoryManager.Instance.GetAllHistory();
        decidedHand = ai.Decide(history);
    }

    //// --- IJankenAI の実装

    ///// <summary>
    ///// GameAI( 手を決める )
    ///// </summary>
    ///// <returns>決まった手</returns>
    //public JankenHand Decide()
    //{
    //    // 
    //    // Dictionary<癖、>
    //    // 選択肢者現在の癖][どの手から始まる癖].Decide
    //    // ここで同じ手しか出さない相手だとしたら
    //    // ワンパターン   という名前を付けたとして

    //    // PlayerHand= npc[ワンパターン][グー].Decide
    //    // ↓
    //    // 結果
    //    // 同じものを選ぶ癖があってグーならグーばかり出してしまうなら
    //    // パーを出せば勝てる
    //    // みたいな風に書きたい
    //    Debug.Log($"よばれた");
    //    int decideNumber = 0;

    //    var dataHistory = JankenHistoryManager.Instance.GetAllHistory();

    //    // じゃんけんAI
    //    // 一定以上データがあれば、参考にきめる
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
    //        // 序盤はランダム
    //        // JankenHand.Numは手の総数（3）を表すので、0～2のランダム値を使ってキャスト
    //        decideNumber = Random.Range(0, (int)JankenHand.Num);
    //    }

    //    isDecided = true;
    //    return (JankenHand)decideNumber;
    //}





    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////

    // 本来はDIやFactoryパターンで注入
}

//| 役割          | クラス                  | メリット                                 |
//| --------      | -------------------     | ---------------------                    |
//| 手を決める戦略| `IJankenAI` +派生クラス | AIを変更・拡張しやすくなる               |
//| NPC演出・連携 | `JankenNPC`             | 表示とAIが明確に分離され、テストしやすい |
//| ロジック管理  | `JankenManager`         | ステート制御に集中できる                 |

