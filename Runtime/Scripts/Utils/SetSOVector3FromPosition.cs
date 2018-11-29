using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SetSOVector3FromPosition : MonoBehaviour
{
    [SerializeField]
    [SOVariableMode(SOVariableActionMode.WriteOnly)]
    [Tooltip("Output the current GameObject transform to into it")]
    private SOVector3 _location;
    public SOVector3 Location
    {
        get { return _location; }
        set { _location = value; }
    }

    [SerializeField]
    private Space _space;
    public Space Space
    {
        get { return _space; }
        set { _space = value; }
    }

    private void OnValidate()
    {
        UpdateLocation();
    }

    void Update()
    {
        if (transform.hasChanged)
        {
            if (_location != null)
            {
                UpdateLocation();
                transform.hasChanged = false;
            }
        }
    }

    private void UpdateLocation()
    {
        if (_location != null)
        {
            _location.Value = (_space == Space.World ? this.transform.position : this.transform.localPosition);
        }
    }
}
