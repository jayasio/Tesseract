using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TrackerState
{
    public Vector3 position;
    public Quaternion rotation;
    public bool tracking = false;
}