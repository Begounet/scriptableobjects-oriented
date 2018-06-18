using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SOFloatClamped))]
public class SOFloatClampedEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.DrawDefaultInspector();

        SerializedProperty valueProp = serializedObject.FindProperty("_value");
        SerializedProperty minProp = serializedObject.FindProperty("min");
        SerializedProperty maxProp = serializedObject.FindProperty("max");
        SerializedProperty clampModeProp = serializedObject.FindProperty("clampMode");

        GUI.changed = false;

        SOFloatClamped.ClampMode mode = (SOFloatClamped.ClampMode) clampModeProp.enumValueIndex;
        DrawMininmumAndMaximum(mode);

        if (mode == SOFloatClamped.ClampMode.MinimumAndMaximum)
        {
            EditorGUILayout.Slider(valueProp, minProp.floatValue, maxProp.floatValue);
        }
        else
        {
            SOFloatClamped floatClamped = serializedObject.targetObject as SOFloatClamped;
            floatClamped.Value = EditorGUILayout.FloatField(valueProp.displayName, valueProp.floatValue);
        }

        if (GUI.changed)
        {
            serializedObject.ApplyModifiedProperties();
        }
    }

    private void DrawMininmumAndMaximum(SOFloatClamped.ClampMode mode)
    {
        switch (mode)
        {
            case SOFloatClamped.ClampMode.Minimum:
                DrawMinimum();
                break;

            case SOFloatClamped.ClampMode.Maximum:
                DrawMaximum();
                break;

            case SOFloatClamped.ClampMode.MinimumAndMaximum:
                DrawMinimum();
                DrawMaximum();
                break;

            case SOFloatClamped.ClampMode.Unclamped:
            default:
                break;
        }
    }

    private void DrawMinimum()
    {
        SerializedProperty minProp = serializedObject.FindProperty("min");
        EditorGUILayout.PropertyField(minProp);
    }

    private void DrawMaximum()
    {
        SerializedProperty maxProp = serializedObject.FindProperty("max");
        EditorGUILayout.PropertyField(maxProp);
    }

}
