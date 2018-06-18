using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using System;

public static class WizardTools
{
    /// <summary>
    /// Get the path of the current directory selected in Unity
    /// </summary>
    public static string GetSelectedPath()
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

    /// <summary>
    /// Try to find a GUID for a specified file.
    /// </summary>
    public static string AutoFindFile(string filename, bool logError = false)
    {
        string[] assets = AssetDatabase.FindAssets(filename);
        if (assets.Length > 0)
        {
            return (assets[0]);
        }
        else
        {
            if (logError)
            {
                Debug.LogErrorFormat("'{0}' cannot be found! Add it in the project and restart the wizard.", filename);
            }
            return (string.Empty);
        }
    }

    /// <summary>
    /// Check multiple functions that should return null or an error string if an error occurs.
    /// If an error occurs, the wizard error message will be automatically set.
    /// </summary>
    public static bool CheckValidity(ScriptableWizard wizard, params Func<string>[] checkFunctions)
    {
        for (int i = 0; i < checkFunctions.Length; ++i)
        {
            string errorMsg = checkFunctions[i]();
            if (errorMsg != null)
            {
                wizard.errorString = errorMsg;
                return (false);
            }
        }

        wizard.errorString = string.Empty;
        return (true);
    }

    /// <summary>
    /// Draw a generic text field. Update the value if changes occurred.
    /// </summary>
    /// <returns>True if the field has been changed</returns>
    public static bool DrawTextField(string labelName, ref string value, string tooltip = null)
    {
        GUI.tooltip = tooltip;
        string newValue = EditorGUILayout.TextField(labelName, value);
        if (newValue != value)
        {
            value = newValue;
            return (true);
        }
        GUI.tooltip = null;
        return (false);
    }

    /// <summary>
    /// Draw a text field specific to the category. Update the value if changes occurred.
    /// </summary>
    /// <returns>True if the field has been changed</returns>
    public static bool DrawCategoryField(string labelName, ref string value)
    {
        return DrawTextField(labelName, ref value, "The category will defined where the event will be in the asset menu");
    }
}
