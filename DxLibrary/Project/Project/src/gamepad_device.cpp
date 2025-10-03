#include "gamepad_device.h"
#include <DxLib.h>

bool GamepadDevice::GetButton(const std::string& path) const {
	// TODO：Jumpボタンなどの実装予定
	return false;
}

float GamepadDevice::GetAxis(const std::string& path) const {
	int left_x = 0, left_y = 0;
	int right_x = 0, right_y = 0;

	// 取得されるスティックの値は通常 -1000 〜 +1000 の範囲の整数値
	// 中央は0、左や上が負の値、右や下が正の値
	GetJoypadAnalogInput(&left_x, &left_y, DX_INPUT_PAD1);         // 左スティック
	GetJoypadAnalogInputRight(&right_x, &right_y, DX_INPUT_PAD1);  // 右スティック

	if (path == "gamepad/leftStick/x")  return static_cast<float>(left_x) / 1000.0f;
	// 上に倒したときに前に進んでほしいため符号を逆に
	if (path == "gamepad/leftStick/y")  return static_cast<float>(-left_y) / 1000.0f;
	if (path == "gamepad/rightStick/x") return static_cast<float>(right_x) / 1000.0f;
	if (path == "gamepad/rightStick/y") return static_cast<float>(right_y) / 1000.0f;

	return 0.0f;
}
