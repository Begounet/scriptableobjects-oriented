using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SOTextureSampler))]
public class SOTextureSamplerEditor : SOVariableEditor
{
    public override bool HasPreviewGUI()
    {
        SOTextureSampler textureSampler = target as SOTextureSampler;
        return textureSampler.IsTextureValid;
    }

    public override void OnPreviewGUI(Rect r, GUIStyle background)
    {
        base.OnPreviewGUI(r, background);
        SOTextureSampler textureSampler = target as SOTextureSampler;
        GUI.DrawTexture(r, textureSampler.LookupTable);
    }
}
