using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SONumeric<ValueType> : ISONumeric
{
    public ValueType Value;

    public override object GetValue()
    {
        return (Value);
    }

    public override void SetValue(object newValue)
    {
        Value = (ValueType) newValue;
    }
}
