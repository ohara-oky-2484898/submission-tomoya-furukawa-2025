public static class BitBoardUtils
{
    public static ulong GetFlipsInDirectino(ulong position, ulong player, ulong opponent, int shift, ulong mask)
    {
        ulong flip = 0;
        //ulong temp;
        //if (shift > 0)
        //    temp = mask & (position << shift);
        //else
        //    temp = mask & (position >> -shift);
        ulong temp = shift > 0 ? mask & (position << shift) : mask & (position >> -shift);

        if ((temp & opponent) == 0) return 0;
        flip |= temp;

        //while (true)
        //{
        //    //temp = shift > 0 ? mask & (position << shift) : mask & (position >> -shift);
        //    //if (temp == 0) return 0;
        //    //if ((temp | player) != 0) return flip;
        //    //flip |= temp;
        //    //if ((temp & opponent) == 0) return 0;
        //    if (shift > 0)
        //        temp = mask & (temp << shift);
        //    else
        //        temp = mask & (temp >> -shift);

        //    if (temp == 0) return 0;

        //    if ((temp & player) != 0)
        //        return flip;

        //    if ((temp & opponent) == 0)
        //        return 0;

        //    flip |= temp;
        //}
        while ((temp != 0) && (temp & opponent) != 0)
        {
            flip |= temp;
            temp = shift > 0 ? mask & (temp << shift) : mask & (temp >> -shift);
        }

        return (temp & player) != 0 ? flip : 0;
    }

    public static ulong GenerateMovesInDirection(ulong player, ulong opponent, int shift, ulong mask)
    {
        //ulong potential = 0;
        //ulong candidates;

        //if (shift > 0)
        //    candidates = (player << shift) & mask;
        //else
        //    candidates = (player >> (-shift)) & mask;

        //candidates &= opponent;

        //for (int i = 0; i < 5; i++)  // 最大5回連続挟みチェック（盤面は8x8）
        //{
        //    if (shift > 0)
        //        candidates = (candidates << shift) & mask;
        //    else
        //        candidates = (candidates >> (-shift)) & mask;

        //    if ((candidates & opponent) == 0) break;

        //    potential |= candidates & opponent;
        //}

        //ulong moves;
        //if (shift > 0)
        //    moves = (candidates << shift) & mask;
        //else
        //    moves = (candidates >> (-shift)) & mask;

        //return moves & empty;

        ///////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////

        //ulong flips = 0;
        //ulong temp = shift > 0 ? mask & (player << shift) : mask & (player >> -shift);
        //temp &= opponent;

        //for (int i = 0; i < 5; i++)
        //{
        //    ulong next = shift > 0 ? mask & (temp << shift) : mask & (temp >> -shift);
        //    next &= opponent;
        //    if (next == 0) break;
        //    temp |= next;
        //}

        //ulong potential = shift > 0 ? mask & (temp << shift) : mask & (temp >> -shift);
        //return potential;


        ///////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////


        //ulong temp = shift > 0 ? mask & (player << shift) : mask & (player >> -shift);
        //temp &= opponent;

        //ulong moves = 0;
        //for (int i = 0; i < 5; i++)
        //{
        //    ulong next = shift > 0 ? mask & (temp << shift) : mask & (temp >> -shift);
        //    // 次に自分の石がくるならその手は合法
        //    if ((next & opponent) == 0)
        //    {
        //        moves = next;
        //        break;
        //    }
        //    temp = next;
        //}
        //return moves;

        ///////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////

        ulong candidates = 0;
        ulong temp = player;

        for (int i = 0; i < 5; i++)
        {
            temp = shift > 0 ? mask & (temp << shift) : mask & (temp >> -shift);
            ulong captured = temp & opponent;
            if (captured == 0) break;

            candidates |= captured;
            temp = captured;
        }

        temp = shift > 0 ? mask & (temp << shift) : mask & (temp >> -shift);
        return temp & ~(player | opponent);  // 空きマスのみ返す

    }

    /// <summary>
    /// プレイヤーと相手の盤面から置けるマスを返す
    /// </summary>
    public static ulong GetPlaceableDiscs(ulong player, ulong opponent)
    {
        ulong placeable = 0;
        ulong empty = ~(player | opponent);
        foreach (var dir in ReversiConstants.GameConstants.AllDirections)
        {
            int shift = (int)dir.direction;
            ulong mask = dir.mask;
            placeable |= GenerateMovesInDirection(player, opponent, shift, mask);
        }
        return placeable & empty;
    }

}
