/// <summary>
/// �r�b�g�{�[�h�ŃI�Z��������Ă݂�
/// </summary>
using System.Collections.Generic;
using ReversiConstants;
// ���V�t�g�|���Z�A�E�V�t�g����Z2~n
// �E�V�t�g�݂̂͂����͂��܂�A���V�t�g�̓I�[�o�[�t���[
// �_���V�t�g�@�������l�����Ȃ�
// �Z�p�V�t�g�@�������l������

// ���@��

// MEMO�Fbit���Z
// &(AND)       �F�P�ɂȂ適�����P�̂��̂���
// |(OR)        �F�P�ɂȂ適�Е��ł��P�Ȃ�
// ^(XOR)       �F�P�ɂȂ適�Е������P�Ȃ�
// ~(NOT)       �F�P�ɂȂ適�O/�P�𔽓]������
// <<(���V�t�g) �F�P�ɂȂ適�E�ӂ̐��������炷
// >>(�E�V�t�g) �F�P�ɂȂ適�E�ӂ̐��������炷

// �Ֆʂ͑S�Ă�or�v�Z
// �����bit�����������邱�Ƃ�bit�}�X�N�Ƃ���
// ���߂��Ֆʂ��ׂĂ�not(���]������)�S�ĂP��bit��or�v�Z�����
�@

// ���g�����������Ă��邽��
// MinMax�⃿���@���g������
// �ċN�����猳�����Ă��܂��A�j�]


/// <summary>
/// 
/// </summary>
public class ReversiLogic
{
    // �I�Z���� 8 �~ 8 ��64�}�X�Ȃ̂�64bit�ł��A
    // ����bit���ז��ɂȂ�̂�ulong ���g��
    // ���p�Ɣ��p��2�g�����R�́A0/1 �̕\�������ł�
    // 1�������ƍ����u����Ă��Ȃ��Ƃ��ɔ����u����Ă��邱�ƂɂȂ��Ă��܂�����

	/// <summary> �ǖ� </summary>
	private ulong blackDisk;
	private ulong whiteDisk;

	private bool isBlackTurn;

    public ulong AllDisc => blackDisk | whiteDisk;

    public ulong BlackDisk => blackDisk;
    // ���΂�u�����\�b�h
    public void SetBlackDisk(ulong bitPosition)
    {
        blackDisk |= bitPosition;  // OR ���Z�Ŏw��ʒu�ɍ��΂��Z�b�g
    }
    public ulong WhiteDisk => whiteDisk;

    public void SetWhiteDisk(ulong bitPosition)
    {
        whiteDisk |= bitPosition;  // OR ���Z�Ŏw��ʒu�ɍ��΂��Z�b�g
    }

    /// <summary>
    /// ���݂ǂ����̃^�[����
    /// </summary>
    public DiscColors CurrentTurnColor => isBlackTurn ? DiscColors.black : DiscColors.white;

    public bool IsBlackTurn
	{
        get => isBlackTurn;
        set => isBlackTurn = value;
	}

    public void Init()
	{
		// �����z�u(�N���X)�̐ݒ�
		// blackDisk = 0b00000000_00000000_00000000_00010000_00001000_00000000_00000000_00000000;
		// (1UL << 27) | (1UL << 36)����́��Ɠ������Ƃ����Ă�
		// "1UL" �� "1" ��\���܂� "...0001" �ƂȂ��Ă����
		// " << n " n �r�b�g�V�t�g( n ���ɂ��炷)
		// "|" (�r�b�gOR���Z�q)
		// ���������̂� 1 �ɂ������Ƃ��Ɏg��
        blackDisk = (1UL << 28) | (1UL << 35);
        whiteDisk = (1UL << 27) | (1UL << 36);


		isBlackTurn = true;  // �ŏ��͍��̃^�[��
	}


    /// <summary>
    /// �u����ꏊ���`�F�b�N���Ď擾
    /// </summary>
    /// <param name="player">player���̐΂̔z�u</param>
    /// <param name="opponent">�ΐ푊��̐΂̔z�u</param>
    /// <param name="placeable">�ΐ푊��̐΂̔z�u</param>
    /// <returns>�u����ꏊ����ł������true�^�Ȃ����false</returns>
    public bool GetPlaceableDiscs(ulong player, ulong opponent, out ulong placeable)
    {
        // �󂢂Ă�ꏊ���擾
        ulong empty = ~(player | opponent);
        // �u����ꏊ
        placeable = 0;

        placeable |= CalcDir(player, opponent, (int)DirectionList.Right, GameConstants.NotHFile);      // ��
        placeable |= CalcDir(player, opponent, (int)DirectionList.Left, GameConstants.NotAFile);     // ��
        placeable |= CalcDir(player, opponent, (int)DirectionList.Down, GameConstants.FullBoardMask); // ��
        placeable |= CalcDir(player, opponent, (int)DirectionList.Up, GameConstants.FullBoardMask);// ��
        placeable |= CalcDir(player, opponent, (int)DirectionList.UpLeft, GameConstants.NotAFile);     // ?
        placeable |= CalcDir(player, opponent, (int)DirectionList.UpRight, GameConstants.NotHFile);     // ?
        placeable |= CalcDir(player, opponent, (int)DirectionList.DownLeft, GameConstants.NotAFile);      // ?
        placeable |= CalcDir(player, opponent, (int)DirectionList.DownRight, GameConstants.NotHFile);      // 

        placeable &= empty;

        return placeable != 0;
    }


    /// <summary>
    /// �w��̕����ɑ΂���player�̐΂ő���̐΂�����ŗ��Ԃ���ʒu���v�Z
    /// </summary>
    /// <param name="player">player�̐΂̔z�u</param>
    /// <param name="opponent">����̐΂̔z�u</param>
    /// <param name="shift">���ׂ�����̃V�t�g��</param>
    /// <param name="mask">��Ւ[�̃I�[�o�[�t���[�h�~�}�X�N</param>
    /// <returns></returns>
    private ulong CalcDir(ulong player, ulong opponent, int shift, ulong mask)
    {
        ulong flips = 0;
        ulong temp = 0;

        if (shift > 0)
            temp = mask & (player << shift);
        // �}�C�i�X�V�t�g�͂ł��Ȃ����ߕ����ƃV�t�g�������t�ɂ��Ă�
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
    /// �΂�u�����Ƃ��Ђ�����Ԃ�����
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

            if ((temp & player) != 0) return flip; // �����̐΂ŕ�����flip�m��

            flip |= temp;
            if ((temp & opponent) == 0) return 0;
        }

    }





    // �Q�[���I������
    public bool GameFinish()
	{
        // �ǂ��炩�̐΂�1���Ȃ�(�܂�A���̂ǂ��炩�����߂Ȃ� = �u���Ȃ����ߏI��)
        bool zeroPoint = blackDisk == 0 || whiteDisk == 0;
        // ���ׂĖ��܂��Ă���
        bool maxBoard = (blackDisk | whiteDisk) == GameConstants.FullBoardMask;

        // �ǂ��炩������Ȃ�
        // �܂��͑S�����܂��Ă���
        return zeroPoint || maxBoard;
    }
}
