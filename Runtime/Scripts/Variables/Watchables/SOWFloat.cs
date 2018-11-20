﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SOWFloat", menuName = "SO/Atomics/Float/Watchable", order = 1)]
public class SOWFloat : SOFloat, ISOWatchable
{
    public event Action<ISOWatchable> onSOChanged;
    public Action<float> onValueChanged;

    [SerializeField]
    private float _value;
    public override float Value
    {
        get { return _value; }

        set
        {
            _value = value;
            EventHelper.Raise(onValueChanged, _value);
            EventHelper.Raise(onSOChanged, this);
        }
    }

#if UNITY_EDITOR
    protected virtual void OnValidate()
    {
        Value = _value;
    }
#endif
}