using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Synchronize a UnityEngine.UI.Toggle with a SOWBool
/// </summary>
public class UIToggleSyncSOWBool : MonoBehaviour
{
    [SOVariableMode(SOVariableActionMode.ReadWrite)]
    public SOWBool soBoolean;
    public Toggle toggle;

	void Start ()
    {
        SyncSOWBoolToToggle();
        soBoolean.onValueChanged += OnBoolValueChanged;
        toggle.onValueChanged.AddListener(OnToggleValueChanged);
    }

    private void OnToggleValueChanged(bool newValue)
    {
        SyncToggleToSOWBool();
    }

    private void OnBoolValueChanged(bool newValue)
    {
        SyncSOWBoolToToggle();
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (AreDataValid())
        {
            // Watch the evolution of the asset so we can sync in editor!

            // Assure to subscribe only once
            soBoolean.onValueChanged -= OnBoolValueChanged;
            soBoolean.onValueChanged += OnBoolValueChanged;
        }

        SyncSOWBoolToToggle();
    }
#endif

    void SyncSOWBoolToToggle()
    {
        if (AreDataValid() && !IsSynchronized())
        {
            toggle.isOn = soBoolean.Value;
        }
    }

    void SyncToggleToSOWBool()
    {
        if (AreDataValid() && !IsSynchronized())
        {
            soBoolean.Value = toggle.isOn;
        }
    }

    bool IsSynchronized()
    {
        return soBoolean.Value == toggle.isOn;
    }

    bool AreDataValid()
    {
        return soBoolean != null && toggle != null;
    }
}
