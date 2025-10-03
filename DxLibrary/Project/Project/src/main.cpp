//#include "DxLib.h"
//#pragma region 動作確認用
////// プログラムは WinMain から始まります
////int WINAPI WinMain(HINSTANCE hInstance, HINSTANCE hPrevInstance, LPSTR lpCmdLine, int nCmdShow)
////{
////	if (DxLib_Init() == -1)		// ＤＸライブラリ初期化処理
////	{
////		return -1;			// エラーが起きたら直ちに終了
////	}
////
////	DrawPixel(320, 240, GetColor(255, 255, 255));	// 点を打つ
////
////	WaitKey();				// キー入力待ち
////
////	DxLib_End();				// ＤＸライブラリ使用の終了処理
////
////	return 0;				// ソフトの終了 
////}
//
//int WINAPI WinMain(HINSTANCE hInstance, HINSTANCE hPrevInstance, LPSTR lpCmdLine, int nCmdShow)
//{
//    int Cr;
//    int InputX, InputY;
//    char String[64];
//
//    SetWaitVSyncFlag(FALSE);
//
//    if (DxLib_Init() == -1)    // ＤＸライブラリ初期化処理
//    {
//        return -1;    // エラーが起きたら直ちに終了
//    }
//
//    // 描画先画面を裏画面にする
//    SetDrawScreen(DX_SCREEN_BACK);
//
//    // 白色の値を取得
//    Cr = GetColor(255, 255, 255);
//
//    // ＥＳＣキーが押されるまでループ
//    while ((GetJoypadInputState(DX_INPUT_KEY_PAD1) & PAD_INPUT_9) == 0)
//    {
//        // メッセージ処理
//        if (ProcessMessage() == -1)
//        {
//            break;    // エラーが発生したらループを抜ける
//        }
//
//        // パッド１の入力を取得
//        GetJoypadAnalogInput(&InputX, &InputY, DX_INPUT_KEY_PAD1);
//
//        // 画面に入力状態を表示する
//        {
//            ClearDrawScreen();
//
//            wsprintf(String, "X = %d", InputX);
//            DrawString(0, 0, String, Cr);
//
//            wsprintf(String, "Y = %d", InputY);
//            DrawString(0, 16, String, Cr);
//        }
//
//        // 裏画面の内容を表画面に反映させる
//        ScreenFlip();
//        WaitVSync(1);
//
//        //WaitTimer();
//    }
//
//    DxLib_End();        // ＤＸライブラリ使用の終了処理
//
//    return 0;        // ソフトの終了
//}
//#pragma endregion