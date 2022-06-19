using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointCommand : CommandCommonInfo, ICommand
{
    public CheckpointCommand()
    {

    }

    public bool Execute()
    {
        return false;
    }

    public bool Redo()
    {
        return false;
    }

    public bool Undo()
    {
        return false;
    }
}
