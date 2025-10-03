using UnityEngine;
using System.Collections.Generic;

using ReversiConstants;

/// <summary>
/// ゲームの進行を管理
/// 初期化
/// ゲームループ
/// {
///		黒から始まる
///		①盤面表示
///		②playerの合法手表示
///		③打てるなら入力受付／ないならSkip
///		④石を置き、ひっくりかえす
///		⑤終了条件の確認
///		⑥交代
///		
///		⑦終了したら勝敗判定、結果表示
/// }
/// </summary>
public class GameMainManager : MonoBehaviour
{
	[SerializeField] DisplayReversiBoard displayBoard;

	public enum GameState { 
		Init,
		ShowBoard,
		ShowValidMoves,
		WaitPlayerInput,
		ApplyMove,
		CheckEnd,
		ChangeTurn,
		Result,
	}

	private GameState _currentState = GameState.Init;

	private BitBoard _bitBoard;
	private BitBoard _prevBitBoard;
	private List<IReversiPlayer> _players;
	private int _currentPlayerIndex = 0;

	private void Awake()
	{
		_players = new List<IReversiPlayer>() 
		{ 
			new ReversiPlayer(DiscColors.black),
			new ReversiAI(DiscColors.white),
		};

		GameInit();
	}
	private void Start()
	{
		displayBoard.DisplayBoard(_bitBoard);
	}

	private void Update()
	{
		switch (_currentState)
		{
			case GameState.Init:
				GameInit();
				_currentState = GameState.ShowBoard;
				break;
			
			case GameState.ShowBoard:
				displayBoard.DisplayBoard(_bitBoard);
				_currentState = GameState.ShowValidMoves;
				break;
			
			case GameState.ShowValidMoves:
				displayBoard.ShowValidMoves(_bitBoard);
				_currentState = GameState.WaitPlayerInput;
				_prevBitBoard = _bitBoard;
				break;

			case GameState.WaitPlayerInput:
				_players[_currentPlayerIndex].Update();
				
				if(_bitBoard != _prevBitBoard)
				{
					_currentState = GameState.ApplyMove;
				}
				break;

			case GameState.ApplyMove:
				displayBoard.DisplayBoard(_bitBoard);
				break;

			case GameState.CheckEnd:
				if(_bitBoard.IsGameOver())
				{
					_currentState = GameState.Result;
				}
				else
				{
					_currentState = GameState.ChangeTurn;
				}
				break;

			case GameState.ChangeTurn:
				_currentPlayerIndex = _bitBoard.IsBlackTurn ? 0 : 1;
				_currentState = GameState.ShowBoard;
				break;

			case GameState.Result:
			default:
				Debug.Log("変なの来たぞ");
				break;
		}
	}

	
	
	private void GameInit()
	{
		_bitBoard = BitBoard.Init();
		foreach (var player in _players)
		{
			player.Init();
		}
		_currentPlayerIndex = 0;
	}
}
