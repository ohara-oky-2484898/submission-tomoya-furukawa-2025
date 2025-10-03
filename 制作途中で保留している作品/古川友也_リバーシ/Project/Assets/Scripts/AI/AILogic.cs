using System.Collections.Generic;

public class AlternateMazeState
{
    public BitBoard Board { get; private set; }

    public AlternateMazeState(BitBoard board)
    {
        Board = board;
    }

    public List<int> LegalActions()
    {
        ulong moves = Board.GenerateLegalMoves();
        var actions = new List<int>();
        while (moves != 0)
        {
            int pos = BitOperations.BitScanForward(moves);
            moves &= ~(1UL << pos);
            actions.Add(pos);
        }
        return actions;
    }

    public void Advance(int action)
    {
        if (Board.IsBlackTurn)
            Board = Board.PlaceBlackDisc(1UL << action);
        else
            Board = Board.PlaceWhiteDisc(1UL << action);
    }

    public bool IsDone()
    {
        return Board.IsGameOver();
    }

    public int GetScore()
    {
        return Board.Evaluate();
    }

    public AlternateMazeState Clone()
    {
        return new AlternateMazeState(new BitBoard(Board));
    }
}


public static class MiniMax
{
    public static int MiniMaxScore(AlternateMazeState state, int depth)
    {
        if (state.IsDone() || depth == 0)
        {
            return state.GetScore();
        }

        var legalActions = state.LegalActions();
        if (legalActions.Count == 0)
        {
            return state.GetScore();
        }

        int bestScore = -int.MaxValue;

        foreach (var action in legalActions)
        {
            AlternateMazeState nextState = state.Clone();
            nextState.Advance(action);

            int score = -MiniMaxScore(nextState, depth - 1);

            if (score > bestScore)
            {
                bestScore = score;
            }
        }

        return bestScore;
    }
}



//public class AILogic : MonoBehaviour
//{
//	public int GetScore()
//	{
//		// 自身のコマ数+敵のコマ数
//		return 0;
//	}

//	public int MinMaxScore(readonly int score, const int depth)
//	{
//		return score + depth;
//	}
//}
