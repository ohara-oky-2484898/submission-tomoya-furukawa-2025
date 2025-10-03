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
    /// �w�肵���Ֆʈʒu�ix,y�j�ɐ΂�u��
    /// </summary>
    /// <param name="x">0�`7</param>
    /// <param name="y">0�`7</param>
    /// <returns>�u������true�A�u���Ȃ����false</returns>
    public bool TryPlaceDisc(int x, int y)
    {
        ulong pos = BitBoard.PositionToBit(x, y);

        if (!IsLegalMove(pos))
        {
            Debug.LogWarning($"�u���Ȃ��ꏊ�ł�: ({x},{y})");
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
    /// ���@�肩�ǂ����𔻒肷��
    /// </summary>
    public bool IsLegalMove(ulong pos)
    {
        ulong legalMoves = _board.GenerateLegalMoves();
        return (legalMoves & pos) != 0;
    }

    /// <summary>
    /// ���݂̍��@����r�b�g�}�b�v�Ŏ擾����
    /// </summary>
    public ulong GetLegalMoves()
    {
        return _board.GenerateLegalMoves();
    }

    /// <summary>
    /// ���@�肪���邩�ǂ����i�p�X����j
    /// </summary>
    public bool HasLegalMove()
    {
        return GetLegalMoves() != 0;
    }

    /// <summary>
    /// ���ǖʂ��Q�[���I����Ԃ��ǂ���
    /// </summary>
    public bool IsGameOver()
    {
        return _board.IsGameOver();
    }

    /// <summary>
    /// ���̐΂̐�
    /// </summary>
    public int CountBlackDiscs()
    {
        return BitOperations.PopCount(_board.BlackDiscs);
    }

    /// <summary>
    /// ���̐΂̐�
    /// </summary>
    public int CountWhiteDiscs()
    {
        return BitOperations.PopCount(_board.WhiteDiscs);
    }

    /// <summary>
    /// ���݂̃^�[���������ǂ���
    /// </summary>
    public bool IsBlackTurn()
    {
        return _board.IsBlackTurn;
    }

    /// <summary>
    /// �^�[�����p�X����i���@�肪�Ȃ����Ɏg�p�j
    /// </summary>
    /// <returns>�p�X����������true</returns>
    public bool PassTurn()
    {
        if (HasLegalMove())
        {
            Debug.LogWarning("�p�X�͂ł��܂���B���@�肪����܂��B");
            return false;
        }
        // �^�[��������サ�ċǖʂ͕ς��Ȃ�
        _board = new BitBoard(_board.BlackDiscs, _board.WhiteDiscs, !_board.IsBlackTurn);
        return true;
    }
}
