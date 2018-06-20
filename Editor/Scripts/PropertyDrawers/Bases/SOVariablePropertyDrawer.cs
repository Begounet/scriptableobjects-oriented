using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[CustomPropertyDrawer(typeof(SOBaseVariable), true)]
public class SOVariablePropertyDrawer : PropertyDrawer
{
    protected const int iconSize = 20;
    protected const int controlsSpace = 3;

    public override void OnGUI(UnityEngine.Rect position, UnityEditor.SerializedProperty property, UnityEngine.GUIContent label)
    {
        position = DrawPrefixLabel(position, property, label);
        DrawVariableProperty(position, property, label);
    }
    
    protected virtual Rect DrawPrefixLabel(UnityEngine.Rect position, UnityEditor.SerializedProperty property, UnityEngine.GUIContent label)
    {
        string tooltip = GetTooltip(property, label);
        position = EditorGUI.PrefixLabel(position, new GUIContent(label.text, label.image, tooltip));
        ClearTooltip();
        return (position);
    }

    protected virtual string GetTooltip(SerializedProperty property, GUIContent label)
    {
        SOBaseVariable baseVariable = property.objectReferenceValue as SOBaseVariable;
        if (baseVariable != null && !string.IsNullOrEmpty(baseVariable.Description))
        {
            string tooltip = label.tooltip;
            if (!string.IsNullOrEmpty(tooltip))
            {
                tooltip += "\n";
            }
            return tooltip + baseVariable.Description;
        }
        return label.tooltip;
    }

    private void ClearTooltip()
    {
        GUI.tooltip = string.Empty;
    }

    protected virtual Rect DrawVariableProperty(UnityEngine.Rect position, UnityEditor.SerializedProperty property, UnityEngine.GUIContent label)
    {
        SOBaseVariable baseVariable = property.objectReferenceValue as SOBaseVariable;
        SOVariableModeAttribute variableModeAttr = null;

        int numIcons = 0;

        if (baseVariable != null)
        {
            ++numIcons;
        }

        variableModeAttr = GetVariableModeAttribute();
        if (variableModeAttr != null)
        {
            ++numIcons;
        }

        position.width -= (numIcons * (iconSize + controlsSpace));
        
        EditorGUI.PropertyField(position, property, GUIContent.none);
        position.x += position.width + controlsSpace;
        position.width = iconSize;

        if (baseVariable != null)
        {
            position = DrawModeIcon(position, baseVariable);
            position.x += controlsSpace;
        }

        if (variableModeAttr != null)
        {
            position = DrawVariableModeAttributeIcon(position, variableModeAttr.mode);
            position.x += controlsSpace;
        }

        return (position);
    }

    protected Rect DrawModeIcon(Rect position, SOBaseVariable baseVariable)
    {
        SOTextureResourceManager.Resource iconMode =
            (baseVariable.Mode == SOVariableMode.Constant ? SOTextureResourceManager.Resource.IconConstant : SOTextureResourceManager.Resource.IconRuntime);

        DrawIcon(position, iconMode, baseVariable.Mode.ToString());

        position.x += iconSize;
        return (position);
    }

    protected Rect DrawVariableModeAttributeIcon(Rect position, SOVariableActionMode actionMode)
    {
        SOTextureResourceManager.Resource iconMode;
        string tooltip;

        switch (actionMode)
        {
            case SOVariableActionMode.ReadOnly:
                iconMode = SOTextureResourceManager.Resource.IconRead;
                tooltip = "Read Only";
                break;

            case SOVariableActionMode.WriteOnly:
                iconMode = SOTextureResourceManager.Resource.IconWrite;
                tooltip = "Write Only";
                break;

            case SOVariableActionMode.ReadWrite:
                iconMode = SOTextureResourceManager.Resource.IconReadWrite;
                tooltip = "Read & Write";
                break;

            default:
                return (position);
        }

        DrawIcon(position, iconMode, tooltip);

        position.x += iconSize;
        return (position);
    }

    protected SOVariableModeAttribute GetVariableModeAttribute()
    {
        object[] variableModeAttrs = fieldInfo.GetCustomAttributes(typeof(SOVariableModeAttribute), true);
        if (variableModeAttrs.Length > 0)
        {
            return (variableModeAttrs[0] as SOVariableModeAttribute);
        }
        return (null);
    }

    protected void  DrawIconBackground(Rect position)
    {
        DrawIcon(position, SOTextureResourceManager.Resource.IconBackground, string.Empty, false);
    }

    protected void  DrawIcon(Rect position, SOTextureResourceManager.Resource resource, string tooltip = "", bool shouldDrawBackground = true)
    {
        if (shouldDrawBackground)
        {
            DrawIconBackground(position);
        }

        Texture icon = SOTextureResourceManager.GetTexture(resource);
        if (!string.IsNullOrEmpty(tooltip))
        {
            GUI.Box(position, new GUIContent(string.Empty, icon, tooltip));
        }
        else
        {
            GUI.DrawTexture(position, icon, ScaleMode.ScaleToFit, true, 1.0f);
        }
    }
}
