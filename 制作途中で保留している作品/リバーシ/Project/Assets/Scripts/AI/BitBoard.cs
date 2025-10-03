using ReversiConstants;
using UnityEngine;

public class BitBoard
{
    public ulong Black { get; }
    public ulong White { get; }
    public bool IsBlackTurn { get; }

    public ulong All => Black | White;

    public BitBoard(ulong black, ulong white, bool isBlackTurn)
	{
        Black = black;
        White = white;
        IsBlackTurn = isBlackTurn;
	}

    public BitBoard(BitBoard board)
	{
        Black = board.Black;
        White = board.White;
        IsBlackTurn = board.IsBlackTurn;
	}

    /// <summary>
    /// 局面の初期化処理
    /// 黒が先手固定
    /// </summary>
    /// <returns>初期化された局面</returns>
    public static BitBoard Init()
	{
        // 最初は黒から始まる
        return new BitBoard(
            (1UL << 27) | (1UL << 36),
            (1UL << 28) | (1UL << 35),
            true
            );
	}

    public BitBoard Flip(ulong position)
    {
        ulong flips = 0;  // ここは0で初期化
        ulong player = IsBlackTurn ? Black : White;
        ulong opponent = IsBlackTurn ? White : Black;  // 修正: 相手は逆ターンの石

        foreach (var dir in GameConstants.AllDirections)
        {
            int shift = (int)dir.direction;
            ulong mask = dir.mask;
            flips |= BitBoardUtils.GetFlipsInDirectino(position, player, opponent, shift, mask);
        }

        ulong newBlack = Black;
        ulong newWhite = White;

        if (IsBlackTurn)
        {
            newBlack |= position | flips;
            newWhite &= ~flips;
        }
        else
        {
            newWhite |= position | flips;
            newBlack &= ~flips;
        }

        return new BitBoard(newBlack, newWhite, !IsBlackTurn);
    }

    public ulong GenerateLegalMoves()
	{
        ulong player = IsBlackTurn ? Black : White;
        ulong opponent = IsBlackTurn ? White : Black;
        ulong empty = ~(player | opponent);
        ulong placeable = 0;

        foreach (var dir in GameConstants.AllDirections)
        {
            int shift = (int)dir.direction;
            ulong mask = dir.mask;
            placeable |= BitBoardUtils.GenerateMovesInDirection(player, opponent, shift, mask);
        }

        return placeable & empty;
    }


    public bool IsGameOver()
    {
        if ((Black | White) == GameConstants.FullBoardMask || Black == 0 || White == 0)
            return true;

        bool blackHasMoves = GenerateLegalMoves() != 0;
        var switched = new BitBoard(Black, White, !IsBlackTurn);
        bool whiteHasMoves = switched.GenerateLegalMoves() != 0;

        return !blackHasMoves && !whiteHasMoves;
    }

    /// <summary>
    /// 評価関数／局面を評価する
    /// </summary>
    /// <returns></returns>
    public int Evaluate()
    {
        int blackCount = BitOperations.PopCount(Black);
        int whiteCount = BitOperations.PopCount(White);
        return blackCount - whiteCount;
    }

    public static ulong PositionToBit(int x, int y)
    {
        return 1UL << (y * 8 + x);
    }

    public ulong BlackDiscs => Black;
    public ulong WhiteDiscs => White;

    /// <summary>
    /// 指定インデックスに石が存在するか確認
    /// </summary>
    public bool HasDiscAt(int index)
    {
        ulong mask = 1UL << index;
        return (All & mask) != 0;
    }

    /// <summary>
    /// Vector2Int 盤面座標に石が存在するか確認
    /// </summary>
    public bool HasDiscAt(Vector2Int pos)
    {
        return HasDiscAt(pos.y * 8 + pos.x);
    }

    /// <summary>
    /// 黒を置く（反転含む）
    /// </summary>
    public BitBoard PlaceBlackDisc(ulong position)
    {
        if (!IsBlackTurn)
            Debug.LogWarning("白ターンなのに黒を置こうとしています");

        return Flip(position);
    }

    /// <summary>
    /// 白を置く（反転含む）
    /// </summary>
    public BitBoard PlaceWhiteDisc(ulong position)
    {
        if (IsBlackTurn)
            Debug.LogWarning("黒ターンなのに白を置こうとしています");

        return Flip(position);
    }

}



//| 関数           | 処理 |
//| -------------- | ---------------------------- |
//| **PopCount * * | Brian Kernighan法：ビット1を1個ずつ潰す |
//| **Forward * *  | 下位ビットから数えて最初に1が立つビット位置 |
//| **Reverse * *  | 上位ビットから数えて最初に1が立つビット位置 |

/// <summary>
/// 自作ビット演算クラス
/// </summary>
public static class BitOperations
{
    public static int BitScanForward(ulong bb)
    {
        if (bb == 0) return -1;
        int index = 0;
        while ((bb & 1) == 0)
        {
            bb >>= 1;
            index++;
        }
        return index;
    }

    public static int BitScanReverse(ulong bb)
    {
        if (bb == 0) return -1;
        int index = 63;
        while ((bb & (1UL << 63)) == 0)
        {
            bb <<= 1;
            index--;
        }
        return index;
    }


    public static int PopCount(ulong x)
    {
        // Brian Kernighan's algorithm
        int count = 0;
        while (x != 0)
        {
            x &= (x - 1);
            count++;
        }
        return count;
    }
}