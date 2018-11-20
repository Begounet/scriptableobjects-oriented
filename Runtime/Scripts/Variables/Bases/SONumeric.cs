using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SONumeric<ValueType> : ISONumeric
{
    public virtual ValueType Value { get; set; }

    public override object GetValue()
    {
        return (Value);
    }

    public override void SetValue(object newValue)
    {
        Value = (ValueType) newValue;
    }

    public static implicit operator ValueType(SONumeric<ValueType> v)
    {
        return (v.Value);
    }

    public override object GetObject()
    {
        return Value;
    }
}
