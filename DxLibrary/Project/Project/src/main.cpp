//#include "DxLib.h"
//#pragma region ����m�F�p
////// �v���O������ WinMain ����n�܂�܂�
////int WINAPI WinMain(HINSTANCE hInstance, HINSTANCE hPrevInstance, LPSTR lpCmdLine, int nCmdShow)
////{
////	if (DxLib_Init() == -1)		// �c�w���C�u��������������
////	{
////		return -1;			// �G���[���N�����璼���ɏI��
////	}
////
////	DrawPixel(320, 240, GetColor(255, 255, 255));	// �_��ł�
////
////	WaitKey();				// �L�[���͑҂�
////
////	DxLib_End();				// �c�w���C�u�����g�p�̏I������
////
////	return 0;				// �\�t�g�̏I�� 
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
//    if (DxLib_Init() == -1)    // �c�w���C�u��������������
//    {
//        return -1;    // �G���[���N�����璼���ɏI��
//    }
//
//    // �`����ʂ𗠉�ʂɂ���
//    SetDrawScreen(DX_SCREEN_BACK);
//
//    // ���F�̒l���擾
//    Cr = GetColor(255, 255, 255);
//
//    // �d�r�b�L�[���������܂Ń��[�v
//    while ((GetJoypadInputState(DX_INPUT_KEY_PAD1) & PAD_INPUT_9) == 0)
//    {
//        // ���b�Z�[�W����
//        if (ProcessMessage() == -1)
//        {
//            break;    // �G���[�����������烋�[�v�𔲂���
//        }
//
//        // �p�b�h�P�̓��͂��擾
//        GetJoypadAnalogInput(&InputX, &InputY, DX_INPUT_KEY_PAD1);
//
//        // ��ʂɓ��͏�Ԃ�\������
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
//        // ����ʂ̓��e��\��ʂɔ��f������
//        ScreenFlip();
//        WaitVSync(1);
//
//        //WaitTimer();
//    }
//
//    DxLib_End();        // �c�w���C�u�����g�p�̏I������
//
//    return 0;        // �\�t�g�̏I��
//}
//#pragma endregion