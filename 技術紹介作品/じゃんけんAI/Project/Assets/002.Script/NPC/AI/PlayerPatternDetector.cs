using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerPatternDetector
{
    public string Detect(IReadOnlyList<JankenHistory> history)
    {
        if (history.Count < 3) return "random";

        if (history.All(h => h.PlayerHand == history[0].PlayerHand))
            return "repeat";

        if (IsCycle(history, new[] { JankenHand.Rock, JankenHand.Scissors, JankenHand.Paper }))
            return "cycle123";

        if (IsCycle(history, new[] { JankenHand.Paper, JankenHand.Scissors, JankenHand.Rock }))
            return "cycle321";

        if (IsAlternating(history))
            return "markov";

        if (HasStrongBias(history))
            return "probability";

        return "random";
    }

    private bool IsCycle(IReadOnlyList<JankenHistory> history, JankenHand[] cycle)
    {
        for (int i = 0; i < history.Count; i++)
        {
            if (history[i].PlayerHand != cycle[i % cycle.Length])
                return false;
        }
        return true;
    }

    private bool IsAlternating(IReadOnlyList<JankenHistory> history)
    {
        if (history.Count < 4) return false;
        var a = history[history.Count - 1].PlayerHand;
        var b = history[history.Count - 2].PlayerHand;

        for (int i = history.Count - 3; i >= 0; i--)
        {
            var current = history[i].PlayerHand;
            if ((i % 2 == 0 && current != a) || (i % 2 == 1 && current != b))
                return false;
        }

        return true;
    }

    private bool HasStrongBias(IReadOnlyList<JankenHistory> history)
    {
        int[] counts = new int[3];
        foreach (var h in history)
        {
            counts[(int)h.PlayerHand]++;
        }

        int max = Mathf.Max(counts[0], counts[1], counts[2]);
        return max > history.Count * 0.6f;
    }
}
