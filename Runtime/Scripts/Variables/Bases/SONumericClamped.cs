using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SONumericClamped<ValueType> : ISONumeric
{
    public enum ClampMode
    {
        Unclamped,
        Minimum,
        Maximum,
        MinimumAndMaximum
    }

    public ClampMode clampMode;

    [HideInInspector]
    public ValueType min;

    [HideInInspector]
    public ValueType max;

    [SerializeField]
    [HideInInspector]
    private ValueType _value;

    public ValueType Value
    {
        get { return _value; }
        set
        {
            _value = value;
            switch (clampMode)
            {
                case ClampMode.Minimum:
                    _value = ClampByMaximum(min);
                    break;

                case ClampMode.Maximum:
                    _value = ClampByMinimum(max);
                    break;

                case ClampMode.MinimumAndMaximum:
                    _value = Clamp(min, max);
                    break;

                default:
                    break;
            }
        }
    }

    protected abstract ValueType ClampByMinimum(ValueType Min);
    protected abstract ValueType ClampByMaximum(ValueType Max);
    protected abstract ValueType Clamp(ValueType Min, ValueType Max);

    public override object GetValue()
    {
        return (_value);
    }

    public override void SetValue(object newValue)
    {
        Value = (ValueType) newValue;
    }

    public override object GetObject()
    {
        return Value;
    }
}
