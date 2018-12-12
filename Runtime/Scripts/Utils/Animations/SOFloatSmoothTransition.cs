using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Update CurrentValue smoothly until it reach TargetValue
/// </summary>
[ExecuteInEditMode]
public class SOFloatSmoothTransition : MonoBehaviour
{
    [SerializeField]
    [SOBindingType(SO.BindingType.Required)]
    [SOVariableMode(SOVariableActionMode.ReadWrite)]
    private SOFloat _currentValue;
    public SOFloat CurrentValue
    {
        get { return _currentValue; }
        set { _currentValue = value; }
    }

    [SerializeField]
    [SOBindingType(SO.BindingType.Required)]
    [SOVariableMode(SOVariableActionMode.ReadOnly)]
    private SOFloat _targetValue;
    public SOFloat TargetValue
    {
        get { return _targetValue; }
        set { _targetValue = value; }
    }

    [SerializeField]
    private bool _shouldSetCurrentToTargetValueAtStart;

    public float smoothTime = 1;
    public bool shouldExecuteInEditMode = true;

    private float _currentVelocity;

    private void Awake()
    {
        _currentVelocity = 0;
    }

    private void Start()
    {
        if (_shouldSetCurrentToTargetValueAtStart && _currentValue != null && _targetValue != null)
        {
            _currentValue.Value = _targetValue.Value;
        }
    }

    private void Update()
    {
        bool shouldExecute = true;

#if UNITY_EDITOR
        shouldExecute = (Application.isPlaying || shouldExecuteInEditMode);
#endif

        if (_currentValue == null || _targetValue == null || !shouldExecute)
        {
            return;
        }

        _currentValue.Value = Mathf.SmoothDamp(_currentValue.Value, _targetValue.Value, ref _currentVelocity, smoothTime);
    }
}
