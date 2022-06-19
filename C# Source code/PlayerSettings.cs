using System;
using System.Collections.Generic;

[Serializable]
public class PlayerSettings
{
    public float MouseSensitivity = 0.5f;
    public float MouseWheelSensitivity = 1f;
    public int curvesQuality = 10;
    public float lockedSensitivity = 0.1f;
    public List<SubGraphInfo> MyGraphs;
    public List<string> MyGraphNames;



    public bool ctrlToggleControl; //if false then controls are for holding
    public bool shiftToggleControl; //if false then controls are for holding
    public bool addingToggleControl; //if false then controls are for holding
}
