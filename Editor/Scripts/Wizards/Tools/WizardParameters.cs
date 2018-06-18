using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[System.Serializable]
public class WizardParameters
{
    private static readonly string[] PresetTypes =
    {
        "Custom",
        "int",
        "float",
        "bool",
        "string",
        "Vector",
        "Vector2",
        "Vector3",
        "Quaternion",
        "Transform",
        "GameObject",
    };

    [System.Serializable]
    public class ParameterType
    {
        public string parameterName = "param";
        public string parameterTypeName;
        public int parameterTypeIndex;
    }

    public ParameterType[] parameterInfos;
    public int numMaxParameters;
    public string prefixName;
    public bool canEditParameterName;

    private WizardSettings _associatedWizardSettings;

    public WizardParameters()
    {
        numMaxParameters = int.MaxValue;
        prefixName = "Parameters";
        canEditParameterName = true;
    }

    public void AssociateWizardSettings(WizardSettings wizardSettings)
    {
        _associatedWizardSettings = wizardSettings;
    }
    
    public bool DrawParametersEditor(SerializedProperty wizardParameterProperty)
    {
        bool hasPropertyChanged = false;

        SerializedProperty parameterInfosSO = wizardParameterProperty.FindPropertyRelative("parameterInfos");
        
        if (numMaxParameters == 1)
        {
            DrawInlineParameterEdition(parameterInfosSO);
        }
        else
        {
            DrawMultipleParametersEdition(parameterInfosSO);
        }

        return (hasPropertyChanged);
    }

    bool DrawInlineParameterEdition(SerializedProperty parameterInfosSO)
    {
        bool hasPropertyChanged = false;

        using (new EditorGUILayout.HorizontalScope())
        {
            EditorGUILayout.PrefixLabel(prefixName);
            parameterInfosSO.arraySize = 1;

            SerializedProperty parameterInfoSO = parameterInfosSO.GetArrayElementAtIndex(0);
            hasPropertyChanged |= DrawParameterTypeLayout(parameterInfoSO, 0, false);
        }

        return (hasPropertyChanged);
    }

    bool DrawMultipleParametersEdition(SerializedProperty parameterInfosSO)
    {
        bool hasPropertyChanged = false;

        EditorGUILayout.PrefixLabel(prefixName);
        ++EditorGUI.indentLevel;
        {
            parameterInfosSO.arraySize = EditorGUILayout.DelayedIntField("Number", parameterInfosSO.arraySize);
            parameterInfosSO.arraySize = Mathf.Clamp(parameterInfosSO.arraySize, 0, numMaxParameters);
            for (int parameterIndex = 0; parameterIndex < parameterInfosSO.arraySize; ++parameterIndex)
            {
                SerializedProperty parameterInfoSO = parameterInfosSO.GetArrayElementAtIndex(parameterIndex);
                hasPropertyChanged |= DrawParameterTypeLayout(parameterInfoSO, parameterIndex, true);
            }
        }
        --EditorGUI.indentLevel;

        return (hasPropertyChanged);
    }

    bool DrawParameterTypeLayout(SerializedProperty parameterInfosO, int parameterIndex = -1, bool shouldDisplayPrefix = true)
    {
        bool hasPropertyChanged = false;

        EditorGUILayout.BeginHorizontal();
        {
            if (parameterIndex > -1)
            {
                if (shouldDisplayPrefix)
                {
                    EditorGUILayout.PrefixLabel("Param " + parameterIndex);
                }

                SerializedProperty parameterNameSO = parameterInfosO.FindPropertyRelative("parameterName");
                SerializedProperty parameterTypeNameSO = parameterInfosO.FindPropertyRelative("parameterTypeName");
                SerializedProperty parameterTypeIndexSO = parameterInfosO.FindPropertyRelative("parameterTypeIndex");

                if (canEditParameterName)
                {
                    GUI.tooltip = "Name of the parameter";
                    string newName = EditorGUILayout.TextField(parameterNameSO.stringValue);
                    if (newName != parameterNameSO.stringValue)
                    {
                        parameterNameSO.stringValue = newName;
                        hasPropertyChanged = true;
                    }
                }

                GUI.tooltip = "Type of the parameter";
                string newTypeName = EditorGUILayout.TextField(parameterTypeNameSO.stringValue);
                if (newTypeName != parameterTypeNameSO.stringValue)
                {
                    parameterTypeNameSO.stringValue = newTypeName;
                    hasPropertyChanged = true;

                    // Reset the parameterTypeIndex to set it on "Custom"
                    parameterTypeIndexSO.intValue = 0;
                }

                GUI.tooltip = "Preset of parameter type";
                string[] allPresetTypes = BuildAllPresetTypes();
                int newParameterTypeIndexValue = EditorGUILayout.Popup(parameterTypeIndexSO.intValue, allPresetTypes);
                if (newParameterTypeIndexValue != parameterTypeIndexSO.intValue)
                {
                    parameterTypeIndexSO.intValue = newParameterTypeIndexValue;
                    hasPropertyChanged = true;

                    parameterTypeNameSO.stringValue = allPresetTypes[newParameterTypeIndexValue];
                }

                GUI.tooltip = string.Empty;
            }
        }
        EditorGUILayout.EndHorizontal();

        return (hasPropertyChanged);
    }

    private string[] BuildAllPresetTypes()
    {
        SOSettings settings = (_associatedWizardSettings != null) ? _associatedWizardSettings.Settings : null;
        if (settings == null || settings.additionalCommonTypes.Length == 0)
        {
            return (WizardParameters.PresetTypes);
        }

        string[] newPresetTypes = new string[WizardParameters.PresetTypes.Length + settings.additionalCommonTypes.Length];
        for (int i = 0; i < WizardParameters.PresetTypes.Length; ++i)
        {
            newPresetTypes[i] = WizardParameters.PresetTypes[i];
        }
        for (int i = 0; i < settings.additionalCommonTypes.Length; ++i)
        {
            newPresetTypes[i + WizardParameters.PresetTypes.Length] = settings.additionalCommonTypes[i];
        }

        return (newPresetTypes);
    }

    public string CheckValidity()
    {
        try
        {
            if (parameterInfos != null && parameterInfos.Length > 0)
            {
                for (int i = 0; i < parameterInfos.Length; ++i)
                {
                    CheckParameter(i, parameterInfos[i]);
                }
            }
            return (null);
        }
        catch (System.Exception ex)
        {
            return (ex.Message);
        }
    }

    private void CheckParameter(int paramIndex, WizardParameters.ParameterType parameterInfo)
    {
        if (canEditParameterName && string.IsNullOrEmpty(parameterInfo.parameterName))
        {
            throw new UnityException(string.Format("Parameter {0} - name is empty", paramIndex));
        }

        if (string.IsNullOrEmpty(parameterInfo.parameterTypeName))
        {
            throw new UnityException(string.Format("Parameter {0} - type name is empty", paramIndex));
        }
    }
}
