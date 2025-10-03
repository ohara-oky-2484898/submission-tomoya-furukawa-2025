namespace ReversiConstants
{
    public enum DiscColors
	{
        black = 0,
        white,
        maxColorNumber,
	}

    public enum DirectionList
    {
        Left = -1,
        Right = 1,
        Up = -8,
        Down = 8,
        UpLeft = -9,
        UpRight = -7,
        DownLeft = 7,
        DownRight = 9
    }

    public static class GameConstants
    {
        // �萔

        /// <summary> ���o�[�V�̊�Ղ̍s�� </summary>
        public const int BoardRows = 8;
        /// <summary> ���o�[�V�̊�Ղ̗� </summary>
        public const int BoardColumns = 8;

        /// <summary>
        /// �^�v��
        /// </summary>
        public static readonly (DirectionList direction, ulong mask)[] AllDirections = new[]
        {
            (DirectionList.Right, NotHFile),
            (DirectionList.Left, NotAFile),
            (DirectionList.Up, FullBoardMask),
            (DirectionList.Down, FullBoardMask),
            (DirectionList.UpLeft, NotAFile),
            (DirectionList.UpRight, NotHFile),
            (DirectionList.DownLeft, NotAFile),
            (DirectionList.DownRight, NotHFile)
        };


        #region �r�b�g�}�X�N�̐錾


        /// <summary>�ՖʑS�̂̃r�b�g�}�X�N�i���ׂẴr�b�g��1�j</summary>
        public const ulong FullBoardMask = ulong.MaxValue;  // 0xFFFFFFFFFFFFFFFFUL

        /// <summary>
        /// ���o�[�V�Ֆʂ�H��i�E�[��j�����O����r�b�g�}�X�N�B
        /// ���̃}�X�N���g�����ƂŁAH�񂩂�͂ݏo��ړ���h���B
        /// </summary>
        public const ulong NotHFile = 0xfefefefefefefefeUL; // 1111_1110 ��8��

        /// <summary>
        /// ���o�[�V�Ֆʂ�A��i�E�[��j�����O����r�b�g�}�X�N�B
        /// ���̃}�X�N���g�����ƂŁAA�񂩂�͂ݏo��ړ���h���B
        /// </summary>
        public const ulong NotAFile = 0x7f7f7f7f7f7f7f7fUL; // 0111_1111 ��8��
        
        #endregion
    }
}