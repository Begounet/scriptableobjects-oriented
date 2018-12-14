using System;
using UnityEngine;

[CreateAssetMenu(fileName = "SOFloat", menuName = "SO/Atomics/Float (Clamped)/Watchable", order = 1)]
public class SOWFloatClamped : SOFloatClamped, ISOWatchable
{
    public event Action<ISOWatchable> onSOChanged;

    public Action<float> onValueChanged;

    public override float Value
    {
        get { return _value; }
        set
        {
            var previousValue = _value;
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

            if (previousValue != _value)
            {
#if UNITY_EDITOR
                _cachedValue = _value;
#endif

                SO.EventHelper.Raise(onValueChanged, _value);
                SO.EventHelper.Raise(onSOChanged, this);
            }
        }
    }

#if UNITY_EDITOR

    private float _cachedValue;

    public SOWFloatClamped()
    {
        _cachedValue = _value;
    }

    protected virtual void OnValidate()
    {
        if (_cachedValue != _value)
        {
            float newValue = _value; // Save new value
            _value = _cachedValue; // Restore old value
            Value = newValue; // Update the value in the way we want
        }
    }

#endif
}