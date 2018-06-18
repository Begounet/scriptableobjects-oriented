using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SOSettings))]
public class SOSettingsEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        bool hasChanged = false;

        // hasChanged |= DrawEditorScriptPath();

        if (hasChanged)
        {
            serializedObject.ApplyModifiedProperties();
        }
    }

    bool    DrawEditorScriptPath()
    {
        bool hasChanged = false;

        SerializedProperty editorScriptPathGUID = serializedObject.FindProperty("editorScriptPathGUID");
        string editorScriptPath = AssetDatabase.GUIDToAssetPath(editorScriptPathGUID.stringValue);

        EditorGUILayout.BeginHorizontal();
        {
            EditorGUILayout.LabelField("Generated Editor Script Path");

            if (GUILayout.Button("Browse..."))
            {
                string defaultPath = Application.dataPath;
                if (!string.IsNullOrEmpty(editorScriptPathGUID.stringValue))
                {
                    defaultPath = editorScriptPath;
                }

                string result = EditorUtility.OpenFolderPanel("Select folder where your editor scripts will be generated automatically.", defaultPath, "");
                if (!string.IsNullOrEmpty(result))
                {
                    var dataPathUri = new Uri(Application.dataPath);
                    var resultPathUri = new Uri(result);

                    var relativeUri = dataPathUri.MakeRelativeUri(resultPathUri);
                    var relativePath = Uri.UnescapeDataString(relativeUri.ToString());
                    
                    editorScriptPathGUID.stringValue = AssetDatabase.AssetPathToGUID(relativePath);
                    hasChanged = true;
                }
            }
        }
        EditorGUILayout.EndHorizontal();

        GUI.enabled = false;
        EditorGUILayout.TextField(editorScriptPath);
        GUI.enabled = true;

        return (hasChanged);
    }

}
