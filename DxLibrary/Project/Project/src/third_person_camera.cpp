#include "third_person_camera.h"
#include "DxLib.h"
#include "math_utils.h"
#include "time_manager.h"
#include <algorithm>
#include "vector2.h"


ThirdPersonCamera::ThirdPersonCamera() {
	// 初期カメラ距離を最大に設定
	camera_distance_ = camera_distance_min_max_.y;
}

ThirdPersonCamera::ThirdPersonCamera(const VECTOR& target, const VECTOR& local_cam_pos, float sensitivity) {
	SetTarget(target);
	SetCameraTransform(local_cam_pos);
	SetSensitivity(sensitivity);
}


void ThirdPersonCamera::Update(Vector2 right_stick_input) {
	float dt = TimeManager::delta_time();
	OnLook(right_stick_input.x, right_stick_input.y);
	UpdateRotation();
	UpdateCameraCollision();
}


// 右スティック入力による回転処理
void ThirdPersonCamera::OnLook(float delta_x, float delta_y) {
	yaw_ += delta_x * camera_sensitivity_  * TimeManager::delta_time();
	pitch_ += delta_y * camera_sensitivity_ * TimeManager::delta_time();

	// 上下角度制限
	pitch_ = math_utils::Clamp(pitch_, pitch_min_max_.x, pitch_min_max_.y);
}


// カメラの回転更新（ターゲット中心に回転）
void ThirdPersonCamera::UpdateRotation() {

	VECTOR target_rot = VGet(pitch_, yaw_, 0.0f);
	
	// スムージングをかけた回転ベクトルにする
	current_rotation_ = VAdd(
		current_rotation_,
		VScale(VSub(target_rot, current_rotation_), smoothing_)
	);

	// 回転角度を元に視線方向を計算
	MATRIX rot_y = MGetRotY(current_rotation_.y * DX_PI_F / 180.0f);
	MATRIX rot_x = MGetRotX(current_rotation_.x * DX_PI_F / 180.0f);
	MATRIX rot = MMult(rot_y, rot_x);

	camera_direction_ = VTransform(VGet(0.0f, 0.0f, -1.0f), rot);
}


// カメラと障害物との衝突処理
void ThirdPersonCamera::UpdateCameraCollision() {

	// ターゲットから後方に向かったカメラ位置を計算
	VECTOR desired_pos = VAdd(
		target_position_,
		VScale(camera_direction_, camera_distance_)
	);

	// 今は衝突判定せず、そのまま反映（仮実装）
	camera_local_position_ = VAdd(
		camera_local_position_,
		VScale(VSub(desired_pos, camera_local_position_), smooth_time_)
	);
}


// 注視対象の座標を設定
void ThirdPersonCamera::SetTarget(const VECTOR& target) {
	target_position_ = target;
}


// カメラのローカル初期位置を設定
void ThirdPersonCamera::SetCameraTransform(const VECTOR& local_pos) {
	camera_local_position_ = VNorm(local_pos);
	camera_direction_ = camera_local_position_;
}


// 衝突レイヤーを設定（使う側がビットマスクで指定）
void ThirdPersonCamera::SetObstacleLayer(int layer_mask) {
	obstacle_layer_ = layer_mask;
}

//　カメラの感度を設定
void ThirdPersonCamera::SetSensitivity(float sensitivity) {
	camera_sensitivity_ = sensitivity;
}


VECTOR ThirdPersonCamera::GetCameraPosition() const {
	// 注視点のワールド座標 + ローカル方向 * 距離 をワールド座標で返す
	return VAdd(target_position_, VScale(camera_direction_, camera_distance_));
}


VECTOR ThirdPersonCamera::GetTargetPosition() const {
	return target_position_;
}


VECTOR ThirdPersonCamera::TransformPoint(const VECTOR& local) {
	return VAdd(target_position_, local);
}
