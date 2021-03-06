﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SOFloat", menuName = "SO/Atomics/Float (Clamped)/Simple")]
public class SOFloatClamped : SONumericClamped<float>, ISOFloat
{
    protected override float ClampByMinimum(float Min)
    {
        return Mathf.Min(Value, Min);
    }
    
    protected override float ClampByMaximum(float Max)
    {
        return Mathf.Max(Value, Max);
    }
    
    protected override float Clamp(float Min, float Max)
    {
        return Mathf.Clamp(Value, Min, Max);
    }

    public override void Reset()
    {
        base.Reset();
        Value = 0.0f;
    }

    public float Get()
    {
        return Value;
    }

    public void Set(float newValue)
    {
        Value = newValue;
    }
}
