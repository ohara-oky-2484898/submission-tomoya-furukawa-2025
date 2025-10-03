#include "input_action_asset.h"

void InputActionMap::AddAction(const std::string& name, std::shared_ptr<IInputAction> action) {
    actions_[name] = action;
}

std::shared_ptr<IInputAction> InputActionMap::GetAction(const std::string& name) const {
    return actions_.at(name);
}

void InputActionAsset::AddMap(const std::string& name, const InputActionMap& map) {
    maps_[name] = map;
}

InputActionMap& InputActionAsset::GetMap(const std::string& name) {
    return maps_[name];
}


