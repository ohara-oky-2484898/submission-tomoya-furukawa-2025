
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �`�[���̎��
/// </summary>
public enum Team
{
	Ally,    // ����
	Enemy,   // �G
			 // ����bool�ł��ǂ����A
			 // �����ł��G�ł��Ȃ��������ł��Ă�
			 // �ǂ��悤��enum�ɂ��Ă���
}

/// <summary>
/// �L�����N�^�[�̖����i���[���j
/// </summary>
public enum Role
{
	Hero,
	Warrior,
	Monk,
	Mage
}

/// <summary>
/// �키�l
/// </summary>
public interface IBattler
{
	/// <summary> �v���p�e�B </summary>
	string Name { get; }
	CharacterStatus Status { get; }
	Sprite Sprite { get; }
	bool IsAlive { get; }

	// �o�g���}�l�[�W���[�́��̂悤�Ɉꊇ�Ǘ��������̂�
	// ���s�����ŏ����𕪂�����悤�ɐw�n���������Ă���
	//foreach (var battler in turnOrder)
	//{
	//	battler.DecideAction(_allBattlers);
	//}
	Team Team { get; }

	Role Roll { get; }
	// �ʏ�U���̎��
	IBasicAttackStrategy BasicAttackStrategy { get; }
	IBattleAction ReservedAction { get; }


	/// <summary> ���\�b�h </summary>

	void Init();

	/// <summary>
	/// ��{(�ʏ�)�U
	/// <summary>
	/// ����������
	/// </summary>���p�̃X�e�[�^�X�l�擾�p
	/// </summary>
	/// <returns>�U���͂��Q�ƂȂ̂��A���͂��Q�ƂȂ̂��Ȃ�</returns>
	int GetBasicAttackStatValue();


	/// <summary>
	/// MP����Ȃ��̊�{�I�Ȍy���ʏ�U�������s
	/// </summary>
	/// <param name="target">�U���Ώ�</param>
	/// <returns>�U�������������ǂ���</returns>
	bool BasicAttack(IBattler target);

	/// <summary>
	/// ���S����
	/// </summary>
	void Dead();


	// �s�������߂�(�\�񂷂�)�֐�
	void DecideAction(List<IBattler> allBattlers);
	// �\��ς݂̍s�������s����֐�
	void ExecuteAction();

}
