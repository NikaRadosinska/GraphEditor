using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AccessBehaviour : MonoBehaviour
{
    public Settings Settings => GetAssignedClass<Settings>();
    public GraphManager GraphManager => GetAssignedClass<GraphManager>(); 
    public UIManager UIManager => GetAssignedClass<UIManager>();

    private T GetAssignedClass<T>() where T : AccessBehaviour
    {
        return MainModel.GetAssignedClass<T>();
    }
}
