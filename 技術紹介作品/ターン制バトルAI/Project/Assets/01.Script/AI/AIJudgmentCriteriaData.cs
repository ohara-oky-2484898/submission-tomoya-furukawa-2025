using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// AI�̔��f��f�[�^���܂Ƃ߂��N���X
/// </summary>
[CreateAssetMenu(fileName = "AIJudgmentCriteriaData", menuName = "ScriptableObjects/AIJudgmentCriteriaData", order = 1)]
public class AIJudgmentCriteriaData : ScriptableObject
{
    /// <summary>
    /// HP�̔��f��t�B�[���h
    /// </summary>
    [Header("���ȉ񕜂̔��f�")]
    [Tooltip("���ȉ񕜂��s��HP�����i�w�聓�ȉ��Ŕ����j")]
    [SerializeField, Range(0f, 1f)] private float _selfHealingCriteria = 0.3f;

    [Header("�P�̉񕜂̔��f�")]
    [Tooltip("�P�̉񕜂��s��HP�����i�w�聓�ȉ��Ŕ����j")]
    [SerializeField, Range(0f, 1f)] private float _singleHealingCriteria = 0.5f;

    [Header("�S�̉񕜂̔��f�")]
    [Tooltip("�S�̉񕜂��s��HP�����i�w�聓�ȉ��Ŕ����j")]
    [SerializeField, Range(0f, 1f)] private float _allHealingCriteria = 0.7f;

    [Tooltip("�S�̉񕜂𔭓����邽�߂ɕK�v�ȏ����𖞂��������̐�")]
    [SerializeField] private int _requiredTeammatesForHealing = 2;

    /// <summary>
    /// �G�̗D��^�[�Q�b�g�֘A
    /// </summary>
    [Header("�^�[�Q�b�g�D��ݒ�")]

    [Tooltip("���[���̗D��x����HP�̒Ⴓ��D�悷�邩�ǂ���")]
    /// <summary>���[���̗D��x����HP�̒Ⴓ��D�悷�邩�ǂ���</summary>
    [SerializeField] private bool _prioritizeLowHpEnemyOverRole = false;
    [Tooltip("�G���[�����Ƃ̃^�[�Q�b�g�D�揇�ʁi�l���������قǗD��j")]
    /// <summary>�G���[�����Ƃ̃^�[�Q�b�g�D�揇�ʁi�l���������قǗD��j</summary>
    [SerializeField] private List<RolePriorityPair> _rolePriority = new List<RolePriorityPair>();



    /// <summary>
    /// �O���Q�Ɨp�v���p�e�B
    /// </summary>
    public float SelfHealingCriteria => _selfHealingCriteria;
    public float SingleHealingCriteria => _singleHealingCriteria;
    public float AllHealingCriteria => _allHealingCriteria;
    public int RequiredTeammatesForHealing => _requiredTeammatesForHealing;
    public bool PrioritizeLowHpEnemyOverRole => _prioritizeLowHpEnemyOverRole;


    /// <summary>
    /// �w�肳�ꂽ���[���̗D�揇�ʂ��擾����
    /// </summary>
    /// <param name="role">����Ώۂ̃��[��</param>
    /// <returns>�D�揇�ʁi�l���������قǗD��B����`�̏ꍇ�͍ő�l�j</returns>
    public int GetRolePriority(Role role)
    {
        foreach (var pair in _rolePriority)
        {
            if (pair.key == role)
            { 
                Debug.Log($"�Ăяo���F{role}���^�[���F{pair.key}");
                return pair.value;
            }
        }
        return int.MaxValue;
    }
}


[System.Serializable]
public class RolePriorityPair
{
    public Role key;   // ���[��
    public int value;  // �D��x
}

