#pragma once
#include "frame_counter.h"

FrameCounter::FrameCounter() :
	start_time_(0),
	frame_counter_(0),
	measured_fps_(0.0f)
{}

bool FrameCounter::Update() {
	if (frame_counter_ == 0) { //1フレーム目なら時刻を記憶
		start_time_ = GetNowCount();
	}
	if (frame_counter_ == kFpsSampleCount) {
		//60フレーム目(サンプル数到達)なら平均を計算する
		int now = GetNowCount();
		measured_fps_ = 1000.0f / ((now - start_time_) / static_cast<float>(kFpsSampleCount));
		frame_counter_ = 0;
		start_time_ = now;
	}
	frame_counter_++;
	return true;
}

void FrameCounter::Draw() {
	// FPSを見やすく黒背景に白文字で表示
	DrawBox(0, 0, 80, 20, GetColor(0, 0, 0), TRUE);	// 背景ボックス
	DrawFormatString(5, 2, GetColor(255, 255, 255), "%.1f", measured_fps_);
}

void FrameCounter::Wait() {
	int tookTime = GetNowCount() - start_time_;	//かかった時間
	int waitTime = frame_counter_ * 1000 / kTargetFps - tookTime;	//待つべき時間
	if (waitTime > 0) {
		Sleep(waitTime);	//待機
	}
}