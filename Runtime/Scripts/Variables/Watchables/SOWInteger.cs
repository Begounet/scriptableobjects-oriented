using System;
using UnityEngine;

[CreateAssetMenu(fileName = "SOWInteger", menuName = "SO/Atomics/Integer/Watchable", order = 1)]
public class SOWInteger : SOInteger, ISOWatchable
{
    public event Action<ISOWatchable> onSOChanged;

    public Action<int> onValueChanged;

    public override int Value
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

                SO.EventHelper.Raise(onValueChanged, _value);
                SO.EventHelper.Raise(onSOChanged, this);
            }
        }
    }

#if UNITY_EDITOR

    private int _cachedValue;

    public SOWInteger()
    {
        _cachedValue = _value;
    }

    protected virtual void OnValidate()
    {
        if (_cachedValue != _value)
        {
            int newValue = _value; // Save new value
            _value = _cachedValue; // Restore old value
            Value = newValue; // Update the value in the way we want
        }
    }

#endif
}