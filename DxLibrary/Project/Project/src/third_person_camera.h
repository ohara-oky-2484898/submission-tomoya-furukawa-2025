#pragma once
#include "DxLib.h"
#include "math_utils.h"
#include "vector2.h"

class ThirdPersonCamera {
public:
	ThirdPersonCamera();
	ThirdPersonCamera(const VECTOR& target, const VECTOR& local_cam_pos, float sensitivity);


	void Update(Vector2 right_stick_input);
	void OnLook(float delta_x, float delta_y);			// カメラ回転入力（右スティック）
	void SetTarget(const VECTOR& target);				// 注視対象を設定
	void SetCameraTransform(const VECTOR& local_pos);	// カメラ位置の初期設定
	void SetObstacleLayer(int layer_mask);				// 衝突レイヤーを設定
	void SetSensitivity(float sensitivity);

	// カメラ位置・注視点取得用
	VECTOR GetCameraPosition() const;
	VECTOR GetTargetPosition() const;
	// 内部で変更を加えない／読み取り専用として安心
	// const VECTOR GetCameraPosition2(); // 戻り値がconstということだけ

private:
	void UpdateRotation();						// カメラ回転処理
	void UpdateCameraCollision();				// 障害物との衝突処理
	VECTOR TransformPoint(const VECTOR& local);	// ローカル座標をワールド座標に変換

	// カメラ回転制御
	float camera_sensitivity_ = 200.0f;					// カメラ回転感度
	VECTOR target_position_ = VGet(0.0f, 0.0f, 0.0f);	// 注視対象の位置
	VECTOR camera_direction_ = VGet(0.0f, 0.0f, -1.0f);	// カメラの向き（後方）
	float yaw_ = 0.0f;									// 左右回転
	float pitch_ = 0.0f;								// 上下回転
	Vector2 pitch_min_max_ = Vector2(-40.0f, 85.0f);	// 上下回転の制限角度（最小・最大）

	// カメラ回転のスムージング
	float smoothing_ = 0.12f;									// 回転補間の滑らかさ
	VECTOR rotation_smooth_velocity_ = VGet(0.0f, 0.0f, 0.0f);	// 補間速度
	VECTOR current_rotation_ = VGet(0.0f, 0.0f, 0.0f);			// 現在の回転角度
	float smooth_time_ = 0.1f;									// カメラ距離の補間時間

	// カメラ距離制御
	float camera_distance_ = 300.0f;  // 実際のカメラ距離
	Vector2 camera_distance_min_max_ = Vector2(5.0f, 300.0f);  // 距離の最小・最大
	VECTOR velocity_ = VGet(0.0f, 0.0f, 0.0f);  // 補間速度（距離用）

	// カメラ位置
	VECTOR camera_local_position_ = VGet(0.0f, 0.0f, 0.0f);  // カメラの現在のローカル位置

	// 障害物レイヤー
	int obstacle_layer_ = -1;  // 障害物として判定するレイヤーマスク
};
