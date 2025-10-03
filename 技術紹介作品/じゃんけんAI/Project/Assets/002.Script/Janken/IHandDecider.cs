
using System.Collections;
/// <summary>
/// ������߂邱�Ƃ��ł���N���X�̃C���^�t�F�[�X
/// </summary>
public enum JankenHand
{
    Rock,      // �O�[
    Paper,     // �p�[
    Scissors,   // �`���L
    Num         // ����
}


// �C���^�t�F�[�X
// �C���^�t�F�[�X���݂�΂Ȃɂ��ł��邩�킩��悤�ɂ���

/// <summary>
/// ������߂邱�Ƃ��ł���
///// ����o�����Ƃ��ł���
/// </summary>
public interface IHandDecider
{
    // �v���p�e�B
    JankenHand SelectHand { get; }

    //bool IsDecided { get; } // ���߂���

    /// <summary>
    /// ������߂鏈��
    /// "����"�ł͂Ȃ�"�I��"�\��
    /// </summary>
    /// <returns>���߂���</returns>
    IEnumerator DecideHand();
    // �����ꂾ�ƑI���ł���݂����Ɏv�����̂ŕύX
    //JankenHand HandSelection(); 


    /// <summary>
    ///  �I�񂾉摜��\�����鏈��
    /// </summary>
    //void DisplayHand();
    // �����ꂾ�Ǝ�𐶐��ł���݂����Ɏv�����̂ŕύX
    //void GenerateHand();
}
