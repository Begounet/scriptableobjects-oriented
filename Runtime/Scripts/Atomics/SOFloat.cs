using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SOFloat", menuName = "SO/Atomics/Float")]
public class SOFloat : SOBaseVariable
{
    public float Value;

    public override void Reset()
    {
        base.Reset();
        Value = 0.0f;
    }
}
