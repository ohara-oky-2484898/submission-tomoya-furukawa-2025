// �@�Ֆʂ̕\��
// �A�Ֆʂ��N���b�N���Đ΂�u��
// �B���܂ꂽ�R�}���Ђ����肩����
// �C�R�}��u����}�X���u���Ȃ��}�X�̋�ʂ�����
using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using ReversiConstants;

// �g�������Ȃ��̈ꗗ�o�����Z�������������Q���E�p
// ������
// �I�Z���̍�(��)�^��(�Z)
// ���]��
// �]���̒��Ł��}�E�X�̈ʒu�ɍ��킹�Č���}�X(�����ʒu�Ȃ�)


public class ReversiGameManager : MonoBehaviour
{
    ///// <summary> ��� </summary>
    //[SerializeField] private Image _panel;
    ///// <summary> �ǖ� </summary>
    //[SerializeField] private TextMeshProUGUI diskText;
    //public Image Panel => _panel;

    [SerializeField] GameCanvas gameCanvas;
    private ulong _prevDisx = 0;
    private List<IReversiPlayer> players;
    private ReversiLogic logic = new ReversiLogic();
    private DiscColors _currentColors;

    private int _turnCount = 0;

    /// <summary> �v���p�e�B </summary>
    //public static ReversiGameManager Instance { get; private set; }
    //public ReversiLogic Logic => logic;

    ulong _flipsDisc = 0;
    bool getPlaceableDisc = false;
    bool inGame = true;

    private void Awake()
    {
        //if (Instance == null)
        //{
        //    Instance = this;
        //    logic.Init();
        //    DontDestroyOnLoad(gameObject);
            
        //}
        //else
        //{
        //    Destroy(this);
        //}
    }

    void Start()
    {
        players = new List<IReversiPlayer>()
        {
            new ReversiPlayer(DiscColors.black),
            new ReversiAI(DiscColors.white),
        };

        foreach (var player in players)
		{
            player.Init();
		}

        _currentColors = DiscColors.black;
        PrintBoard();
        _prevDisx = logic.AllDisc;

        gameCanvas.UpDateUI(
            isSkip: getPlaceableDisc,
            currentColor: players[(int)_currentColors].MyDiscColor,
            turnCount: ++_turnCount,
            blackNum: CountBits(logic.BlackDisk),
            whiteNum: CountBits(logic.WhiteDisk)
            );
    }
    //private void Update()
    //{
    //       if (!inGame) return;

    //       // �ǖʂ��ς���Ă�����A�܂��͒u����ꏊ���Ȃ��Q�b�g�ł��Ȃ������i�p�X�󋵁j���������Ă�����
    //       if(_prevDisx != logic.AllDisc || !getPlaceableDisc)
    //	{
    //           _prevDisx = logic.AllDisc;
    //           logic.IsBlackTurn = !logic.IsBlackTurn;
    //           _currentColors = logic.CurrentTurnColor;
    //           UpDateUI();
    //       }

    //       if (players[(int)_currentColors].MyDiscColor == DiscColors.black)
    //	{
    //           getPlaceableDisc = logic.GetPlaceableDiscs(logic.BlackDisk, logic.WhiteDisk, out _disc);
    //       }
    //       //      else
    //       //{
    //       //          getPlaceableDisc = logic.GetPlaceableDiscs(logic.WhiteDisk, logic.BlackDisk, out _disc);
    //       //      }

    //       if (players[(int)_currentColors] is ReversiAI ai)
    //       {
    //           if (ai.PlaceDisk(logic))
    //           {
    //               _prevDisx = logic.AllDisc;
    //               logic.IsBlackTurn = !logic.IsBlackTurn;
    //               _currentColors = logic.CurrentTurnColor;
    //               UpDateUI();
    //           }
    //       }


    //       players[(int)_currentColors].Update();

    //       PrintBoard();


    //       if(logic.GameFinish())
    //	{
    //           inGame = false;
    //           playerCallText.text = "�Q�[���Z�b�g\n";
    //           int blackDiscCount = CountBits(logic.BlackDisk);
    //           int whiteDiscCount = CountBits(logic.WhiteDisk);
    //           playerCallText.text = blackDiscCount > whiteDiscCount
    //               ? "����̂����I"
    //               : "����̂����I";

