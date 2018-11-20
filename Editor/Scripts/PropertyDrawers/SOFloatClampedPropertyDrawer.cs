using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(SOFloatClamped), true)]
internal class SOFloatClampedPropertyDrawer : SONumericClampedPropertyDrawer<float>
{
    protected override void DrawSimpleValue(Rect position, SerializedObject objectProperty, SerializedProperty valueProp)
    {
        SOFloatClamped floatClamped = objectProperty.targetObject as SOFloatClamped;
        floatClamped.Value = EditorGUI.FloatField(position, GUIContent.none, floatClamped.Value);
    }

    protected override void DrawSliderMinMax(Rect position, SerializedObject objectProperty, SerializedProperty valueProp)
    {
        SerializedProperty minProp = objectProperty.FindProperty("min");
        SerializedProperty maxProp = objectProperty.FindProperty("max");

        EditorGUI.Slider(position, valueProp, minProp.floatValue, maxProp.floatValue, GUIContent.none);
    }
}