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
        // 定数

        /// <summary> リバーシの基盤の行数 </summary>
        public const int BoardRows = 8;
        /// <summary> リバーシの基盤の列数 </summary>
        public const int BoardColumns = 8;

        /// <summary>
        /// タプル
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


        #region ビットマスクの宣言


        /// <summary>盤面全体のビットマスク（すべてのビットが1）</summary>
        public const ulong FullBoardMask = ulong.MaxValue;  // 0xFFFFFFFFFFFFFFFFUL

        /// <summary>
        /// リバーシ盤面のH列（右端列）を除外するビットマスク。
        /// このマスクを使うことで、H列からはみ出る移動を防ぐ。
        /// </summary>
        public const ulong NotHFile = 0xfefefefefefefefeUL; // 1111_1110 が8つ

        /// <summary>
        /// リバーシ盤面のA列（右端列）を除外するビットマスク。
        /// このマスクを使うことで、A列からはみ出る移動を防ぐ。
        /// </summary>
        public const ulong NotAFile = 0x7f7f7f7f7f7f7f7fUL; // 0111_1111 が8つ
        
        #endregion
    }
}