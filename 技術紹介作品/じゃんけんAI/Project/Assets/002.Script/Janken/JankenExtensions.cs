/// <summary>
/// 共通処理など
/// 
/// 拡張メソッド用
/// (this IHandDecider self, IHandDecider other)
/// ↑これをつけることで呼び出し側が
/// player.〇〇(npc)
/// このようにそいつのメソッドかのように扱えるようになる！
/// </summary>
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class JankenExtensions
{
    /// <summary>
    /// 自分の手が相手に対して有利かどうかを判定する拡張メソッド
    /// </summary>
    /// <param name="self">自分</param>
    /// <param name="other">相手</param>
    /// <returns> 勝敗 </returns>
    public static bool IsAdvantageous(this IHandDecider self, IHandDecider other)
    {
        return JankenLogic.IsAdvantageous(self.SelectHand, other.SelectHand);
    }

    /// <summary>
    /// 自分の手と相手の手が同じかどうかを判定する拡張メソッド
    /// </summary>
    /// <param name="self">自分</param>
    /// <param name="other">相手</param>
    /// <returns> 引き分けかどうか </returns>
    public static bool IsDraw(this IHandDecider self, IHandDecider other)
    {
        return JankenLogic.IsDraw(self.SelectHand, other.SelectHand);
    }

    ///// <summary>
    ///// 手が選ばれるまで待つ（コルーチン用）の拡張メソッド
    ///// </summary>
    //public static IEnumerator WaitForHandSelection(this IHandDecider self)
    //{
    //    //if (!self.IsDecided && self is JankenNPC)
    //    //{
    //    //    yield return new WaitForSeconds(1.0f); // NPCが悩む演出
    //    //    self.DecideHand(); // 自動で決定させる
    //    //}
    //    yield return new WaitUntil(() => self.IsDecided);
    //}
}

// コルーチン
// Unityにおける処理を時間で分割して行う仕組み
// 通常のメソッドは "一気にすべて実行される" が
// コルーチンを使えば特定のタイミングまで一時停止してから再開可能

//普通のメソッド：一気にしゃべる人（止まらない）
//コルーチン：途中で「ちょっと待ってね！」って言ってくれる人

// 基本の形
//IEnumerator MyCoroutine()
//{
//    Debug.Log("処理1");

//    // 1秒待つ
//    yield return new WaitForSeconds(1f);

//    Debug.Log("処理2");
//}

// StartCoroutine(MyCoroutine());

//書き方 意味
//yield return null; 1フレーム待つ
//yield return new WaitForSeconds(1); 指定した秒数待つ
//yield return new WaitUntil(() => 条件); 条件が成立するまで待つ
//yield return new WaitWhile(() => 条件); 条件が成立している間待つ
//yield break; コルーチンを途中で終了する

//シーン例 使いどころ
//演出	UIに「3...2...1...スタート！」と表示
//アニメーション	動きの間に時間差をつけたいとき
//入力待ち	プレイヤーがボタンを押すまで処理を止めたいとき
//フェードイン・アウト	時間をかけて透明度を変える処理
//敵の攻撃パターン	一定時間ごとに攻撃するロジック

//注意点 内容
//yield return がないと意味がない	普通の関数と変わらなくなる
//StartCoroutine しないと動かない	呼び出しを忘れると動かない
//MonoBehaviourでしか使えない	他のクラスでは基本使えない
