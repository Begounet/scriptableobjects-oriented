using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SOIntegerClamped), true)]
public class SOIntegerClampedEditor : SONumericClampedEditor
{
    protected override void DrawSlider()
    {
        SerializedProperty valueProp = serializedObject.FindProperty("_value");
        SerializedProperty minProp = serializedObject.FindProperty("min");
        SerializedProperty maxProp = serializedObject.FindProperty("max");

        EditorGUILayout.IntSlider(valueProp, minProp.intValue, maxProp.intValue);
    }

    protected override void DrawValueProperty()
    {
        SOIntegerClamped intClamped = serializedObject.targetObject as SOIntegerClamped;
        SerializedProperty valueProp = serializedObject.FindProperty("_value");
        intClamped.Value = EditorGUILayout.IntField(valueProp.displayName, valueProp.intValue);
    }
}
