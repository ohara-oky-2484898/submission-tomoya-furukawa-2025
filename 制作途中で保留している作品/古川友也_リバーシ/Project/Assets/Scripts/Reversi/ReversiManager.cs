using UnityEngine;
using ReversiConstants;

public class ReversiManager
{
    private BitBoard _board;

    public BitBoard Board => _board;

    public ReversiManager()
    {
        _board = BitBoard.Init();
    }

    /// <summary>
    /// 指定した盤面位置（x,y）に石を置く
    /// </summary>
    /// <param name="x">0～7</param>
    /// <param name="y">0～7</param>
    /// <returns>置けたらtrue、置けなければfalse</returns>
    public bool TryPlaceDisc(int x, int y)
    {
        ulong pos = BitBoard.PositionToBit(x, y);

        if (!IsLegalMove(pos))
        {
            Debug.LogWarning($"置けない場所です: ({x},{y})");
            return false;
        }

        if (_board.IsBlackTurn)
        {
            _board = _board.PlaceBlackDisc(pos);
        }
        else
        {
            _board = _board.PlaceWhiteDisc(pos);
        }

        return true;
    }

    /// <summary>
    /// 合法手かどうかを判定する
    /// </summary>
    public bool IsLegalMove(ulong pos)
    {
        ulong legalMoves = _board.GenerateLegalMoves();
        return (legalMoves & pos) != 0;
    }

    /// <summary>
    /// 現在の合法手をビットマップで取得する
    /// </summary>
    public ulong GetLegalMoves()
    {
        return _board.GenerateLegalMoves();
    }

    /// <summary>
    /// 合法手があるかどうか（パス判定）
    /// </summary>
    public bool HasLegalMove()
    {
        return GetLegalMoves() != 0;
    }

    /// <summary>
    /// 現局面がゲーム終了状態かどうか
    /// </summary>
    public bool IsGameOver()
    {
        return _board.IsGameOver();
    }

    /// <summary>
    /// 黒の石の数
    /// </summary>
    public int CountBlackDiscs()
    {
        return BitOperations.PopCount(_board.BlackDiscs);
    }

    /// <summary>
    /// 白の石の数
    /// </summary>
    public int CountWhiteDiscs()
    {
        return BitOperations.PopCount(_board.WhiteDiscs);
    }

    /// <summary>
    /// 現在のターンが黒かどうか
    /// </summary>
    public bool IsBlackTurn()
    {
        return _board.IsBlackTurn;
    }

    /// <summary>
    /// ターンをパスする（合法手がない時に使用）
    /// </summary>
    /// <returns>パス成功したらtrue</returns>
    public bool PassTurn()
    {
        if (HasLegalMove())
        {
            Debug.LogWarning("パスはできません。合法手があります。");
            return false;
        }
        // ターンだけ交代して局面は変えない
        _board = new BitBoard(_board.BlackDiscs, _board.WhiteDiscs, !_board.IsBlackTurn);
        return true;
    }
}
