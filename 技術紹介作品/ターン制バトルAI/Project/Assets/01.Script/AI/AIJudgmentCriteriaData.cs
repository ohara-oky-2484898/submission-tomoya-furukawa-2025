using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// AIの判断基準データをまとめたクラス
/// </summary>
[CreateAssetMenu(fileName = "AIJudgmentCriteriaData", menuName = "ScriptableObjects/AIJudgmentCriteriaData", order = 1)]
public class AIJudgmentCriteriaData : ScriptableObject
{
    /// <summary>
    /// HPの判断基準フィールド
    /// </summary>
    [Header("自己回復の判断基準")]
    [Tooltip("自己回復を行うHP割合（指定％以下で発動）")]
    [SerializeField, Range(0f, 1f)] private float _selfHealingCriteria = 0.3f;

    [Header("単体回復の判断基準")]
    [Tooltip("単体回復を行うHP割合（指定％以下で発動）")]
    [SerializeField, Range(0f, 1f)] private float _singleHealingCriteria = 0.5f;

    [Header("全体回復の判断基準")]
    [Tooltip("全体回復を行うHP割合（指定％以下で発動）")]
    [SerializeField, Range(0f, 1f)] private float _allHealingCriteria = 0.7f;

    [Tooltip("全体回復を発動するために必要な条件を満たす味方の数")]
    [SerializeField] private int _requiredTeammatesForHealing = 2;

    /// <summary>
    /// 敵の優先ターゲット関連
    /// </summary>
    [Header("ターゲット優先設定")]

    [Tooltip("ロールの優先度よりもHPの低さを優先するかどうか")]
    /// <summary>ロールの優先度よりもHPの低さを優先するかどうか</summary>
    [SerializeField] private bool _prioritizeLowHpEnemyOverRole = false;
    [Tooltip("敵ロールごとのターゲット優先順位（値が小さいほど優先）")]
    /// <summary>敵ロールごとのターゲット優先順位（値が小さいほど優先）</summary>
    [SerializeField] private List<RolePriorityPair> _rolePriority = new List<RolePriorityPair>();



    /// <summary>
    /// 外部参照用プロパティ
    /// </summary>
    public float SelfHealingCriteria => _selfHealingCriteria;
    public float SingleHealingCriteria => _singleHealingCriteria;
    public float AllHealingCriteria => _allHealingCriteria;
    public int RequiredTeammatesForHealing => _requiredTeammatesForHealing;
    public bool PrioritizeLowHpEnemyOverRole => _prioritizeLowHpEnemyOverRole;


    /// <summary>
    /// 指定されたロールの優先順位を取得する
    /// </summary>
    /// <param name="role">判定対象のロール</param>
    /// <returns>優先順位（値が小さいほど優先。未定義の場合は最大値）</returns>
    public int GetRolePriority(Role role)
    {
        foreach (var pair in _rolePriority)
        {
            if (pair.key == role)
            { 
                Debug.Log($"呼び出し：{role}リターン：{pair.key}");
                return pair.value;
            }
        }
        return int.MaxValue;
    }
}


[System.Serializable]
public class RolePriorityPair
{
    public Role key;   // ロール
    public int value;  // 優先度
}

