using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class EdgeInfoWithIds
{
    bool locked;

    public Vector3 leftEdgeEndPos;
    public Vector3 middlePos;
    public Vector3 rightEdgeEndPos;

    public int leftEdgeEndID;
    public int middleID;
    public int rightEdgeEndID;

    public EdgeInfoWithIds(bool locked, Vector3 leftEdgeEndPos, Vector3 middlePos, Vector3 rightEdgeEndPos, int leftEdgeEndID, int middleID, int rightEdgeEndID)
    {
        this.locked = locked;
        this.leftEdgeEndPos = leftEdgeEndPos;
        this.middlePos = middlePos;
        this.rightEdgeEndPos = rightEdgeEndPos;
        this.leftEdgeEndID = leftEdgeEndID;
        this.middleID = middleID;
        this.rightEdgeEndID = rightEdgeEndID;
    }
}
