using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Check if the field requiering binding (with attribute SOBindingType) is correctly set on selected ScriptableObjects and GameObject components
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
        List<Object> objectsWithMissingBinding = new List<Object>();
        List<string> missingBindings = new List<string>();

        for (int objectIndex = 0; objectIndex < Selection.objects.Length; ++objectIndex)
        {
            UnityEngine.Object obj = Selection.objects[objectIndex];
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
                bool isGameObjectMissingBinding = false;

                GameObject go = obj as GameObject;
                Component[] components = go.GetComponents<Component>();
                for (int componentIndex = 0; componentIndex < components.Length; ++componentIndex)
                {
                    Component comp = components[componentIndex];
                    if (IsBindingMissing(comp, ref missingBindings))
                    {
                        isGameObjectMissingBinding = true;
                        Debug.LogWarningFormat(go, "Missing binding on GameObject({0}), Component({1}) - [{2}]", go.name, comp, JoinStringListAsOneString(missingBindings));
                    }
                }

                if (isGameObjectMissingBinding)
                {
                    objectsWithMissingBinding.Add(go);
                }
            }
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
            if (Selection.objects[i] is ScriptableObject || 
                Selection.objects[i] is GameObject)
            {
                return true;
            }
        }
        return false;
    }

    private static bool IsBindingMissing(Object obj, ref List<string> missingBindingFields)
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
