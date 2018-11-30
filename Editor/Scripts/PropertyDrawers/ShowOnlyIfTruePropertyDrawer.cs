using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace SO
{
    [CustomPropertyDrawer(typeof(ShowOnlyIfTrueAttribute))]
    public class ShowOnlyIfTruePropertyDrawer : PropertyDrawer
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
            ShowOnlyIfTrueAttribute showOnlyIfTrueAttribute = attribute as ShowOnlyIfTrueAttribute;
            SerializedProperty dependencyProperty = property.serializedObject.FindProperty(showOnlyIfTrueAttribute.propertyDependencyName);
            return (dependencyProperty == null || dependencyProperty.boolValue);
        }
    }
}