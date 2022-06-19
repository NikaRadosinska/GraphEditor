using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class IDManager
{
    private static Dictionary<int, IGraphPart> iGraphParts = new Dictionary<int, IGraphPart>();

    private static int currentId = int.MinValue;



    public static int SetId(IGraphPart gp)
    {
        if (iGraphParts.ContainsValue(gp))
        {
            return gp.GetID();
        }
        //Debug.Log("created id: " + currentId);
        gp.SetID(currentId);
        iGraphParts.Add(currentId, gp);
        currentId++;
        return currentId - 1;
    }

    public static void SetById(IGraphPart gp, int id)
    {
        iGraphParts[id] = gp;
        gp.SetID(id);
    }

    public static IGraphPart GetGP(int ID)
    {
        //Debug.Log("asked id: " + currentId);
        if(!iGraphParts.ContainsKey(ID)) return null;
        return iGraphParts[ID];
    }

    public static void DeleteRecord(int ID)
    {
        iGraphParts.Remove(ID);
    }
}
