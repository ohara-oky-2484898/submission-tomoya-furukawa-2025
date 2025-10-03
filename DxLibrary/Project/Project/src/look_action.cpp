#include "look_action.h"
#include "keyboard_device.h"
#include "gamepad_device.h"
#include "mouse_device.h"
#include "vector2.h"

#include <cmath>

void LookAction::AddBinding(const std::string& control_path) {
    bindings_.emplace_back(Binding{ control_path });
}

Vector2 LookAction::ReadValue(const std::vector<std::shared_ptr<IInputDevice>>& devices) const {
    Vector2 look_delta{};

    for (const auto& binding : bindings_) {
        const std::string& path = binding.controlPath;

        for (const auto& device : devices) {
            // ƒ{ƒ^ƒ“‰Ÿ‰º‚ÍŽ‹“_‘€ì‚É‚Í’ÊíŽg‚í‚È‚¢‚Ì‚Å–³Ž‹

            // Ž²“ü—Í
            float axis = device->GetAxis(path);

            if (std::abs(axis) <= 0.001f) {
                // ‚Ù‚Úƒ[ƒ‚È‚ç–³Ž‹
                continue;
            }
            // path‚²‚Æ‚É•ª‚¯‚Ä‰ÁŽZ
            if (path == "gamepad/rightStick/x") {
                look_delta.x += axis;
            }
            else if (path == "gamepad/rightStick/y") {
                look_delta.y += axis;
            }
            else if (path == "mouse/move/x") {
                look_delta.x += axis;
            }
            else if (path == "mouse/move/y") {
                look_delta.y += axis;
            }
        }
    }
    return look_delta;
}
