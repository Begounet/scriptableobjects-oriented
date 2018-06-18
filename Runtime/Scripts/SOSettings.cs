using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Settings", menuName = "SO/Create/Settings", order = 100)]
public class SOSettings : ScriptableObject
{
    /// <summary>
    /// GUI pointing the path where the generated editor script will be added.
    /// However, there is no generated editor scripts anymore.
    /// Still present just in case.
    /// </summary>
    private string editorScriptPathGUID;

    /// <summary>
    /// Additional types that will be added to the Event Wizard
    /// </summary>
    [Tooltip("Additional types that will be added to the Event Wizard")]
    public string[] additionalCommonTypes;


}
