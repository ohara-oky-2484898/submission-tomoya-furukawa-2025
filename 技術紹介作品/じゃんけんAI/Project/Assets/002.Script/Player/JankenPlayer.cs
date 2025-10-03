using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Player
/// ①手の決定ができる
/// ②その手を出すことができる
/// 
/// ③入力することができる(入力をすることで選択、決定)
/// </summary>
public class JankenPlayer : MonoBehaviour, IHandDecider
{
    // 最後に決められた手
    private JankenHand decidedHand;
    // 決めたかどうか
    private bool isDecided;

    // --- IHandDecider のプロパティ実装

    public JankenHand SelectHand => decidedHand;
    //public bool IsDecided => isDecided;

    /// <summary>
    /// 手を決める
    /// </summary>
    /// <returns></returns>
    public IEnumerator DecideHand()
    {

        isDecided = false;

        // 手が選ばれるまで待つ処理
        UIManager.Instance.ButtonSelectText(true);

        yield return new WaitUntil(() => isDecided);//条件); 条件が成立するまで待つ

        // UI入力待ちとか、ボタンのクリック待ちとか非表示
        UIManager.Instance.ButtonSelectText(false);
    }

    /// <summary>
    /// まだ手を決めていないとき手を決める
    /// ボタンから設定してる
    /// </summary>
    /// <param name="hand"></param>
    public void SetSelectHand(JankenHand hand)
    {
        //Debug.Log("選択呼ばれたよ！");
        // 選択可能の場合のみ
        if (!isDecided)
		{
			decidedHand = hand;
            isDecided = true;// 決定した
        }
    }

    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////
}
