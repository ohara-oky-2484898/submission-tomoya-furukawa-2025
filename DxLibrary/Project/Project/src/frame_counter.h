#pragma once
#include <math.h>
#include "DxLib.h"

/// <summary>
/// �t���[�����Œ肷��p�̃N���X
/// </summary>
class FrameCounter
{
public:
	FrameCounter();

	bool Update();
	void Draw();
	void Wait();

private:
	int start_time_;
	int frame_counter_;
	float measured_fps_;// �v�Z���ʂ�fps
	
	static const int kFpsSampleCount = 60;// fps�̌v�Z�Ɏg���t���[����
	static const int kTargetFps = 60;	//�ݒ肵��FPS
};