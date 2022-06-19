using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class EdgeInfo
{
    bool locked;

    Vector2 indexName;

    public Vector3 leftEdgeEndPos;
    public Vector3 middlePos;
    public Vector3 rightEdgeEndPos;

    public EdgeInfo(bool locked, Vector3 leftEdgeEndPos, Vector3 middlePos, Vector3 rightEdgeEndPos)
    {
        this.locked = locked;
        this.leftEdgeEndPos = leftEdgeEndPos;
        this.middlePos = middlePos;
        this.rightEdgeEndPos = rightEdgeEndPos;
    }
}
