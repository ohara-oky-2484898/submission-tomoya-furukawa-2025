using ReversiConstants;
using UnityEngine;
using UnityEngine.UI;
//using static ReversiConstants.GameConstants;  // GameConstantsを静的にインポート


public class ReversiPlayer :IReversiPlayer
{
    public ReversiPlayer(DiscColors discColor)
	{
        MyDiscColor = discColor;
	}

    private Image _panel;
    private BitBoard _bitBoard;

    // nullラベル（値が入っていないかもしれない整数型）
    private Vector2Int? _hoverPosition = null;

    public Vector2Int? HoverPosition => _hoverPosition;
    public void Init()
	{
        //_panel = ReversiGameManager.Instance.Panel;
        //_bitBoard = ReversiGameManager.Instance.Board;
        _hoverPosition = null;
    }

    public DiscColors MyDiscColor { get; set; }

    public bool PlaceDisk(BitBoard currentBoard, out BitBoard nextBoard) { nextBoard = null; return true; }

    public void Update()
    {
        // マウスのスクリーン座標を取得
        Vector2 mousePos = Input.mousePosition;
        // ワールド座標を _panel のローカル座標に変換
        Vector2 localPos = _panel.rectTransform.InverseTransformPoint(mousePos);
        
        if (IsClickInsideBoard(localPos))
        {
            Vector2Int boardPos = ConvertLocalPosToBoardIndex(localPos);

            int pos = boardPos.y * GameConstants.BoardColumns + boardPos.x;

            // 石が置かれていない場所だけ対象
            //if (((_logic.BlackDisk >> pos) & 1) == 0 && ((_logic.WhiteDisk >> pos) & 1) == 0)
            if (!_bitBoard.HasDiscAt(pos))
            {
                _hoverPosition = boardPos;
            }
            else
            {
                _hoverPosition = null;
            }
        }
        else
        {
            _hoverPosition = null;
        }

        // 左クリック（マウスボタン0）を検出
        if (Input.GetMouseButtonDown(0))
        {
            // クリック位置が盤面内かどうかチェック（クリック位置が有効な範囲に収まっているか）
            if (IsClickInsideBoard(localPos))
            {
                // クリックされた位置を盤面のセルに変換
                Vector2Int boardPos = ConvertLocalPosToBoardIndex(localPos);

                // 盤面インデックスをビットボードに変換
                ulong bitPosition = GetBitPosition(boardPos);

                // すでに石が置かれている場所に置けないようにする
                //if (((_logic.BlackDisk >> (boardPos.y * GameConstants.BoardColumns + boardPos.x)) & 1) != 0 ||
                //    ((_logic.WhiteDisk >> (boardPos.y * GameConstants.BoardColumns + boardPos.x)) & 1) != 0)
                if (_bitBoard.HasDiscAt(boardPos))
                {
                    Debug.Log("この場所にはすでに石が置かれています。");
                    return;
                }

                // ビットボードの取得
                //ulong myDiscs = MyDiscColor == DiscColors.black ? _logic.BlackDisk : _logic.WhiteDisk;
                //ulong oppDiscs = MyDiscColor == DiscColors.black ? _logic.WhiteDisk : _logic.BlackDisk;
                //_logic.GetPlaceableDiscs(myDiscs, oppDiscs, out ulong placeable);
                ulong myDiscs = MyDiscColor == DiscColors.black ? _bitBoard.BlackDiscs : _bitBoard.WhiteDiscs;
                ulong oppDiscs = MyDiscColor == DiscColors.black ? _bitBoard.WhiteDiscs : _bitBoard.BlackDiscs;
                ulong placeable = BitBoardUtils.GetPlaceableDiscs(myDiscs, oppDiscs);

                if ((placeable & bitPosition) == 0)
                {
                    Debug.Log("この場所には石を置けません。（挟めない）");
                    return;
                }


                //            // ここでクリック位置に黒石（●）を置く処理を追加
                //            if (MyDiscColor == DiscColors.black)
                //{
                //                _logic.SetBlackDisk(bitPosition);  // クリックされた位置に黒石を置く
                //            }
                //            else
                //{
                //                _logic.SetWhiteDisk(bitPosition);
                //}

                //            _logic.FlipDiscs(bitPosition);
                if (MyDiscColor == DiscColors.black)
                {
                    _bitBoard = _bitBoard.PlaceBlackDisc(bitPosition);
                }
                else
                {
                    _bitBoard = _bitBoard.PlaceWhiteDisc(bitPosition);
                }

                // デバッグ用に表示
                Debug.Log($"Clicked at (boardPos.x, boardPos.y): ({boardPos.x}, {boardPos.y}), Bit position: {bitPosition}");
            }
        }
    }

    /// <summary>
    /// クリック位置が盤面内かどうか判定
    /// </summary>
    /// <param name="boardLocalPos">クリックした位置</param>
    /// <returns></returns>
    private bool IsClickInsideBoard(Vector2 boardLocalPos)
    {
        // 盤面の幅と高さ（パネルのサイズから計算）
        RectTransform panelRect = _panel.rectTransform;

        // 中心から左右上下にはみ出てないかをチェック
        bool isInBoardWidth = Mathf.Abs(boardLocalPos.x) <= panelRect.rect.width / 2;
        bool isInBoardHeight = Mathf.Abs(boardLocalPos.y) <= panelRect.rect.height / 2;
   //     Debug.Log($"現在の位置" +
   //         $"X{Mathf.Abs(boardLocalPos.x)} ／maxX{panelRect.rect.width / 2}　｜｜｜　" +
			//$"Y{Mathf.Abs(boardLocalPos.y)} ／maxY{panelRect.rect.height / 2}");

        // クリック位置が盤面内であるかを確認
        return isInBoardWidth && isInBoardHeight;
    }


    /// <summary>
    /// 基盤のローカル座標を盤面の座標に変換
    /// </summary>
    /// <param name="localPos">座標</param>
    /// <param name="row"></param>
    /// <param name="col"></param>
    private Vector2Int ConvertLocalPosToBoardIndex(Vector2 localPos)
    {
        // 盤面のセルサイズ1つ当たりのサイズを求める
        float cellWidth = _panel.rectTransform.rect.width / GameConstants.BoardColumns;
        float cellHeight = _panel.rectTransform.rect.height / GameConstants.BoardRows;

        // 左上が(0，0)右下が(7，7)になるように
        // x軸　-200 のとき 0、 x軸　+200 のとき 7
        // y軸　+200 のとき 0、 y軸　-200 のとき 7
        int col = Mathf.FloorToInt((localPos.x + _panel.rectTransform.rect.width / 2) / cellWidth);     // 列を求める
        int row = Mathf.FloorToInt(((_panel.rectTransform.rect.height / 2) - localPos.y) / cellHeight); // 行を求める
        //Debug.Log($"インデックス行{row}, 列{col}");
        
        return new Vector2Int(col, row);
    }


    /// <summary>
    /// 盤面の行列からビットボードの位置を計算
    /// </summary>
    /// <param name="row">行<数/param>
    /// <param name="col">列数</param>
    /// <returns></returns>
    private ulong GetBitPosition(Vector2Int boardPos)
    {
        // 8x8 の盤面での位置を計算（0～63 のインデックスに変換）
        int index = boardPos.y * GameConstants.BoardColumns + boardPos.x;

        // ビットボードでその位置を表す
        return 1UL << index;  // 1 を左にインデックス分シフト
    }

    //public static ulong GetBitPosition(Vector2Int boardPos)
    //{
    //    return 1UL << (boardPos.y * 8 + boardPos.x);
    //}

}
