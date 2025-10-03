using UnityEngine;


public class CharacterData
{
    // 自動実装プロパティ
    // ↓のようにフィールドがなくても
    // コンパイル時に内部でprivate string name;
    // という非公開のフィールドが自動で生成される

    //public string name; // ←これはフィールド
                        // public フィールドは、クラス外部から自由にアクセスされるため、
                        // データの不整合や予期しない変更を招くことがあるため
                        // カプセル化の原則に反しているとみなされる

    // どんなときにpulicのフィールドを使うの？
    // データへの直接アクセスが問題にならない場合。
    // たとえば、読み取り専用のデータや、シンプルなデータオブジェクトに使うことがある
    /// <summary>
    /// だけど！そんな場合でもUnityはプロパティを推奨している
    /// </summary>

    // または"雑"にインスペクターからいじりたいとき
    // 構造体や、設定クラスのようにただのデータの入れ物として使うクラスでは
    // 冗長なゲッターセッターを省略してシンプルに書くことがある
    /// <summary>
    /// だけど！そんな場合でもUnityはプロパティを推奨している
    /// </summary>
    // ↓これは現実的かな？
    // 定数または readonly の場合

    //public string Name { get; set; }

    // プロパティにしておけば読み取り専用にしたいときにできる
    public string Name { get;  }
    public CharacterStatus Status { get;  }  // キャラのステータス（HP, 攻撃力, 魔力, MP, 速度）
    public Sprite Sprite { get;  }    
    public string Team { get;  }  // "Ally" または "Enemy"
    public IBasicAttackStrategy BasicAttackStrategy { get;  }  // "SingleHit", "Combo", "Ranged"
    public string Role { get;  }  // 役職 ("Hero", "Warrior", "Monk", "Mage", etc.)

    public CharacterData(string name, CharacterStatus status, string team, IBasicAttackStrategy basicAttackStrategy, string role, Sprite sprite)
    {
        this.Name = name;
        Status = status;
        Team = team;
        BasicAttackStrategy = basicAttackStrategy;
        Role = role;
        Sprite = sprite;
    }

    public CharacterData(CharacterData data)
    {
        Name = data.Name;
        Status = data.Status;
        Team = data.Team;
        BasicAttackStrategy = data.BasicAttackStrategy;
        Role = data.Role;
        Sprite = data.Sprite;
    }
}


