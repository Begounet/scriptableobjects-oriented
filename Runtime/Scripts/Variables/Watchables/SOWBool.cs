using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SOWBool", menuName = "SO/Atomics/Bool/Watchable", order = 1)]
public class SOWBool : SOBool, ISOWatchable
{
    public event Action<ISOWatchable> onSOChanged;
    public Action<bool> onValueChanged;

    [SerializeField]
    private bool _value;
    public override bool Value
    {
        get { return _value; }
        set
        {
            if (_value != value)
            {
                _value = value;

#if UNITY_EDITOR
                _cachedValue = _value;
#endif

                EventHelper.Raise(onValueChanged, _value);
                EventHelper.Raise(onSOChanged, this);
            }
        }
    }

#if UNITY_EDITOR

    private bool _cachedValue;

    public SOWBool()
    {
        _cachedValue = _value;
    }

    protected virtual void OnValidate()
    {
        if (_cachedValue != _value)
        {
            bool newValue = _value; // Save new value
            _value = _cachedValue; // Restore old value
            Value = newValue; // Update the value in the way we want
        }
    }

#endif
}
