using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Check if the field requiering binding (with attribute [SOBindingType]) is correctly set on selected ScriptableObjects and GameObject components
/// </summary>
public class SOBindingChecker
{
    [MenuItem("GameObject/SO/Check required bindings", isValidateFunction: false, priority = 10)]
    private static void CheckSOBindingOnGameObject(MenuCommand menuCommand)
    {
        CheckSOBinding();
    }

    [MenuItem("Assets/SO/Check required bindings")]
    private static void CheckSOBinding()
    {
        List<UnityEngine.Object> objectsWithMissingBinding = new List<UnityEngine.Object>();
        List<string> missingBindings = new List<string>();

        for (int objectIndex = 0; objectIndex < Selection.objects.Length; ++objectIndex)
        {
            UnityEngine.Object obj = Selection.objects[objectIndex];
            CheckSOBindingForUnityObject(obj, ref missingBindings, ref objectsWithMissingBinding);
        }

        if (objectsWithMissingBinding.Count > 0)
        {
            // Select objects with missing binding
            Selection.objects = objectsWithMissingBinding.ToArray();
        }
        else
        {
            Debug.Log("All required bindings are correctly set. Enjoy!");
        }
    }

    private static void CheckSOBindingForUnityObject(UnityEngine.Object obj, ref List<string> missingBindings, ref List<UnityEngine.Object> objectsWithMissingBinding)
    {
        if (obj is ScriptableObject)
        {
            if (IsBindingMissing(obj, ref missingBindings))
            {
                objectsWithMissingBinding.Add(obj);
                Debug.LogWarningFormat(obj, "Missing binding on ScriptableObject({0}) - [{1}]", obj.name, JoinStringListAsOneString(missingBindings));
            }                
        }
        else if (obj is GameObject)
        {
            CheckSOBindingForGameObjectAndChildren(obj as GameObject, objectsWithMissingBinding, missingBindings);
        }
        else if (obj is DefaultAsset)
        {
            string assetPath = AssetDatabase.GetAssetPath(obj);
            if (Directory.Exists(assetPath))
            {
                CheckSOBindingForSOInDirectoryRecursively(assetPath, ref missingBindings, ref objectsWithMissingBinding);
            }
        }
    }

    private static void CheckSOBindingForSOInDirectoryRecursively(string directoryPath, ref List<string> missingBindings, ref List<UnityEngine.Object> objectsWithMissingBinding)
    {
        string[] assetPaths = Directory.GetFiles(directoryPath);
        for (int i = 0; i < assetPaths.Length; ++i)
        {
            ScriptableObject scriptableObject = AssetDatabase.LoadAssetAtPath<ScriptableObject>(assetPaths[i]);
            CheckSOBindingForUnityObject(scriptableObject, ref missingBindings, ref objectsWithMissingBinding);
        }

        string[] subDirectories = Directory.GetDirectories(directoryPath);
        for (int i = 0; i < subDirectories.Length; ++i)
        {
            CheckSOBindingForSOInDirectoryRecursively(subDirectories[i], ref missingBindings, ref objectsWithMissingBinding);
        }
    }

    private static void CheckSOBindingForGameObjectAndChildren(GameObject rootGameObject, List<UnityEngine.Object> objectsWithMissingBinding, List<string> missingBindings)
    {
        bool isGameObjectMissingBinding = false;
        
        Component[] components = rootGameObject.GetComponents<Component>();
        for (int componentIndex = 0; componentIndex < components.Length; ++componentIndex)
        {
            Component comp = components[componentIndex];
            if (IsBindingMissing(comp, ref missingBindings))
            {
                isGameObjectMissingBinding = true;
                Debug.LogWarningFormat(rootGameObject, "Missing binding on GameObject({0}), Component({1}) - [{2}]", rootGameObject.name, comp, JoinStringListAsOneString(missingBindings));
            }
        }

        if (isGameObjectMissingBinding)
        {
            objectsWithMissingBinding.Add(rootGameObject);
        }

        // Check children too
        for (int childIdx = 0; childIdx < rootGameObject.transform.childCount; ++childIdx)
        {
            CheckSOBindingForGameObjectAndChildren(rootGameObject.transform.GetChild(childIdx).gameObject, objectsWithMissingBinding, missingBindings);
        }
    }

