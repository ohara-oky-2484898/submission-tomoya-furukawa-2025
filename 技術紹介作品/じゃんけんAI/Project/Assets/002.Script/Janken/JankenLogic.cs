/// <summary>
/// じゃんけんのルールに関するもの
/// </summary>
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class JankenLogic
{

    /// <summary>
    /// じゃんけんの勝敗をきめるだけのもの
    /// </summary>
    /// <param name="myHand">自分の手</param>
    /// <param name="opponentHand">相手の手</param>
    /// <returns> 勝敗 </returns>
    public static bool IsAdvantageous(JankenHand myHand, JankenHand opponentHand)
    {
        switch (myHand)
        {
            case JankenHand.Rock:
                // 自分がグーの時、相手がチョキなら有利
                return opponentHand == JankenHand.Scissors;
            case JankenHand.Scissors:
                // 自分がチョキの時、相手がパーなら有利だよね
                return opponentHand == JankenHand.Paper;
            case JankenHand.Paper:
                // 自分がパーの時、相手がグーなら有利だよね
                return opponentHand == JankenHand.Rock;

            default:
                // 例外
                return false;
        }
        //return false;
    }

    /// <summary>
    /// 引き分けかどうか決めるだけのもの
    /// </summary>
    /// <param name="player">自分の手</param>
    /// <param name="opponent">相手の手</param>
    /// <returns> 引き分けかどうか </returns>
    public static bool IsDraw(JankenHand myHand, JankenHand opponentHand)
    {
        return myHand == opponentHand;
    }


    /// <summary>
    /// 受け取った手に対して有利な手を返す
    /// </summary>
    /// <param name="hand">受け取る手</param>
    /// <returns>受け取った手に有利な手</returns>
    public static JankenHand AdvantageousHand(JankenHand hand)
    {
        switch (hand)
        {
            case JankenHand.Rock:
                return JankenHand.Paper;    // グーにはパーが有利
            case JankenHand.Paper:
                return JankenHand.Scissors; // パーにはチョキが有利
            case JankenHand.Scissors:
                return JankenHand.Rock;     // チョキにはグーが有利
            default:
                return hand;  // 他の場合（あり得ないが一応）
        }
    }
    
    
    /// <summary>
    /// 受け取った手に対して不利な手を返す
    /// </summary>
    /// <param name="hand">受け取る手</param>
    /// <returns>受け取った手に不利な手</returns>
    public static JankenHand DisadvantageousHand(JankenHand hand)
    {
        switch (hand)
        {
            case JankenHand.Rock:
                return JankenHand.Scissors;    // グーにはチョキが不利
            case JankenHand.Paper:
                return JankenHand.Rock; // パーにはグーが不利
            case JankenHand.Scissors:
                return JankenHand.Paper;     // チョキにはパーが不利
            default:
                return hand;  // 他の場合（あり得ないが一応）
        }
    }



}

