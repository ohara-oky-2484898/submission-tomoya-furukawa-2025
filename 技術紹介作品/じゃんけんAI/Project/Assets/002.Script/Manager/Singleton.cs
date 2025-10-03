/// <summary>
/// �V���O���g���p
/// </summary>

using UnityEngine;


// Singleton
// �Ȃ����O������Ȃ́H
// MonoBehaviourSingleton�Ƃ������O�������炵����
// MonoBehaviour���g��Ȃ��Ȃ�
// static�ϐ��ł���΂�����˂Ƃ����Ӑ}

// GameMnager�̏ꍇ
// T ��GameMnager�ɒu�������ƍl����
public class Singleton<T> : MonoBehaviour where T : Singleton<T>
	// where T : Singleton<T>
	// ����ŃV���O���g���N���X���p�����Ă�����̂Ɍ���
	// ��������Ȃ��ƑS�R�֌W�Ȃ����̂��w��ł��Ă��܂�
	// ����� "Instance" �̒l�����������Ȃ��Ă��܂�
{
	// �p�����true�ɂ���΃Q�[���I�u�W�F�N�g���Ə�����
	protected virtual bool DestroyTargetGameObject => false;

	public static T Instance { get; private set; } = null;

	/// <summary>
	/// Singleton���L����
	/// null ���ǂ����`�F�b�N�������Ƃ��������Ǝv����
	/// </summary>
	public static bool IsValid() => Instance != null;

	private void Awake()
	{
		if (Instance == null)
		{
			// GameMnager�̂Ƃ���GameMnager���ƌ^���킩���Ă�����
			// ����͉��̌^���킩��Ȃ����߃L���X�g���Ă�����K�v������
			Instance = this as T;
			Instance.Init();
			return;
		}

		// �{���ǂ���̏����ł�������
		// �����Awake()�̒��ɓ���Ă��܂��Ă���̂�
		// �p����őI�ׂ�悤��
		// bool�ϐ���p�ӂ��Ă���
		if (DestroyTargetGameObject)
		{
			// �ʂɂ��̕ӂ̏�����
			// �p�����Init()�̒��ȂǂŎ��s���Ă���������
			Destroy(gameObject);
		}
		else
		{
			Destroy(this);
		}
	}

	/// <summary>
	/// �h���N���X�p��Awake
	/// ������Awake()�͂����g���Ă��܂�������
	/// �p����ŏ������������������Ƃ��悤��
	/// �o�[�`�������\�b�h��p��
	/// </summary>
	protected virtual void Init()
	{
	}

	private void OnDestroy()
	{
		if (Instance == this)
		{
			Instance = null;
		}
		OnRelease();
	}

	/// <summary>
	/// �h���N���X�p��OnDestroy
	/// </summary>
	protected virtual void OnRelease()
	{
	}
}

//public static class GameParameters
//{
//	public static int Score { get; private set; }

//	public static void AddScore(int value)
//	{
//		Score += value;
//	}

//	public static void ResetScore()
//	{
//		Score = 0;
//	}
//}