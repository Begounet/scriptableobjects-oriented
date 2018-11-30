using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SetMaterialFloatFromSOFloat : MonoBehaviour
{
    [SerializeField]
    [SOVariableMode(SOVariableActionMode.ReadOnly)]
    private SOFloat _floatValue;
    public SOFloat FloatValue
    {
        get { return _floatValue; }
    }

    [SerializeField]
    [Tooltip("If the float value can be watched, the value will be updated automatically")]
    private bool _shouldUpdateEachFrame;
    public bool ShouldUpdateEachFrame
    {
        get { return _shouldUpdateEachFrame; }
        set { _shouldUpdateEachFrame = value; }
    }

#if UNITY_EDITOR
    [SerializeField]
    private bool _shouldUpdateInEditor;
#endif

    public MeshRenderer meshRenderer;

    [SerializeField]
    private int _materialIndex;
    public int MaterialIndex
    {
        get { return _materialIndex; }
        set
        {
            _materialIndex = value;
            UpdateMaterialPropertyValue();
        }
    }

    [SerializeField]
    private string _propertyName;
    public string PropertyName
    {
        get { return _propertyName; }
        set
        {
            _propertyName = value;
            CacheParameterId();
            UpdateMaterialPropertyValue();
        }
    }

    [SerializeField]
    private bool _useAnimatedTransition;
    public bool UseAnimatedTransition
    {
        get { return _useAnimatedTransition; }
        set { _useAnimatedTransition = value; }
    }

    [SerializeField]
    [SO.ShowOnlyIfTrue("_useAnimatedTransition")]
    private float _transitionSmoothTime;
    public float TransitionSmoothTime
    {
        get { return _transitionSmoothTime; }
        set { _transitionSmoothTime = value; }
    }

    [SerializeField]
    [SOVariableMode(SOVariableActionMode.WriteOnly)]
    [Tooltip("Optional - If the transition is animated, the current value will be saved into it")]
    [SO.ShowOnlyIfTrue("_useAnimatedTransition")]
    private SOFloat _animatedFloatValue;
    public SOFloat AnimatedFloatValue
    {
        get { return _animatedFloatValue; }
        set { _animatedFloatValue = value; }
    }

    public bool IsTransitionAnimating { get; private set; }

    private int _propertyId;
    private float _currentVelocity;

    private void Awake()
    {
        CacheParameterId();
        StopAllCoroutines();
        IsTransitionAnimating = false;
    }

    private void Start()
    {
        TryWatchFloatChanges();
        UpdateMaterialPropertyValue();
    }

    private void TryWatchFloatChanges()
    {
        if (_floatValue != null && _floatValue is ISOWatchable)
        {
            ISOWatchable watchableFloatValue = _floatValue as ISOWatchable;
            watchableFloatValue.onSOChanged -= OnValueChanged;
            watchableFloatValue.onSOChanged += OnValueChanged;
        }
    }

    private void CacheParameterId()
    {
        _propertyId = Shader.PropertyToID(_propertyName);
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (!Application.isPlaying)
        {
            UnityEditor.EditorApplication.playModeStateChanged += OnPlayStateChanged;

            CacheParameterId();

            if (_shouldUpdateInEditor)
            {
                TryWatchFloatChanges();
                UpdateMaterialPropertyValue();
            }
        }
    }

    private void OnPlayStateChanged(UnityEditor.PlayModeStateChange stateChange)
    {
        if (_floatValue != null && _floatValue is ISOWatchable)
        {
            ISOWatchable watchableFloatValue = _floatValue as ISOWatchable;
            watchableFloatValue.onSOChanged -= OnValueChanged;
        }
    }
#endif

    private void OnValueChanged(ISOWatchable obj)
    {
        UpdateMaterialPropertyValue();
    }

    void Update ()
    {
        bool shouldUpdate = _shouldUpdateEachFrame;

#if UNITY_EDITOR
        shouldUpdate = (!Application.isPlaying && _shouldUpdateInEditor);
#endif

        if (shouldUpdate)
        {
            UpdateMaterialPropertyValue();
        }
	}

    void UpdateMaterialPropertyValue()
    {
        if (this == null || !CanUpdateValue())
        {
            return;
        }

        if (isActiveAndEnabled && _useAnimatedTransition && !IsTransitionAnimating)
        {
            StartCoroutine(Coroutine_UpdateMaterialPropertyValueAnimated());
        }
        else
        {
            SetMaterialPropertyValue(_floatValue.Value);
        }
    }

    bool CanUpdateValue()
    {
        return meshRenderer != null &&
            _materialIndex >= 0 &&
            _materialIndex < meshRenderer.sharedMaterials.Length;
    }

    IEnumerator Coroutine_UpdateMaterialPropertyValueAnimated()
    {
        IsTransitionAnimating = true;
        {
            Material material = meshRenderer.sharedMaterials[_materialIndex];
            float currentValue = material.GetFloat(_propertyId);
            _currentVelocity = 0.0f;

            while (!Mathf.Approximately(currentValue, _floatValue.Value))
            {
                currentValue = Mathf.SmoothDamp(currentValue, _floatValue.Value, ref _currentVelocity, _transitionSmoothTime);
                SetMaterialPropertyValue(currentValue);
                yield return null;
            }

            SetMaterialPropertyValue(_floatValue.Value);
        }
        IsTransitionAnimating = false;
    }

    void SetMaterialPropertyValue(float value)
    {
        Material material = meshRenderer.sharedMaterials[_materialIndex];

        if (material != null && material.HasProperty(_propertyId))
        {
            material.SetFloat(_propertyId, value);
        }

        if (_animatedFloatValue != null)
        {
            _animatedFloatValue.Value = value;
        }
    }
}
