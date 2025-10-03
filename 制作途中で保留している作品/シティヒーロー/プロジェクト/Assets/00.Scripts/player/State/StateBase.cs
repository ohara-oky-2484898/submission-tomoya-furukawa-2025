public abstract class StateBase<TMachine>
{
	/// <summary>
	/// �C���X�^���X
	/// </summary>
	protected TMachine _machine;

	// �R���X�g���N�^
	public StateBase(TMachine mashine) { _machine = mashine; }

	/// <summary>
	/// �������u�ԁA�����������Ȃ�
	/// </summary>
	public virtual void OnEnterState() { }

	/// <summary>
	/// ���͏����E�A�j���[�V��������EUI�E�^�C�}�[�Ȃ�
	/// Transform�̕ύX
	/// </summary>
	public virtual void OnUpdate(){}

	/// <summary>
	/// ���������ERigidbody�̑���
	/// Physics�Ȃǂ̓����蔻��
	/// </summary>
	public virtual void OnFixedUpdate() { }

	/// <summary>
	/// �o��u�ԁA���Z�b�g�����Ȃ�
	/// </summary>
	public virtual void OnExitState() { }
}
