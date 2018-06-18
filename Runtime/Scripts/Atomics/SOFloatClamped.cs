using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SOFloat", menuName = "SO/Atomics/Float (Clamped)")]
public class SOFloatClamped : SOBaseVariable
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
    public float min;

    [HideInInspector]
    public float max;

    [SerializeField]
    [HideInInspector]
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
