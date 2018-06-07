using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;
using System.Reflection;
using System;

public class SOEventWizard : ScriptableWizard
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

    private const string soEventTemplateFilename = "SOEventTemplate";
    private const string soEventWizard_SettingsGUID_EditorPrefsKey = "SOEventWizard_SettingsGUID";

    public string AssetPath { get; set; }

    private SOSettings _settings;
    private string _eventName = "SOEventName";
    private string _categoryName = "Game";

    [System.Serializable]
    public class ParameterType
    {
        public string parameterName = "param";
        public string parameterTypeName;
        public int parameterTypeIndex;
    }
    
    public ParameterType[] parameterInfos;

    private static string soEventTemplateFileGUID;

    [MenuItem("Assets/Create/SO/Create/Event", priority = 0)]
    static void CreateWizard()
    {
        SOEventWizard wizard = ScriptableWizard.DisplayWizard<SOEventWizard>("ScriptableObject Event Creation Wizard", "Create");
        wizard.AssetPath = GetSelectedPath();
    }

    public void OnEnable()
    {
        if (string.IsNullOrEmpty(soEventTemplateFileGUID))
        {
            string[] assets = AssetDatabase.FindAssets(soEventTemplateFilename);
            if (assets.Length > 0)
            {
                soEventTemplateFileGUID = assets[0];
            }
            else
            {
                Debug.LogErrorFormat("'{0}' cannot be found! Add it in the project and restart the wizard.", soEventTemplateFilename);
            }
        }

        if (_settings == null)
        {
            string settingsGUID = EditorPrefs.GetString(soEventWizard_SettingsGUID_EditorPrefsKey);
            _settings = (SOSettings) AssetDatabase.LoadMainAssetAtPath(AssetDatabase.GUIDToAssetPath(settingsGUID));
        }
    }

    protected override bool DrawWizardGUI()
    {
        if (string.IsNullOrEmpty(soEventTemplateFilename))
        {
            EditorGUILayout.LabelField("Error! SOEventTemplate not found! Add to the project and restart the wizard!");
            return (false);
        }

        bool hasPropertyChanged = false;

        EditorGUILayout.LabelField("Asset Path", AssetPath);

        hasPropertyChanged |= DrawSettingsField();
        hasPropertyChanged |= DrawEventName();
        hasPropertyChanged |= DrawCategory();

        SerializedObject eventWizardSO = new SerializedObject(this);
        SerializedProperty parameterInfosSO = eventWizardSO.FindProperty("parameterInfos");
        eventWizardSO.Update();

        EditorGUILayout.LabelField("Parameters");
        ++EditorGUI.indentLevel;
        {
            parameterInfosSO.arraySize = EditorGUILayout.DelayedIntField("Number", parameterInfosSO.arraySize);
            for (int parameterIndex = 0; parameterIndex < parameterInfosSO.arraySize; ++parameterIndex)
            {
                SerializedProperty parameterInfosO = parameterInfosSO.GetArrayElementAtIndex(parameterIndex);
                hasPropertyChanged |= DrawParameterTypeLayout(parameterInfosO, parameterIndex);
            }
        }        
        --EditorGUI.indentLevel;

        hasPropertyChanged |= eventWizardSO.ApplyModifiedProperties();

        isValid = CheckValidity();
        if (isValid)
        {
            errorString = string.Empty;
        }

        return (hasPropertyChanged);
    }

    bool DrawSettingsField()
    {
        UnityEngine.Object newSettingsObj = EditorGUILayout.ObjectField("Settings", _settings, typeof(SOSettings), false);
        if (newSettingsObj != null)
        {
            _settings = newSettingsObj as SOSettings;
            EditorPrefs.SetString(soEventWizard_SettingsGUID_EditorPrefsKey, AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(_settings)));
            return (true);
        }
        return (false);
    }

    bool DrawEventName()
    {
        string newEventName = EditorGUILayout.DelayedTextField("Event name", _eventName);
        if (newEventName != _eventName)
        {
            _eventName = newEventName;
            return (true);
        }
        return (false);
    }

    bool DrawCategory()
    {
        bool hasPropertyChanged = false;

        GUI.tooltip = "The category will defined where the event will be in the asset menu";
        string newCategoryName = EditorGUILayout.DelayedTextField("Category", _categoryName);
        if (newCategoryName != _categoryName)
        {
            _categoryName = newCategoryName;
            hasPropertyChanged = true;
        }
        GUI.tooltip = string.Empty;

        return (hasPropertyChanged);
    }

    bool DrawParameterTypeLayout(SerializedProperty parameterInfosO, int parameterIndex = -1)
    {
        bool hasPropertyChanged = false;

        EditorGUILayout.BeginHorizontal();
        {
            if (parameterIndex > -1)
            {
                EditorGUILayout.LabelField("Param " + parameterIndex);

                SerializedProperty parameterNameSO = parameterInfosO.FindPropertyRelative("parameterName");
                SerializedProperty parameterTypeNameSO = parameterInfosO.FindPropertyRelative("parameterTypeName");
                SerializedProperty parameterTypeIndexSO = parameterInfosO.FindPropertyRelative("parameterTypeIndex");

                GUI.tooltip = "Name of the parameter";
                string newName = EditorGUILayout.DelayedTextField(parameterNameSO.stringValue);
                if (newName != parameterNameSO.stringValue)
                {
                    parameterNameSO.stringValue = newName;
                    hasPropertyChanged = true;
                }

                GUI.tooltip = "Type of the parameter";
                string newTypeName = EditorGUILayout.DelayedTextField(parameterTypeNameSO.stringValue);
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
    
    void OnWizardCreate()
    {
        CreateAsset();
    }

    void CreateAsset()
    {
        string soEventTemplateFilePath = AssetDatabase.GUIDToAssetPath(soEventTemplateFileGUID);
        string soEventTemplateContent = File.ReadAllText(soEventTemplateFilePath);

        soEventTemplateContent = soEventTemplateContent.Replace("{EventName}", _eventName);

        if (!string.IsNullOrEmpty(_categoryName))
        {
            soEventTemplateContent = soEventTemplateContent.Replace("{Category}", "/" + _categoryName);
        }

        soEventTemplateContent = soEventTemplateContent.Replace("{UnityEventParams}", BuildUnityEventParamsString());
        soEventTemplateContent = soEventTemplateContent.Replace("{FunctionParamsDeclaration}", BuildFunctionParamsDeclaration());
        soEventTemplateContent = soEventTemplateContent.Replace("{AllParamNames}", BuildAllParamNames());
        soEventTemplateContent = soEventTemplateContent.Replace("{AllEditorParamNames}", BuildAllEditorParamNames());
        soEventTemplateContent = soEventTemplateContent.Replace("{AllEditorParamsDeclarations}", BuildAllEditorParamDeclarations());

        string dstFilePath = Path.Combine(AssetPath, _eventName + ".cs");
        File.WriteAllText(dstFilePath, soEventTemplateContent, System.Text.Encoding.UTF8);

        AssetDatabase.Refresh();
    }

    private static string GetSelectedPath()
    {
        string path = "Assets";
        foreach (UnityEngine.Object obj in Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.Assets))
        {
            path = AssetDatabase.GetAssetPath(obj);
            if (File.Exists(path))
            {
                path = Path.GetDirectoryName(path);
                break;
            }
        }
        return (path);
    }

    private string BuildUnityEventParamsString()
    {
        string ueParams = string.Empty;

        if (parameterInfos != null && parameterInfos.Length > 0)
        {
            for (int i = 0; i < parameterInfos.Length; ++i)
            {
                ueParams += parameterInfos[i].parameterTypeName;
                if (i + 1 < parameterInfos.Length)
                {
                    ueParams += ", ";
                }
            }

            if (!string.IsNullOrEmpty(ueParams))
            {
                ueParams = "<" + ueParams + ">";
            }
        }

        return (ueParams);
    }

    private string BuildFunctionParamsDeclaration()
    {
        string functionParams = string.Empty;

        if (parameterInfos != null && parameterInfos.Length > 0)
        {
            for (int i = 0; i < parameterInfos.Length; ++i)
            {
                functionParams += parameterInfos[i].parameterTypeName + " " + parameterInfos[i].parameterName;
                if (i + 1 < parameterInfos.Length)
                {
                    functionParams += ", ";
                }
            }
        }

        return (functionParams);
    }

    private string BuildAllParamNames()
    {
        string allParamNames = string.Empty;

        if (parameterInfos != null && parameterInfos.Length > 0)
        {
            for (int i = 0; i < parameterInfos.Length; ++i)
            {
                allParamNames += parameterInfos[i].parameterName;
                if (i + 1 < parameterInfos.Length)
                {
                    allParamNames += ", ";
                }
            }
        }

        return (allParamNames);
    }

    private string BuildAllEditorParamNames()
    {
        string allParamNames = string.Empty;

        if (parameterInfos != null && parameterInfos.Length > 0)
        {
            for (int i = 0; i < parameterInfos.Length; ++i)
            {
                allParamNames += "_" + parameterInfos[i].parameterName;
                if (i + 1 < parameterInfos.Length)
                {
                    allParamNames += ", ";
                }
            }
        }

        return (allParamNames);
    }

    private string BuildAllEditorParamDeclarations()
    {
        string allParamNames = string.Empty;

        if (parameterInfos != null && parameterInfos.Length > 0)
        {
            for (int i = 0; i < parameterInfos.Length; ++i)
            {
                allParamNames += "[SerializeField]";
                allParamNames += System.Environment.NewLine + GetIndentationSpaces();
                allParamNames += string.Format("private {0} _{1};", parameterInfos[i].parameterTypeName, parameterInfos[i].parameterName);
                if (i + 1 < parameterInfos.Length)
                {
                    allParamNames += System.Environment.NewLine + GetIndentationSpaces();
                }
            }
        }

        return (allParamNames);
    }

    private string GetIndentationSpaces()
    {
        return "    ";
    }

    private bool CheckValidity()
    {
        return 
            CheckEventName() &&
            CheckParameters();
    }

    private bool CheckEventName()
    {
        if (string.IsNullOrEmpty(_eventName))
        {
            errorString = "Event name is null";
            return (false);
        }

        return (true);
    }

    private bool CheckParameters()
    {
        if (parameterInfos != null && parameterInfos.Length > 0)
        {
            for (int i = 0; i < parameterInfos.Length; ++i)
            {
                if (!CheckParameter(i, parameterInfos[i]))
                {
                    return (false);
                }
            }
        }
        return (true);
    }

    private bool CheckParameter(int paramIndex, ParameterType parameterInfo)
    {
        if (string.IsNullOrEmpty(parameterInfo.parameterName))
        {
            errorString = string.Format("Parameter {0} - name is empty", paramIndex);
            return (false);
        }

        if (string.IsNullOrEmpty(parameterInfo.parameterTypeName))
        {
            errorString = string.Format("Parameter {0} - type name is empty", paramIndex);
            return (false);
        }

        return (true);
    }

    private string[] BuildAllPresetTypes()
    {
        if (_settings == null || _settings.additionalCommonTypes.Length == 0)
        {
            return (PresetTypes);
        }

        string[] newPresetTypes = new string[PresetTypes.Length + _settings.additionalCommonTypes.Length];
        for (int i = 0; i < PresetTypes.Length; ++i)
        {
            newPresetTypes[i] = PresetTypes[i];
        }
        for (int i = 0; i < _settings.additionalCommonTypes.Length; ++i)
        {
            newPresetTypes[i + PresetTypes.Length] = _settings.additionalCommonTypes[i];
        }

        return (newPresetTypes);
    }

}
