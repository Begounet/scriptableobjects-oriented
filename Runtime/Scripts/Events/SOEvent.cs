using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SOEvent : ScriptableObject
{
#if UNITY_EDITOR

    [TextArea]
    [Tooltip("Describes when your event will be called.")]
    public string description;
    
    public abstract void EditorRaise();

#endif
}
