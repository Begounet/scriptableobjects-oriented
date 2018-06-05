using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "{EventName}", menuName = "Assets/SO{Category}/{EventName}/")]
public class {EventName} : SOEvent
{
    [System.Serializable]
    public class Event : UnityEvent<{UnityEventParams}> { }

    private Event _event = new Event();

    public void Raise({FunctionParamsDeclaration})
    {
        _event.Invoke({AllParamNames});
    }

    public void AddListener(UnityAction<{UnityEventParams}> listener)
    {
        _event.AddListener(listener);
    }

    public void RemoveListener(UnityAction<{UnityEventParams}> listener)
    {
        _event.RemoveListener(listener);
    }
}
