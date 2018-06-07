using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SOEvent), true)]
public class SOEventEditor : Editor {
    
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUI.enabled = Application.isPlaying;
        if (GUILayout.Button("Raise"))
        {
            RaiseEvent();
        }
    }

    private void RaiseEvent()
    {
        SOEvent evt = target as SOEvent;
        evt.EditorRaise();
    }
}
