#pragma once
#include "i_input_device.h"
#include <string>

class KeyboardDevice : public IInputDevice {
public:
    bool GetButton(const std::string& path) const override;
    float GetAxis(const std::string& path) const override;

private:
    bool IsKeyPressed(int key) const;
};