using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ISONumeric : SOBaseVariable
{
    public abstract object GetValue();
    public abstract void SetValue(object newValue);
}
