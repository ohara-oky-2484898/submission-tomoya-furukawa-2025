#pragma once
#include "i_input_device.h"

class GamepadDevice : public IInputDevice {
public:
	bool GetButton(const std::string& path) const override;
	float GetAxis(const std::string& path) const override;
};
