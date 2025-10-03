#pragma once
#include "DxLib.h"

class TimeManager {
public:	
	static void Initialize();     // 初期化：明示的に呼び出す
	static void Update();         // 毎フレーム呼び出す
	static float delta_time();    // 前フレームからの経過時間（秒）
	static float time();          // ゲーム開始からの経過時間（秒

private:
	TimeManager() = default;  // 外部から生成させない
	~TimeManager() = default;
	// コピーコンストラクタ・代入演算子を削除して複製禁止
	TimeManager(const TimeManager&) = delete;
	TimeManager& operator=(const TimeManager&) = delete;
	
	static int start_time_ms_;    // ゲーム開始時刻（ミリ秒）
	static int last_time_ms_;     // 前フレーム時刻（ミリ秒）
	static float delta_time_;     // フレーム間の経過時間（秒）
};
