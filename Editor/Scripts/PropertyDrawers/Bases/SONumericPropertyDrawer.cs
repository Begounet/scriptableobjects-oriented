﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

internal abstract class SONumericPropertyDrawer : SOVariablePropertyDrawer
{
    public override void OnGUI(UnityEngine.Rect position, UnityEditor.SerializedProperty property, UnityEngine.GUIContent label)
    {
        bool shouldDrawProp = false;
        SerializedObject objectProp = null;

        if (property.objectReferenceValue != null)
        {
            objectProp = new SerializedObject(property.objectReferenceValue);
            shouldDrawProp = ShouldDrawProperty(objectProp);
        }

        position = DrawPrefixLabel(position, property, label);
        
        int numControls = 1;

        if (property.objectReferenceValue != null && shouldDrawProp)
        {
            numControls = 2;
        }

        float singleControlWidth = position.width / numControls;
        position.width = singleControlWidth;

        if (property.objectReferenceValue != null && shouldDrawProp)
        {
            position.width -= controlsSpace;

            EditorGUI.BeginChangeCheck();
            DrawValuePropertyIfRequired(objectProp, property, position);

            if (EditorGUI.EndChangeCheck())
            {
                objectProp.ApplyModifiedProperties();
            }

            position.x += singleControlWidth + controlsSpace;
        }

        EditorGUI.BeginChangeCheck();
        DrawVariableProperty(position, property, label);
        if (EditorGUI.EndChangeCheck())
        {
            property.serializedObject.ApplyModifiedProperties();
        }
    }

    void DrawValuePropertyIfRequired(SerializedObject objectProperty, SerializedProperty property, Rect position)
    {
        if (ShouldDrawProperty(objectProperty))
        {
            DrawValueProperty(objectProperty, property, position);
        }
    }

    bool ShouldDrawProperty(SerializedObject objectProperty)
    {
        SerializedProperty modeProp = objectProperty.FindProperty("mode");
        bool isForRuntime = (SOVariableMode)modeProp.enumValueIndex == SOVariableMode.Runtime;

        return (!isForRuntime || Application.isPlaying);
    }

    protected abstract void DrawValueProperty(SerializedObject objectProperty, SerializedProperty property, Rect position);
}
