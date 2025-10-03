// MEMO:Unityは列オーダー
// typename = 型,classはclssの代入を想定
// 少しでもUnityと同じようにゲームを作れるような設計が目標する

#pragma once
#include <iostream>
#include <vector>
#include "DxLib.h"
#include "vector2.h"
#include <memory>
#include <string>
#include <unordered_map>
#include <typeindex>

// 前方宣言
class GameObject;

/// <summary>
/// 最上位クラス
/// すべてのコンポーネント、ゲームオブジェクトはこれを継承する
/// </summary>
class Object
{
public:
    virtual ~Object() = default;
};

class Component : public Object
{

};


/// <summary>
/// Transformコンポーネント
/// 全ゲームオブジェクトに必ずついてあるもの
/// </summary>
class Transform : public Component
{
public:
    Vector3 position;
    Quaternion rotate;
    Vector3 scale;

    // デフォルトコンストラクタ
    Transform() : position(0, 0, 0), rotate(0, 0, 0, 1), scale(1, 1, 1) {}
};

/// <summary>
/// ゲームオブジェクト
/// </summary>
class GameObject : public Object
{
public:
    GameObject();
    GameObject(const std::string &name);
    virtual ~GameObject();

    // 基本プロパティ
    const std::string &GetName() const { return name_; }
    void SetName(const std::string &name) { name_ = name; }

    const std::string &GetTag() const { return tag_; }
    void SetTag(const std::string &tag) { tag_ = tag; }

    // 有効/無効状態
    bool IsActive() const { return active_; }
    void SetActive(bool active);

    // Component管理
    template <typename T>
    std::shared_ptr<T> AddComponent()
    {
        static_assert(std::is_base_of<Component, T>::value, "T must inherit from Component");

        auto component = std::make_shared<T>();
        // component->SetGameObject(this);
        // component->SetName(typeid(T).name());

        components_.push_back(component);
        componentMap_[typeid(T)] = component;

        // Awake()を呼び出し
        // component->Awake();

        return component;
    }

    template <typename T>
    std::shared_ptr<T> GetComponent()
    {
        static_assert(std::is_base_of<Component, T>::value, "T must inherit from Component");

        auto it = componentMap_.find(typeid(T));
        if (it != componentMap_.end())
        {
            return std::static_pointer_cast<T>(it->second);
        }
        return nullptr;
    }

    template <typename T>
    bool HasComponent() const
    {
        static_assert(std::is_base_of<Component, T>::value, "T must inherit from Component");
        return componentMap_.find(typeid(T)) != componentMap_.end();
    }

    template <typename T>
    void RemoveComponent()
    {
        static_assert(std::is_base_of<Component, T>::value, "T must inherit from Component");

        auto it = componentMap_.find(typeid(T));
        if (it != componentMap_.end())
        {
            auto component = it->second;
            // component->OnDestroy();
            // component->MarkDestroyed();

            // リストから削除
            components_.erase(
                std::remove(components_.begin(), components_.end(), component),
                components_.end());

            componentMap_.erase(it);
        }
    }

    // ライフサイクル管理
    void Awake();       // 全ComponentのAwake()を呼び出し
    void Start();       // 全ComponentのStart()を呼び出し
    void Update();      // 全ComponentのUpdate()を呼び出し
    void FixedUpdate(); // 全ComponentのFixedUpdate()を呼び出し
    void LateUpdate();  // 全ComponentのLateUpdate()を呼び出し
    void OnDestroy();   // 全ComponentのOnDestroy()を呼び出し

    // 子オブジェクト管理
    void AddChild(std::shared_ptr<GameObject> child);
    void RemoveChild(std::shared_ptr<GameObject> child);
    std::vector<std::shared_ptr<GameObject>> GetChildren() const { return children_; }

    // 親オブジェクト管理
    GameObject *GetParent() const { return parent_; }
    void SetParent(GameObject *parent);

private:
    std::string name_;
    std::string tag_;
    bool active_;

    std::vector<std::shared_ptr<Component>> components_;
    std::unordered_map<std::type_index, std::shared_ptr<Component>> componentMap_;

    std::vector<std::shared_ptr<GameObject>> children_;
    GameObject *parent_;

    bool started_;
    bool destroyed_;
};


/// <summary>
/// 
/// </summary>
class Rigidbody : public Component
{
public:
    float mass;
    Vector3 velocity;

    // デフォルトコンストラクタ
    Rigidbody() : mass(1.0f)
    {
        velocity.x = velocity.y = velocity.z = 0.0f;
    }

    // パラメータ付きコンストラクタ
    Rigidbody(float m) : mass(m)
    {
        velocity.x = velocity.y = velocity.z = 0.0f;
    }

    void ApplyForce(float force[3])
    {
        // 質量に基づいて速度を変更
    }
};

/// <summary>
/// 当たり判定用のコンポーネント
/// </summary>
class Collider : public Component
{
public:
    Collider() : enable(true), isTrigger(false) {}

    bool enable;
    bool isTrigger;
};

class BoxCollider : public Collider
{
public:
    BoxCollider() : size(Vector3::zero()), center(Vector3::zero()) {}
    BoxCollider(Vector3 size_, Vector3 center_) : size(size_), center(center_) {}
    ~BoxCollider() {}

private:
    Vector3 size; // 衝突体積を示すサイズ
    Vector3 center;
};

class SphereCollider : public Collider
{
};
