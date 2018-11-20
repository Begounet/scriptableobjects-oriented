using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SOVector2", menuName = "SO/Atomics/Vector2")]
public class SOVector2 : SOBaseVariable
{
    public Vector2 value;

    public override void Reset()
    {
        base.Reset();
        value = Vector2.zero;
    }

    public override object GetObject()
    {
        return value;
    }
}
