/// <summary>
/// ビットボードでオセロを作ってみる
/// </summary>
using System.Collections.Generic;
using ReversiConstants;
// 左シフト掛け算、右シフト割り算2~n
// 右シフトのはみだしはあまり、左シフトはオーバーフロー
// 論理シフト　→符号考慮しない
// 算術シフト　→符号考慮する

// 合法手

// MEMO：bit演算
// &(AND)       ：１になる←両方１のものだけ
// |(OR)        ：１になる←片方でも１なら
// ^(XOR)       ：１になる←片方だけ１なら
// ~(NOT)       ：１になる←０/１を反転させる
// <<(左シフト) ：１になる←右辺の数だけずらす
// >>(右シフト) ：１になる←右辺の数だけずらす

// 盤面は全てをor計算
// 特定のbitを処理をすることをbitマスクという
// 求めた盤面すべてにnot(反転させて)全て１のbitとor計算すると
　

// 自身を書き換えているため
// MinMaxやαβ法を使った際
// 再起したら元が壊れてしまう、破綻


/// <summary>
/// 
/// </summary>
public class ReversiLogic
{
    // オセロは 8 × 8 の64マスなので64bitでかつ、
    // 符号bitが邪魔になるのでulong を使う
    // 黒用と白用で2つ使う理由は、0/1 の表現だけでは
    // 1つだけだと黒が置かれていないときに白が置かれていることになってしまうため

	/// <summary> 局面 </summary>
	private ulong blackDisk;
	private ulong whiteDisk;

	private bool isBlackTurn;

    public ulong AllDisc => blackDisk | whiteDisk;

    public ulong BlackDisk => blackDisk;
    // 黒石を置くメソッド
    public void SetBlackDisk(ulong bitPosition)
    {
        blackDisk |= bitPosition;  // OR 演算で指定位置に黒石をセット
    }
    public ulong WhiteDisk => whiteDisk;

    public void SetWhiteDisk(ulong bitPosition)
    {
        whiteDisk |= bitPosition;  // OR 演算で指定位置に黒石をセット
    }

    /// <summary>
    /// 現在どっちのターンか
    /// </summary>
    public DiscColors CurrentTurnColor => isBlackTurn ? DiscColors.black : DiscColors.white;

    public bool IsBlackTurn
	{
        get => isBlackTurn;
        set => isBlackTurn = value;
	}

    public void Init()
	{
		// 初期配置(クロス)の設定
		// blackDisk = 0b00000000_00000000_00000000_00010000_00001000_00000000_00000000_00000000;
		// (1UL << 27) | (1UL << 36)これは↑と同じことをしてる
		// "1UL" は "1" を表すつまり "...0001" となってこれを
		// " << n " n ビットシフト( n 個左にずらす)
		// "|" (ビットOR演算子)
		// 複数同時のに 1 にしたいときに使う
        blackDisk = (1UL << 28) | (1UL << 35);
        whiteDisk = (1UL << 27) | (1UL << 36);


		isBlackTurn = true;  // 最初は黒のターン
	}


    /// <summary>
    /// 置ける場所をチェックして取得
    /// </summary>
    /// <param name="player">player側の石の配置</param>
    /// <param name="opponent">対戦相手の石の配置</param>
    /// <param name="placeable">対戦相手の石の配置</param>
    /// <returns>置ける場所が一つでもあればtrue／なければfalse</returns>
    public bool GetPlaceableDiscs(ulong player, ulong opponent, out ulong placeable)
    {
        // 空いてる場所を取得
        ulong empty = ~(player | opponent);
        // 置ける場所
        placeable = 0;

        placeable |= CalcDir(player, opponent, (int)DirectionList.Right, GameConstants.NotHFile);      // →
        placeable |= CalcDir(player, opponent, (int)DirectionList.Left, GameConstants.NotAFile);     // ←
        placeable |= CalcDir(player, opponent, (int)DirectionList.Down, GameConstants.FullBoardMask); // ↓
        placeable |= CalcDir(player, opponent, (int)DirectionList.Up, GameConstants.FullBoardMask);// ↑
        placeable |= CalcDir(player, opponent, (int)DirectionList.UpLeft, GameConstants.NotAFile);     // ?
        placeable |= CalcDir(player, opponent, (int)DirectionList.UpRight, GameConstants.NotHFile);     // ?
        placeable |= CalcDir(player, opponent, (int)DirectionList.DownLeft, GameConstants.NotAFile);      // ?
        placeable |= CalcDir(player, opponent, (int)DirectionList.DownRight, GameConstants.NotHFile);      // 

        placeable &= empty;

        return placeable != 0;
    }


