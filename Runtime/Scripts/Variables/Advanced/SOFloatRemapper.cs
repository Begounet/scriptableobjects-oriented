using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Describe a float remapper - allowing to remap a float from [inMinValue, inMaxValue] to [outMinValue, outMaxValue]
/// </summary>
[CreateAssetMenu(fileName = "Remapper", menuName = "SO/Advanced/Remappers/Float")]
public class SOFloatRemapper : SOBaseVariable, ISOWatchable
{
    public event Action<ISOWatchable> onSOChanged;


    [SerializeField]
    private float _inMinValue;
    public float InMinValue
    {
        get { return _inMinValue; }
        set { _inMinValue = value; RaiseChange(); }
    }

    [SerializeField]
    private float _inMaxValue;
    public float InMaxValue
    {
        get { return _inMaxValue; }
        set { _inMaxValue = value; RaiseChange(); }
    }

    [SerializeField]
    private float _outMinValue;
    public float OutMinValue
    {
        get { return _outMinValue; }
        set { _outMinValue = value; RaiseChange(); }
    }

    [SerializeField]
    private float _outMaxValue;
    public float OutMaxValue
    {
        get { return _outMaxValue; }
        set { _outMaxValue = value; RaiseChange(); }
    }


    public float Remap(float value)
    {
        return _outMinValue + ((value - _inMinValue) / (_inMaxValue - _inMinValue)) * (_outMaxValue - _outMinValue);
    }

    private void RaiseChange()
    {
        SO.EventHelper.Raise(onSOChanged, this);
    }
}
