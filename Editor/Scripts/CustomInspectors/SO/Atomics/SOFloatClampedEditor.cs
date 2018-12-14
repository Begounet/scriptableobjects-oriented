using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SOFloatClamped), true)]
public class SOFloatClampedEditor : SONumericClampedEditor
{
    protected override void DrawSlider()
    {
        SerializedProperty valueProp = serializedObject.FindProperty("_value");
        SerializedProperty minProp = serializedObject.FindProperty("min");
        SerializedProperty maxProp = serializedObject.FindProperty("max");
        
        EditorGUILayout.Slider(valueProp, minProp.floatValue, maxProp.floatValue);
    }

    protected override void DrawValueProperty()
    {
        SOFloatClamped floatClamped = serializedObject.targetObject as SOFloatClamped;
        SerializedProperty valueProp = serializedObject.FindProperty("_value");
        floatClamped.Value = EditorGUILayout.FloatField(valueProp.displayName, valueProp.floatValue);
    }
}
