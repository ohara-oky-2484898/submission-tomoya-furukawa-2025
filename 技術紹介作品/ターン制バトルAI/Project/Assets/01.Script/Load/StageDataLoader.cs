using System.Collections.Generic;
using UnityEngine;

public static class StageDataLoader
{
    // スクリプト内で基準となるパス
    private static string _imagePathPrefix = "Characters/";

    // 読み込み用
    private enum CharacterDataIndex
    {
        Name = 0,
        HP,
        ATK,
        MAG,
        MP,
        SPD,
        Team,
        AttackType,
        Role,
        ImagePath = 9
    }

    // Stageごとのデータを読み込む
    public static List<StageData> LoadStageData()
    {
        var stageList = new List<StageData>();

        // Resourcesフォルダ内のCSVファイルを読み込む
        TextAsset csvFile = Resources.Load<TextAsset>("CharacterData");

        if (csvFile == null)
        {
            Debug.LogError("LoadStageData：CSVファイルが見つかりません！");
            return stageList;
        }

        // 改行コードを正しく処理
        var lines = csvFile.text.Split(new[] { '\r', '\n' }, System.StringSplitOptions.RemoveEmptyEntries);

        int i = 0;
        while (i < lines.Length)
        {
            // ステージ開始判定
            if (lines[i].StartsWith("Stage"))
            {
                var stageNum = int.Parse(lines[i].Split(',')[1]);
                i++;

                // TurnOrderの読み取り
                //string turnOrderType = lines[i].Trim();  
                // ↑これは一行分全て（"TurnOrder,AllyThenEnemy,,,,,,,"）

                //string line = "TurnOrder,AllyThenEnemy,,,,,,,";
                //string[] parts = line.Split(',');　// 指定した文字列で区切る
                //parts[0] = "TurnOrder"
                //parts[1] = "AllyThenEnemy"
                //parts[2] = ""
                //parts[3] = ""
                // セルごとに扱えるようになる

                // TurnOrderの読み取り
                string[] turnOrderParts = lines[i].Split(',');  // カンマで分割
                // turnOrderParts[1] == 2番目の部分がターン順戦略名{ターンオーダー、素早さ順、、、}のように入っているから
                string turnOrderType = turnOrderParts.Length > 1 ? turnOrderParts[1].Trim() : string.Empty;
                ITurnOrderStrategy turnOrderStrategy = null;

                // 空でない場合にのみ、戦略名を使用して戦略を生成
                if (!string.IsNullOrEmpty(turnOrderType))
                {
                    turnOrderStrategy = StrategyFactory.GetTurnOrderStrategy(turnOrderType);
                }
                else
                {
                    Debug.LogError("LoadStageData：ターン順戦略が指定されていません");
                }

                i++;

                // ヘッダスキップ
                i++;

                List<CharacterData> characterList = new List<CharacterData>();

                //while (i < lines.Length && !string.IsNullOrWhiteSpace(lines[i]) && !lines[i].StartsWith("Stage"))
                // キャラクター行を読み込み
                while (i < lines.Length && !string.IsNullOrWhiteSpace(lines[i]))
                    {
                    var columns = lines[i].Split(',');

                    // 空行（カンマだけ）なら終了
                    if (columns.Length == 0 || string.Join("", columns).Trim() == "") break;

                    if (columns.Length < 10)
                    {
                        Debug.LogWarning($"LoadStageData：不正なキャラ行をスキップ: {lines[i]}");
                        i++;
                        continue;
                    }

                    // ステータス読み取り（文字列をintに変換, "15"→15）
                    int.TryParse(columns[(int)CharacterDataIndex.HP], out int hp);
                    int.TryParse(columns[(int)CharacterDataIndex.ATK], out int atk);
                    int.TryParse(columns[(int)CharacterDataIndex.MAG], out int mag);
                    int.TryParse(columns[(int)CharacterDataIndex.MP], out int mp);
                    int.TryParse(columns[(int)CharacterDataIndex.SPD], out int spd);

                    var status = new CharacterStatus(hp, atk, mag, mp, spd);


                    // 攻撃種類の読み取り
                    string attackTypeStr = columns[(int)CharacterDataIndex.AttackType].Trim();  // 攻撃タイプ名を取得
                    // Factoryを使用（攻撃タイプ名が存在したら対応するものを生成）
                    var basicAttackStrategy = StrategyFactory.GetAttackStrategy(attackTypeStr);

                    // 画像パスの読み込み ＆ 画像パスを動的に設定
                    string teamName = columns[(int)CharacterDataIndex.Team].Trim();  // Ally か Enemyを受け取り
                    string imageName = columns[(int)CharacterDataIndex.ImagePath].Trim(); ;   // 画像名
                    string fullImagePath = _imagePathPrefix + teamName + "/" + imageName;
                    // "Characters/Ally/hero"のようになる
                    // これをすることによってデータ(csvファイル)のほうで画像の名前指定だけで可能になる
                    Sprite sprite = Resources.Load<Sprite>(fullImagePath);  // 完全なパスを組み合わせてスプライトをロード


                    var character = new CharacterData(
                        columns[(int)CharacterDataIndex.Name],      // 名前
                        status,
                        teamName,
                        //columns[(int)CharacterDataIndex.Team],      // チーム
                        basicAttackStrategy,                        // 攻撃戦略
                        columns[(int)CharacterDataIndex.Role],      // 役職
                        sprite
                    );

                    characterList.Add(character);
                    i++;
                }

                // ステージデータ登録
                stageList.Add(new StageData(stageNum, turnOrderStrategy, characterList));
            }
            else
            {
                // 空行などスキップ
                i++;
            }
        }

		// デバッグ出力
		//foreach (var stage in stageList)
		//{
		//	Debug.Log($"ステージ{stage.StageNum} - ターン順: {stage.TurnOrderStrategy} - キャラ数: {stage.Characters.Count}");
		//	foreach (var character in stage.Characters)
		//	{
		//		Debug.Log($"キャラ: {character.Name}, チーム: {character.Team}, 攻撃: {character.BasicAttackStrategy}, 役職: {character.Role}");
		//	}
		//}

		return stageList;
    }
}