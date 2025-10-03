#pragma once
#pragma once
#include "inputdevice.h"
#include <vector>

class InputSystem {
private:
    static std::vector<InputDevice*> devices;

public:
    static void RegisterDevice(InputDevice* device);
    static void Update();
};

std::vector<InputDevice*> InputSystem::devices;

void InputSystem::RegisterDevice(InputDevice* device) {
    devices.push_back(device);
}

void InputSystem::Update() {
    for (auto* d : devices) {
        d->Update();
    }
}