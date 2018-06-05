using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;

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

    public string AssetPath { get; set; }

    private string _eventName = "SOEvent";
    private string _categoryName = "Game";
    private List<string> _parameterInfos;

    [System.Serializable]
    public class ParameterType
    {
        public string parameterName = "param";
        public string parameterTypeName;
        public int parameterTypeIndex;
    }
    
    public ParameterType[] parameterInfos;

    private static string soEventTemplateFileGUID;

    [MenuItem("Assets/Create/SO/Event", priority = 0)]
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

        string newEventName = EditorGUILayout.TextField("Event name", _eventName);
        if (newEventName != _eventName)
        {
            _eventName = newEventName;
            hasPropertyChanged = true;
        }

        GUI.tooltip = "The category will defined where the event will be in the asset menu";
        string newCategoryName = EditorGUILayout.TextField("Category", _categoryName);
        if (newCategoryName != _categoryName)
        {
            _categoryName = newCategoryName;
            hasPropertyChanged = true;
        }
        GUI.tooltip = string.Empty;
        
        SerializedObject eventWizardSO = new SerializedObject(this);
        SerializedProperty parameterInfosSO = eventWizardSO.FindProperty("parameterInfos");
        eventWizardSO.Update();

        EditorGUILayout.LabelField("Parameters");
        ++EditorGUI.indentLevel;
        {
            parameterInfosSO.arraySize = EditorGUILayout.IntField("Number", parameterInfosSO.arraySize);
            for (int parameterIndex = 0; parameterIndex < parameterInfosSO.arraySize; ++parameterIndex)
            {
                SerializedProperty parameterInfosO = parameterInfosSO.GetArrayElementAtIndex(parameterIndex);
                hasPropertyChanged |= DrawParameterTypeLayout(parameterInfosO, parameterIndex);
            }
        }
        
        --EditorGUI.indentLevel;

        hasPropertyChanged |= eventWizardSO.ApplyModifiedProperties();
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
                string newName = EditorGUILayout.TextField(parameterNameSO.stringValue);
                if (newName != parameterNameSO.stringValue)
                {
                    parameterNameSO.stringValue = newName;
                    hasPropertyChanged = true;
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
                int newParameterTypeIndexValue = EditorGUILayout.Popup(parameterTypeIndexSO.intValue, PresetTypes);
                if (newParameterTypeIndexValue != parameterTypeIndexSO.intValue)
                {
                    parameterTypeIndexSO.intValue = newParameterTypeIndexValue;
                    hasPropertyChanged = true;

                    parameterTypeNameSO.stringValue = PresetTypes[newParameterTypeIndexValue];
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

        for (int i = 0; i < parameterInfos.Length; ++i)
        {
            ueParams += parameterInfos[i].parameterTypeName;
            if (i + 1 < parameterInfos.Length)
            {
                ueParams += ", ";
            }
        }

        return (ueParams);
    }

    private string BuildFunctionParamsDeclaration()
    {
        string functionParams = string.Empty;

        for (int i = 0; i < parameterInfos.Length; ++i)
        {
            functionParams += parameterInfos[i].parameterTypeName + " " + parameterInfos[i].parameterName;
            if (i + 1 < parameterInfos.Length)
            {
                functionParams += ", ";
            }
        }

        return (functionParams);
    }

    private string BuildAllParamNames()
    {
        string allParamNames = string.Empty;

        for (int i = 0; i < parameterInfos.Length; ++i)
        {
            allParamNames += parameterInfos[i].parameterName;
            if (i + 1 < parameterInfos.Length)
            {
                allParamNames += ", ";
            }
        }

        return (allParamNames);
    }

}
