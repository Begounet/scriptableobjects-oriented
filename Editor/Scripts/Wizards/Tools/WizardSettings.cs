using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class WizardSettings
{
    private const string soEventWizard_SettingsGUID_EditorPrefsKey = "SOEventWizard_SettingsGUID";

    private SOSettings _settings;

    public SOSettings Settings
    {
        get
        {
            return (_settings);
        }
    }
    
    public WizardSettings()
    {
        string settingsGUID = EditorPrefs.GetString(soEventWizard_SettingsGUID_EditorPrefsKey);
        _settings = (SOSettings)AssetDatabase.LoadMainAssetAtPath(AssetDatabase.GUIDToAssetPath(settingsGUID));
    }

    public bool DrawSettings()
    {
        UnityEngine.Object newSettingsObj = EditorGUILayout.ObjectField("Settings", _settings, typeof(SOSettings), false);
        if (newSettingsObj != _settings)
        {
            _settings = newSettingsObj as SOSettings;
            EditorPrefs.SetString(soEventWizard_SettingsGUID_EditorPrefsKey, AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(_settings)));
            return (true);
        }
        return (false);
    }
}
