using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public abstract class SONumericPropertyDrawer : PropertyDrawer
{
    public override void OnGUI(UnityEngine.Rect position, UnityEditor.SerializedProperty property, UnityEngine.GUIContent label)
    {
        position = EditorGUI.PrefixLabel(position, label);

        int numControls = 1;

        if (property.objectReferenceValue != null)
        {
            numControls = 2;
        }

        float singleControlWidth = position.width / numControls;
        position.width = singleControlWidth;

        if (property.objectReferenceValue != null)
        {
            GUI.changed = false;
            {
                SerializedObject objectProp = new SerializedObject(property.objectReferenceValue);
                DrawValueProperty(objectProp, position);

                if (GUI.changed)
                {
                    objectProp.ApplyModifiedProperties();
                }
            }

            position.x += singleControlWidth;
        }

        GUI.changed = false;
        EditorGUI.PropertyField(position, property, GUIContent.none, false);
        if (GUI.changed)
        {
            property.serializedObject.ApplyModifiedProperties();
        }
    }

    protected abstract void DrawValueProperty(SerializedObject objectProperty, Rect position);
}
