using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaptureCommand : ChessCommand
{
    private int location;
    public CaptureCommand(int atLocation){
        location = atLocation;
    }

    public override void Execute(List<GameObject> board)
    {
        throw new System.NotImplementedException();
    }
}
