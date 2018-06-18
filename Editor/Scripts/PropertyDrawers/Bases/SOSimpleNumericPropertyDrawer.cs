using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(SOFloat), true)]
[CustomPropertyDrawer(typeof(SOInteger), true)]
public class SOSimpleNumericPropertyDrawer : SONumericPropertyDrawer
{
    protected override void DrawValueProperty(SerializedObject objectProperty, Rect position)
    {
        SerializedProperty valueProp = objectProperty.FindProperty("Value");
        EditorGUI.PropertyField(position, valueProp, GUIContent.none, false);
    }
}
