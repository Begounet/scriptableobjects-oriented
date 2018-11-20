using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SOQuaternion", menuName = "SO/Atomics/Quaternion")]
public class SOQuaternion : SOBaseVariable
{
    public Quaternion value;

    public override void Reset()
    {
        base.Reset();
        value = Quaternion.identity;
    }

    public override object GetObject()
    {
        return value;
    }
}
