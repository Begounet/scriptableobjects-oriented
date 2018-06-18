using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SOVector3", menuName = "SO/Atomics/Vector3")]
public class SOVector3 : SOBaseVariable
{
    public Vector3 value;

    public override void Reset()
    {
        base.Reset();
        value = Vector3.zero;
    }
}
