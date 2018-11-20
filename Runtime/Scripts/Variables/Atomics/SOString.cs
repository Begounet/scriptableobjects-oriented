using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SOString", menuName = "SO/Atomics/String")]
public class SOString : SOBaseVariable
{
    public string value;

    public override void Reset()
    {
        base.Reset();
        value = null;
    }

    public override object GetObject()
    {
        return value;
    }
}
