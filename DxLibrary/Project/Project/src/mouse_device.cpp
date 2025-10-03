#include "mouse_device.h"
#include <DxLib.h>

MouseDevice::MouseDevice()
{
    // 初期化時に現在のマウス座標を取得しておく
    GetMousePoint(&last_mouse_x_, &last_mouse_y_);
    delta_x_ = 0;
    delta_y_ = 0;
}

void MouseDevice::UpdateMouseMovement() const
{
    int current_x, current_y;
    GetMousePoint(&current_x, &current_y);

    delta_x_ = current_x - last_mouse_x_;
    delta_y_ = current_y - last_mouse_y_;

    last_mouse_x_ = current_x;
    last_mouse_y_ = current_y;
}

bool MouseDevice::GetButton(const std::string &path) const
{
    // 左中右ボタンの対応
    if (path == "mouse/leftButton")
        return (GetMouseInput() & MOUSE_INPUT_LEFT) != 0;
    if (path == "mouse/rightButton")
        return (GetMouseInput() & MOUSE_INPUT_RIGHT) != 0;
    if (path == "mouse/middleButton")
        return (GetMouseInput() & MOUSE_INPUT_MIDDLE) != 0;
    return false;
}

float MouseDevice::GetAxis(const std::string &path) const
{
    if (path == "mouse/move/x")
    {
        // マウス移動を適切なスケールに調整（感度を下げる）
        float value = static_cast<float>(delta_x_) * 0.05f;
        return value;
    }
    if (path == "mouse/move/y")
    {
        // マウス移動を適切なスケールに調整（感度を下げる）
        float value = static_cast<float>(delta_y_) * 0.05f;
        return value;
    }
    return 0.0f;
}