    /// <summary>
    /// 指定の方向に対してplayerの石で相手の石を挟んで裏返せる位置を計算
    /// </summary>
    /// <param name="player">playerの石の配置</param>
    /// <param name="opponent">相手の石の配置</param>
    /// <param name="shift">調べる方向のシフト量</param>
    /// <param name="mask">基盤端のオーバーフロー防止マスク</param>
    /// <returns></returns>
    private ulong CalcDir(ulong player, ulong opponent, int shift, ulong mask)
    {
        ulong flips = 0;
        ulong temp = 0;

        if (shift > 0)
            temp = mask & (player << shift);
        // マイナスシフトはできないため符号とシフト方向を逆にしてる
        else
            temp = mask & (player >> -shift);

        temp &= opponent;

        for (int i = 0; i < 5; i++)
        {
            ulong next;
            if (shift > 0)
                next = mask & (temp << shift);
            else
                next = mask & (temp >> -shift);

            next &= opponent;

            if (next == 0) break;

            temp |= next;
        }

        ulong potential;
        if (shift > 0)
            potential = mask & (temp << shift);
        else
            potential = mask & (temp >> -shift);

        return potential;
    }

    /// <summary>
    /// 石を置いたときひっくり返す処理
    /// </summary>
    public void FlipDiscs(ulong position)
    {
        ulong flips = 0;
        ulong player = isBlackTurn ? blackDisk : whiteDisk;
        ulong opponent = isBlackTurn ? whiteDisk : blackDisk;

        flips |= GetFlipDiscs(position, player, opponent, (int)DirectionList.Right, GameConstants.NotHFile);
        flips |= GetFlipDiscs(position, player, opponent, (int)DirectionList.Left, GameConstants.NotAFile);
        flips |= GetFlipDiscs(position, player, opponent, (int)DirectionList.Down, GameConstants.FullBoardMask);
        flips |= GetFlipDiscs(position, player, opponent, (int)DirectionList.Up, GameConstants.FullBoardMask);
        flips |= GetFlipDiscs(position, player, opponent, (int)DirectionList.UpLeft, GameConstants.NotAFile);
        flips |= GetFlipDiscs(position, player, opponent, (int)DirectionList.UpRight, GameConstants.NotHFile);
        flips |= GetFlipDiscs(position, player, opponent, (int)DirectionList.DownLeft, GameConstants.NotAFile);
        flips |= GetFlipDiscs(position, player, opponent, (int)DirectionList.DownRight, GameConstants.NotHFile);

        if (isBlackTurn)
        {
            blackDisk |= position | flips;
            whiteDisk &= ~flips;
        }
        else
        {
            whiteDisk |= position | flips;
            blackDisk &= ~flips;
        }
    }

    public ulong GetFlipDiscs(ulong position, ulong player, ulong opponent, int shift, ulong mask)
	{
        ulong flip = 0;
        ulong temp = 0;

        if (shift > 0)
            temp = mask & (position << shift);
        else
            temp = mask & (position >> -shift);

        if ((temp & opponent) == 0) return 0;

        flip |= temp;

        while (true)
        {
            if (shift > 0)
                temp = mask & (temp << shift);
            else
                temp = mask & (temp >> -shift);

            if (temp == 0) return 0;

            if ((temp & player) != 0) return flip; // 自分の石で閉じた→flip確定

            flip |= temp;
            if ((temp & opponent) == 0) return 0;
        }

    }





    // ゲーム終了判定
    public bool GameFinish()
	{
        // どちらかの石が1つもない(つまり、そのどちらかが挟めない = 置けないため終了)
        bool zeroPoint = blackDisk == 0 || whiteDisk == 0;
        // すべて埋まっている
        bool maxBoard = (blackDisk | whiteDisk) == GameConstants.FullBoardMask;

        // どちらかが一つもない
        // または全部埋まっている
        return zeroPoint || maxBoard;
    }
}
