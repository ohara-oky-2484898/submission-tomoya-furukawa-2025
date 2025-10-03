#pragma once
#include "DxLib.h"

/// <summary>
/// Unity ‚É‚ ‚éŒ^‚ð’è‹`
/// </summary>

struct Vector2
{
    float x = 0.0f;
    float y = 0.0f;

    Vector2(int x_, int y_) : x(x_), y(y_) {}

    bool operator!=(const Vector2 &other) const
    {
        return x != other.x || y != other.y;
    }

    Vector2 operator+(const Vector2 &other) const
    {
        return {x + other.x, y + other.y};
    }

    Vector2 operator*(float scalar) const
    {
        return {x * scalar, y * scalar};
    }

    friend Vector2 operator*(float scalar, const Vector2 &vector)
    {
        return {vector.x * scalar, vector.y * scalar};
    }

    Vector2(float x_, float y_) : x(x_), y(y_) {}
    Vector2() : x(0.0f), y(0.0f) {}
};

struct Vector3
{
public:
    static Vector3 zero() { return Vector3(0.0f, 0.0f, 0.0f); }

    float x = 0.0f;
    float y = 0.0f;
    float z = 0.0f;

    bool operator!=(const Vector3 &other) const
    {
        return x != other.x || y != other.y || z != other.z;
    }

    Vector3 operator+(const Vector3 &other) const
    {
        return {x + other.x,
                y + other.y,
                z + other.z};
    }

    Vector3(float x_, float y_, float z_) : x(x_), y(y_), z(z_) {}
    Vector3() : x(0.0f), y(0.0f), z(0.0f) {}
};

struct Quaternion
{
    float x = 0.0f;
    float y = 0.0f;
    float z = 0.0f;
    float w = 0.0f;

    bool operator!=(const Quaternion &other) const
    {
        return x != other.x || y != other.y || z != other.z || w != other.w;
    }

    Quaternion operator+(const Quaternion &other) const
    {
        return {x + other.x,
                y + other.y,
                z + other.z,
                w + other.w};
    }

    Quaternion(float x_, float y_, float z_, float w_) : x(x_), y(y_), z(z_), w(w_) {}
    Quaternion() : x(0.0f), y(0.0f), z(0.0f), w(0.0f) {}
};