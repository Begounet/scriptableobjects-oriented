using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SOEvent : ScriptableObject
{
    [TextArea]
    [Tooltip("Describes when your event will be called.")]
    public string description;

#if UNITY_EDITOR
    public abstract void EditorRaise();
#endif
}
