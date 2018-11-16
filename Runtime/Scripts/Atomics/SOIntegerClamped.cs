using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SOInteger", menuName = "SO/Atomics/Integer (Clamped)")]
public class SOIntegerClamped : SONumericClamped<int>
{
    protected override int ClampByMinimum(int Min)
    {
        return Mathf.Min(Value, Min);
    }

    protected override int ClampByMaximum(int Max)
    {
        return Mathf.Max(Value, Max);
    }

    protected override int Clamp(int Min, int Max)
    {
        return Mathf.Clamp(Value, Min, Max);
    }

    public override void Reset()
    {
        base.Reset();
        Value = 0;
    }
}

