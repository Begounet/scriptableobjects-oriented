using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SOFloat", menuName = "SO/Atomics/Float")]
public class SOFloat : SONumeric<float>, ISOFloat
{
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
