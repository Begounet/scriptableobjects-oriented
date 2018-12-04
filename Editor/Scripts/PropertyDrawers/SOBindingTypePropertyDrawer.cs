using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(SOBindingType))]
public class SOBindingTypePropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        SOBindingType bindingType = attribute as SOBindingType;
        Color c = GUI.color;

        if (bindingType.Binding == SO.BindingType.Required && 
            property.propertyType == SerializedPropertyType.ObjectReference &&
            property.objectReferenceValue == null)
        {
            GUI.color = Color.red;
        }

        EditorGUI.PropertyField(position, property, label, true);

        GUI.color = c;
    }
}
