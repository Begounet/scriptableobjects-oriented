using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SOVariableMode
{
    Constant,
    Runtime
}

public class SOBaseVariable : ScriptableObject
{
#if UNITY_EDITOR

    [Tooltip("Tell explicitly if your variable purpose is to be a constant data or a value to be set during the runtime")]
    public SOVariableMode mode;

    [TextArea]
    [Tooltip("Describes when your event will be called.")]
    public string description;

#endif

    public virtual void Reset() {}
}
