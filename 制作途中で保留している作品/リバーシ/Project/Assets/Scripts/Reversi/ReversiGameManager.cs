// ①盤面の表示
// ②盤面をクリックして石を置く
// ③挟まれたコマをひっくりかえす
// ④コマを置けるマスち置けないマスの区別をする
using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using ReversiConstants;

// 使えそうなもの一覧｛□◎〇●■▲▽☆◇＿※・｝
// 今現在
// オセロの黒(●)／白(〇)
// □余白
// 余白の中で■マウスの位置に合わせて光るマス(同じ位置なら)


public class ReversiGameManager : MonoBehaviour
{
    ///// <summary> 基盤 </summary>
    //[SerializeField] private Image _panel;
    ///// <summary> 局面 </summary>
    //[SerializeField] private TextMeshProUGUI diskText;
    //public Image Panel => _panel;

    [SerializeField] GameCanvas gameCanvas;
    private ulong _prevDisx = 0;
    private List<IReversiPlayer> players;
    private ReversiLogic logic = new ReversiLogic();
    private DiscColors _currentColors;

    private int _turnCount = 0;

    /// <summary> プロパティ </summary>
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

    //       // 局面が変わっていたら、または置ける場所がなくゲットできなかった（パス状況）が発生していたら
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
    //           playerCallText.text = "ゲームセット\n";
    //           int blackDiscCount = CountBits(logic.BlackDisk);
    //           int whiteDiscCount = CountBits(logic.WhiteDisk);
    //           playerCallText.text = blackDiscCount > whiteDiscCount
    //               ? "くろのかち！"
    //               : "しろのかち！";

    //           _disc = 0;
    //           PrintBoard();
    //       }
    //   }
    private void Update()
    {
  //      if (!inGame) return;

  //      // 現在のプレイヤーの石の配置を取得
  //      ulong myDiscs = _currentColors == DiscColors.black ? logic.BlackDisk : logic.WhiteDisk;
  //      ulong oppDiscs = _currentColors == DiscColors.black ? logic.WhiteDisk : logic.BlackDisk;

  //      // 合法手の取得
  //      getPlaceableDisc = logic.GetPlaceableDiscs(myDiscs, oppDiscs, out _flipsDisc);

  //      // AIのターンなら自動で手を打つ
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
  //              // パス処理
  //              Debug.Log("AI: パスします");
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
  //          // 人間プレイヤーのターンで局面が変わったらターン切り替え
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

  //          // プレイヤーの入力処理
  //          players[(int)_currentColors].Update();
		//}


		//// 盤面表示
		//PrintBoard();

  //      // ゲーム終了判定
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
            // MEMO： マイナスの値を人が読む簡単な方法は
            // 2の補数を使う(notする)
            // 11111011 なら 00000100 に変換後 1 (0000001)を足して
            // 00000101 これで "5" 変換前の最上位ビットが1のため
            // 符号をマイナスにして "-5"

            // もっとも右にある1を削除
            x &= (x - 1);
            count++;  // ← 1ビット消したので、+1
        }
        return count;
    }

    /// <summary>
    /// 盤面を更新し表示をする
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
                // 短絡評価(より引っかかるほうを先に評価させる)

                // ひっくり返るところ(_discには現在のおいている位置(■)に置いた際のひっくり返る位置がbitboard入ってある)
                if (((_flipsDisc >> pos) & 1) != 0)
                {
                    line += "・"; // ひっくり返るところ
                }
                else if (((logic.BlackDisk >> pos) & 1) != 0)
                {
                    line += "●"; // 黒
                }
                else if (((logic.WhiteDisk >> pos) & 1) != 0)
                {
                    line += "〇"; // 白
                }
                // x = いくつ横に並ばれてる？ => 列数／col
                // y = いくつ積み重なってる？ => 行数／row

                // 今がplayerならマウスのポジションに■を表示
                // C#7.0以降のパターンマッチング（型確認、キャスト、変数宣言一気にできる）
                // 変数　is 型 新しい変数
                // 変数が型と同じだったら変数に新しい変数に代入するもの
                // true/falseで返って来る
                // trueのときだけ宣言された変数は使える
                else if (players[(int)_currentColors] is ReversiPlayer player &&
                         player.HoverPosition.HasValue &&
                         player.HoverPosition.Value.y == row &&
                         player.HoverPosition.Value.x == col)
                {// こいつが一番上だと無駄な比較が63回起きてしまう可能性がある
                    line += "■"; // マウスが重なってる場所
                }
                else
                {
                    line += "□"; // 空き
                }
            }
            currentBoard += line + '\n';
        }

        //diskText.text = currentBoard;
    }
}
