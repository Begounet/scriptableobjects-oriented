using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SOEventActionMode
{
    Listener,
    Broadcaster
}

[System.AttributeUsage(System.AttributeTargets.Field)]
public class SOEventModeAttribute : System.Attribute
{
    public SOEventActionMode mode;

    public SOEventModeAttribute(SOEventActionMode Mode)
    {
        mode = Mode;
    }
}
