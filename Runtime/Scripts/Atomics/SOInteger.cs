using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SOInteger", menuName = "SO/Atomics/Integer")]
public class SOInteger : SONumeric<int>
{
    public override void Reset()
    {
        base.Reset();
        Value = 0;
    }
}
