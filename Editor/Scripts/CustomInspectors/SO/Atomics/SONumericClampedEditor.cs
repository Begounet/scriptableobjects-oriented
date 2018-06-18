using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public abstract class SONumericClampedEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.DrawDefaultInspector();

        SerializedProperty clampModeProp = serializedObject.FindProperty("clampMode");

        GUI.changed = false;

        SOFloatClamped.ClampMode mode = (SOFloatClamped.ClampMode)clampModeProp.enumValueIndex;
        DrawMininmumAndMaximum(mode);

        if (mode == SOFloatClamped.ClampMode.MinimumAndMaximum)
        {
            DrawSlider();
        }
        else
        {
            DrawValueProperty();
        }

        if (GUI.changed)
        {
            serializedObject.ApplyModifiedProperties();
        }
    }

    protected abstract void DrawSlider();
    protected abstract void DrawValueProperty();

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

