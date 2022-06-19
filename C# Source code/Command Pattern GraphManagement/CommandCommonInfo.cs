using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandCommonInfo : MonoBehaviour
{
    protected static Vertex vertexPrefab;
    protected static GameObject edgePrefab;

    protected static SubGraph selected;
    protected static SubGraph allEdges;
    protected static SubGraph allVertices;

    protected static Transform vertexParent;
    protected static Transform edgesParent;

    public static void MyStart(SubGraph _selected, SubGraph _allEdges, SubGraph _allVertices, Vertex prefabVertex, GameObject prefabEdge, Transform _vertexParent, Transform _edgeParent)
    {
        selected = _selected;
        allEdges = _allEdges;
        allVertices = _allVertices;
        vertexPrefab = prefabVertex;
        edgePrefab = prefabEdge;
        edgesParent = _edgeParent;
        vertexParent = _vertexParent;
    }
}
