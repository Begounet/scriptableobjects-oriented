using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITextSync : MonoBehaviour
{
    public Text text;
    public string format;
    public SOBaseVariable[] variables;

    private object[] _cachedObjects;

	void Start ()
    {
        WatchVariablesIfPossible();
        UpdateText();
	}

    private void WatchVariablesIfPossible()
    {
        if (variables != null)
        {
            for (int i = 0; i < variables.Length; ++i)
            {
                ISOWatchable watchableVariable = variables[i] as ISOWatchable;
                watchableVariable.onSOChanged -= OnSOVariableChanged; // Assure to subscribe only once
                watchableVariable.onSOChanged += OnSOVariableChanged;
            }
        }
    }

    private void OnSOVariableChanged(ISOWatchable soVariable)
    {
        UpdateText();
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        WatchVariablesIfPossible();
        UpdateText();
    }
#endif

    public void UpdateText()
    {
        if (text == null)
        {
            return;
        }

        if (variables != null && variables.Length > 0)
        {
            // Allocate only if required
            if (_cachedObjects == null || _cachedObjects.Length != variables.Length)
            {
                _cachedObjects = new object[variables.Length];
            }

            // Cache all the main object from the SO variables
            for (int i = 0; i < variables.Length; ++i)
            {
                if (variables[i] != null)
                {
                    _cachedObjects[i] = variables[i].GetObject();
                }
            }

            // Format the text using the object.ToString of each object
            text.text = string.Format(format, _cachedObjects);
        }
        else
        {
            text.text = format;
        }
    }
}
