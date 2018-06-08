using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(SOEvent), true)]
public class SOEventPropertyDrawer : PropertyDrawer
{
    private const float ActionModeWidth = 20;
    private const float Space = 5;

    private bool _isInitialized;
    private Texture _iconBroadcaster;
    private Texture _iconListener;

    public override void OnGUI(UnityEngine.Rect position, UnityEditor.SerializedProperty property, UnityEngine.GUIContent label)
    {
        if (!_isInitialized)
        {
            Init();
        }

        EditorGUI.BeginProperty(position, label, property);
        {
            SOEventModeAttribute eventMode = FindEventModeAttribute();
            bool shouldDisplayActionMode = (eventMode != null);

            if (shouldDisplayActionMode)
            {
                position.width -= ActionModeWidth;
            }
            EditorGUI.PropertyField(position, property);

            if (shouldDisplayActionMode)
            {
                position.x += position.width + Space;
                position.width = ActionModeWidth;

                object[] attributes = fieldInfo.GetCustomAttributes(typeof(SOEventModeAttribute), true);
                if (attributes.Length > 0)
                {
                    SOEventModeAttribute eventModeAttribute = attributes[0] as SOEventModeAttribute;
                    Texture selectedIcon = (eventModeAttribute.mode == SOEventActionMode.Broadcaster) ? _iconBroadcaster : _iconListener;
                    GUI.DrawTexture(position, selectedIcon, ScaleMode.ScaleToFit, true, 1.0f);
                }
            }
        }
        EditorGUI.EndProperty();
    }

    private void Init()
    {
        _isInitialized = true;

        _iconBroadcaster = Resources.Load<Texture>("IconBroadcaster");
        _iconListener = Resources.Load<Texture>("IconListener");
    }

    private SOEventModeAttribute FindEventModeAttribute()
    {
        object[] attributes = fieldInfo.GetCustomAttributes(typeof(SOEventModeAttribute), true);
        if (attributes.Length > 0)
        {
            return attributes[0] as SOEventModeAttribute;
        }
        return (null);
    }
}