#pragma once
#include <string>

class InputDevice {
protected:
    std::string name;

public:
    InputDevice(const std::string& name) : name(name) {}
    virtual void Update() = 0;
    std::string GetName() const { return name; }
    virtual ~InputDevice() {}
};