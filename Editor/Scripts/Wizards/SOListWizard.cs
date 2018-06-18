using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class SOListWizard : ScriptableWizard
{
    private const string soListTemplateFilename = "SOListTemplate";

    public string AssetPath { get; set; }

    public WizardParameters wizardParameters;
    private WizardSettings _wizardSettings;

    private string _listName = "SOListName";
    private string _categoryName = "Game/Lists";

    private static string soListTemplateFileGUID;

    [MenuItem("Assets/Create/SO/Create/Containers/List", priority = 1)]
    static void CreateWizard()
    {
        SOListWizard wizard = DisplayWizard<SOListWizard>("ScriptableObject List Creation Wizard", "Create");
        wizard.AssetPath = WizardTools.GetSelectedPath();
    }

    public void OnEnable()
    {
        wizardParameters = new WizardParameters();
        _wizardSettings = new WizardSettings();

        wizardParameters.numMaxParameters = 1;
        wizardParameters.AssociateWizardSettings(_wizardSettings);
        wizardParameters.prefixName = "List Item Type";
        wizardParameters.canEditParameterName = false;

        if (string.IsNullOrEmpty(soListTemplateFileGUID))
        {
            soListTemplateFileGUID = WizardTools.AutoFindFile(soListTemplateFilename, true);
        }
    }

    protected override bool DrawWizardGUI()
    {
        if (string.IsNullOrEmpty(soListTemplateFileGUID))
        {
            EditorGUILayout.LabelField("Error! '" + soListTemplateFilename + "' not found! Add to the project and restart the wizard!");
            return (false);
        }

        bool hasPropertyChanged = false;

        EditorGUILayout.LabelField("Asset Path", AssetPath);

        hasPropertyChanged |= _wizardSettings.DrawSettings();
        hasPropertyChanged |= WizardTools.DrawTextField("List name", ref _listName);
        hasPropertyChanged |= WizardTools.DrawCategoryField("Category", ref _categoryName);

        SerializedObject wizardSO = new SerializedObject(this);
        wizardSO.Update();

        SerializedProperty wizardParametersSO = wizardSO.FindProperty("wizardParameters");
        hasPropertyChanged |= wizardParameters.DrawParametersEditor(wizardParametersSO);

        hasPropertyChanged |= wizardSO.ApplyModifiedProperties();

        WizardTools.CheckValidity(this, CheckListName, wizardParameters.CheckValidity);

        return (hasPropertyChanged);
    }

    private string CheckListName()
    {
        if (string.IsNullOrEmpty(_listName))
        {
            return ("List name is empty");
        }

        return (null);
    }

    void OnWizardCreate()
    {
        CreateAsset();
    }

    void CreateAsset()
    {
        string soListTemplateFilePath = AssetDatabase.GUIDToAssetPath(soListTemplateFileGUID);
        string soListTemplateContent = File.ReadAllText(soListTemplateFilePath);

        soListTemplateContent = soListTemplateContent.Replace("{ListName}", _listName);

        if (!string.IsNullOrEmpty(_categoryName))
        {
            soListTemplateContent = soListTemplateContent.Replace("{Category}", "/" + _categoryName);
        }

        soListTemplateContent = soListTemplateContent.Replace("{ListItemType}", wizardParameters.parameterInfos[0].parameterTypeName);

        string dstFilePath = Path.Combine(AssetPath, _listName + ".cs");
        File.WriteAllText(dstFilePath, soListTemplateContent, System.Text.Encoding.UTF8);

        AssetDatabase.Refresh();
    }
}
