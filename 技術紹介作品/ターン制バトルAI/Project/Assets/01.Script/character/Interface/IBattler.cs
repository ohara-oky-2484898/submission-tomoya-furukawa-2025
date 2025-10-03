
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// チームの種類
/// </summary>
public enum Team
{
	Ally,    // 味方
	Enemy,   // 敵
			 // 今はboolでも良いが、
			 // 味方でも敵でもない中立ができても
			 // 良いようにenumにしている
}

/// <summary>
/// キャラクターの役割（ロール）
/// </summary>
public enum Role
{
	Hero,
	Warrior,
	Monk,
	Mage
}

/// <summary>
/// 戦う人
/// </summary>
public interface IBattler
{
	/// <summary> プロパティ </summary>
	string Name { get; }
	CharacterStatus Status { get; }
	Sprite Sprite { get; }
	bool IsAlive { get; }

	// バトルマネージャーは↓のように一括管理したいので
	// 実行内部で処理を分けられるように陣地を持たせておく
	//foreach (var battler in turnOrder)
	//{
	//	battler.DecideAction(_allBattlers);
	//}
	Team Team { get; }

	Role Roll { get; }
	// 通常攻撃の種類
	IBasicAttackStrategy BasicAttackStrategy { get; }
	IBattleAction ReservedAction { get; }


	/// <summary> メソッド </summary>

	void Init();

	/// <summary>
	/// 基本(通常)攻
	/// <summary>
	/// 初期化処理
	/// </summary>撃用のステータス値取得用
	/// </summary>
	/// <returns>攻撃力を参照なのか、魔力を参照なのかなど</returns>
	int GetBasicAttackStatValue();


	/// <summary>
	/// MP消費なしの基本的な軽い通常攻撃を実行
	/// </summary>
	/// <param name="target">攻撃対象</param>
	/// <returns>攻撃成功したかどうか</returns>
	bool BasicAttack(IBattler target);

	/// <summary>
	/// 死亡処理
	/// </summary>
	void Dead();


	// 行動を決める(予約する)関数
	void DecideAction(List<IBattler> allBattlers);
	// 予約済みの行動を実行する関数
	void ExecuteAction();

}
