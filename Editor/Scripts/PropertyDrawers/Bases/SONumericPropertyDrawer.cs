using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public abstract class SONumericPropertyDrawer : SOVariablePropertyDrawer
{
    public override void OnGUI(UnityEngine.Rect position, UnityEditor.SerializedProperty property, UnityEngine.GUIContent label)
    {
        position = DrawPrefixLabel(position, property, label);
        
        int numControls = 1;

        if (property.objectReferenceValue != null)
        {
            numControls = 2;
        }

        float singleControlWidth = position.width / numControls;
        position.width = singleControlWidth;

        if (property.objectReferenceValue != null)
        {
            position.width -= controlsSpace;

            EditorGUI.BeginChangeCheck();

            SerializedObject objectProp = new SerializedObject(property.objectReferenceValue);
            DrawValueProperty(objectProp, position);

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

    protected abstract void DrawValueProperty(SerializedObject objectProperty, Rect position);
}
