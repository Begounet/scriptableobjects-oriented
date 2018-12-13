using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SOInteger", menuName = "SO/Atomics/Integer")]
public class SOInteger : SONumeric<int>
{
    [SerializeField]
    protected int _value;
    public override int Value
    {
        get { return _value; }
        set { _value = value; }
    }

    public override void Reset()
    {
        base.Reset();
        Value = 0;
    }
}
