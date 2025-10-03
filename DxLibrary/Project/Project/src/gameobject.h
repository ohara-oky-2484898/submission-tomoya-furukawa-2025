// MEMO:Unity�͗�I�[�_�[
// typename = �^,class��clss�̑����z��
// �����ł�Unity�Ɠ����悤�ɃQ�[��������悤�Ȑ݌v���ڕW����

#pragma once
#include <iostream>
#include <vector>
#include "DxLib.h"
#include "vector2.h"
#include <memory>
#include <string>
#include <unordered_map>
#include <typeindex>

// �O���錾
class GameObject;

/// <summary>
/// �ŏ�ʃN���X
/// ���ׂẴR���|�[�l���g�A�Q�[���I�u�W�F�N�g�͂�����p������
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
/// Transform�R���|�[�l���g
/// �S�Q�[���I�u�W�F�N�g�ɕK�����Ă������
/// </summary>
class Transform : public Component
{
public:
    Vector3 position;
    Quaternion rotate;
    Vector3 scale;

    // �f�t�H���g�R���X�g���N�^
    Transform() : position(0, 0, 0), rotate(0, 0, 0, 1), scale(1, 1, 1) {}
};

/// <summary>
/// �Q�[���I�u�W�F�N�g
/// </summary>
class GameObject : public Object
{
public:
    GameObject();
    GameObject(const std::string &name);
    virtual ~GameObject();

    // ��{�v���p�e�B
    const std::string &GetName() const { return name_; }
    void SetName(const std::string &name) { name_ = name; }

    const std::string &GetTag() const { return tag_; }
    void SetTag(const std::string &tag) { tag_ = tag; }

    // �L��/�������
    bool IsActive() const { return active_; }
    void SetActive(bool active);

    // Component�Ǘ�
    template <typename T>
    std::shared_ptr<T> AddComponent()
    {
        static_assert(std::is_base_of<Component, T>::value, "T must inherit from Component");

        auto component = std::make_shared<T>();
        // component->SetGameObject(this);
        // component->SetName(typeid(T).name());

        components_.push_back(component);
        componentMap_[typeid(T)] = component;

        // Awake()���Ăяo��
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

            // ���X�g����폜
            components_.erase(
                std::remove(components_.begin(), components_.end(), component),
                components_.end());

            componentMap_.erase(it);
        }
    }

    // ���C�t�T�C�N���Ǘ�
    void Awake();       // �SComponent��Awake()���Ăяo��
    void Start();       // �SComponent��Start()���Ăяo��
    void Update();      // �SComponent��Update()���Ăяo��
    void FixedUpdate(); // �SComponent��FixedUpdate()���Ăяo��
    void LateUpdate();  // �SComponent��LateUpdate()���Ăяo��
    void OnDestroy();   // �SComponent��OnDestroy()���Ăяo��

    // �q�I�u�W�F�N�g�Ǘ�
    void AddChild(std::shared_ptr<GameObject> child);
    void RemoveChild(std::shared_ptr<GameObject> child);
    std::vector<std::shared_ptr<GameObject>> GetChildren() const { return children_; }

    // �e�I�u�W�F�N�g�Ǘ�
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

    // �f�t�H���g�R���X�g���N�^
    Rigidbody() : mass(1.0f)
    {
        velocity.x = velocity.y = velocity.z = 0.0f;
    }

    // �p�����[�^�t���R���X�g���N�^
    Rigidbody(float m) : mass(m)
    {
        velocity.x = velocity.y = velocity.z = 0.0f;
    }

    void ApplyForce(float force[3])
    {
        // ���ʂɊ�Â��đ��x��ύX
    }
};

/// <summary>
/// �����蔻��p�̃R���|�[�l���g
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
    Vector3 size; // �Փˑ̐ς������T�C�Y
    Vector3 center;
};

class SphereCollider : public Collider
{
};
