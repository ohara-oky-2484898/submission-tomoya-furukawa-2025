using System.Collections.Generic;
using UnityEngine;

public class PatternBasedAISelector
{
    private Dictionary<string, IJankenAI> patternToAIMap;
    private PlayerPatternDetector detector = new PlayerPatternDetector();

    private IJankenAI currentAI;
    private string currentStrategy;

    public PatternBasedAISelector()
    {
        patternToAIMap = new Dictionary<string, IJankenAI>
        {
            { "repeat", new CounterLastPlayerMoveAI() },
            { "cycle123", new MirrorAI() },
            { "cycle321", new LoseToLastPlayerMoveAI() },
            { "probability", new PatternAI() },
            { "markov", new PatternPredictionAI() }, // PatternPredictionAIの使用
            { "random", new RandomAI() }
        };

        currentAI = patternToAIMap["random"];
        currentStrategy = "random";
    }

    public IJankenAI SelectAI(IReadOnlyList<JankenHistory> history)
    {
        string pattern = detector.Detect(history);

        //if (patternToAIMap.ContainsKey(pattern))
        //    return patternToAIMap[pattern];

        //return new RandomAI();

        Debug.Log("現在のAIを変更");

        if (patternToAIMap.ContainsKey(pattern))
        {
            Debug.Log("現在のAIを変更if→" + pattern);
            currentAI = patternToAIMap[pattern];
            currentStrategy = pattern;
        }
        else
        {
            Debug.Log("現在のAIを変更else→ランダム");
            currentAI = patternToAIMap["random"];
            currentStrategy = "random";
        }

        return currentAI;
    }

    public IJankenAI GetCurrentAI() => currentAI;

    public string GetCurrentStrategy() => currentStrategy;

    // 特定条件で「ランダム」に戻す
    // わからなくなったときよう
    public void RevertToDefault()
    {
        currentAI = patternToAIMap["random"];
        currentStrategy = "random";
    }
}
