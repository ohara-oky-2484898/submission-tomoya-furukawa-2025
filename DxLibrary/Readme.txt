DXライブラリでUnityのように開発が出来るように作成中です。
最初にInputSystemのようなものを作ろうとしていました。


PADの左スティック／キーボードWASD➡移動
PADの右スティック／マウス　　　　➡視点移動

できたこと
	std::shared_ptr<MoveAction> move = std::make_shared<MoveAction>();
	move->AddBinding("keyboard/w");
	move->AddBinding("keyboard/a");
	move->AddBinding("keyboard/s");
	move->AddBinding("keyboard/d");
	move->AddBinding("gamepad/leftStick/x");
	move->AddBinding("gamepad/leftStick/y");
	gameMap.AddAction("Move", move);
	
	➡一見Unityと同じように設定ができること

インプットデバイスという基底クラスを作り
ポリモーフィズムを活用してPAD.Mouse,Keyboardの入力を検知すること



できなかったこと
	if (path == "gamepad/rightStick/y") return static_cast<float>(right_y) / 1000.0f;
	実際は文字列で比較しているので
	他のキーに割り当てたいときに現状ここも変えないといけないので
	アクションを作成するときに引数でもらったものをキャッシュしておく必要がある

	その他の実装ができていない

今後の取り組み
	GameObject、Componentを実装しモノビヘイビアーのライフサイクルのような動きをする関数を作っていきたい
	今のところゲームとしては移動と当たり判定だけ実する予定です

	UIではUnityのキャンバスのソードオーダーのようなものを実装し
	プログラムを書いた順ではなく、この値が大きなものが上にくるようにする