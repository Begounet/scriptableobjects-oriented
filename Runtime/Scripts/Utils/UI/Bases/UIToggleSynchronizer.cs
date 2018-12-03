using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Synchronize a UnityEngine.UI.Toggle with a ISOWatchable variable
/// </summary>
public abstract class UIToggleSynchronizer : MonoBehaviour
{
    public Toggle toggle;

    public abstract ISOWatchable GetWatchableObject();
    protected abstract void SyncToggleToSOValue();
    protected abstract void SyncSOValueToToggle();
    protected abstract bool IsSynchronized();


#if UNITY_EDITOR

    protected virtual void OnValidate()
    {
        if (GetWatchableObject() != null)
        {
            // Watch the evolution of the asset so we can sync in editor!

            // Assure to subscribe only once
            ISOWatchable watchableValue = GetWatchableObject();
            watchableValue.onSOChanged -= OnValueChanged;
            watchableValue.onSOChanged += OnValueChanged;
        }

        SafeSyncSOValueToToggle();
    }

#endif

    void Start()
    {
        ISOWatchable watchableValue = GetWatchableObject();
        if (watchableValue != null)
        {
            watchableValue.onSOChanged += OnValueChanged;
        }
        toggle.onValueChanged.AddListener(OnToggleValueChanged);

        SafeSyncSOValueToToggle();
    }

    private void OnValueChanged(ISOWatchable watchableObject)
    {
        SafeSyncSOValueToToggle();
    }

    private void OnToggleValueChanged(bool newValue)
    {
        SafeSyncToggleToSOValue();
    }

    private void SafeSyncSOValueToToggle()
    {
        if (AreDataValid() && !IsSynchronized())
        {
            SyncSOValueToToggle();
        }
    }

    private void SafeSyncToggleToSOValue()
    {
        if (AreDataValid() && !IsSynchronized())
        {
            SyncToggleToSOValue();
        }
    }

    protected virtual bool AreDataValid()
    {
        return GetWatchableObject() != null && toggle != null;
    }
}
