using ReversiConstants;

/// <summary>
/// ���o�[�V�ŗV�Ԑl
/// </summary>
public interface IReversiPlayer
{
	DiscColors MyDiscColor { get; set; }

	/// <summary>
	/// ����������
	/// </summary>
	void Init();

	void Update();

	/// <summary>
	/// �΂�u��
	/// </summary>
	/// <returns> �u������ </returns>
	bool PlaceDisk(BitBoard currentBoard, out BitBoard nextBoard);
}
