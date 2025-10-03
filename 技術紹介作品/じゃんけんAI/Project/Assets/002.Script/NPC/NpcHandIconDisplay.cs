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

        HideAllIcons(); // ���������ɑS�Ĕ�\��
    }

    /// <summary>
    /// �w��̎�A�C�R����\��
    /// </summary>
    public void ShowIcon(JankenHand hand)
    {
        Debug.Log($"��΂ꂽ{hand}");
        if (decideIcons.TryGetValue(hand, out var icon))
        {
            // �ꉞ�ЂƂO�̗v�f��\�����Ă�ł���
            HideIcon();

            //icon.gameObject.SetActive(true);
            nowShowImage = icon;
            nowShowImage.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// �\�����Ă����A�C�R�����\��
    /// </summary>
    public void HideIcon()
    {
        // ?��null�`�F�b�N
        nowShowImage?.gameObject.SetActive(false);
    }

    /// <summary>
    /// �S�ẴA�C�R�����\��
    /// </summary>
    public void HideAllIcons()
    {
        foreach (var icon in decideIcons.Values)
        {
            icon.gameObject.SetActive(false);
        }
    }
}

