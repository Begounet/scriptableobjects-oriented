using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(SOIntegerClamped), true)]
internal class SOIntegerClampedPropertyDrawer : SONumericClampedPropertyDrawer<float>
{
    protected override void DrawSimpleValue(Rect position, SerializedObject objectProperty, SerializedProperty valueProp)
    {
        SOIntegerClamped intClamped = objectProperty.targetObject as SOIntegerClamped;
        intClamped.Value = EditorGUI.IntField(position, GUIContent.none, intClamped.Value);
    }

    protected override void DrawSliderMinMax(Rect position, SerializedObject objectProperty, SerializedProperty valueProp)
    {
        SerializedProperty minProp = objectProperty.FindProperty("min");
        SerializedProperty maxProp = objectProperty.FindProperty("max");

        EditorGUI.IntSlider(position, valueProp, minProp.intValue, maxProp.intValue, GUIContent.none);
    }
}
