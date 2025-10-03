using ReversiConstants;

/// <summary>
/// リバーシで遊ぶ人
/// </summary>
public interface IReversiPlayer
{
	DiscColors MyDiscColor { get; set; }

	/// <summary>
	/// 初期化処理
	/// </summary>
	void Init();

	void Update();

	/// <summary>
	/// 石を置く
	/// </summary>
	/// <returns> 置けたか </returns>
	bool PlaceDisk(BitBoard currentBoard, out BitBoard nextBoard);
}
