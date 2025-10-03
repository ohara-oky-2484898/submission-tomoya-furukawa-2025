using System.Collections.Generic;
using UnityEngine;

public static class StageDataLoader
{
    // �X�N���v�g���Ŋ�ƂȂ�p�X
    private static string _imagePathPrefix = "Characters/";

    // �ǂݍ��ݗp
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

    // Stage���Ƃ̃f�[�^��ǂݍ���
    public static List<StageData> LoadStageData()
    {
        var stageList = new List<StageData>();

        // Resources�t�H���_����CSV�t�@�C����ǂݍ���
        TextAsset csvFile = Resources.Load<TextAsset>("CharacterData");

        if (csvFile == null)
        {
            Debug.LogError("LoadStageData�FCSV�t�@�C����������܂���I");
            return stageList;
        }

        // ���s�R�[�h�𐳂�������
        var lines = csvFile.text.Split(new[] { '\r', '\n' }, System.StringSplitOptions.RemoveEmptyEntries);

        int i = 0;
        while (i < lines.Length)
        {
            // �X�e�[�W�J�n����
            if (lines[i].StartsWith("Stage"))
            {
                var stageNum = int.Parse(lines[i].Split(',')[1]);
                i++;

                // TurnOrder�̓ǂݎ��
                //string turnOrderType = lines[i].Trim();  
                // ������͈�s���S�āi"TurnOrder,AllyThenEnemy,,,,,,,"�j

                //string line = "TurnOrder,AllyThenEnemy,,,,,,,";
                //string[] parts = line.Split(',');�@// �w�肵��������ŋ�؂�
                //parts[0] = "TurnOrder"
                //parts[1] = "AllyThenEnemy"
                //parts[2] = ""
                //parts[3] = ""
                // �Z�����ƂɈ�����悤�ɂȂ�

                // TurnOrder�̓ǂݎ��
                string[] turnOrderParts = lines[i].Split(',');  // �J���}�ŕ���
                // turnOrderParts[1] == 2�Ԗڂ̕������^�[�����헪��{�^�[���I�[�_�[�A�f�������A�A�A}�̂悤�ɓ����Ă��邩��
                string turnOrderType = turnOrderParts.Length > 1 ? turnOrderParts[1].Trim() : string.Empty;
                ITurnOrderStrategy turnOrderStrategy = null;

                // ��łȂ��ꍇ�ɂ̂݁A�헪�����g�p���Đ헪�𐶐�
                if (!string.IsNullOrEmpty(turnOrderType))
                {
                    turnOrderStrategy = StrategyFactory.GetTurnOrderStrategy(turnOrderType);
                }
                else
                {
                    Debug.LogError("LoadStageData�F�^�[�����헪���w�肳��Ă��܂���");
                }

                i++;

                // �w�b�_�X�L�b�v
                i++;

                List<CharacterData> characterList = new List<CharacterData>();

                //while (i < lines.Length && !string.IsNullOrWhiteSpace(lines[i]) && !lines[i].StartsWith("Stage"))
                // �L�����N�^�[�s��ǂݍ���
                while (i < lines.Length && !string.IsNullOrWhiteSpace(lines[i]))
                    {
                    var columns = lines[i].Split(',');

                    // ��s�i�J���}�����j�Ȃ�I��
                    if (columns.Length == 0 || string.Join("", columns).Trim() == "") break;

                    if (columns.Length < 10)
                    {
                        Debug.LogWarning($"LoadStageData�F�s���ȃL�����s���X�L�b�v: {lines[i]}");
                        i++;
                        continue;
                    }

                    // �X�e�[�^�X�ǂݎ��i�������int�ɕϊ�, "15"��15�j
                    int.TryParse(columns[(int)CharacterDataIndex.HP], out int hp);
                    int.TryParse(columns[(int)CharacterDataIndex.ATK], out int atk);
                    int.TryParse(columns[(int)CharacterDataIndex.MAG], out int mag);
                    int.TryParse(columns[(int)CharacterDataIndex.MP], out int mp);
                    int.TryParse(columns[(int)CharacterDataIndex.SPD], out int spd);

                    var status = new CharacterStatus(hp, atk, mag, mp, spd);


                    // �U����ނ̓ǂݎ��
                    string attackTypeStr = columns[(int)CharacterDataIndex.AttackType].Trim();  // �U���^�C�v�����擾
                    // Factory���g�p�i�U���^�C�v�������݂�����Ή�������̂𐶐��j
                    var basicAttackStrategy = StrategyFactory.GetAttackStrategy(attackTypeStr);

                    // �摜�p�X�̓ǂݍ��� �� �摜�p�X�𓮓I�ɐݒ�
                    string teamName = columns[(int)CharacterDataIndex.Team].Trim();  // Ally �� Enemy���󂯎��
                    string imageName = columns[(int)CharacterDataIndex.ImagePath].Trim(); ;   // �摜��
                    string fullImagePath = _imagePathPrefix + teamName + "/" + imageName;
                    // "Characters/Ally/hero"�̂悤�ɂȂ�
                    // ��������邱�Ƃɂ���ăf�[�^(csv�t�@�C��)�̂ق��ŉ摜�̖��O�w�肾���ŉ\�ɂȂ�
                    Sprite sprite = Resources.Load<Sprite>(fullImagePath);  // ���S�ȃp�X��g�ݍ��킹�ăX�v���C�g�����[�h


                    var character = new CharacterData(
                        columns[(int)CharacterDataIndex.Name],      // ���O
                        status,
                        teamName,
                        //columns[(int)CharacterDataIndex.Team],      // �`�[��
                        basicAttackStrategy,                        // �U���헪
                        columns[(int)CharacterDataIndex.Role],      // ��E
                        sprite
                    );

                    characterList.Add(character);
                    i++;
                }

                // �X�e�[�W�f�[�^�o�^
                stageList.Add(new StageData(stageNum, turnOrderStrategy, characterList));
            }
            else
            {
                // ��s�ȂǃX�L�b�v
                i++;
            }
        }

		// �f�o�b�O�o��
		//foreach (var stage in stageList)
		//{
		//	Debug.Log($"�X�e�[�W{stage.StageNum} - �^�[����: {stage.TurnOrderStrategy} - �L������: {stage.Characters.Count}");
		//	foreach (var character in stage.Characters)
		//	{
		//		Debug.Log($"�L����: {character.Name}, �`�[��: {character.Team}, �U��: {character.BasicAttackStrategy}, ��E: {character.Role}");
		//	}
		//}

		return stageList;
    }
}