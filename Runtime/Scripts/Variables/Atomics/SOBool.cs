using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SOBool", menuName = "SO/Atomics/Bool/Simple", order = 0)]
public class SOBool : SOBaseVariable
{
    public virtual bool Value { get; set; }

    public void SetValue(bool newValue)
    {
        Value = newValue;
    }

    public bool GetValue()
    {
        return Value;
    }

    public override void Reset()
    {
        base.Reset();
        Value = false;
    }

    public override object GetObject()
    {
        return Value;
    }
}
