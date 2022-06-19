using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveAllFromSelectedCommand : CommandCommonInfo, ICommand
{
    private List<int> IDs;

    public RemoveAllFromSelectedCommand()
    {
        IDs = new List<int>();
        foreach (IGraphPart gp in selected.GetGraphPartsInList())
        {
            IDs.Add(gp.GetID());
        }
    }

    public bool Execute()
    {
        selected.UnSelect();
        selected.Clear();
        return true;
    }

    public bool Redo()
    {
        Execute();
        return true;
    }

    public bool Undo()
    {
        foreach (int ID in IDs)
        {
            IGraphPart part = IDManager.GetGP(ID);

            if (part.GetIsSelected())
                return true;
            selected.Add(part);
            part.Select();
            return true;
        }
        return true;
    }
}
