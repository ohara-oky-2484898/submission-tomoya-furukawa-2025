#pragma once
#include "DxLib.h"

class TimeManager {
public:	
	static void Initialize();     // �������F�����I�ɌĂяo��
	static void Update();         // ���t���[���Ăяo��
	static float delta_time();    // �O�t���[������̌o�ߎ��ԁi�b�j
	static float time();          // �Q�[���J�n����̌o�ߎ��ԁi�b

private:
	TimeManager() = default;  // �O�����琶�������Ȃ�
	~TimeManager() = default;
	// �R�s�[�R���X�g���N�^�E������Z�q���폜���ĕ����֎~
	TimeManager(const TimeManager&) = delete;
	TimeManager& operator=(const TimeManager&) = delete;
	
	static int start_time_ms_;    // �Q�[���J�n�����i�~���b�j
	static int last_time_ms_;     // �O�t���[�������i�~���b�j
	static float delta_time_;     // �t���[���Ԃ̌o�ߎ��ԁi�b�j
};
