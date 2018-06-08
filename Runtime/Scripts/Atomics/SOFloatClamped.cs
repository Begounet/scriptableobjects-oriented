using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SOFloatClamped : SOBaseVariable
{
    public enum ClampMode
    {
        Unclamped,
        Minimum,
        Maximum,
        MinimumAndMaximum
    }

    public float min;
    public float max;

    public ClampMode clampMode;

    [SerializeField]
    private float _value;

    public float Value
    {
        get { return _value; }
        set
        {
            switch (clampMode)
            {
                case ClampMode.Unclamped:
                    _value = value;
                    break;

                case ClampMode.Minimum:
                    _value = Mathf.Max(value, min);
                    break;

                case ClampMode.Maximum:
                    _value = Mathf.Min(value, max);
                    break;

                case ClampMode.MinimumAndMaximum:
                    _value = Mathf.Clamp(value, min, max);
                    break;

                default:
                    _value = value;
                    break;
            }
        }
    }
}
