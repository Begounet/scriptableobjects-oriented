using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SOVariableMode
{
    Constant,
    Runtime
}

/// <summary>
/// Every SO variable should inherit from it.
/// </summary>
public class SOBaseVariable : ScriptableObject
{
#if UNITY_EDITOR

    [SerializeField]
    [Tooltip("Tell explicitly if your variable purpose is to be a constant data or a value to be set during the runtime")]
    private SOVariableMode mode = SOVariableMode.Constant;
    public SOVariableMode Mode
    {
        get { return (mode); }
        set { mode = value; }
    }

    [TextArea]
    [SerializeField]
    [Tooltip("Describes when your event will be called.")]
    private string description;
    public string Description
    {
        get { return (description); }
        set { description = value; }
    }

#endif

    public virtual void Reset() {}

    // Can be overriden to return the main object contained
    // in the scriptable object.
    public virtual object GetObject() { return this; }
}
