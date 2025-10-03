#include "time_manager.h"

// staticƒƒ“ƒo‚Ì’è‹`‚Æ‰Šú‰»
int TimeManager::start_time_ms_ = 0.0f;
int TimeManager::last_time_ms_ = 0.0f;
float TimeManager::delta_time_ = 0.0f;


void TimeManager::Initialize() {
	int now = GetNowCount();
	start_time_ms_ = now;
	last_time_ms_ = now;
	delta_time_ = 0.0f;
}

void TimeManager::Update() {
	int now = GetNowCount();
	delta_time_ = (now - last_time_ms_) / 1000.0f;  // ƒ~ƒŠ•b¨•b
	last_time_ms_ = now;
}

float TimeManager::delta_time() {
	return delta_time_;
}

float TimeManager::time() {
	return (last_time_ms_ - start_time_ms_) / 1000.0f;
}