    //           _disc = 0;
    //           PrintBoard();
    //       }
    //   }
    private void Update()
    {
  //      if (!inGame) return;

  //      // ���݂̃v���C���[�̐΂̔z�u���擾
  //      ulong myDiscs = _currentColors == DiscColors.black ? logic.BlackDisk : logic.WhiteDisk;
  //      ulong oppDiscs = _currentColors == DiscColors.black ? logic.WhiteDisk : logic.BlackDisk;

  //      // ���@��̎擾
  //      getPlaceableDisc = logic.GetPlaceableDiscs(myDiscs, oppDiscs, out _flipsDisc);

  //      // AI�̃^�[���Ȃ玩���Ŏ��ł�
  //      if (players[(int)_currentColors] is ReversiAI ai)
  //      {
  //          if (getPlaceableDisc && ai.PlaceDisk(logic))
  //          {
  //              _prevDisx = logic.AllDisc;
  //              logic.IsBlackTurn = !logic.IsBlackTurn;
  //              _currentColors = logic.CurrentTurnColor;

  //              gameCanvas.UpDateUI(
  //              isSkip: getPlaceableDisc,
  //              currentColor: players[(int)_currentColors].MyDiscColor,
  //              turnCount: ++_turnCount,
  //              blackNum: CountBits(logic.BlackDisk),
  //              whiteNum: CountBits(logic.WhiteDisk)
  //              );
  //          }
  //          else if (!getPlaceableDisc)
  //          {
  //              // �p�X����
  //              Debug.Log("AI: �p�X���܂�");
  //              logic.IsBlackTurn = !logic.IsBlackTurn;
  //              _currentColors = logic.CurrentTurnColor;

  //              gameCanvas.UpDateUI(
  //              isSkip: getPlaceableDisc,
  //              currentColor: players[(int)_currentColors].MyDiscColor,
  //              turnCount: ++_turnCount,
  //              blackNum: CountBits(logic.BlackDisk),
  //              whiteNum: CountBits(logic.WhiteDisk)
  //              );
  //          }
  //      }
  //      else
  //      {
  //          // �l�ԃv���C���[�̃^�[���ŋǖʂ��ς������^�[���؂�ւ�
  //          if (_prevDisx != logic.AllDisc)
  //          {
  //              _prevDisx = logic.AllDisc;
  //              logic.IsBlackTurn = !logic.IsBlackTurn;
  //              _currentColors = logic.CurrentTurnColor;

  //              gameCanvas.UpDateUI(
  //              isSkip: getPlaceableDisc,
  //              currentColor: players[(int)_currentColors].MyDiscColor,
  //              turnCount: ++_turnCount,
  //              blackNum: CountBits(logic.BlackDisk),
  //              whiteNum: CountBits(logic.WhiteDisk)
  //              );
  //          }

  //          // �v���C���[�̓��͏���
  //          players[(int)_currentColors].Update();
		//}


		//// �Ֆʕ\��
		//PrintBoard();

  //      // �Q�[���I������
  //      if (logic.GameFinish())
  //      {
  //          inGame = false;
  //          int blackDiscCount = CountBits(logic.BlackDisk);
  //          int whiteDiscCount = CountBits(logic.WhiteDisk);


  //          gameCanvas.ShowGameEndText(blackDiscCount > whiteDiscCount);
  //          _flipsDisc = 0;
  //          PrintBoard();
  //      }
    }

    int CountBits(ulong x)
    {
        int count = 0;
        while (x != 0)
        {
            // MEMO�F �}�C�i�X�̒l��l���ǂފȒP�ȕ��@��
            // 2�̕␔���g��(not����)
            // 11111011 �Ȃ� 00000100 �ɕϊ��� 1 (0000001)�𑫂���
            // 00000101 ����� "5" �ϊ��O�̍ŏ�ʃr�b�g��1�̂���
            // �������}�C�i�X�ɂ��� "-5"

            // �����Ƃ��E�ɂ���1���폜
            x &= (x - 1);
            count++;  // �� 1�r�b�g�������̂ŁA+1
        }
        return count;
    }

    /// <summary>
    /// �Ֆʂ��X�V���\��������
    /// </summary>
    public void PrintBoard()
    {
        string currentBoard = "";


        for (int row = 0; row < GameConstants.BoardRows; row++)
        {
            string line = "";
            for (int col = 0; col < GameConstants.BoardColumns; col++)
            {
                int pos = row * GameConstants.BoardColumns + col;
                // �Z���]��(������������ق����ɕ]��������)

                // �Ђ�����Ԃ�Ƃ���(_disc�ɂ͌��݂̂����Ă���ʒu(��)�ɒu�����ۂ̂Ђ�����Ԃ�ʒu��bitboard�����Ă���)
                if (((_flipsDisc >> pos) & 1) != 0)
                {
                    line += "�E"; // �Ђ�����Ԃ�Ƃ���
                }
                else if (((logic.BlackDisk >> pos) & 1) != 0)
                {
                    line += "��"; // ��
                }
                else if (((logic.WhiteDisk >> pos) & 1) != 0)
                {
                    line += "�Z"; // ��
                }
                // x = �������ɕ��΂�Ă�H => �񐔁^col
                // y = �����ςݏd�Ȃ��Ă�H => �s���^row

                // ����player�Ȃ�}�E�X�̃|�W�V�����Ɂ���\��
                // C#7.0�ȍ~�̃p�^�[���}�b�`���O�i�^�m�F�A�L���X�g�A�ϐ��錾��C�ɂł���j
                // �ϐ��@is �^ �V�����ϐ�
                // �ϐ����^�Ɠ�����������ϐ��ɐV�����ϐ��ɑ���������
                // true/false�ŕԂ��ė���
                // true�̂Ƃ������錾���ꂽ�ϐ��͎g����
                else if (players[(int)_currentColors] is ReversiPlayer player &&
                         player.HoverPosition.HasValue &&
                         player.HoverPosition.Value.y == row &&
                         player.HoverPosition.Value.x == col)
                {// ��������ԏゾ�Ɩ��ʂȔ�r��63��N���Ă��܂��\��������
                    line += "��"; // �}�E�X���d�Ȃ��Ă�ꏊ
                }
                else
                {
                    line += "��"; // ��
                }
            }
            currentBoard += line + '\n';
        }

        //diskText.text = currentBoard;
    }
}
