using UnityEngine;
using TMPro;
using ReversiConstants;

public class GameCanvas : MonoBehaviour
{
    /// <summary> 簡易UI用 </summary>
    [SerializeField] private TextMeshProUGUI playerCallText;
    [SerializeField] private TextMeshProUGUI currentDiskText;


    public void UpDateUI(bool isSkip, DiscColors currentColor, int turnCount, int blackNum, int whiteNum)
	{
        string skipAnnounce = isSkip ? "" : "スキップ\n";
        string nowTurnPlayerText = currentColor == DiscColors.black
            ? "- ターン -\n '黒'(●) "
            : "- ターン -\n '白'(〇) ";
        // テキスト表示更新
        playerCallText.text = skipAnnounce + nowTurnPlayerText;

        // ターン表示更新
        string turn = $"{turnCount} 手目\n\n";
        // 石の数表示更新
        string discNum =
            $"しろ(〇)\n　：{whiteNum}個" +
            $"くろ(〇)\n　：{blackNum}個";
        currentDiskText.text = turn + discNum;
    }

    public void ShowGameEndText(bool isBlackWin)
	{
        playerCallText.text = isBlackWin
        ? "黒の勝利！"
        : "白の勝利！";
    }
}
