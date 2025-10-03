#pragma once

#include "i_input_action.h"

#include <string>
#include <unordered_map>
#include <memory>

class InputActionMap {
public:
    void AddAction(const std::string& name, std::shared_ptr<IInputAction> action);
    std::shared_ptr<IInputAction> GetAction(const std::string& name) const;

private:
    std::unordered_map<std::string, std::shared_ptr<IInputAction>> actions_;
};

class InputActionAsset {
public:
    void AddMap(const std::string& name, const InputActionMap& map);
    InputActionMap& GetMap(const std::string& name);

private:
    std::unordered_map<std::string, InputActionMap> maps_;
};
