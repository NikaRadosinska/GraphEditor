using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGraphPart 
{
    Vector3 GetWorldPos();
    void Remove(IGraphPart part);
    void MoveBy(Vector3 delta);
    void MoveAt(Vector3 position);
    void Select();
    void ExpandedSelect();
    void UnSelect();
    void ChangeWidth(float widthValue);
    IGraphPart GetGraphBeh();
    GraphPartType GetGraphPartType();
    bool isContainingGraphPart(IGraphPart part);
    bool GetIsSelected();
    void StopMovement();
    void Destroy();
    float GetBiggestX();
    float GetSmallestX();
    float GetBiggestY();
    float GetSmallestY();
    Vector2 GetNumOfVerticesAndEdges();
    List<Vertex> GetVertices();
    List<Edge> GetEdges();
    void SetID(int id);
    int GetID();
}
