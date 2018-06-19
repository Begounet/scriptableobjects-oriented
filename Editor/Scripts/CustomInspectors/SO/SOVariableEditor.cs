﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SOBaseVariable), true)]
public class SOVariableEditor : Editor
{
    protected const int modeIconWidth = 20;
    protected const int space = 5;

    private bool _isInitialized = false;

    protected Texture _iconBackground;
    private Texture _iconConstant;
    private Texture _iconRuntime;

    public override void OnInspectorGUI()
    {
        if (!_isInitialized)
        {
            Init();
            _isInitialized = true;
        }

        bool hasChanged = false;

        DrawScriptHeader();
        hasChanged |= DrawMode();
        hasChanged |= DrawDescription();

        if (hasChanged)
        {
            serializedObject.ApplyModifiedProperties();
        }
    }

    protected virtual void  Init()
    {
        _iconBackground = SOTextureResourceManager.GetTexture(SOTextureResourceManager.Resource.IconBackground);
        _iconConstant = SOTextureResourceManager.GetTexture(SOTextureResourceManager.Resource.IconConstant);
        _iconRuntime = SOTextureResourceManager.GetTexture(SOTextureResourceManager.Resource.IconRuntime);
    }

    protected void DrawScriptHeader()
    {
        GUI.enabled = false;
        EditorGUILayout.ObjectField("Script", MonoScript.FromScriptableObject(target as ScriptableObject), typeof(SOBaseVariable), false);
        GUI.enabled = true;
    }

    protected bool DrawMode()
    {
        GUI.changed = false;

        SerializedProperty modeProp = serializedObject.FindProperty("mode");
        Rect position = EditorGUILayout.BeginHorizontal();
        {
            EditorGUILayout.PrefixLabel("");
            position = EditorGUI.PrefixLabel(position, new GUIContent(modeProp.displayName));
            position.width -= (modeIconWidth + space);

            EditorGUI.PropertyField(position, modeProp, GUIContent.none);

            position.x += position.width + space;
            position.width = modeIconWidth;

            SOVariableMode mode = (SOVariableMode) modeProp.enumValueIndex;

            Texture iconTexture = 
                (mode == SOVariableMode.Constant ? _iconConstant : _iconRuntime);

            GUI.DrawTexture(position, _iconBackground, ScaleMode.ScaleToFit, true, 1.0f);
            GUI.DrawTexture(position, iconTexture, ScaleMode.ScaleToFit, true, 1.0f);
        }
        EditorGUILayout.EndHorizontal();

        return (GUI.changed);
    }

    protected bool DrawDescription()
    {
        GUI.changed = false;
        EditorGUILayout.PropertyField(serializedObject.FindProperty("description"));
        return (GUI.changed);
    }


}
