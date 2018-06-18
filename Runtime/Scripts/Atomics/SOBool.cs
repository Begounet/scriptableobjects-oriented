using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SOBool", menuName = "SO/Atomics/Bool")]
public class SOBool : SOBaseVariable
{
    public bool value;

    public override void Reset()
    {
        base.Reset();
        value = false;
    }
}
