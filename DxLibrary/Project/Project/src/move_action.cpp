#include "move_action.h"
#include "keyboard_device.h"
#include "gamepad_device.h"

#include <iostream>
#include "DxLib.h"


int logLine = 0;


void MoveAction::AddBinding(const std::string& control_path) {
    bindings_.emplace_back(Binding{ control_path });
}


Vector2 MoveAction::ReadValue(const std::vector<std::shared_ptr<IInputDevice>>& devices) const {
    Vector2 move{};

    for (const auto& binding : bindings_) {
        const std::string& path = binding.controlPath;

        for (const auto& device : devices) {
            bool pressed = device->GetButton(path);
            float axis = device->GetAxis(path);

            if (pressed) {
                if (path == "keyboard/w") move.y += 1.0f;
                else if (path == "keyboard/s") move.y -= 1.0f;
                else if (path == "keyboard/d") move.x += 1.0f;
                else if (path == "keyboard/a") move.x -= 1.0f;
            }
            if (std::abs(axis) > 0.01f) {
                if (path == "gamepad/leftStick/x") move.x += axis;
                else if (path == "gamepad/leftStick/y") move.y += axis;
            }
        }
    }
    return move;
}



