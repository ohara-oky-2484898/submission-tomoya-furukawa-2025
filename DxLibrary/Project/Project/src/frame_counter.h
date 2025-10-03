#pragma once
#include <math.h>
#include "DxLib.h"

/// <summary>
/// フレームを固定する用のクラス
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
	float measured_fps_;// 計算結果のfps
	
	static const int kFpsSampleCount = 60;// fpsの計算に使うフレーム数
	static const int kTargetFps = 60;	//設定したFPS
};