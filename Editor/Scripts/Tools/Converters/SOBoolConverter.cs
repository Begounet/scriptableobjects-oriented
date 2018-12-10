using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;

public static class SOBoolConverter
{
    [MenuItem("Assets/SO/Convert/Bool/Simple to Watchable", priority = 1)]
    static void ConvertSOBoolToSOWBools()
    {
        List<Object> convertedAssets = new List<Object>();

        string[] assetsGUIDs = Selection.assetGUIDs;
        for (int i = 0; i < assetsGUIDs.Length; ++i)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(assetsGUIDs[i]);
            SOBool soBool = AssetDatabase.LoadAssetAtPath<SOBool>(assetPath);
            if (soBool && !(soBool is SOWBool))
            {
                SOWBool newSOWBool = ConvertSOBoolToSOWBool(soBool, assetPath);
                convertedAssets.Add(newSOWBool);
            }
        }

        Selection.objects = convertedAssets.ToArray();
    }

    static SOWBool ConvertSOBoolToSOWBool(SOBool soBool, string assetPath)
    {
        SOWBool newWatchableBool = ScriptableObject.CreateInstance<SOWBool>();

        newWatchableBool.Mode = soBool.Mode;
        newWatchableBool.Description = soBool.Description;
        newWatchableBool.Value = soBool.Value;

        AssetDatabase.DeleteAsset(assetPath);
        AssetDatabase.CreateAsset(newWatchableBool, assetPath);
        return newWatchableBool;
    }

    [MenuItem("Assets/SO/Convert/Bool/Simple to Watchable", priority = 1, validate = true)]
    static bool Validate_ConvertSOBoolToSOWBools()
    {
        for (int i = 0; i < Selection.objects.Length; ++i)
        {
            if (Selection.objects[i] is SOBool && !(Selection.objects[i] is SOWBool))
            {
                return true;
            }
        }
        return false;
    }
}
