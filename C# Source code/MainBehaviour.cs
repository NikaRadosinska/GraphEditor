using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainBehaviour : AccessBehaviour
{
    public void AssignClass(AccessBehaviour common)
    {
        MainModel.CommonBehaviours.Add(common);
    }

    public virtual void OnDestroy()
    {
        if (MainModel.CommonBehaviours.Contains(this))
        {
            MainModel.CommonBehaviours.Remove(this);
        }
    }

    public virtual void Awake()
    {
        AssignClass(this);
    }
}
