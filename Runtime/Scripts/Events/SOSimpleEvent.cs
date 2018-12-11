using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "SOSimpleEvent", menuName = "SO/Events/Simple Event")]
public class SOSimpleEvent : SOEvent
{
    [System.Serializable]
    public class Event : UnityEvent { }

    private Event _event = new Event();

    public void Raise()
    {
        _event.Invoke();
    }

    public void AddListener(UnityAction listener)
    {
        _event.AddListener(listener);
    }

    public void AddUniqueListener(UnityAction listener)
    {
        RemoveListener(listener);
        AddListener(listener);
    }

    public void RemoveListener(UnityAction listener)
    {
        _event.RemoveListener(listener);
    }

    public void Subscribe(UnityAction listener, bool subscribe)
    {
        if (subscribe) AddListener(listener);
        else RemoveListener(listener);
    }

#if UNITY_EDITOR

    public override void EditorRaise()
    {
        Raise();
    }    

#endif
}
