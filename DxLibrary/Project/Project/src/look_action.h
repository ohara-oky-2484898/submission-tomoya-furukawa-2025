#pragma once
#include "vector2.h"
#include "i_input_device.h"
#include "i_input_action.h"
#include <string>
#include <vector>
#include <memory>


class LookAction : public IInputAction{
public:
    LookAction() = default;
    LookAction(const LookAction& other) = default;
    LookAction& operator=(const LookAction& other) = default;

    void AddBinding(const std::string& control_path);
    Vector2 ReadValue(const std::vector<std::shared_ptr<IInputDevice>>& devices) const override;
private:
    std::vector<Binding> bindings_;
};
