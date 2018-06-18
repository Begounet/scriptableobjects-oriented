using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;
using System.Reflection;
using System;

public class SOEventWizard : ScriptableWizard
{
    private const string soEventTemplateFilename = "SOEventTemplate";

    public string AssetPath { get; set; }

    public WizardParameters wizardParameters;

    private WizardSettings _wizardSettings;
    private string _eventName = "SOEventName";
    private string _categoryName = "Game/Events";

    private static string soEventTemplateFileGUID;

    [MenuItem("Assets/Create/SO/Create/Event", priority = 0)]
    static void CreateWizard()
    {
        SOEventWizard wizard = ScriptableWizard.DisplayWizard<SOEventWizard>("ScriptableObject Event Creation Wizard", "Create");
        wizard.AssetPath = WizardTools.GetSelectedPath();
    }

    public void OnEnable()
    {
        _wizardSettings = new WizardSettings();
        wizardParameters = new WizardParameters();
        wizardParameters.AssociateWizardSettings(_wizardSettings);

        if (string.IsNullOrEmpty(soEventTemplateFileGUID))
        {
            soEventTemplateFileGUID = WizardTools.AutoFindFile(soEventTemplateFilename, true);
        }
    }

    protected override bool DrawWizardGUI()
    {
        if (string.IsNullOrEmpty(soEventTemplateFilename))
        {
            EditorGUILayout.LabelField("Error! '" + soEventTemplateFilename + "' not found! Add to the project and restart the wizard!");
            return (false);
        }

        bool hasPropertyChanged = false;

        EditorGUILayout.LabelField("Asset Path", AssetPath);

        hasPropertyChanged |= _wizardSettings.DrawSettings();
        hasPropertyChanged |= WizardTools.DrawTextField("Event name", ref _eventName);
        hasPropertyChanged |= WizardTools.DrawCategoryField("Category", ref _categoryName);

        SerializedObject eventWizardSO = new SerializedObject(this);
        eventWizardSO.Update();

        SerializedProperty parameterInfosSO = eventWizardSO.FindProperty("wizardParameters");
        hasPropertyChanged |= wizardParameters.DrawParametersEditor(parameterInfosSO);

        hasPropertyChanged |= eventWizardSO.ApplyModifiedProperties();

        WizardTools.CheckValidity(this, CheckEventName, wizardParameters.CheckValidity);

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

    private string BuildUnityEventParamsString()
    {
        string ueParams = string.Empty;

        WizardParameters.ParameterType[] parameterInfos = wizardParameters.parameterInfos;
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

        WizardParameters.ParameterType[] parameterInfos = wizardParameters.parameterInfos;
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

        WizardParameters.ParameterType[] parameterInfos = wizardParameters.parameterInfos;
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

        WizardParameters.ParameterType[] parameterInfos = wizardParameters.parameterInfos;
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

        WizardParameters.ParameterType[] parameterInfos = wizardParameters.parameterInfos;
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

    private string CheckEventName()
    {
        if (string.IsNullOrEmpty(_eventName))
        {
            return ("Event name is empty");
        }

        return (null);
    }
}
