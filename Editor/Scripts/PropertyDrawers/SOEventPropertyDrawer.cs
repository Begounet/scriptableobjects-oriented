using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(SOEvent), true)]
internal class SOEventPropertyDrawer : PropertyDrawer
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

                    string tooltip;
                    Texture selectedIcon;

                    switch (eventModeAttribute.mode)
                    {
                        case SOEventActionMode.Broadcaster:
                            tooltip = "Broadcaster";
                            selectedIcon = _iconBroadcaster;
                            break;

                        case SOEventActionMode.Listener:
                            tooltip = "Listener";
                            selectedIcon = _iconListener;
                            break;

                        default:
                            tooltip = string.Empty;
                            selectedIcon = null;
                            break;
                    }

                    if (selectedIcon != null)
                    {
                        GUI.DrawTexture(position, selectedIcon, ScaleMode.ScaleToFit, true, 1.0f);
                        GUI.Label(position, new GUIContent("", tooltip));
                    }
                }
            }
        }
        EditorGUI.EndProperty();
    }

    private void Init()
    {
        _isInitialized = true;

        _iconBroadcaster = SOTextureResourceManager.GetTexture(SOTextureResourceManager.Resource.IconBroadcaster);
        _iconListener = SOTextureResourceManager.GetTexture(SOTextureResourceManager.Resource.IconListener);
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