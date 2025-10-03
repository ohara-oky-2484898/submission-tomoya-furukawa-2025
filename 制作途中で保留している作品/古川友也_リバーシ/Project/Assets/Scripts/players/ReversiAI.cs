using UnityEngine;
using ReversiConstants;
using System.Collections.Generic;
using System.Linq;

public class ReversiAI : IReversiPlayer
{
    public DiscColors MyDiscColor { get; set; }

    public ReversiAI(DiscColors discColor)
    {
        MyDiscColor = discColor;
    }

    public void Init() { }
    public void Update() { }

    public bool PlaceDisk(BitBoard currentBoard, out BitBoard nextBoard)
    {
        int depth = 4;
        bool isMaximizing = (MyDiscColor == DiscColors.black) == currentBoard.IsBlackTurn;
        (int bestEval, BitBoard bestBoard) = Minimax(currentBoard, depth, int.MinValue, int.MaxValue, isMaximizing);

        if (bestBoard.Equals(currentBoard))
        {
            Debug.Log("AI: 置ける場所がありません（パス）");
            nextBoard = currentBoard;
            return false;
        }

        nextBoard = bestBoard;
        return true;
    }

    private (int, BitBoard) Minimax(BitBoard board, int depth, int alpha, int beta, bool maximizingPlayer)
    {
        if (depth == 0 || board.IsGameOver())
        {
            int eval = board.Evaluate();
            return (eval, board);
        }

        ulong legalMoves = board.GenerateLegalMoves();

        if (legalMoves == 0)
        {
            // パス手を打たせる
            BitBoard passed = new BitBoard(board.Black, board.White, !board.IsBlackTurn);
            return Minimax(passed, depth, alpha, beta, !maximizingPlayer);
        }

        List<BitBoard> nextBoards = new List<BitBoard>();
        ulong temp = legalMoves;

        while (temp != 0)
        {
            int pos = BitOperations.BitScanForward(temp);
            ulong move = 1UL << pos;
            BitBoard next = board.Flip(move);
            nextBoards.Add(next);
            temp &= temp - 1;
        }

        Shuffle(nextBoards);

        if (maximizingPlayer)
        {
            int maxEval = int.MinValue;
            BitBoard bestBoard = board;

            foreach (var next in nextBoards)
            {
                int eval = Minimax(next, depth - 1, alpha, beta, false).Item1;
                if (eval > maxEval)
                {
                    maxEval = eval;
                    bestBoard = next;
                }
                alpha = Mathf.Max(alpha, eval);
                if (beta <= alpha) break;
            }
            return (maxEval, bestBoard);
        }
        else
        {
            int minEval = int.MaxValue;
            BitBoard bestBoard = board;

            foreach (var next in nextBoards)
            {
                int eval = Minimax(next, depth - 1, alpha, beta, true).Item1;
                if (eval < minEval)
                {
                    minEval = eval;
                    bestBoard = next;
                }
                beta = Mathf.Min(beta, eval);
                if (beta <= alpha) break;
            }
            return (minEval, bestBoard);
        }
    }

    /// <summary>
    /// 同じ評価値のとき手をランダム化するためのシャッフル
    /// </summary>
    private void Shuffle<T>(List<T> list)
    {
        for (int i = list.Count - 1; i >= 1; i--)
        {
            int j = Random.Range(0, i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }
}
