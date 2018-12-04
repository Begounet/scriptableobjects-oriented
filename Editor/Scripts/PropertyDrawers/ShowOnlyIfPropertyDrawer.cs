using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace SO
{
    [CustomPropertyDrawer(typeof(ShowOnlyIfAttribute))]
    public class ShowOnlyIfPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (ShouldShowProperty(property))
            {
                EditorGUI.PropertyField(position, property, label, true);
            }
        }
        
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (ShouldShowProperty(property))
            {
                return base.GetPropertyHeight(property, label);
            }

            return 0.0f;
        }

        private bool ShouldShowProperty(SerializedProperty property)
        {
            ShowOnlyIfAttribute showOnlyIfTrueAttribute = attribute as ShowOnlyIfAttribute;
            SerializedProperty dependencyProperty = property.serializedObject.FindProperty(showOnlyIfTrueAttribute.propertyDependencyName);
            return (dependencyProperty == null || dependencyProperty.boolValue == showOnlyIfTrueAttribute.expectedValue);
        }
    }
}