using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


// virtualとabstractの使い分け
// defaultでなにか実装を書きたいなら
// virtual
// 全てに個別で実装を書いてほしいなら
// abstract

/// <summary>
/// バトルの基本（抽象）クラス
/// </summary>
public abstract class BattlerBase : IBattler
{
    /// <summary>
    /// どの派生クラスにも共通するプロパティ
    /// 全てのバトラーに共通するものとしてvirtualにせず
    /// 親クラスで一度設定すればよい
    /// 必ず派生先で決めてほしいものにabstract
    /// </summary>

    /// <summary> フィールド </summary>
    protected AIJudgmentCriteriaData _aiCriteriaData;
    // 行動予約したものを一時保存する箱
    private IBattleAction _reservedAction;

    /// <summary> プロパティ </summary>
    public string Name { get; }
    public CharacterStatus Status { get;}
    public Sprite Sprite { get;}
    public bool IsAlive => Status.HP > 0;
    public Team Team { get;}   // 味方か敵か、自分の陣営
    public Role Roll { get;}

    // キャラごと(魔法使いは微力な魔法攻撃を／勇者はさっと攻撃をしてほしい)
    // けど参照先が同じなのは変(魔法なのに攻撃参照？)なのでそれぞれで決められるように
    // 通常攻撃のダメージ計算用にステータスの参照部を取得する関数
    public abstract int GetBasicAttackStatValue();
    public abstract IBasicAttackStrategy BasicAttackStrategy { get; }

    public IBattleAction ReservedAction
    {
        get => _reservedAction;
        // ReservedActionはprotected setterで派生からセット可能に
        protected set => _reservedAction = value;
    }


    /// <summary>
    /// コンストラクタ
    /// 読みこんだキャラクタデータをそのまま渡してinstanceできるように用意
    /// </summary>
    /// <param name="data">キャラデータ</param>
    public BattlerBase(CharacterData data)
    {
        Name = data.Name;
        Status = new CharacterStatus(data.Status);
        Team = ParseTeam(data.Team);
        Sprite = data.Sprite;
        Roll = ParseRoll(data.Role);
    }

    /// <summary> メソッド </summary>
    public virtual void Init()
    {
        // デフォルトでは特に何もしない設定だけ
        _aiCriteriaData =  GameManager.Instance.AiCriteriaData();  // AI判断基準データを取得
    }

    public bool BasicAttack(IBattler target)
    { 
        // 継承先で実装したそれぞれの基本攻撃のパターンどれか呼ぶ
        return BasicAttackStrategy.Execute(this, target); 
    }

    /// <summary> 行動を決める </summary>
    /// <param name="allBattlers">全バトラー</param>
    public abstract void DecideAction(List<IBattler> allBattlers);

    /// <summary> 行動を実行 </summary>
    /// なんらかで、失敗したときは弱攻撃に切り替え
    //public virtual void ExecuteAction()
    public void ExecuteAction()
    {
        if (!IsAlive)
        {
            Debug.Log($"{Name} はすでに死亡しているため、行動できません。");
            return;
        }

        Debug.Log($"[DEBUG] {Name} の ExecuteAction が呼ばれた");

        if (ReservedAction == null)
        {
            Debug.LogWarning($"[DEBUG] {Name} の ReservedAction が null だった");
            return;
        }

        try
        {
            if (ReservedAction.Execute())
            {
                Debug.LogWarning($"{Name} が予約行動を実行した！");
                ReservedAction = null;
            }
            else
            {
                Debug.LogWarning($"{Name} が予約行動を実行したが失敗した！");
                //Debug.LogWarning($"{Name} の行動は失敗したため、フォールバック攻撃を試みます。");
                //var enemies = this.GetEnemies(BattleManager.Instance.AllBattlers)
                //                  .Where(b => b.IsAlive)
                //                  .ToList();

                //var fallbackTarget = BattleSystem.SelectTarget(enemies, _aiCriteriaData);
                //if (fallbackTarget != null)
                //{
                //    BasicAttack(fallbackTarget);
                //}
                //else
                //{
                //    Debug.LogWarning($"{Name} は攻撃対象が見つからず、何もしなかった。");
                //}

                ReservedAction = null;
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"[ERROR] {Name} の ExecuteAction 実行中に例外発生: {ex.Message}\n{ex.StackTrace}");
        }
    }
    public virtual void Dead()
    {
        Debug.Log($"{Name} has fallen!");
    }


    /// <summary> privateメソッド </summary>
    
    /// <summary>
    /// 文字列("Ally")から列挙型(Team.Ally)に変換する関数
    /// </summary>
    /// <param name="team"></param>
    /// <returns></returns>
    private Team ParseTeam(string team)
    {
        // (enum指定)Enum.Parse(enumの型、value、大文字小文字を区別しない)
        // falseなら完全一致のみ
        return (Team)Enum.Parse(typeof(Team), team, ignoreCase: true);
    }
    private Role ParseRoll(string roll)
    {
        // (enum指定)Enum.Parse(enumの型、value、大文字小文字を区別しない)
        // falseなら完全一致のみ
        return (Role)Enum.Parse(typeof(Role), roll, ignoreCase: true);
    }
}
