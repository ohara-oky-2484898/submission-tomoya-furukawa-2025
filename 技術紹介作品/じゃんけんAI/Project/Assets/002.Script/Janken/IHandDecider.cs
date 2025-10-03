
using System.Collections;
/// <summary>
/// 手を決めることができるクラスのインタフェース
/// </summary>
public enum JankenHand
{
    Rock,      // グー
    Paper,     // パー
    Scissors,   // チョキ
    Num         // 総数
}


// インタフェース
// インタフェースをみればなにができるかわかるようにする

/// <summary>
/// 手を決めることができる
///// 手を出すことができる
/// </summary>
public interface IHandDecider
{
    // プロパティ
    JankenHand SelectHand { get; }

    //bool IsDecided { get; } // 決めたか

    /// <summary>
    /// 手を決める処理
    /// "入力"ではなく"選定"表現
    /// </summary>
    /// <returns>決めた手</returns>
    IEnumerator DecideHand();
    // ↓これだと選択できるみたいに思えたので変更
    //JankenHand HandSelection(); 


    /// <summary>
    ///  選んだ画像を表示する処理
    /// </summary>
    //void DisplayHand();
    // ↓これだと手を生成できるみたいに思えたので変更
    //void GenerateHand();
}
