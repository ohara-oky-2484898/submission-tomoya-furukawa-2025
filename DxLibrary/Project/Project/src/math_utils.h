#pragma once

/// <summary>
/// C++14‚Ì‚½‚ß constexpr‘Î‰‚Å clamp ‚ğ©ì
/// ”Šw‚ÌŠÖ”‚ğ‚Ü‚Æ‚ß‚½‚à‚Ì
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
