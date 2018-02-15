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
        if(board[location].GetComponent<Square>().Piece != null){
            // We may want to do something other than just destroy the GameObject.  This will do for now
            GameObject.Destroy(board[location].GetComponent<Square>().Piece.gameObject);
            board[location].GetComponent<Square>().Piece = null;
        }else{
            Debug.LogError("No piece found at board[location].  Tried index: " + location);
        }
    }
}
