/// <summary>
/// キャラのステータス
/// </summary>
public class CharacterStatus
{
    // プロパティ
    // フィールドは自動生成
    public int MaxHP { get; }
    public int HP { get; set; }

    public int Attack { get; }
    public int Magic { get; }

    public int MaxMP { get; }
    public int MP { get; set; }

    public int Speed { get; }

    public CharacterStatus(int maxHP, int attack, int magic, int maxMp, int speed)
    {
        MaxHP = HP = maxHP;
        Attack = attack;
        Magic = magic;
        MaxMP = MP = maxMp;
        Speed = speed;
    }

    public CharacterStatus(CharacterStatus status)
    {
        HP = MaxHP = status.MaxHP;
        Attack = status.Attack;
        Magic = status.Magic;
        MP = MaxMP = status.MaxMP;
        Speed = status.Speed;
    }
}

