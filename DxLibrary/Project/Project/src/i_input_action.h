#pragma once


#include "vector2.h"
#include "vector"
#include <memory>
#include "i_input_device.h"

template<typename T>
T Clamp(T val, T minVal, T maxVal) {
    if (val < minVal) return minVal;
    if (val > maxVal) return maxVal;
    return val;
}

struct Binding {
    std::string controlPath;
};

class IInputAction {
public:
    virtual ~IInputAction() = default;
    // ����ReadValue��Vector2��Ԃ�����̃C���^�[�t�F�[�X
    virtual Vector2 ReadValue(const std::vector<std::shared_ptr<IInputDevice>>& devices) const = 0;
};

