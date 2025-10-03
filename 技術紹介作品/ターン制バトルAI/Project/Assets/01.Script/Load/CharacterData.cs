using UnityEngine;


public class CharacterData
{
    // ���������v���p�e�B
    // ���̂悤�Ƀt�B�[���h���Ȃ��Ă�
    // �R���p�C�����ɓ�����private string name;
    // �Ƃ�������J�̃t�B�[���h�������Ő��������

    //public string name; // ������̓t�B�[���h
                        // public �t�B�[���h�́A�N���X�O�����玩�R�ɃA�N�Z�X����邽�߁A
                        // �f�[�^�̕s������\�����Ȃ��ύX���������Ƃ����邽��
                        // �J�v�Z�����̌����ɔ����Ă���Ƃ݂Ȃ����

    // �ǂ�ȂƂ���pulic�̃t�B�[���h���g���́H
    // �f�[�^�ւ̒��ڃA�N�Z�X�����ɂȂ�Ȃ��ꍇ�B
    // ���Ƃ��΁A�ǂݎ���p�̃f�[�^��A�V���v���ȃf�[�^�I�u�W�F�N�g�Ɏg�����Ƃ�����
    /// <summary>
    /// �����ǁI����ȏꍇ�ł�Unity�̓v���p�e�B�𐄏����Ă���
    /// </summary>

    // �܂���"�G"�ɃC���X�y�N�^�[���炢���肽���Ƃ�
    // �\���̂�A�ݒ�N���X�̂悤�ɂ����̃f�[�^�̓��ꕨ�Ƃ��Ďg���N���X�ł�
    // �璷�ȃQ�b�^�[�Z�b�^�[���ȗ����ăV���v���ɏ������Ƃ�����
    /// <summary>
    /// �����ǁI����ȏꍇ�ł�Unity�̓v���p�e�B�𐄏����Ă���
    /// </summary>
    // ������͌����I���ȁH
    // �萔�܂��� readonly �̏ꍇ

    //public string Name { get; set; }

    // �v���p�e�B�ɂ��Ă����Γǂݎ���p�ɂ������Ƃ��ɂł���
    public string Name { get;  }
    public CharacterStatus Status { get;  }  // �L�����̃X�e�[�^�X�iHP, �U����, ����, MP, ���x�j
    public Sprite Sprite { get;  }    
    public string Team { get;  }  // "Ally" �܂��� "Enemy"
    public IBasicAttackStrategy BasicAttackStrategy { get;  }  // "SingleHit", "Combo", "Ranged"
    public string Role { get;  }  // ��E ("Hero", "Warrior", "Monk", "Mage", etc.)

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


