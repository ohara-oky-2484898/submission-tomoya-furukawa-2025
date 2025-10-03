using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class HandIconPair
{
    public JankenHand hand;
    public Image icon;
}

public class NpcHandIconDisplay : MonoBehaviour
{
    [SerializeField]
    private List<HandIconPair> iconList;

    private Dictionary<JankenHand, Image> decideIcons;
    private Image nowShowImage;

    private void Awake()
    {
        decideIcons = new Dictionary<JankenHand, Image>();
        foreach (var pair in iconList)
        {
            if (!decideIcons.ContainsKey(pair.hand))
            {
                decideIcons.Add(pair.hand, pair.icon);
            }
        }

        HideAllIcons(); // 初期化時に全て非表示
    }

    /// <summary>
    /// 指定の手アイコンを表示
    /// </summary>
    public void ShowIcon(JankenHand hand)
    {
        Debug.Log($"よばれた{hand}");
        if (decideIcons.TryGetValue(hand, out var icon))
        {
            // 一応ひとつ前の要素非表示を呼んでおく
            HideIcon();

            //icon.gameObject.SetActive(true);
            nowShowImage = icon;
            nowShowImage.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// 表示している手アイコンを非表示
    /// </summary>
    public void HideIcon()
    {
        // ?はnullチェック
        nowShowImage?.gameObject.SetActive(false);
    }

    /// <summary>
    /// 全てのアイコンを非表示
    /// </summary>
    public void HideAllIcons()
    {
        foreach (var icon in decideIcons.Values)
        {
            icon.gameObject.SetActive(false);
        }
    }
}

