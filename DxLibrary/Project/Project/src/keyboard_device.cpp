#include "keyboard_device.h"
#include <DxLib.h>

bool KeyboardDevice::IsKeyPressed(int key) const {
    return CheckHitKey(key) != 0;
}

bool KeyboardDevice::GetButton(const std::string& path) const {
    if (path == "keyboard/w") return IsKeyPressed(KEY_INPUT_W);
    if (path == "keyboard/s") return IsKeyPressed(KEY_INPUT_S);
    if (path == "keyboard/a") return IsKeyPressed(KEY_INPUT_A);
    if (path == "keyboard/d") return IsKeyPressed(KEY_INPUT_D);
    return false;
}

float KeyboardDevice::GetAxis(const std::string& path) const {
    return 0.0f; // ç°âÒÇÕñ¢égóp
}
