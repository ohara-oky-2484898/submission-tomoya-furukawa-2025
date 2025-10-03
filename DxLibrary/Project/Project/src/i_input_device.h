#pragma once
#include <string>

class IInputDevice {
public:
    virtual ~IInputDevice() = default;
    virtual bool GetButton(const std::string& path) const = 0;
    virtual float GetAxis(const std::string& path) const = 0;
};
