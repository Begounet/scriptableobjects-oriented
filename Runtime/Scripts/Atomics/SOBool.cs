using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SOBool", menuName = "SO/Atomics/Bool")]
public class SOBool : SOBaseVariable, SOIBool
{
    public bool value;    

    public void SetValue(bool newValue)
    {
        value = newValue;
    }

    public bool GetValue()
    {
        return value;
    }

    public override void Reset()
    {
        base.Reset();
        value = false;
    }
}
