using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Allow to reset a list of SO items at start or manually by script.
/// </summary>
public class SOReset : MonoBehaviour
{
    public bool resetAtStart = true;
    public SOBaseVariable[] items;

	void Start ()
    {
		if (resetAtStart)
        {
            Reset();
        }
	}

    public void Reset()
    {
        for (int i = 0; i < items.Length; ++i)
        {
            items[i].Reset();
        }
    }
}
