using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCommand : ChessCommand {
    private IChessPiece movingPiece;
    private int moveFrom;
    private int moveTo;

    public MoveCommand(IChessPiece piece, int from, int to)
    {
        movingPiece = piece;
        moveFrom = from;
        moveTo = to;
    }

    public override void Execute(List<GameObject> board)
    {
        board[moveFrom].GetComponent<Square>().Piece = null;
        board[moveTo].GetComponent<Square>().Piece = movingPiece;
        movingPiece.gameObject.transform.position = board[moveTo].GetComponent<Square>().transform.position;
    }

}
