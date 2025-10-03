using UnityEngine;

// ロジックモデル
public class MeasuringTapeModel
{
    public float MaxLength { get; }
    public float CurrentLength { get; private set; }
    public bool IsLocked { get; private set; }

    public MeasuringTapeModel(float maxLength)
    {
        MaxLength = maxLength;
        CurrentLength = 1f;
        IsLocked = false;
    }

    public void Extend(float length)
    {
        if (IsLocked) return;
        CurrentLength = Mathf.Min(MaxLength, CurrentLength + length);
    }

    public void Retract(float length)
    {
        if (IsLocked) return;
        CurrentLength = Mathf.Max(1f, CurrentLength - length);
    }

    public void ToggleLock()
    {
        IsLocked = !IsLocked;
    }
}
