using ReversiConstants;
using UnityEngine;
using UnityEngine.UI;
//using static ReversiConstants.GameConstants;  // GameConstants��ÓI�ɃC���|�[�g


public class ReversiPlayer :IReversiPlayer
{
    public ReversiPlayer(DiscColors discColor)
	{
        MyDiscColor = discColor;
	}

    private Image _panel;
    private BitBoard _bitBoard;

    // null���x���i�l�������Ă��Ȃ���������Ȃ������^�j
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
        // �}�E�X�̃X�N���[�����W���擾
        Vector2 mousePos = Input.mousePosition;
        // ���[���h���W�� _panel �̃��[�J�����W�ɕϊ�
        Vector2 localPos = _panel.rectTransform.InverseTransformPoint(mousePos);
        
        if (IsClickInsideBoard(localPos))
        {
            Vector2Int boardPos = ConvertLocalPosToBoardIndex(localPos);

            int pos = boardPos.y * GameConstants.BoardColumns + boardPos.x;

            // �΂��u����Ă��Ȃ��ꏊ�����Ώ�
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

        // ���N���b�N�i�}�E�X�{�^��0�j�����o
        if (Input.GetMouseButtonDown(0))
        {
            // �N���b�N�ʒu���Ֆʓ����ǂ����`�F�b�N�i�N���b�N�ʒu���L���Ȕ͈͂Ɏ��܂��Ă��邩�j
            if (IsClickInsideBoard(localPos))
            {
                // �N���b�N���ꂽ�ʒu��Ֆʂ̃Z���ɕϊ�
                Vector2Int boardPos = ConvertLocalPosToBoardIndex(localPos);

                // �ՖʃC���f�b�N�X���r�b�g�{�[�h�ɕϊ�
                ulong bitPosition = GetBitPosition(boardPos);

                // ���łɐ΂��u����Ă���ꏊ�ɒu���Ȃ��悤�ɂ���
                //if (((_logic.BlackDisk >> (boardPos.y * GameConstants.BoardColumns + boardPos.x)) & 1) != 0 ||
                //    ((_logic.WhiteDisk >> (boardPos.y * GameConstants.BoardColumns + boardPos.x)) & 1) != 0)
                if (_bitBoard.HasDiscAt(boardPos))
                {
                    Debug.Log("���̏ꏊ�ɂ͂��łɐ΂��u����Ă��܂��B");
                    return;
                }

                // �r�b�g�{�[�h�̎擾
                //ulong myDiscs = MyDiscColor == DiscColors.black ? _logic.BlackDisk : _logic.WhiteDisk;
                //ulong oppDiscs = MyDiscColor == DiscColors.black ? _logic.WhiteDisk : _logic.BlackDisk;
                //_logic.GetPlaceableDiscs(myDiscs, oppDiscs, out ulong placeable);
                ulong myDiscs = MyDiscColor == DiscColors.black ? _bitBoard.BlackDiscs : _bitBoard.WhiteDiscs;
                ulong oppDiscs = MyDiscColor == DiscColors.black ? _bitBoard.WhiteDiscs : _bitBoard.BlackDiscs;
                ulong placeable = BitBoardUtils.GetPlaceableDiscs(myDiscs, oppDiscs);

                if ((placeable & bitPosition) == 0)
                {
                    Debug.Log("���̏ꏊ�ɂ͐΂�u���܂���B�i���߂Ȃ��j");
                    return;
                }


                //            // �����ŃN���b�N�ʒu�ɍ��΁i���j��u��������ǉ�
                //            if (MyDiscColor == DiscColors.black)
                //{
                //                _logic.SetBlackDisk(bitPosition);  // �N���b�N���ꂽ�ʒu�ɍ��΂�u��
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

                // �f�o�b�O�p�ɕ\��
                Debug.Log($"Clicked at (boardPos.x, boardPos.y): ({boardPos.x}, {boardPos.y}), Bit position: {bitPosition}");
            }
        }
    }

    /// <summary>
    /// �N���b�N�ʒu���Ֆʓ����ǂ�������
    /// </summary>
    /// <param name="boardLocalPos">�N���b�N�����ʒu</param>
    /// <returns></returns>
    private bool IsClickInsideBoard(Vector2 boardLocalPos)
    {
        // �Ֆʂ̕��ƍ����i�p�l���̃T�C�Y����v�Z�j
        RectTransform panelRect = _panel.rectTransform;

        // ���S���獶�E�㉺�ɂ͂ݏo�ĂȂ������`�F�b�N
        bool isInBoardWidth = Mathf.Abs(boardLocalPos.x) <= panelRect.rect.width / 2;
        bool isInBoardHeight = Mathf.Abs(boardLocalPos.y) <= panelRect.rect.height / 2;
   //     Debug.Log($"���݂̈ʒu" +
   //         $"X{Mathf.Abs(boardLocalPos.x)} �^maxX{panelRect.rect.width / 2}�@�b�b�b�@" +
			//$"Y{Mathf.Abs(boardLocalPos.y)} �^maxY{panelRect.rect.height / 2}");

        // �N���b�N�ʒu���Ֆʓ��ł��邩���m�F
        return isInBoardWidth && isInBoardHeight;
    }


    /// <summary>
    /// ��Ղ̃��[�J�����W��Ֆʂ̍��W�ɕϊ�
    /// </summary>
    /// <param name="localPos">���W</param>
    /// <param name="row"></param>
    /// <param name="col"></param>
    private Vector2Int ConvertLocalPosToBoardIndex(Vector2 localPos)
    {
        // �Ֆʂ̃Z���T�C�Y1������̃T�C�Y�����߂�
        float cellWidth = _panel.rectTransform.rect.width / GameConstants.BoardColumns;
        float cellHeight = _panel.rectTransform.rect.height / GameConstants.BoardRows;

        // ���オ(0�C0)�E����(7�C7)�ɂȂ�悤��
        // x���@-200 �̂Ƃ� 0�A x���@+200 �̂Ƃ� 7
        // y���@+200 �̂Ƃ� 0�A y���@-200 �̂Ƃ� 7
        int col = Mathf.FloorToInt((localPos.x + _panel.rectTransform.rect.width / 2) / cellWidth);     // ������߂�
        int row = Mathf.FloorToInt(((_panel.rectTransform.rect.height / 2) - localPos.y) / cellHeight); // �s�����߂�
        //Debug.Log($"�C���f�b�N�X�s{row}, ��{col}");
        
        return new Vector2Int(col, row);
    }


    /// <summary>
    /// �Ֆʂ̍s�񂩂�r�b�g�{�[�h�̈ʒu���v�Z
    /// </summary>
    /// <param name="row">�s<��/param>
    /// <param name="col">��</param>
    /// <returns></returns>
    private ulong GetBitPosition(Vector2Int boardPos)
    {
        // 8x8 �̔Ֆʂł̈ʒu���v�Z�i0�`63 �̃C���f�b�N�X�ɕϊ��j
        int index = boardPos.y * GameConstants.BoardColumns + boardPos.x;

        // �r�b�g�{�[�h�ł��̈ʒu��\��
        return 1UL << index;  // 1 �����ɃC���f�b�N�X���V�t�g
    }

    //public static ulong GetBitPosition(Vector2Int boardPos)
    //{
    //    return 1UL << (boardPos.y * 8 + boardPos.x);
    //}

}