    [MenuItem("GameObject/SO/Check required bindings", isValidateFunction: true, priority = 10)]
    private static bool ValidateFunction_CheckSOBindingOnGameObject(MenuCommand menuCommand)
    {
        return ValidateFunction_CheckSOBinding();
    }

    [MenuItem("Assets/SO/Check required bindings", isValidateFunction: true)]
    private static bool ValidateFunction_CheckSOBinding()
    {
        for (int i = 0; i < Selection.objects.Length; ++i)
        {
            if (IsAssetValidForSOBindingCheck(Selection.objects[i]) || 
                (Selection.objects[i] is DefaultAsset && DoesDirectoryContainsValidAssetForSOBindingCheck(Selection.objects[i])))
            {
                return true;
            }
        }
        return false;
    }

    private static bool IsAssetValidForSOBindingCheck(UnityEngine.Object obj)
    {
        return (obj is ScriptableObject || obj is GameObject);
    }

    private static bool DoesDirectoryContainsValidAssetForSOBindingCheck(UnityEngine.Object obj)
    {
        if (obj is DefaultAsset)
        {
            string directoryPath = AssetDatabase.GetAssetPath(obj);
            if (Directory.Exists(directoryPath))
            {
                return DoesDirectoryContainsValidAssetForSOBindingCheck(directoryPath);
            }
        }
        return false;
    }

    private static bool DoesDirectoryContainsValidAssetForSOBindingCheck(string directoryPath)
    {
        string[] assetPaths = Directory.GetFiles(directoryPath);
        for (int i = 0; i < assetPaths.Length; ++i)
        {
            if (IsAssetValidForSOBindingCheck(AssetDatabase.LoadAssetAtPath<ScriptableObject>(assetPaths[i])))
            {
                return true;
            }
        }

        string[] subDirectoryPaths = Directory.GetDirectories(directoryPath);
        for (int i = 0; i < subDirectoryPaths.Length; ++i)
        {
            if (DoesDirectoryContainsValidAssetForSOBindingCheck(subDirectoryPaths[i]))
            {
                return true;
            }
        }

        return false;
    }

    private static bool IsBindingMissing(UnityEngine.Object obj, ref List<string> missingBindingFields)
    {
        missingBindingFields.Clear();

        System.Type objectType = obj.GetType();

        SerializedObject so = new SerializedObject(obj);
        SerializedProperty sp = so.GetIterator();
        while (sp.NextVisible(true))
        {
            if (sp.propertyType == SerializedPropertyType.ObjectReference)
            {
                bool isBindingRequired = false;
                string propertyName = sp.name;

                System.Reflection.FieldInfo fieldInfo = objectType.GetField(propertyName, 
                    System.Reflection.BindingFlags.NonPublic | 
                    System.Reflection.BindingFlags.Public | 
                    System.Reflection.BindingFlags.Instance);

                if (fieldInfo != null)
                {
                    isBindingRequired = IsBindingRequiredOnField(fieldInfo);
                }

                if (isBindingRequired && !sp.objectReferenceValue)
                {
                    missingBindingFields.Add(sp.displayName);
                }
            }
        }

        return (missingBindingFields.Count > 0);
    }

    private static bool IsBindingRequiredOnField(System.Reflection.FieldInfo fieldInfo)
    {
        object[] bindingTypes = fieldInfo.GetCustomAttributes(typeof(SOBindingType), false);
        for (int bindingTypesIndex = 0; bindingTypesIndex < bindingTypes.Length; ++bindingTypesIndex)
        {
            SOBindingType bindingType = bindingTypes[bindingTypesIndex] as SOBindingType;
            if (bindingType.Binding == SO.BindingType.Required)
            {
                return true;
            }
        }

        return false;
    }

    private static string JoinStringListAsOneString(List<string> list)
    {
        string result = string.Empty;

        for (int i = 0; i < list.Count; ++i)
        {
            result += list[i];
            if (i + 1 < list.Count)
            {
                result += ", ";
            }
        }

        return result;
    }
}
