#include "third_person_camera.h"
#include "DxLib.h"
#include "math_utils.h"
#include "time_manager.h"
#include <algorithm>
#include "vector2.h"


ThirdPersonCamera::ThirdPersonCamera() {
	// �����J�����������ő�ɐݒ�
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


// �E�X�e�B�b�N���͂ɂ���]����
void ThirdPersonCamera::OnLook(float delta_x, float delta_y) {
	yaw_ += delta_x * camera_sensitivity_  * TimeManager::delta_time();
	pitch_ += delta_y * camera_sensitivity_ * TimeManager::delta_time();

	// �㉺�p�x����
	pitch_ = math_utils::Clamp(pitch_, pitch_min_max_.x, pitch_min_max_.y);
}


// �J�����̉�]�X�V�i�^�[�Q�b�g���S�ɉ�]�j
void ThirdPersonCamera::UpdateRotation() {

	VECTOR target_rot = VGet(pitch_, yaw_, 0.0f);
	
	// �X���[�W���O����������]�x�N�g���ɂ���
	current_rotation_ = VAdd(
		current_rotation_,
		VScale(VSub(target_rot, current_rotation_), smoothing_)
	);

	// ��]�p�x�����Ɏ����������v�Z
	MATRIX rot_y = MGetRotY(current_rotation_.y * DX_PI_F / 180.0f);
	MATRIX rot_x = MGetRotX(current_rotation_.x * DX_PI_F / 180.0f);
	MATRIX rot = MMult(rot_y, rot_x);

	camera_direction_ = VTransform(VGet(0.0f, 0.0f, -1.0f), rot);
}


// �J�����Ə�Q���Ƃ̏Փˏ���
void ThirdPersonCamera::UpdateCameraCollision() {

	// �^�[�Q�b�g�������Ɍ��������J�����ʒu���v�Z
	VECTOR desired_pos = VAdd(
		target_position_,
		VScale(camera_direction_, camera_distance_)
	);

	// ���͏Փ˔��肹���A���̂܂ܔ��f�i�������j
	camera_local_position_ = VAdd(
		camera_local_position_,
		VScale(VSub(desired_pos, camera_local_position_), smooth_time_)
	);
}


// �����Ώۂ̍��W��ݒ�
void ThirdPersonCamera::SetTarget(const VECTOR& target) {
	target_position_ = target;
}


// �J�����̃��[�J�������ʒu��ݒ�
void ThirdPersonCamera::SetCameraTransform(const VECTOR& local_pos) {
	camera_local_position_ = VNorm(local_pos);
	camera_direction_ = camera_local_position_;
}


// �Փ˃��C���[��ݒ�i�g�������r�b�g�}�X�N�Ŏw��j
void ThirdPersonCamera::SetObstacleLayer(int layer_mask) {
	obstacle_layer_ = layer_mask;
}

//�@�J�����̊��x��ݒ�
void ThirdPersonCamera::SetSensitivity(float sensitivity) {
	camera_sensitivity_ = sensitivity;
}


VECTOR ThirdPersonCamera::GetCameraPosition() const {
	// �����_�̃��[���h���W + ���[�J������ * ���� �����[���h���W�ŕԂ�
	return VAdd(target_position_, VScale(camera_direction_, camera_distance_));
}


VECTOR ThirdPersonCamera::GetTargetPosition() const {
	return target_position_;
}


VECTOR ThirdPersonCamera::TransformPoint(const VECTOR& local) {
	return VAdd(target_position_, local);
}
