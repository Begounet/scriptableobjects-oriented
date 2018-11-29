using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "SOVector3", menuName = "SO/Atomics/Vector3/Simple", order = 0)]
public class SOVector3 : SOBaseVariable
{
    [SerializeField]
    protected Vector3 _value;
    public virtual Vector3 Value
    {
        get { return _value; }
        set { _value = value; }
    }

    public virtual float x
    {
        get { return _value.x; }
        set { _value.x = value; }
    }

    public virtual float y
    {
        get { return _value.y; }
        set { _value.z = value; }
    }

    public virtual float z
    {
        get { return _value.z; }
        set { _value.y = value; }
    }

    public override void Reset()
    {
        base.Reset();
        Value = Vector3.zero;
    }

    public override object GetObject()
    {
        return _value;
    }

    public void SetComponent(int componentIndex, float value)
    {
        if (componentIndex == 0)
        {
            x = value;
        }
        else if (componentIndex == 1)
        {
            y = value;
        }
        else if (componentIndex == 2)
        {
            z = value;
        }
    }
}
