using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public abstract class SONumericClampedPropertyDrawer<ValueType> : SONumericPropertyDrawer
{
    protected override void DrawValueProperty(SerializedObject objectProperty, SerializedProperty property, Rect position)
    {
        SerializedProperty modeProp = objectProperty.FindProperty("clampMode");
        SerializedProperty valueProp = objectProperty.FindProperty("_value");

        SONumericClamped<ValueType>.ClampMode mode = (SONumericClamped<ValueType>.ClampMode)modeProp.enumValueIndex;
        switch (mode)
        {
            case SONumericClamped<ValueType>.ClampMode.MinimumAndMaximum:
                DrawSliderMinMax(position, objectProperty, valueProp);
                break;

            case SONumericClamped<ValueType>.ClampMode.Unclamped:
            case SONumericClamped<ValueType>.ClampMode.Minimum:
            case SONumericClamped<ValueType>.ClampMode.Maximum:
            default:
                DrawSimpleValue(position, objectProperty, valueProp);
                break;
        }

        EditorUtility.SetDirty(property.serializedObject.targetObject);
    }

    protected abstract void DrawSimpleValue(Rect position, SerializedObject objectProperty, SerializedProperty valueProp);
    protected abstract void DrawSliderMinMax(Rect position, SerializedObject objectProperty, SerializedProperty valueProp);
}
