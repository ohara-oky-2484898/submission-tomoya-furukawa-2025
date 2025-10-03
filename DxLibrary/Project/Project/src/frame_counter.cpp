#pragma once
#include "frame_counter.h"

FrameCounter::FrameCounter() :
	start_time_(0),
	frame_counter_(0),
	measured_fps_(0.0f)
{}

bool FrameCounter::Update() {
	if (frame_counter_ == 0) { //1�t���[���ڂȂ玞�����L��
		start_time_ = GetNowCount();
	}
	if (frame_counter_ == kFpsSampleCount) {
		//60�t���[����(�T���v�������B)�Ȃ畽�ς��v�Z����
		int now = GetNowCount();
		measured_fps_ = 1000.0f / ((now - start_time_) / static_cast<float>(kFpsSampleCount));
		frame_counter_ = 0;
		start_time_ = now;
	}
	frame_counter_++;
	return true;
}

void FrameCounter::Draw() {
	// FPS�����₷�����w�i�ɔ������ŕ\��
	DrawBox(0, 0, 80, 20, GetColor(0, 0, 0), TRUE);	// �w�i�{�b�N�X
	DrawFormatString(5, 2, GetColor(255, 255, 255), "%.1f", measured_fps_);
}

void FrameCounter::Wait() {
	int tookTime = GetNowCount() - start_time_;	//������������
	int waitTime = frame_counter_ * 1000 / kTargetFps - tookTime;	//�҂ׂ�����
	if (waitTime > 0) {
		Sleep(waitTime);	//�ҋ@
	}
}