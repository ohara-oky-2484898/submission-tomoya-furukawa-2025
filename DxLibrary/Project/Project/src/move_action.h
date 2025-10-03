#pragma once
#include "vector2.h"
#include "i_input_action.h"
#include "i_input_device.h"
#include <string>
#include <vector>
#include <memory>


class MoveAction : public IInputAction{
public:
    MoveAction() = default;
    MoveAction(const MoveAction& other) = default;
    MoveAction& operator=(const MoveAction& other) = default;

    void AddBinding(const std::string& control_path);
    Vector2 ReadValue(const std::vector<std::shared_ptr<IInputDevice>>& devices) const override;


private:
    std::vector<Binding> bindings_;
};