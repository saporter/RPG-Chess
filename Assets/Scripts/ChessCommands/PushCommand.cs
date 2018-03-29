using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushCommand : ChessCommand
{
    private int location;
    int direction;

    public PushCommand(int atLocation)
    {
        location = atLocation;
    }

    public override void Execute(List<GameObject> board)
    {
        List<ChessCommand> moves = new List<ChessCommand>();
        var Piece = board[location].GetComponent<Square>().Piece;
        if(Piece != null)
        {
            int x = location % 4;
            int y = location / 4;
            if(Piece.Team == Affiliation.White)
                moves.Add(new MoveCommand(Piece, location, Library.GetBoardIndex(x, y + 1)));
            else
                moves.Add(new MoveCommand(Piece, location, Library.GetBoardIndex(x, y - 1)));
        }
        else
        {
            Debug.LogError("No piece found at board[location].  Tried index: " + location);
        }
    }
}
