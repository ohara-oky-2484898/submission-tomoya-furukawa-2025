#pragma once

/// <summary>
/// C++14�̂��� constexpr�Ή��� clamp ������
/// ���w�̊֐����܂Ƃ߂�����
/// </summary>
namespace math_utils {


	template <typename T>
	constexpr T Clamp(const T val, const T min_val, const T max_val) {
		return (val < min_val) ? min_val : (val > max_val) ? max_val : val;
	}

	template <typename T>
	constexpr T Max(const T lhs, const T rhs) {
		return (lhs > rhs) ? lhs : rhs;
	}

	template <typename T>
	constexpr T Min(const T lhs, const T rhs) {
		return (lhs < rhs) ? lhs : rhs;
	}

}
