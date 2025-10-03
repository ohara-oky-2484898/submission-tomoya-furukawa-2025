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
            // �{�^�������͎��_����ɂ͒ʏ�g��Ȃ��̂Ŗ���

            // ������
            float axis = device->GetAxis(path);

            if (std::abs(axis) <= 0.001f) {
                // �قڃ[���Ȃ疳��
                continue;
            }
            // path���Ƃɕ����ĉ��Z
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
