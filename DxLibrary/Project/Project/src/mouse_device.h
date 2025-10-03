#pragma once
#include "i_input_device.h"
#include <string>

class MouseDevice : public IInputDevice
{
public:
	MouseDevice();
	bool GetButton(const std::string &path) const override;
	float GetAxis(const std::string &path) const override;
	void UpdateMouseMovement() const;

private:
	mutable int last_mouse_x_ = 0;
	mutable int last_mouse_y_ = 0;
	mutable int delta_x_ = 0;
	mutable int delta_y_ = 0;
};
