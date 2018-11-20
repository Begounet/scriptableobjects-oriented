using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class EventHelper
{
    internal static void Raise(Action evt)
    {
        if (evt != null)
        {
            evt.Invoke();
        }
    }

    internal static void Raise<T0>(Action<T0> evt, T0 arg1)
    {
        if (evt != null)
        {
            evt.Invoke(arg1);
        }
    }

    internal static void Raise<T0, T1>(Action<T0, T1> evt, T0 arg1, T1 arg2)
    {
        if (evt != null)
        {
            evt.Invoke(arg1, arg2);
        }
    }
}
