using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SO
{
    public enum BindingType
    {
        Optional,
        Required
    }
}

/// <summary>
/// Allow to tell if a SO variable should be bound 
/// because it is really important or just optional.
/// </summary>
public class SOBindingType : PropertyAttribute
{
    private SO.BindingType _binding;
    public SO.BindingType Binding
    {
        get { return _binding; }
    }

    public SOBindingType(SO.BindingType binding)
    {
        _binding = binding;
    }
}
