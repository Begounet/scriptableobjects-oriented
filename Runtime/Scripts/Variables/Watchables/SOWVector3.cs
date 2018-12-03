using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "SOWVector3", menuName = "SO/Atomics/Vector3/Watchable", order = 1)]
public class SOWVector3 : SOVector3, ISOWatchable
{
    public event Action<ISOWatchable> onSOChanged;
    public Action<Vector3> onValueChanged;

    public override Vector3 Value
    {
        get { return base.Value; }
        set
        {
            base.Value = value;

#if UNITY_EDITOR
            _cachedValue = value;
#endif

            SO.EventHelper.Raise(onValueChanged, value);
            SO.EventHelper.Raise(onSOChanged, this);
        }
    }

#if UNITY_EDITOR

    private Vector3 _cachedValue;

    protected virtual void OnValidate()
    {
        if (_cachedValue != _value)
        {
            ForceUpdateValue();
        }
    }

    private void ForceUpdateValue()
    {
        Vector3 newValue = _value;
        _value = _cachedValue;
        Value = newValue;
    }

#endif

}
