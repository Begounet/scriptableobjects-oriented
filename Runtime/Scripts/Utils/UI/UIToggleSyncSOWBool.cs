using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Synchronize a UnityEngine.UI.Toggle with a SOWBool
/// </summary>
public class UIToggleSyncSOWBool : UIToggleSynchronizer
{
    [SOVariableMode(SOVariableActionMode.ReadWrite)]
    public SOWBool soBoolean;
    
    public override ISOWatchable GetWatchableObject()
    {
        return soBoolean;
    }

    protected override void SyncToggleToSOValue()
    {
        soBoolean.Value = toggle.isOn;
    }

    protected override void SyncSOValueToToggle()
    {
        toggle.isOn = soBoolean.Value;
    }

    protected override bool IsSynchronized()
    {
        return soBoolean.Value == toggle.isOn;
    }
}
