#include "time_manager.h"
#include "frame_counter.h"
#include "math_utils.h"
#include "game_common.h"
#include "vector2.h"
#include "third_person_camera.h"

#include "input_action_asset.h"
#include "keyboard_device.h"
#include "gamepad_device.h"
#include "mouse_device.h"

#include "move_action.h"
#include "look_action.h"

#include "gameobject.h"

#include <string>
#include <memory>
#include <DxLib.h>
using namespace math_utils;
// 構造体とclassの使い分け
// データの集まり→構造体					：例：vector3
// objectを意識→class(Method、カプセル化)	：例：player
// C#とは違い、どちらも値型ほんとにデフォのアクセス修飾子が違うだけ

void DebugLog(float vertical_offset, const char* label, float value)
{
	DrawBox(0, vertical_offset, 200, 20 + vertical_offset, GetColor(0, 0, 0), TRUE);
	DrawFormatString(5, 2 + vertical_offset, GetColor(255, 255, 255), "%s: %.2f", label, value);
}


// カメラ
// 視点：見る位置、注視点：どこをみるか、上方向
int WINAPI WinMain(HINSTANCE hI, HINSTANCE hP, LPSTR lpC, int nC)
{
	InputActionAsset inputAsset;
	InputActionMap gameMap;
	//InputActionMap gameMap("Game");  // 名前など引数を渡して初期化

	auto keyboard = std::make_shared<KeyboardDevice>();
	auto gamepad = std::make_shared<GamepadDevice>();
	auto mouse = std::make_shared<MouseDevice>();
	std::vector<std::shared_ptr<IInputDevice>> devices = {keyboard, gamepad, mouse};

	std::shared_ptr<MoveAction> move = std::make_shared<MoveAction>();
	move->AddBinding("keyboard/w");
	move->AddBinding("keyboard/a");
	move->AddBinding("keyboard/s");
	move->AddBinding("keyboard/d");
	move->AddBinding("gamepad/leftStick/x");
	move->AddBinding("gamepad/leftStick/y");
	gameMap.AddAction("Move", move);

	std::shared_ptr<LookAction> look = std::make_shared<LookAction>();
	look->AddBinding("gamepad/rightStick/x");
	look->AddBinding("gamepad/rightStick/y");
	look->AddBinding("mouse/move/x");
	look->AddBinding("mouse/move/y");
	gameMap.AddAction("Look", look);

	inputAsset.AddMap("Game", gameMap);

	int model1, anim_nutral, anim_run, attachindx, rootflm;
	bool running = FALSE;
	float anim_totaltime, playtime = 0.0f;

	VECTOR playerPos = VGet(600.0f, 300.0f, -400.0f);
	VECTOR cpos = VGet(600.0f, 600.0f, -400.0f), ctgt = VGet(600.0f, 300.0f, -400.0f);
	int key;
	enum Direction
	{
		DOWN,
		LEFT,
		UP,
		RIGHT
	} direction = DOWN;
	MATRIX mat1, mat2;

	// ウィンドウモードか全画面モードか選択できる(false:全画面)
	ChangeWindowMode(TRUE);
	// 描画対象(ウィンドウサイズ)の画面を指定
	SetGraphMode(GameConfig::kScreenWidth, GameConfig::kScreenHeight, GameConfig::kColorBit);
	// 初期化失敗
	if (DxLib_Init() == 1) return -1;

	// モデル読み込み
	model1 = MV1LoadModel("rsc\\Player\\PC.mv1");
	if (model1 == -1) return -1;
	// アニメーション読み込み
	anim_nutral = MV1LoadModel("rsc\\Player\\Anim_Neutral.mv1");
	if (anim_nutral == -1) return -1;
	anim_run = MV1LoadModel("rsc\\Player\\Anim_Run.mv1");
	if (anim_run == -1) return -1;
	attachindx = MV1AttachAnim(model1, 0, anim_nutral);
	anim_totaltime = MV1GetAttachAnimTotalTime(model1, attachindx);
	rootflm = MV1SearchFrame(model1, "root");
	MV1SetFrameUserLocalMatrix(model1, rootflm, MGetIdent());

	SetDrawScreen(DX_SCREEN_BACK);

	// カメラの設定
	// カメラの視点注視点上方向を設定
	//SetCameraPositionAndTargetAndUpVec(cpos, ctgt, VGet(0.0f, 0.0f, 1.0f));

	// カメラのオフセット（後ろ＋上から見下ろす位置）
	VECTOR camOffset = VGet(0.0f, 300.0f, -300.0f);

	// カメラ生成（追従対象、オフセット、感度）
	ThirdPersonCamera camera(playerPos, camOffset, 200.0f);
	FrameCounter frame_counter;

	TimeManager::Initialize();
	int frameCount = 0;
	//								// 定数で指定したkeyが押されているかチェック：押されてたら 1 
	while (ProcessMessage() == 0 && CheckHitKey(KEY_INPUT_ESCAPE) == 0)
	{
		TimeManager::Update();
		frame_counter.Update();

		// マウスデバイスの更新を明示的に行う
		mouse->UpdateMouseMovement();

		// マウス入力のデバッグ情報を表示
		float mouseX = mouse->GetAxis("mouse/move/x");
		float mouseY = mouse->GetAxis("mouse/move/y");
		DrawFormatString(10, 200, GetColor(255, 255, 0), "Mouse Delta: (%.2f, %.2f)", mouseX, mouseY);

		// アニメーション更新
		playtime += 0.5;
		if (playtime > anim_totaltime)
			playtime = 0.0f;
		MV1SetAttachAnimTime(model1, attachindx, playtime);

		Vector2 moveInput = inputAsset.GetMap("Game").GetAction("Move")->ReadValue(devices);
		Vector2 lookInput = inputAsset.GetMap("Game").GetAction("Look")->ReadValue(devices);

		playerPos.x += moveInput.x * 4.0f;
		playerPos.z += moveInput.y * 4.0f;

		if (moveInput.x == 0 && moveInput.y == 0)
		{
			running = false;
			MV1DetachAnim(model1, attachindx);
			attachindx = MV1AttachAnim(model1, 0, anim_nutral);
			anim_totaltime = MV1GetAttachAnimTotalTime(model1, attachindx);
		}
		else
		{
			if (!running)
			{
				running = true;
				MV1DetachAnim(model1, attachindx);
				attachindx = MV1AttachAnim(model1, 0, anim_run);
				anim_totaltime = MV1GetAttachAnimTotalTime(model1, attachindx);
			}
		}

		// 背景設定
		ClearDrawScreen();
		DrawBox(0, 0, 1200, 800, GetColor(255, 255, 255), 1);

		if (moveInput.y < 0)
			direction = DOWN;
		else if (moveInput.y > 0)
			direction = UP;
		else if (moveInput.x < 0)
			direction = LEFT;
		else if (moveInput.x > 0)
			direction = RIGHT;

		// 回転更新(ラジアン)
		// 回転更新(ラジアン)
		MV1SetRotationXYZ(model1, VGet(0.0f, 1.57f * direction, 0.0f));
		//MV1SetScale()	// 大きさはこれ
		// 位置更新
		MV1SetPosition(model1, playerPos);

		//// 行列を使えば、回転／移動／Scaleそれぞれ計算が違うものでも
		//// 同じ計算式でまとめて処理できるようになる(同じ計算を繰り返す、PCの得意分野)
		// mat1 = MGetRotY(1.57f * direction);
		// mat2 = MGetTranslate(pos);
		// MV1SetMatrix(model1, MMult(mat1, mat2));

		MV1DrawModel(model1);

		// pos, targetPos,upVector
		// VECTOR Position = playerPos + VECTOR{ 0.0f, 300.0f, -300.0f };
		// VECTOR Postion = AddVector(playerPos, VECTOR{ 0.0f, 300.0f, -300.0f });
		VECTOR Postion = VAdd(playerPos, VECTOR{0.0f, 300.0f, -300.0f});
		VECTOR Target = playerPos;
		VECTOR UpVector = { 0.0f, 1.0f, 0.0f };  // 普通の上方向（Y軸）

		float playerOffset = 100.0f;
		camera.SetTarget(VGet(playerPos.x, playerPos.y + playerOffset, playerPos.z));
		// プレイヤー位置が変わったのでカメラにも更新を通知
		// Note: right stick is currently not implemented. Set dummy input
		camera.Update(lookInput);

		// カメラ位置と注視点を反映
		SetCameraPositionAndTargetAndUpVec(
			camera.GetCameraPosition(),   // カメラ位置
			camera.GetTargetPosition(),   // 注視点(ターゲット位置)
			VGet(0.0f, 1.0f, 0.0f)        // 上方向ベクトル
		);

		// スティックの入力値確認
		DebugLog(80.0f, "look.x>", lookInput.x);
		DebugLog(100.0f, "look.y>", lookInput.y);
		DebugLog(120, "move.x>", moveInput.x);
		DebugLog(140.0f, "move.y>", moveInput.y);
		DebugLog(160.0f, "framecount", frameCount++);


		frame_counter.Draw();

		// 表裏入れ替え
		ScreenFlip();
		frame_counter.Wait();
	}
	// 終了処理
	DxLib_End();

	return 0;
}