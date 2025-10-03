using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // static�Ő錾�����1�������݂��Ȃ�
    public static GameManager Instance = null;

    // ��Փx����ɔ�Ⴕ�Đ����������܂�
    public int gameLevel = 1;
    // ���݂̖ڕW�B����
    public int clearNum= 0;
    // �N���A�{�[�_�[(�ő��)
    public int clearborder = 0;

    // ���ꂼ��̒B����
    public int numRed = 0;
    public int numBlue = 0;
    public int numYellow = 0;
    public int numGreen = 0;

    // ���ꂼ��̐������̕ۊǗp
    public int numRedMax = 0;
    public int numBlueMax = 0;
    public int numYellowMax = 0;
    public int numGreenMax = 0;

    SwitchScene switchScene;


    private bool IsClear = false;
    void Awake()
    {
        if (Instance == null)
        {
            // Instance�����݂��Ȃ���Ύ��M��o�^����
            // ��������݂��Ȃ�����null�Ȃ�
            Instance = this;
            // �j������Ȃ��悤�ɂ���
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            // Instance�����݂���ꍇ�͏d�����Ȃ��悤�ɍ폜����
            Debug.Log("�d�������̂ō폜�I"); //�m�F�p
            Destroy(this.gameObject);
        }

        switchScene = GetComponent<SwitchScene>();
    }
    
    void Update()
    {
        if (IsClear) return;

        if (clearborder == clearNum)
		{
            switchScene.ClearSwitchScene();
            Debug.Log($"GameClear:�N���A�ڕW{clearborder}�N���A������{clearNum}");
            IsClear = true;
        }

    }
}
