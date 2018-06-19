using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SOVariableActionMode
{
    ReadOnly,
    WriteOnly,
    ReadWrite
}

[System.AttributeUsage(System.AttributeTargets.Field, Inherited = true)]
public class SOVariableModeAttribute : System.Attribute
{
    public SOVariableActionMode mode;

    public SOVariableModeAttribute(SOVariableActionMode Mode)
    {
        mode = Mode;
    }
}
