using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SOFloat", menuName = "SO/Atomics/Float/Simple", order = 0)]
public class SOFloat : SONumeric<float>, ISOFloat
{
    [SerializeField]
    protected float _value;
    public override float Value
    {
        get { return _value; }
        set { _value = value; }
    }

    public override void Reset()
    {
        base.Reset();
        Set(0.0f);
    }

    public float Get()
    {
        return Value;
    }

    public void Set(float newValue)
    {
        SetValue(newValue);
    }
}
